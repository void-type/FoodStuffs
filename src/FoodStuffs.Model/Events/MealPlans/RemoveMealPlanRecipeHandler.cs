using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.MealPlans.Models;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

public class RemoveMealPlanRecipeHandler : CustomEventHandlerAbstract<RemoveMealPlanRecipeRequest, EntityResponse<GetMealPlanResponse>>
{
    private readonly FoodStuffsContext _data;

    public RemoveMealPlanRecipeHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityResponse<GetMealPlanResponse>>> Handle(RemoveMealPlanRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var maybeMealPlan = await MealPlanQueryHelper.FindWithAllRelatedAsync(
            _data, request.Id, GetTag(), cancellationToken);

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

        _data.MealPlans.Update(mealPlan);
        await _data.SaveChangesAsync(cancellationToken);

        var reloadedMealPlan = await MealPlanQueryHelper.GetWithAllRelatedAsync(
            _data, request.Id, GetTag(), cancellationToken);

        return Ok(EntityResponse.Create("Recipe removed from meal plan.", reloadedMealPlan.ToGetMealPlanResponse()));
    }
}
