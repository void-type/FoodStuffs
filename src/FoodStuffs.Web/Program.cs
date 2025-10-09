using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.ImageCompression;
using FoodStuffs.Model.Search;
using FoodStuffs.Model.Search.GroceryItems;
using FoodStuffs.Model.Search.Recipes;
using FoodStuffs.Web.Auth;
using FoodStuffs.Web.Startup;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
using VoidCore.Model.Auth;
using VoidCore.Model.Configuration;
using VoidCore.Model.Time;

try
{
    // Suppress browser refresh during development because it refreshes the page during Lucene indexing.
    Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPEXCLUDEASSEMBLIES", "Microsoft.AspNetCore.Watch.BrowserRefresh");

    var builder = WebApplication.CreateBuilder(args);
    var env = builder.Environment;
    var config = builder.Configuration;
    var services = builder.Services;

    builder.Host.UseDefaultServiceProvider((_, options) =>
    {
        options.ValidateScopes = true;
        options.ValidateOnBuild = true;
    });

    Log.Logger = new LoggerConfiguration()
        // Set a default logger if none configured or configuration not found.
        .WriteTo.Console()
        .ReadFrom.Configuration(config)
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Configuring host for {Name} v{Version}", ThisAssembly.AssemblyTitle, ThisAssembly.AssemblyInformationalVersion);

    // Settings
    services.AddSettingsSingleton<WebApplicationSettings>(config, true).Validate();
    services.AddSettingsSingleton<SearchSettings>(config, true).Validate();

    // Infrastructure
    services.AddControllers();
    services.AddSpaSecurityServices(env);
    services.AddApiExceptionFilter();
    services.AddSwagger(env);

    // Authorization
    services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();

    // Dependencies
    services.AddHttpContextAccessor();
    services.AddSingleton<IDateTimeService, NowDateTimeService>();
    services.AddSingleton<IImageCompressionService, ImageCompressionService>();

    config.GetRequiredConnectionString<FoodStuffsContext>();
    services.AddDbContext<FoodStuffsContext>(options => options
        .UseSqlServer("Name=FoodStuffs", b => b.MigrationsAssembly(typeof(FoodStuffsContext).Assembly.FullName)));

    services.AddScoped<IRecipeIndexService, RecipeIndexService>();
    services.AddScoped<IRecipeQueryService, RecipeQueryService>();

    services.AddScoped<IGroceryItemIndexService, GroceryItemIndexService>();
    services.AddScoped<IGroceryItemQueryService, GroceryItemQueryService>();

    services.AddScoped<ISearchIndexService, SearchIndexService>();
    services.AddHostedService<EnsureIndexHostedService>();

    // Auto-register Domain Events
    services.AddDomainEvents(
        ServiceLifetime.Scoped,
        typeof(GetWebClientInfo).Assembly,
        typeof(SearchRecipesHandler).Assembly);

    // Workers and background services

    var app = builder.Build();

    // Middleware pipeline
    app.UseAlwaysOnShortCircuit();
    app.UseSpaExceptionPage(env);
    app.UseSecureTransport(env);
    app.UseSecurityHeaders(env, config);
    app.MapStaticAssets();
    app.UseRouting();
    app.UseRequestLoggingScope();
    app.UseSerilogRequestLogging();
    app.UseCurrentUserLogging();
    app.UseSwaggerAndUi();
    app.UseSpaEndpoints();

    Log.Information("Starting host.");
    await app.RunAsync();
    return 0;
}
catch (HostAbortedException)
{
    // For EF tooling, let the exception throw and the tooling will catch it.
    throw;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.Information("Stopping host.");
    await Log.CloseAndFlushAsync();
}
