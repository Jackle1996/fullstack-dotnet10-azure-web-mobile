var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("DefaultConnection")
                 .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.HealthTracking_Api>("healthtracking-Api")
    .WithReference(sql)
    .WaitFor(sql)
    .WithUrl("openapi", "Open API UI");

builder.AddProject<Projects.HealthTracking_Web>("healthtracking-web");

builder.Build().Run();
