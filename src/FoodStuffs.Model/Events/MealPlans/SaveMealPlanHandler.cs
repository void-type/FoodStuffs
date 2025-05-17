using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.MealPlans.Models;
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
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        var mealPlanToEdit = maybeMealPlan.Unwrap(() => new MealPlan());

        Transfer(request, mealPlanToEdit);
        await ManageRecipesAsync(request, mealPlanToEdit, cancellationToken);
        await ManageExcludedGroceryItemsAsync(request, mealPlanToEdit, cancellationToken);

        if (maybeMealPlan.HasValue)
        {
            _data.MealPlans.Update(mealPlanToEdit);
        }
        else
        {
            _data.MealPlans.Add(mealPlanToEdit);
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
                    Order = 0,
                }));

        mealPlan.RecipeRelations
            .ForEach(relation =>
            {
                var requestedRecipe = request.Recipes
                    .Find(x => x.Id == relation.Recipe.Id);

                if (requestedRecipe == null)
                {
                    return;
                }

                relation.Order = requestedRecipe.Order;
                relation.IsComplete = requestedRecipe.IsComplete;
            });
    }

    private async Task ManageExcludedGroceryItemsAsync(SaveMealPlanRequest request, MealPlan mealPlan, CancellationToken cancellationToken)
    {
        var requestedItemIds = request.ExcludedGroceryItems
            .Select(x => x.Id)
            .ToArray();

        // Remove extra items.
        mealPlan.ExcludedGroceryItemRelations.RemoveAll(x => !requestedItemIds.Contains(x.GroceryItem.Id));

        // Add missing items. We'll let the database throw when ID's don't exist.
        var missingItemIds = requestedItemIds
            .Where(x => !mealPlan.ExcludedGroceryItemRelations.Select(x => x.GroceryItem.Id).Contains(x))
            .ToList();

        var specification = new GroceryItemsSpecification(missingItemIds);

        var missingItems = await _data.GroceryItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        var missingItemRelations = missingItems
            .Select(item =>
            {
                return new MealPlanExcludedGroceryItemRelation
                {
                    GroceryItem = item
                };
            });

        mealPlan.ExcludedGroceryItemRelations.AddRange(missingItemRelations);

        // Set properties
        foreach (var item in mealPlan.ExcludedGroceryItemRelations)
        {
            var requestedItem = request.ExcludedGroceryItems
                .Find(x => x.Id == item.GroceryItem.Id);

            if (requestedItem == null)
            {
                continue;
            }

            item.Quantity = requestedItem.Quantity;
        }
    }
}
