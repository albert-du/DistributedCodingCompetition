var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var codeExecution = builder.AddProject<Projects.DistributedCodingCompetition_CodeExecution>("codeexecution");
var judge = builder.AddProject<Projects.DistributedCodingCompetition_Judge>("judge").WithReference(codeExecution);

builder.AddProject<Projects.DistributedCodingCompetition_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(judge)
    .WithReference(codeExecution);

builder.Build().Run();
