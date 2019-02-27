using FoodStuffs.Model.Events.Recipes;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;

namespace FoodStuffs.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<GetWebApplicationInfo.Handler>();
            services.AddScoped<GetWebApplicationInfo.Logger>();

            services.AddScoped<GetRecipe.Handler>();
            services.AddScoped<GetRecipe.Logger>();

            services.AddScoped<ListRecipes.Handler>();
            services.AddScoped<ListRecipes.Logger>();

            services.AddScoped<SaveRecipe.Handler>();
            services.AddScoped<SaveRecipe.RequestValidator>();
            services.AddScoped<SaveRecipe.Logger>();

            services.AddScoped<DeleteRecipe.Handler>();
            services.AddScoped<DeleteRecipe.Logger>();
        }
    }
}
