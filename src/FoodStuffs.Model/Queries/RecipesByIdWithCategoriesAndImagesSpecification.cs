using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Queries
{
    public class RecipesByIdWithCategoriesAndImagesSpecification : QuerySpecificationAbstract<Recipe>
    {
        public RecipesByIdWithCategoriesAndImagesSpecification(int id) : base()
        {
            AddCriteria(r => r.Id == id);
            AddInclude($"{nameof(Recipe.CategoryRecipe)}.{nameof(CategoryRecipe.Category)}");
            AddInclude($"{nameof(Recipe.Image)}");
        }
    }
}
