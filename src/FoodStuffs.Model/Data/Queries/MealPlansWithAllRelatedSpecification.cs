using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data.Queries;

public class MealPlansWithAllRelatedSpecification : QuerySpecificationAbstract<MealPlan>
{
    public MealPlansWithAllRelatedSpecification(int id)
    {
        AddOrderBy(r => r.Id);

        AddCriteria(r => r.Id == id);

        AddInclude($"{nameof(MealPlan.ExcludedGroceryItemRelations)}.{nameof(MealPlanExcludedGroceryItemRelation.GroceryItem)}");

        var recipe = $"{nameof(MealPlan.RecipeRelations)}.{nameof(MealPlanRecipeRelation.Recipe)}";
        AddInclude($"{recipe}.{nameof(Recipe.Categories)}");
        AddInclude($"{recipe}.{nameof(Recipe.Images)}");
        AddInclude($"{recipe}.{nameof(Recipe.PinnedImage)}");
        AddInclude($"{recipe}.{nameof(Recipe.GroceryItemRelations)}.{nameof(RecipeGroceryItemRelation.GroceryItem)}.{nameof(GroceryItem.GroceryAisle)}");
    }
}
