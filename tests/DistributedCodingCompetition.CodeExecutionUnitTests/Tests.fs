module DistributedCodingCompetition.CodeExecutionUnitTests.Tests

open System
open Xunit
open DistributedCodingCompetition.CodeExecution.Services;
open DistributedCodingCompetition.CodeExecution.Models;

[<Fact>]
let ``Can balance 1 instance`` () =
    let runner weight = 
        { new IWeighted with member _.Weight = weight }
    
    let balancer = LoadBalancer()

    let instance = runner 100
    
    let result = balancer.BalanceRequest([instance])
    
    Assert.Equal(instance, result)

[<Fact>]
let ``Can balance 2 instances`` () =
    let runner weight = 
        { new IWeighted with member _.Weight = weight }
    
    let balancer = LoadBalancer()

    let instance1 = runner 50
    let instance2 = runner 50
    
    // run it 100 times and make sure it's balanced
    let results = [for _ in 1..100 -> balancer.BalanceRequest [instance1; instance2]]
    
    let i1 = results |> Seq.where (fun x -> x = instance1) |> Seq.length
    let i2 = results |> Seq.where (fun x -> x = instance2) |> Seq.length
    Assert.True(i1 > 0)
    Assert.True(i2 > 0)
    Assert.Equal(i1 + i2, 100);
    // make sure either one isn't off by more than 10%
    Assert.True(abs(float i1 - float i2) < 10.0)


[<Fact>]
let ``Can balance batch requests`` () =
    let runner weight = 
        { new IWeighted with member _.Weight = weight }

    let balancer = LoadBalancer()
    
    let requestCount = 10000

    let requests = [for _ in 1..requestCount -> Guid.NewGuid()]
    let runners = [for _ in 1..10 -> runner 1000]

    let result = balancer.BalanceRequests(runners, requests)
    let resultIds = result |> Seq.map (fun struct (x,_) -> x) |> Set.ofSeq
    // make sure results are all there
    Assert.Equal(Seq.length result, requestCount)

    Assert.True(Seq.forall resultIds.Contains requests)

    // make sure it's balanced
    let counts = result |> Seq.groupBy (fun struct (_,x) -> x) |> Seq.map (fun (x, y) -> (x, Seq.length y))
    let min = counts |> Seq.minBy (fun (_,x) -> x) |> snd
    let max = counts |> Seq.maxBy (fun (_,x) -> x) |> snd
    Assert.True(max - min < 100)
    