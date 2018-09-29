using FoodStuffs.Model.DomainEvents.Recipes;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStuffs.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomainEvents(this IServiceCollection services)
        {
            services.AddTransient<GetRecipe.Handler>();
            services.AddTransient<GetRecipe.Logging>();

            services.AddTransient<ListRecipes.Handler>();
            services.AddTransient<ListRecipes.Logging>();

            services.AddTransient<SaveRecipe.Handler>();
            services.AddTransient<SaveRecipe.RequestValidator>();
            services.AddTransient<SaveRecipe.Logging>();

            services.AddTransient<DeleteRecipe.Handler>();
            services.AddTransient<DeleteRecipe.Logging>();
        }
    }
}
