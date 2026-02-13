var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("health-db");

var seq = builder.AddSeq("seq")
    .WithLifetime(ContainerLifetime.Persistent)
    .ExcludeFromManifest()
    .WithEnvironment("ACCEPT_EULA", "Y");

builder.AddProject<Projects.CodeLab_HealthChecks_Api>("codelab-healthchecks-api")
    .WithEnvironment("ConnectionStrings__HealthDb", db)
    .WithReference(db)
    .WithReference(seq)
    .WaitFor(db);

builder.Build().Run();
