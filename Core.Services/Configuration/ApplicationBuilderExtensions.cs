using Core.Services.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Core.Services.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddMvcSpaRoute(this IApplicationBuilder app)
        {
            return app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "spa-fallback",
                    template: "{*url}",
                    defaults : new { controller = "Home", action = "Index" });
            });
        }

        public static IApplicationBuilder UseExceptionPage(this IApplicationBuilder app, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseStatusCodePages(context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;
                var path = request.Path.ToString();

                if (response.StatusCode == 403)
                {
                    if (!path.StartsWith(ApiRoute.BasePath))
                    {
                        response.Redirect("/forbidden");
                    }
                }
                return Task.FromResult<object>(null);
            });
            return app;
        }
    }
}
