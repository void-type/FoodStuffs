﻿using FoodStuffs.Model.Data;
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
        await ManagePantryShoppingItems(request, mealPlanToEdit, cancellationToken);

        if (maybeMealPlan.HasValue)
        {
            _data.MealPlans.Update(mealPlanToEdit);
        }
        else
        {
            await _data.MealPlans.AddAsync(mealPlanToEdit, cancellationToken);
        }

        await _data.SaveChangesAsync(cancellationToken);

        return Ok(EntityMessage.Create($"Meal plan {(maybeMealPlan.HasValue ? "updated" : "added")}.", mealPlanToEdit.Id));
    }

    private static void Transfer(SaveMealPlanRequest request, MealPlan mealPlan)
    {
        mealPlan.Name = request.Name;
    }

    private async Task ManagePantryShoppingItems(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
    {
        var requestedItemIds = request.PantryShoppingItems
            .Select(x => x.Id)
            .ToArray();

        // Remove extra items.
        mealPlan.PantryShoppingItemRelations.RemoveAll(x => !requestedItemIds.Contains(x.ShoppingItem.Id));

        // Add missing items. We'll let the database throw when ID's don't exist.
        var missingItemIds = requestedItemIds
            .Where(x => !mealPlan.PantryShoppingItemRelations.Select(x => x.ShoppingItem.Id).Contains(x));

        var specification = new ShoppingItemsSpecification(missingItemIds);

        var missingItems = await _data.ShoppingItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        mealPlan.PantryShoppingItemRelations
            .AddRange(missingItems
                .Select(item => new MealPlanPantryShoppingItemRelation
                {
                    ShoppingItem = item,
                    Quantity = request.PantryShoppingItems
                        .Find(req => req.Id == item.Id)?
                        .Quantity ?? int.MinValue,
                }));
    }

    private async Task ManageRecipes(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
    {
        var requestedRecipeIds = request.Recipes
            .Select(x => x.Id)
            .ToArray();

        // Remove extra recipes.
        mealPlan.RecipeRelations.RemoveAll(x => !requestedRecipeIds.Contains(x.Recipe.Id));

        // Add missing recipes. We'll let the database throw when ID's don't exist.
        var missingRecipeIds = requestedRecipeIds
            .Where(x => !mealPlan.RecipeRelations.Select(x => x.Recipe.Id).Contains(x));

        var specification = new RecipesSpecification(missingRecipeIds);

        var missingRecipes = await _data.Recipes
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        mealPlan.RecipeRelations
            .AddRange(missingRecipes
                .Select(recipe => new MealPlanRecipeRelation
                {
                    Recipe = recipe,
                    Order = request.Recipes
                        .Find(req => req.Id == recipe.Id)?
                        .Order ?? int.MaxValue,
                }));
    }
}