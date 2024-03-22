using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.MealSets;

public class SaveMealSetHandler : CustomEventHandlerAbstract<SaveMealSetRequest, EntityMessage<int>>
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
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
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
        mealSet.PantryIngredients.Clear();
        mealSet.PantryIngredients.AddRange(request.PantryIngredients
            .Select(x => new MealSetPantryIngredient
            {
                Name = x.Name,
                Quantity = x.Quantity,
            }));
    }

    private async Task ManageRecipes(SaveMealSetRequest request, MealSet mealSet, CancellationToken cancellationToken)
    {
        // Remove extra recipes.
        mealSet.Recipes.RemoveAll(x => !request.RecipeIds.Contains(x.Id));

        // Add missing recipes. We'll let the database throw when ID's don't exist.
        var missingRecipeIds = request.RecipeIds
            .Where(x => !mealSet.Recipes.Select(x => x.Id).Contains(x));

        var specification = new RecipesSpecification(missingRecipeIds);

        var missingRecipes = await _data.Recipes
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        mealSet.Recipes.AddRange(missingRecipes);
    }
}
