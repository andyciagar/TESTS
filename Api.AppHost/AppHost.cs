var builder = DistributedApplication.CreateBuilder(args);

var bd = builder.AddSqlServer("bdserver").AddDatabase("bd");

builder.AddProject<Projects.Api>("api").WithExternalHttpEndpoints().WithReference(bd);

builder.Build().Run();
