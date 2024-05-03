using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealPlansWithAllRelatedSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        IncludeAll();
    }

    private void IncludeAll()
    {
        AddInclude($"{nameof(MealPlan.Recipes)}.{nameof(Recipe.Categories)}");
        AddInclude($"{nameof(MealPlan.Recipes)}.{nameof(Recipe.Images)}");
        AddInclude($"{nameof(MealPlan.Recipes)}.{nameof(Recipe.PinnedImage)}");
        AddInclude($"{nameof(MealPlan.Recipes)}.{nameof(Recipe.Ingredients)}");
    }
}
