var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres").WithPgAdmin();

var mongo = builder.AddMongoDB("mongo");

var executorDatabase = postgres.AddDatabase("evaluationdb");

var contestDatabase = postgres.AddDatabase("contestdb");

var authDatabase = mongo.AddDatabase("authdb");

var codePersistanceDatabase = mongo.AddDatabase("codepersistencedb");

var auth = builder.AddProject<Projects.DistributedCodingCompetition_AuthService>("authentication")
                  .WithReference(authDatabase);

var codePersistence = builder.AddProject<Projects.DistributedCodingCompetition_CodePersistence>("codepersistence")
                             .WithReference(codePersistanceDatabase);

var codeExecution = builder.AddProject<Projects.DistributedCodingCompetition_CodeExecution>("codeexecution")
                           .WithExternalHttpEndpoints()
                           .WithReference(executorDatabase);

var apiService = builder.AddProject<Projects.DistributedCodingCompetition_ApiService>("apiservice")
                        .WithReference(contestDatabase);

var liveLeaders = builder.AddProject<Projects.DistributedCodingCompetition_LiveLeaders>("liveleaders");

var judge = builder.AddProject<Projects.DistributedCodingCompetition_Judge>("judge")
                   .WithReference(cache)
                   .WithReference(apiService)
                   .WithReference(codeExecution)
                   .WithReference(liveLeaders);


var leaderboard = builder.AddProject<Projects.DistributedCodingCompetition_Leaderboard>("leaderboard")
                         .WithReference(apiService)
                         .WithReference(cache)
                         .WithReference(liveLeaders);

builder.AddProject<Projects.DistributedCodingCompetition_Web>("webfrontend")
       .WithExternalHttpEndpoints()
       .WithReference(cache)
       .WithReference(apiService)
       .WithReference(judge)
       .WithReference(auth)
       .WithReference(codePersistence)
       .WithReference(leaderboard)
       .WithReference(codeExecution);




builder.Build().Run();
