using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.MealPlans.Models;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

public class RemoveMealPlanRecipeHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<RemoveMealPlanRecipeRequest, EntityResponse<GetMealPlanResponse>>
{
    public override async Task<IResult<EntityResponse<GetMealPlanResponse>>> Handle(RemoveMealPlanRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var maybeMealPlan = await MealPlanQueryHelper.FindWithAllRelatedAsync(
            data, request.Id, GetTag(), cancellationToken);

        if (!maybeMealPlan.HasValue)
        {
            return Fail(new MealPlanNotFoundFailure());
        }

        var mealPlan = maybeMealPlan.Value;

        var existingRelation = mealPlan.RecipeRelations.Find(r => r.Recipe.Id == request.RecipeId);

        if (existingRelation is null)
        {
            var response = EntityResponse.Create("Recipe is not on the meal plan.", mealPlan.ToGetMealPlanResponse());
            return Ok(response);
        }

        mealPlan.RecipeRelations.Remove(existingRelation);

        data.MealPlans.Update(mealPlan);
        await data.SaveChangesAsync(cancellationToken);

        var reloadedMealPlan = await MealPlanQueryHelper.GetWithAllRelatedAsync(
            data, request.Id, GetTag(), cancellationToken);

        return Ok(EntityResponse.Create("Recipe removed from meal plan.", reloadedMealPlan.ToGetMealPlanResponse()));
    }
}
