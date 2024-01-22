using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Configuration;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerAndUi(this IApplicationBuilder app, IHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            return app;
        }

        return app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.DocumentTitle = environment.ApplicationName + " API";
                c.UseRequestInterceptor("(req) => { req.headers['X-Csrf-Token'] = window.vt_api_csrf_token; return req; }");
            });
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app, IHostEnvironment environment)
    {
        app.UseContentSecurityPolicy(options =>
        {
            options.BaseUri
                .AllowSelf();

            options.FrameAncestors
                .AllowNone();

            options.DefaultSources
                .AllowSelf();

            options.ObjectSources
                .AllowNone();

            options.ImageSources
                .AllowSelf()
                // Bootstrap and other webpacked assets are loaded from inline data.
                .Allow("data:");

            options.ScriptSources
                .AllowSelf();

            options.StyleSources
                .AllowSelf();

            options.FrameAncestors
                .AllowNone();

            if (environment.IsDevelopment())
            {
                // In development we need to allow unsafe eval of scripts for Vue's runtime compiler.
                options.ScriptSources
                    // Vite dev server
                    .Allow("https://localhost:5173")
                    .AllowUnsafeInline()
                    .AllowUnsafeEval();

                options.StyleSources
                    // Vite dev server
                    .Allow("https://localhost:5173")
                    .AllowUnsafeInline();

                options.ImageSources
                    .Allow("https://localhost:5173");

                // .NET and Vite hot reloading use web sockets
                options.Custom("connect-src")
                    .AllowSelf()
                    // Vite dev server
                    .Allow("https://localhost:5173")
                    .Allow("ws:")
                    .Allow("wss:");
            }
            else
            {
                options.ScriptSources
                    // Add the Swagger UI scripts
                    .AllowNonce();

                options.StyleSources
                    // Add the Swagger UI scripts
                    .AllowNonce();
            }
        });

        app.UseXContentTypeOptionsNoSniff();

        return app;
    }
}
