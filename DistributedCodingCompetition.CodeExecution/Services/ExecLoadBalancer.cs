namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

public class ExecLoadBalancer(ILogger<ExecLoadBalancer> logger) : IExecLoadBalancer
{
    public ExecRunner? SelectRunner(IReadOnlyCollection<ExecRunner> runners, ExecutionRequest request)
    {
        var supportedRunner = runners.Where(r => r.Languages.Contains(request.Language));
        var totalweight = runners.Sum(r => r.Weight);
        var random = new Random();

        var choice = random.NextDouble() * totalweight;

        foreach (var runner in runners)
        {
            choice -= runner.Weight;
            if (choice <= 0)
                return runner;
        }
        // Can't find a runner.
        return null;
    }

    public IReadOnlyList<(ExecutionRequest, ExecRunner?)> BalanceRequests(IReadOnlyCollection<ExecRunner> runners, IReadOnlyCollection<ExecutionRequest> requests)
    {
        Dictionary<string, List<ExecRunner>> supportedRunners = [];
        foreach (var request in requests)
        {
            var lang = request.Language;
            if (!supportedRunners.ContainsKey(lang))
                supportedRunners[lang] = [];
        }
        foreach (var runner in runners)
        {
            foreach (var lang in runner.Languages)
            {
                if (supportedRunners.TryGetValue(lang, out var execRunners))
                    execRunners.Add(runner);
            }
        }
        List<(ExecutionRequest, ExecRunner?)> result = [];
        foreach (var request in requests)
        {
            var runner = SelectRunner(supportedRunners[request.Language], request);
            if (runner is not null)
                result.Add((request, runner));
            else
                result.Add((request, null));

        }
        return result;
    }
}
