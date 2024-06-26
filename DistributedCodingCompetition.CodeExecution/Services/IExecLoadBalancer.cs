namespace DistributedCodingCompetition.CodeExecution.Services;

using DistributedCodingCompetition.CodeExecution.Models;
using DistributedCodingCompetition.ExecutionShared;

/// <summary>
/// Load Balancer for execution requests.
/// </summary>
public interface IExecLoadBalancer
{
    /// <summary>
    /// Find the best runner for the request.
    /// 
    /// Don't use for batched requests.
    /// Does not execute.
    /// </summary>
    /// <param name="runners"></param>
    /// <param name="request"></param>
    /// <returns>Assigned Execution Runner</returns>
    ExecRunner? SelectRunner(IReadOnlyCollection<ExecRunner> runners, ExecutionRequest request);

    /// <summary>
    /// Balences requests for group of requests.
    /// Does not execute.
    /// </summary>
    /// <param name="runners"></param>
    /// <param name="requests"></param>
    /// <returns>Assigned Execution Runners</returns>
    IReadOnlyList<(ExecutionRequest, ExecRunner?)> BalanceRequests(IReadOnlyCollection<ExecRunner> runners, IReadOnlyCollection<ExecutionRequest> requests);
}
