using DataMigratorV14.NewData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV14.NewData.Queries;

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
        AddInclude(nameof(Recipe.PinnedImage));
        AddInclude(nameof(Recipe.Images));
        AddInclude(nameof(Recipe.Categories));
        AddInclude($"{nameof(Recipe.ShoppingItemRelations)}.{nameof(RecipeShoppingItemRelation.ShoppingItem)}");
    }
}
