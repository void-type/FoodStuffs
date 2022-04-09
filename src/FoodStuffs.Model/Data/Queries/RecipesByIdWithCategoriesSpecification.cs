using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesByIdWithCategoriesSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesByIdWithCategoriesSpecification(int id) : base()
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(Recipe.Categories));
        AddInclude(nameof(Recipe.Images));
        AddInclude(nameof(Recipe.Ingredients));
    }
}
