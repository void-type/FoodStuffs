using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Startup;

public static class SwaggerStartupExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IHostEnvironment environment)
    {
        // Generate OpenAPI 3.0 document.
        services.AddOpenApiDocument((configure, provider) =>
        {
            configure.Title = environment.ApplicationName;
            configure.Version = ThisAssembly.AssemblyInformationalVersion.Split('+').FirstOrDefault();
            configure.UseControllerSummaryAsTagDescription = true;
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerAndUi(this IApplicationBuilder app)
    {
        // Add OpenAPI 3.0 document serving middleware
        // Available at: /swagger/v1/swagger.json
        app.UseOpenApi(c =>
        {
            // Clear the current server so the generated httpClient can be environment agnostic.
            c.PostProcess = (document, _) => document.Servers.Clear();
        });


        // Add web UIs to interact with the document
        // Available at: /swagger
        app.UseSwaggerUi(c =>
        {
            // Add script to fetch the csrf token and store it so the dev doesn't have to load the Vue App.
            c.CustomHeadContent = """<script src="/js/swagger-csrf.js"></script>""";

            // This is an intentional script injection to add antiforgery support. https://github.com/RicoSuter/NSwag/issues/4108
            c.AdditionalSettings.Add("requestInterceptor: (req) => { req.headers['" + SecurityConstants.AntiforgeryTokenHeaderName + "'] = window.vt_api_csrf_token; return req; },//", string.Empty);
        });

        return app;
    }
}
