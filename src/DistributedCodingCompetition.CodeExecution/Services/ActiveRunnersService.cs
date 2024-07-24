namespace DistributedCodingCompetition.CodeExecution.Services;

using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;
using System.Text.Json;

/// <inheritdoc/>
public class ActiveRunnersService(IDistributedCache distributedCache, IExecRunnerRepository execRunnerRepository, IExecRunnerService execRunnerService) : IActiveRunnersService
{
    private async Task IndexExecRunnersAsync()
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        // read all the exec runners
        var runners = await execRunnerRepository.GetExecRunnersAsync(enabled: true);
        var tasks = runners.Select(execRunnerService.RefreshExecRunnerAsync).ToArray();
        List<RunnerStatus> statuses = [];
        foreach (var task in tasks)
            statuses.Add(await task);

        Dictionary<string, List<ExecRunner>> languageMap = [];
        for (var i = 0; i < tasks.Length; i++)
        {
            var status = statuses[i];
            var runner = runners[i];
            if (status.Ready)
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
        // read the synchronous check
        var languages = await distributedCache.GetStringAsync("languages");

        if (languages is null)
            await IndexExecRunnersAsync();

        languages = await distributedCache.GetStringAsync("languages") ?? "";

        if (!languages.Contains(language))
            return null;

        // pull the exec runners from cache
        var execRunnerString = await distributedCache.GetStringAsync(language);
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

        var target = Random.Shared.Next(totalWeight);
        foreach (var (id, weight) in execRunners)
        {
            target -= weight;
            if (target < 0)
            {
                var runnerString = await distributedCache.GetStringAsync(id.ToString());
                if (runnerString is null)
                    return null;
                return JsonSerializer.Deserialize<ExecRunner>(runnerString);
            }
        }
        return null;
    }
}
