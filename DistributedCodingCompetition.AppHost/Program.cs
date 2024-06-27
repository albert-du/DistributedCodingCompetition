using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres");

//if (builder.Environment.IsDevelopment())
//    postgres.WithBindMount("/data/postgres", "/var/lib/postgresql/data");

var executorDatabase = postgres.AddDatabase("evaluationdb");

var contestDatabase = postgres.AddDatabase("contestdb");

var codeExecution = builder.AddProject<Projects.DistributedCodingCompetition_CodeExecution>("codeexecution")
                           .WithExternalHttpEndpoints()
                           .WithReference(executorDatabase);

var apiService = builder.AddProject<Projects.DistributedCodingCompetition_ApiService>("apiservice")
                        .WithReference(contestDatabase);

var judge = builder.AddProject<Projects.DistributedCodingCompetition_Judge>("judge")
                   .WithReference(codeExecution);

builder.AddProject<Projects.DistributedCodingCompetition_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(cache)
       .WithReference(apiService)
       .WithReference(judge)
       .WithReference(codeExecution);

builder.Build().Run();
