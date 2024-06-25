using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres");

if (builder.Environment.IsDevelopment())
{
    postgres.WithBindMount("./data/postgres", "/var/lib/postgresql/data");
}

var executorDatabase = postgres.AddDatabase("evaluationdb");

var codeExecution = builder.AddProject<Projects.DistributedCodingCompetition_CodeExecution>("codeexecution")
                           .WithExternalHttpEndpoints()
                           .WithReference(executorDatabase);

var judge = builder.AddProject<Projects.DistributedCodingCompetition_Judge>("judge")
                   .WithReference(codeExecution);

builder.AddProject<Projects.DistributedCodingCompetition_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(cache)
       .WithReference(judge)
       .WithReference(codeExecution);

builder.Build().Run();
