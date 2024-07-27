namespace DistributedCodingCompetition.CodeExecution.Services;

using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;
using System.Text.Json;

/// <inheritdoc/>
public class ActiveRunnersService(IDistributedCache distributedCache, IExecRunnerRepository execRunnerRepository, IExecRunnerService execRunnerService) : IActiveRunnersService
{

    /// <inheritdoc/>
    public async Task IndexExecRunnersAsync()
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        };

        // read all the exec runners
        var runners = await execRunnerRepository.GetExecRunnersAsync(enabled: true);
        var tasks = runners.Select(execRunnerService.RefreshExecRunnerAsync).ToArray();
        List<RunnerStatus?> statuses = [];
        foreach (var task in tasks)
            statuses.Add(await task);

        Dictionary<string, List<ExecRunner>> languageMap = [];
        for (var i = 0; i < tasks.Length; i++)
        {
            var status = statuses[i];
            var runner = runners[i];
            if (status?.Ready is true)
                foreach (var language in status.Languages.Split('\n'))
                {
                    if (!languageMap.ContainsKey(language))
                        languageMap[language] = [];
                    languageMap[language].Add(runner);
                }
        }

        // final string in redis: 
        // key:RUNNERS-language value:total-weight;uuid=weight,uuid=weight,uuid=weight
        // then a second lookup to bypass the database
        // key:uuid value:json

        foreach (var pair in languageMap)
        {
            var language = pair.Key;
            var execRunners = pair.Value;
            // sum the weights
            var totalWeight = execRunners.Sum(runner => runner.Weight);
            var section = string.Join(',', execRunners.Select(runner => $"{runner.Id}={runner.Weight}"));
            var execRunnerString = $"{totalWeight};{section}";
            distributedCache.SetString($"RUNNERS-{language}", execRunnerString, options);
        }

        // iterate through the exec runners and set the individual values
        foreach (var runner in runners)
        {
            var runnerString = JsonSerializer.Serialize(runner);
            distributedCache.SetString(runner.Id.ToString(), runnerString, options);
        }
        distributedCache.SetString("languages", string.Join(';', languageMap.Keys), options);
    }

    /// <inheritdoc/>
    public async Task<ExecRunner?> FindExecRunnerAsync(string language)
    {
        var languages = await GetLanguagesAsync();

        if (!languages.Contains(language))
            return null;

        // pull the exec runners from cache
        var execRunnerString = await distributedCache.GetStringAsync($"RUNNERS-{language}");
        if (execRunnerString is null)
            return null;

        var parts = execRunnerString.Split(';');
        var totalWeight = int.Parse(parts[0]);
        var execRunners = parts[1].Split(',').Select(part =>
        {
            var pair = part.Split('=');
            return (Guid.Parse(pair[0]), int.Parse(pair[1]));
        }).ToArray();

        // select a random exec runner
        LoadBalancer<ExecutionRequest, RunnerWeight> balancer = new();

        var id = balancer.BalanceRequest([.. execRunners.Select(x => new RunnerWeight(x.Item1, x.Item2))]);

        var runnerString = await distributedCache.GetStringAsync(id.ToString());

        if (runnerString is null)
            return null;

        return JsonSerializer.Deserialize<ExecRunner>(runnerString);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<(ExecutionRequest request, ExecRunner? runner)>> BalanceRequestsAsync(IReadOnlyCollection<ExecutionRequest> requests)
    {
        var languages = await GetLanguagesAsync();

        var byLang = requests.GroupBy(request => request.Language).ToDictionary(group => group.Key, group => group.ToArray());

        List<(ExecutionRequest, ExecRunner?)> results = [];

        foreach (var (language, reqs) in byLang)
        {
            if (!languages.Contains(language))
            {
                foreach (var req in reqs)
                    results.Add((req, null));
                continue;
            }

            // pull the exec runners from cache
            var execRunnerString = await distributedCache.GetStringAsync($"RUNNERS-{language}");
            if (execRunnerString is null)
            {
                foreach (var req in reqs)
                    results.Add((req, null));
                continue;
            }

            var parts = execRunnerString.Split(';');
            var totalWeight = int.Parse(parts[0]);
            var execRunners = parts[1].Split(',').Select(part =>
            {
                var pair = part.Split('=');
                return (Guid.Parse(pair[0]), int.Parse(pair[1]));
            }).ToArray();
            // select a random exec runner

            var runners = execRunners.Select(x => new RunnerWeight(x.Item1, x.Item2)).ToArray();
            LoadBalancer<ExecutionRequest, RunnerWeight> balancer = new();
            var pairs = balancer.BalanceRequests(runners, reqs);
            foreach (var (request, runner) in pairs)
            {
                var runnerString = await distributedCache.GetStringAsync(runner.Id.ToString());
                if (runnerString is not null)
                    results.Add((request, JsonSerializer.Deserialize<ExecRunner>(runnerString)));
                else
                    results.Add((request, null));
            }
        }

        return results;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetLanguagesAsync()
    {
        var languages = await distributedCache.GetStringAsync("languages");
        if (languages is null)
            await IndexExecRunnersAsync();
        languages = await distributedCache.GetStringAsync("languages");
        return languages?.Split(';') ?? [];
    }

    private record struct RunnerWeight(Guid Id, int Weight) : IWeighted;
}
