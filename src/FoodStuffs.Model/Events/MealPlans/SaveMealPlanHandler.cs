using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.MealPlans;

public class SaveMealPlanHandler : CustomEventHandlerAbstract<SaveMealPlanRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly SaveMealPlanRequestValidator _validator;

    public SaveMealPlanHandler(FoodStuffsContext data, SaveMealPlanRequestValidator validator)
    {
        _data = data;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveMealPlanRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveMealPlanRequest request, CancellationToken cancellationToken)
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
        await ManageRecipesAsync(request, mealPlanToEdit, cancellationToken);
        await ManagePantryShoppingItemsAsync(request, mealPlanToEdit, cancellationToken);

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

    private async Task ManageRecipesAsync(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
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

    private async Task ManagePantryShoppingItemsAsync(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
    {
        var requestedItemIds = request.PantryShoppingItems
            .Select(x => x.Id)
            .ToArray();

        // Remove extra items.
        mealPlan.PantryShoppingItemRelations.RemoveAll(x => !requestedItemIds.Contains(x.ShoppingItem.Id));

        // Add missing items. We'll let the database throw when ID's don't exist.
        var missingItemIds = requestedItemIds
            .Where(x => !mealPlan.PantryShoppingItemRelations.Select(x => x.ShoppingItem.Id).Contains(x))
            .ToList();

        var specification = new ShoppingItemsSpecification(missingItemIds);

        var missingItems = await _data.ShoppingItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        var missingItemRelations = missingItems
            .Select(item =>
            {
                return new MealPlanPantryShoppingItemRelation
                {
                    ShoppingItem = item
                };
            });

        mealPlan.PantryShoppingItemRelations.AddRange(missingItemRelations);

        // Set properties
        foreach (var item in mealPlan.PantryShoppingItemRelations)
        {
            var requestedItem = request.PantryShoppingItems
                .Find(x => x.Id == item.ShoppingItem.Id);

            if (requestedItem == null)
            {
                continue;
            }

            item.Quantity = requestedItem.Quantity;
        }
    }
}
