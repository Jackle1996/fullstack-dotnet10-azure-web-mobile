var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("DefaultConnection")
                 .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.HealthTracking_WebApi>("healthtracking-webapi")
    .WithReference(sql)
    .WaitFor(sql)
    .WithUrl("openapi", "Open API UI");

builder.Build().Run();
