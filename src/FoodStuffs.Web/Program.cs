﻿using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.Recipes;
using FoodStuffs.Model.ImageCompression;
using FoodStuffs.Model.Search;
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

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
var services = builder.Services;

// Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Configuring host for {Name} v{Version}", ThisAssembly.AssemblyTitle, ThisAssembly.AssemblyInformationalVersion);

    // Settings
    services.AddSettingsSingleton<WebApplicationSettings>(config, true).Validate();
    services.AddSettingsSingleton<RecipeSearchSettings>(config, true).Validate();

    // Infrastructure
    services.AddControllers();
    services.AddSpaSecurityServices(env);
    services.AddApiExceptionFilter();

    // Authorization

    // Dependencies
    services.AddHttpContextAccessor();
    services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
    services.AddSingleton<IDateTimeService, NowDateTimeService>();
    services.AddSingleton<IImageCompressionService, ImageCompressionService>();

    config.GetRequiredConnectionString<FoodStuffsContext>();
    services.AddDbContext<FoodStuffsContext>(options => options
        .UseSqlServer("Name=FoodStuffs", b => b.MigrationsAssembly(typeof(FoodStuffsContext).Assembly.FullName)));

    services.AddScoped<IRecipeIndexService, RecipeIndexService>();
    services.AddScoped<IRecipeQueryService, RecipeQueryService>();

    // Auto-register Domain Events
    services.AddDomainEvents(
        ServiceLifetime.Scoped,
        typeof(GetWebClientInfo).Assembly,
        typeof(SearchRecipesHandler).Assembly);

    services.AddSwaggerWithCsp(env);

    services.AddHostedService<EnsureIndexHostedService>();

    var app = builder.Build();

    // Middleware pipeline
    app.UseAlwaysOnShortCircuit();
    app.UseSpaExceptionPage(env);
    app.UseSecureTransport(env);
    app.UseSecurityHeaders(env);
    app.UseStaticFiles();
    app.UseRouting();
    app.UseRequestLoggingScope();
    app.UseSerilogRequestLogging();
    app.UseCurrentUserLogging();
    app.UseSwaggerAndUi(env);
    app.UseSpaEndpoints();

    Log.Information("Starting host.");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.Information("Stopping host.");
    Log.CloseAndFlush();
}
