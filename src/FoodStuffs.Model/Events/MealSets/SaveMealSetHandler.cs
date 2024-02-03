using FoodStuffs.Model.Data.EntityFramework;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetHandler : EventHandlerAbstract<SaveMealSetRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;

    public SaveMealSetHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveMealSetRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new MealSetsWithAllRelatedSpecification(request.Id);

        var maybeMealSet = await _data.MealSets
            .ApplyEfSpecification(byId)
            .AsSplitQuery()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        var mealSetToEdit = maybeMealSet.Unwrap(() => new MealSet());

        Transfer(request, mealSetToEdit);
        await ManageRecipes(request, mealSetToEdit, cancellationToken);

        if (maybeMealSet.HasValue)
        {
            _data.MealSets.Update(mealSetToEdit);
        }
        else
        {
            _data.MealSets.Add(mealSetToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Meal set {(maybeMealSet.HasValue ? "updated" : "added")}.", mealSetToEdit.Id));
    }

    private static void Transfer(SaveMealSetRequest request, MealSet mealSet)
    {
        mealSet.Name = request.Name;
        mealSet.PantryIngredients = request.PantryIngredients
            .Select(x => new PantryIngredient
            {
                Name = x.Name,
                Quantity = x.Quantity,
            })
            .ToList();
    }

    private async Task ManageRecipes(SaveMealSetRequest request, MealSet mealSet, CancellationToken cancellationToken)
    {
        // Remove extra recipes.
        mealSet.Recipes.RemoveAll(x => !request.RecipeIds.Contains(x.Id));

        // Add missing recipes. We'll let the database throw when ID's don't exist.
        var missingRecipeIds = request.RecipeIds
            .Where(x => !mealSet.Recipes.Select(x => x.Id).Contains(x));

        var missingRecipes = await _data.Recipes
            .ApplyEfSpecification(new RecipesSpecification(missingRecipeIds))
            .ToListAsync(cancellationToken);

        mealSet.Recipes.AddRange(missingRecipes);
    }
}
