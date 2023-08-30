using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesByIdWithCategoriesAndIngredientsSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesByIdWithCategoriesAndIngredientsSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(Recipe.Categories));
        AddInclude(nameof(Recipe.Ingredients));
    }
}
