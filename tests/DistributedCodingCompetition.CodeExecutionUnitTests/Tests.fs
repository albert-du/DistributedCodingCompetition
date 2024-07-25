module Tests

open System
open Xunit
open DistributedCodingCompetition.CodeExecution.Services;
open DistributedCodingCompetition.CodeExecution;

type MockRunner(weight) = 
    interface IWeighted with
        member _.Weight = weight

[<Fact>]
let ``Can balance 1 instance`` () =
    let balancer = LoadBalancer<Guid, MockRunner>()
    balancer.BalanceRequest([runner 1], Guid.NewGuid())
    
    Assert.True(true)
