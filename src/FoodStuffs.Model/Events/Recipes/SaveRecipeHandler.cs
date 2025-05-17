using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;
using VoidCore.Model.RuleValidator;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeHandler : CustomEventHandlerAbstract<SaveRecipeRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;
    private readonly SaveRecipeRequestValidator _validator;

    public SaveRecipeHandler(FoodStuffsContext data, IRecipeIndexService index, SaveRecipeRequestValidator validator)
    {
        _data = data;
        _index = index;
        _validator = validator;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveRecipeRequest request, CancellationToken cancellationToken = default)
    {
        return await request
            .Validate(_validator)
            .ThenAsync(async request => await SaveAsync(request, cancellationToken));
    }

    private async Task<IResult<EntityMessage<int>>> SaveAsync(SaveRecipeRequest request, CancellationToken cancellationToken)
    {
        var byId = new RecipesWithAllRelatedSpecification(request.Id);

        var maybeRecipe = await _data.Recipes
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        var recipeToEdit = maybeRecipe.Unwrap(() => new Recipe());

        // Check for conflicting items by name
        var byName = new RecipesSpecification(request.Name.Trim());

        var conflictingRecipe = await _data.Recipes
            .TagWith(GetTag(byName))
            .AsSplitQuery()
            .ApplyEfSpecification(byName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (conflictingRecipe is not null && conflictingRecipe.Id != request.Id)
        {
            return Fail(new Failure("Recipe name already exists.", "name"));
        }

        Transfer(request, recipeToEdit);

        await ManageCategoriesAsync(request, recipeToEdit, cancellationToken);

        await ManageGroceryItemsAsync(request, recipeToEdit, cancellationToken);

        if (maybeRecipe.HasValue)
        {
            _data.Recipes.Update(recipeToEdit);
        }
        else
        {
            _data.Recipes.Add(recipeToEdit);
        }

        await _data.SaveChangesAsync(cancellationToken);

        _index.AddOrUpdate(recipeToEdit);

        return Ok(EntityMessage.Create($"Recipe {(maybeRecipe.HasValue ? "updated" : "added")}.", recipeToEdit.Id));
    }

    private static void Transfer(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Name = request.Name.Trim();
        recipe.Directions = request.Directions ?? string.Empty;
        recipe.Sides = request.Sides ?? string.Empty;
        recipe.PrepTimeMinutes = request.PrepTimeMinutes;
        recipe.CookTimeMinutes = request.CookTimeMinutes;
        recipe.IsForMealPlanning = request.IsForMealPlanning;
    }

    private async Task ManageCategoriesAsync(SaveRecipeRequest request, Recipe recipe, CancellationToken cancellationToken)
    {
        var requestedNames = request.Categories
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x).Trim())
            .ToArray();

        // Remove extra categories.
        recipe.Categories.RemoveAll(x => !requestedNames.Contains(x.Name, StringComparer.OrdinalIgnoreCase));

        var missingNames = requestedNames
            .Where(n => !recipe.Categories.Select(x => x.Name).Contains(n, StringComparer.OrdinalIgnoreCase))
            .ToList();

        // Find missing categories that already exist.
        var existingCategories = await _data.Categories
            .TagWith(GetTag())
            .Where(x => missingNames.Contains(x.Name))
            .OrderBy(x => x.Id)
            // In case there are duplicates, add only the first.
            .GroupBy(x => x.Name)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        recipe.Categories.AddRange(existingCategories);

        // Create missing categories that don't exist.
        var createdCategories = missingNames
            .Where(x => !recipe.Categories.Select(x => x.Name).Contains(x, StringComparer.OrdinalIgnoreCase))
            .Select(x => new Category { Name = x });

        recipe.Categories.AddRange(createdCategories);
    }

    private async Task ManageGroceryItemsAsync(SaveRecipeRequest request, Recipe recipe, CancellationToken cancellationToken)
    {
        var requestedItemIds = request.GroceryItems
           .Select(x => x.Id)
           .ToArray();

        // Remove extra items.
        recipe.GroceryItemRelations.RemoveAll(x => !requestedItemIds.Contains(x.GroceryItem.Id));

        // Relate missing items.
        var missingItemIds = requestedItemIds
            .Where(x => !recipe.GroceryItemRelations.Select(x => x.GroceryItem.Id).Contains(x))
            .ToList();

        var specification = new GroceryItemsSpecification(missingItemIds);

        var missingItems = await _data.GroceryItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        var missingItemRelations = missingItems
            .Select(item =>
            {
                return new RecipeGroceryItemRelation
                {
                    GroceryItem = item
                };
            });

        recipe.GroceryItemRelations.AddRange(missingItemRelations);

        // Set properties
        foreach (var item in recipe.GroceryItemRelations)
        {
            var requestedItem = request.GroceryItems
                .Find(x => x.Id == item.GroceryItem.Id);

            if (requestedItem == null)
            {
                continue;
            }

            item.Quantity = requestedItem.Quantity;
            item.Order = requestedItem.Order;
        }
    }
}
