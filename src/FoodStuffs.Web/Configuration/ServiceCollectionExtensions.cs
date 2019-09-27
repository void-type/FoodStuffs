using FoodStuffs.Model.Events.Recipes;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;

namespace FoodStuffs.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomainEvents(this IServiceCollection services)
        {
            services.AddSingleton<GetWebApplicationInfo.Handler>();
            services.AddSingleton<GetWebApplicationInfo.Logger>();

            services.AddScoped<GetRecipe.Handler>();
            services.AddSingleton<GetRecipe.Logger>();

            services.AddScoped<ListRecipes.Handler>();
            services.AddSingleton<ListRecipes.Logger>();

            services.AddScoped<SaveRecipe.Handler>();
            services.AddSingleton<SaveRecipe.RequestValidator>();
            services.AddSingleton<SaveRecipe.Logger>();

            services.AddScoped<DeleteRecipe.Handler>();
            services.AddSingleton<DeleteRecipe.Logger>();
        }
    }
}
