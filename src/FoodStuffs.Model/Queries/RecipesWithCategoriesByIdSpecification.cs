using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Queries;

namespace FoodStuffs.Model.Queries
{
    public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdWithCategoriesSpecification(int id) : base(r => r.Id == id)
        {
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
        }
    }
}
