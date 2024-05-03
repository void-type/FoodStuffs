using VoidCore.AspNet.Security;

namespace FoodStuffs.Web.Startup;

public static class SecurityStartupExtensions
{
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
                // Bootstrap and other webpack'd assets are loaded from inline data.
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
                    // Add the Swagger UI style
                    .AllowNonce();
            }
        });

        app.UseXContentTypeOptionsNoSniff();

        return app;
    }
}
