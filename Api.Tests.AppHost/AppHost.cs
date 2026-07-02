var builder = DistributedApplication.CreateBuilder(args);

builder.AddSqlServer("bdserver-tests").AddDatabase("bd");

builder.Build().Run();