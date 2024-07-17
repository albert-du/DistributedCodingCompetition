namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

/// <inheritdoc />
public class ExecLoadBalancer : IExecLoadBalancer
{
    private readonly IReadOnlyDictionary<string, List<ExecRunner>> languageRunners;
    private readonly IReadOnlyDictionary<string, int> totalWeights;

    /// <summary>
    /// Constructor for ExecLoadBalancer.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="execRunnerContext"></param>
    public ExecLoadBalancer(ILogger<ExecLoadBalancer> logger, ExecRunnerContext execRunnerContext)
    {
        Dictionary<string, int> weights = [];
        Dictionary<string, List<ExecRunner>> allRunners = [];
        Console.WriteLine(execRunnerContext.ExecRunners.Count());
        foreach (var runner in execRunnerContext.ExecRunners.AsNoTracking())
        {
            if (!runner.Live || !runner.Available || !runner.Enabled)
                continue;

            foreach (var language in runner.Languages)
            {
                if (allRunners.TryGetValue(language, out var runners))
                    runners.Add(runner);
                else
                    allRunners.Add(language, [runner]);
                if (!weights.ContainsKey(language))
                    weights.Add(language, 0);
                weights[language] += runner.Weight;
            }
        }
        languageRunners = allRunners.ToFrozenDictionary();
        totalWeights = weights.ToFrozenDictionary();

    }

    /// <inheritdoc />
    public ExecRunner? SelectRunner(ExecutionRequest request)
    {
        var runners = languageRunners[request.Language];

        var totalweight = totalWeights[request.Language];

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

    /// <inheritdoc />
    public IReadOnlyList<(ExecutionRequest, ExecRunner?)> BalanceRequests(IReadOnlyCollection<ExecutionRequest> requests)
    {
        // Balance requests by language.
        var balancedRequests = requests.GroupBy(x => x.Language).Select(x => x.ToList()).ToList();
        var results = new List<(ExecutionRequest, ExecRunner?)>(requests.Count);
        foreach (var requestList in balancedRequests)
        {
            var runner = SelectRunner(requestList[0]);
            foreach (var request in requestList)
                results.Add((request, runner));
        }
        return results;
    }
}
