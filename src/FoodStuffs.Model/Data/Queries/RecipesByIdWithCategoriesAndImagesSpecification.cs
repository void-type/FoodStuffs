using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesByIdWithCategoriesAndImagesSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesByIdWithCategoriesAndImagesSpecification(int id) : base()
    {
        AddCriteria(r => r.Id == id);
        AddInclude($"{nameof(Recipe.CategoryRecipes)}.{nameof(CategoryRecipe.Category)}");
        AddInclude(nameof(Recipe.Images));
    }
}
