using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesWithAllRelatedSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesWithAllRelatedSpecification()
    {
        IncludeAll();
    }

    public RecipesWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        IncludeAll();
    }

    private void IncludeAll()
    {
        AddInclude(nameof(Recipe.Categories));
        AddInclude(nameof(Recipe.PinnedImage));
        AddInclude(nameof(Recipe.Images));
        AddInclude(nameof(Recipe.Ingredients));
        AddInclude($"{nameof(Recipe.ShoppingItems)}.{nameof(RecipeShoppingItemRelation.ShoppingItem)}");
    }
}
