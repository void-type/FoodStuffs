using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class RecipesWithAllRelatedSpecification : QuerySpecificationAbstract<Recipe>
{
    public RecipesWithAllRelatedSpecification()
    {
        AddOrderBy(r => r.Id);

        AddInclude(nameof(Recipe.PinnedImage));
        AddInclude(nameof(Recipe.Images));
        AddInclude(nameof(Recipe.Categories));
        AddInclude($"{nameof(Recipe.ShoppingItemRelations)}.{nameof(RecipeShoppingItemRelation.ShoppingItem)}");
    }

    public RecipesWithAllRelatedSpecification(int id) : this()
    {
        AddCriteria(r => r.Id == id);
    }

    public RecipesWithAllRelatedSpecification(IEnumerable<int> ids) : this()
    {
        AddCriteria(r => ids.Contains(r.Id));
    }
}
