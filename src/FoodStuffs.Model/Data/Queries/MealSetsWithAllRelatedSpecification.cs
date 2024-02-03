using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsWithAllRelatedSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        IncludeAll();
    }

    private void IncludeAll()
    {
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Categories)}");
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Images)}");
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.PinnedImage)}");
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Ingredients)}");
    }
}
