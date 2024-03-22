using DataMigratorV13.OldData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV13.OldData.Queries;

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
    }
}
