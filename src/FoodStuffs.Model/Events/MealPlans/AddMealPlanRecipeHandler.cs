using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.MealPlans.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.MealPlans;

public class AddMealPlanRecipeHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<AddMealPlanRecipeRequest, EntityResponse<GetMealPlanResponse>>
{
    public override async Task<IResult<EntityResponse<GetMealPlanResponse>>> Handle(AddMealPlanRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var maybeMealPlan = await MealPlanQueryHelper.FindWithAllRelatedAsync(
            data, request.Id, GetTag(), cancellationToken);

        if (!maybeMealPlan.HasValue)
        {
            return Fail(new MealPlanNotFoundFailure());
        }

        var mealPlan = maybeMealPlan.Value;

        if (mealPlan.RecipeRelations.Any(r => r.Recipe.Id == request.RecipeId))
        {
            var response = EntityResponse.Create("Recipe is already on the meal plan.", mealPlan.ToGetMealPlanResponse());
            return Ok(response);
        }

        var recipeSpec = new RecipesSpecification(request.RecipeId);

        var maybeRecipe = await data.Recipes
            .TagWith(GetTag(recipeSpec))
            .ApplyEfSpecification(recipeSpec)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (!maybeRecipe.HasValue)
        {
            return Fail(new RecipeNotFoundFailure());
        }

        var maxOrder = mealPlan.RecipeRelations.Count > 0
            ? mealPlan.RecipeRelations.Max(r => r.Order)
            : 0;

        mealPlan.RecipeRelations.Add(new MealPlanRecipeRelation
        {
            Recipe = maybeRecipe.Value,
            Order = maxOrder + 1,
        });

        data.MealPlans.Update(mealPlan);
        await data.SaveChangesAsync(cancellationToken);

        var reloadedMealPlan = await MealPlanQueryHelper.GetWithAllRelatedAsync(
            data, request.Id, GetTag(), cancellationToken);

        return Ok(EntityResponse.Create("Recipe added to meal plan.", reloadedMealPlan.ToGetMealPlanResponse()));
    }
}
