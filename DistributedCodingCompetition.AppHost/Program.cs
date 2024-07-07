var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres");

var mongo = builder.AddMongoDB("mongo");

var executorDatabase = postgres.AddDatabase("evaluationdb");

var contestDatabase = postgres.AddDatabase("contestdb");

var authDatabase = mongo.AddDatabase("authdb");

var codePersistanceDatabase = mongo.AddDatabase("authdb");

var auth = builder.AddProject<Projects.DistributedCodingCompetition_AuthService>("authentication")
                  .WithReference(authDatabase);

var codePersistance = builder.AddProject<Projects.DistributedCodingCompetition_CodePersistence>("codepersistance")
                            .WithReference(codePersistanceDatabase);

var codeExecution = builder.AddProject<Projects.DistributedCodingCompetition_CodeExecution>("codeexecution")
                           .WithExternalHttpEndpoints()
                           .WithReference(executorDatabase);

var apiService = builder.AddProject<Projects.DistributedCodingCompetition_ApiService>("apiservice")
                        .WithReference(contestDatabase);

var judge = builder.AddProject<Projects.DistributedCodingCompetition_Judge>("judge")
                   .WithReference(cache)
                   .WithReference(apiService)
                   .WithReference(codeExecution);

builder.AddProject<Projects.DistributedCodingCompetition_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(cache)
       .WithReference(apiService)
       .WithReference(judge)
       .WithReference(auth)
       .WithReference(codePersistance)
       .WithReference(codeExecution);

builder.AddProject<Projects.DistributedCodingCompetition_CodePersistence>("distributedcodingcompetition-codepersistence");

builder.Build().Run();
