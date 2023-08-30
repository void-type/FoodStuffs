using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealSetsByIdWithAllRelatedSpecification : QuerySpecificationAbstract<MealSet>
{
    public MealSetsByIdWithAllRelatedSpecification(int id)
    {
        AddCriteria(r => r.Id == id);
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Categories)}");
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Images)}");
        AddInclude($"{nameof(MealSet.Recipes)}.{nameof(Recipe.Ingredients)}");
    }
}
