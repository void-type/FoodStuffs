using FoodStuffs.Model.Data;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace FoodStuffs.Web
{
    public class Startup
    {
        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSpaExceptionPage(_env)
                .UseSecureTransport(_env)
                .UseStaticFiles()
                .UseSpaMvcRoute();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSettingsSingleton<ApplicationSettings>(_config);
            var connectionStrings = services
                .AddSettingsSingleton<ConnectionStringSettings>(_config.GetSection(ConnectionStringSettings.SectionName));

            services.AddAntiforgery();
            services.AddApiExceptionFilter(_env);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILoggingService, MicrosoftLoggingAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpResponder>();

            services.AddSqlServerDbContext<FoodStuffsContext>(connectionStrings.FoodStuffs);
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
        }

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
    }
}
