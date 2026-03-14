var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .AddDatabase("FoodStuffs");

builder.AddProject<Projects.FoodStuffs_Web>("web")
    .WithReference(sql)
    .WaitFor(sql)
    .WithEnvironment("AutoMigrate", "true")
    .WithEnvironment("SearchSettings__IndexFolder", Path.Combine(Path.GetTempPath(), "FoodStuffs_E2eTest", "Lucene"))
    .WithEnvironment("VueDevServer__Enabled", "false");

builder.Build().Run();
