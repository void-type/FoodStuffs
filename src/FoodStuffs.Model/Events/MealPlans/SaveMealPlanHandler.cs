using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanHandler : CustomEventHandlerAbstract<SaveMealPlanRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public SaveMealPlanHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealPlansWithAllRelatedSpecification(request.Id);

        var maybeMealPlan = await _data.MealPlans
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        var mealPlanToEdit = maybeMealPlan.Unwrap(() => new MealPlan());

        Transfer(request, mealPlanToEdit);
        await ManageRecipes(request, mealPlanToEdit, cancellationToken);

        if (maybeMealPlan.HasValue)
        {
            _data.MealPlans.Update(mealPlanToEdit);
        }
        else
        {
            _data.MealPlans.Add(mealPlanToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Meal set {(maybeMealPlan.HasValue ? "updated" : "added")}.", mealPlanToEdit.Id));
    }

    private static void Transfer(SaveMealPlanRequest request, MealPlan mealPlan)
    {
        mealPlan.Name = request.Name;
        mealPlan.PantryIngredients.Clear();
        mealPlan.PantryIngredients.AddRange(request.PantryIngredients
            .Select(x => new MealPlanPantryIngredient
            {
                Name = x.Name,
                Quantity = x.Quantity,
            }));
    }

    private async Task ManageRecipes(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
    {
        // Remove extra recipes.
        mealPlan.Recipes.RemoveAll(x => !request.RecipeIds.Contains(x.Id));

        // Add missing recipes. We'll let the database throw when ID's don't exist.
        var missingRecipeIds = request.RecipeIds
            .Where(x => !mealPlan.Recipes.Select(x => x.Id).Contains(x));

        var specification = new RecipesSpecification(missingRecipeIds);

        var missingRecipes = await _data.Recipes
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        mealPlan.Recipes.AddRange(missingRecipes);
    }
}
