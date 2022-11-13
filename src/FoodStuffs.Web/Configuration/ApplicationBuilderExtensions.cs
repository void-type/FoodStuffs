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
            .UseSwaggerUI(c => c.DocumentTitle = environment.ApplicationName + " API");
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
                    .AllowUnsafeInline()
                    .AllowUnsafeEval();

                options.StyleSources
                    .AllowUnsafeInline();
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
