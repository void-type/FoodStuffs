using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesByIdWithAllRelatedSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesByIdWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude(nameof(Recipe.Categories));
        AddInclude(nameof(Recipe.Images));
        AddInclude(nameof(Recipe.Ingredients));
    }
}
