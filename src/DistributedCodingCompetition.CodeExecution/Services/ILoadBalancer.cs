namespace DistributedCodingCompetition.CodeExecution.Services;

/// <summary>
/// Load balancer interface
/// </summary>
public interface ILoadBalancer<T, U> where U : IWeighted
{
    /// <summary>
    /// Balance requests across exec runners.
    /// Only 1 language across all requests.
    /// </summary>
    /// <param name="requests"></param>
    /// <param name="runners"></param>
    /// <returns></returns>
    IEnumerable<(T request, U? runner)> BalanceRequests(IReadOnlyCollection<U> runners, IReadOnlyCollection<T> requests);

    /// <summary>
    /// Balance a single request across exec runners.
    /// All exec runners must support the language.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="runners"></param>
    /// <returns></returns>
    U? BalanceRequest(IReadOnlyCollection<U> runners, T request);
}
