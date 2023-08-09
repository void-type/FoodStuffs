using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetHandler : EventHandlerAbstract<SaveMealSetRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;

    public SaveMealSetHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveMealSetRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealSetsByIdWithAllRelatedSpecification(request.Id);

        var maybeMealSet = await _data.MealSets.Get(byId, cancellationToken);

        var mealSetToEdit = maybeMealSet.Unwrap(() => new MealSet());

        Transfer(request, mealSetToEdit);
        await ManageRecipes(request, mealSetToEdit, cancellationToken);

        if (maybeMealSet.HasValue)
        {
            await _data.MealSets.Update(mealSetToEdit, cancellationToken);
        }
        else
        {
            await _data.MealSets.Add(mealSetToEdit, cancellationToken);
        }

        return Ok(EntityMessage.Create($"Meal set {(maybeMealSet.HasValue ? "updated" : "added")}.", mealSetToEdit.Id));
    }

    private void Transfer(SaveMealSetRequest request, MealSet mealSet)
    {
        mealSet.Name = request.Name;
    }

    private async Task ManageRecipes(SaveMealSetRequest request, MealSet mealSet, CancellationToken cancellationToken)
    {
        // Remove extra recipes.
        mealSet.Recipes.RemoveAll(x => !request.RecipeIds.Contains(x.Id));

        // Add missing recipes. We'll let the database throw when ID's don't exist.
        var missingRecipeIds = request.RecipeIds
            .Where(x => !mealSet.Recipes.Select(x => x.Id).Contains(x));

        var missingRecipes = await _data.Recipes.List(new RecipesByIdSpecification(missingRecipeIds), cancellationToken);

        mealSet.Recipes.AddRange(missingRecipes);
    }
}
