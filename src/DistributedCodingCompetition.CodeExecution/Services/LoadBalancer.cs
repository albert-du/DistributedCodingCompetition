namespace DistributedCodingCompetition.CodeExecution.Services;

/// <inheritdoc/>
public class LoadBalancer<T, U> where U : IWeighted, ILoadBalancer<T, U>
{
    /// <inheritdoc/>
    public IEnumerable<(T request, U? runner)> BalanceRequests(IReadOnlyCollection<U> runners, IReadOnlyCollection<T> requests)
    {
        var totalWeight = runners.Sum(runner => runner.Weight);

        foreach (var request in requests)
        {
            var target = Random.Shared.Next(totalWeight);
            foreach (var runner in runners)
            {
                target -= runner.Weight;
                if (target < 0)
                {
                    yield return (request, runner);
                    break;
                }
            }
        }
    }

    /// <inheritdoc/>
    public U? BalanceRequest(IReadOnlyCollection<U> runners, T request)
    {
        var totalWeight = runners.Sum(runner => runner.Weight);
        var target = Random.Shared.Next(totalWeight);
        foreach (var runner in runners)
        {
            target -= runner.Weight;
            if (target < 0)
                return runner;
        }
        return default;
    }
}
