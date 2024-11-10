using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeHandler : CustomEventHandlerAbstract<SaveRecipeRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public SaveRecipeHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveRecipeRequest request, CancellationToken cancellationToken = default)
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

        Transfer(request, recipeToEdit);
        await ManageCategoriesAsync(request, recipeToEdit, cancellationToken);
        ManageIngredients(request, recipeToEdit);
        await ManageShoppingItemsAsync(request, recipeToEdit, cancellationToken);

        if (maybeRecipe.HasValue)
        {
            _data.Recipes.Update(recipeToEdit);
        }
        else
        {
            await _data.Recipes.AddAsync(recipeToEdit, cancellationToken);
        }

        await _data.SaveChangesAsync(cancellationToken);

        _index.AddOrUpdate(recipeToEdit);

        return Ok(EntityMessage.Create($"Recipe {(maybeRecipe.HasValue ? "updated" : "added")}.", recipeToEdit.Id));
    }

    private static void Transfer(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Name = request.Name;
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

        // Remove categories that are no longer used.
        var unusedCategories = await _data.Categories
            .TagWith(GetTag())
            .AsSingleQuery()
            .Include(x => x.Recipes)
            .Where(x => x.Recipes.Count == 0)
            .ToListAsync(cancellationToken);

        // Remove categories that are no longer used.
        _data.Categories.RemoveRange(unusedCategories);
    }

    private static void ManageIngredients(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Ingredients.Clear();

        var ingredientsToAdd = request.Ingredients
            .Select(x => new RecipeIngredient
            {
                Name = x.Name,
                Quantity = x.Quantity,
                Order = x.Order,
                IsCategory = x.IsCategory,
            });

        recipe.Ingredients.AddRange(ingredientsToAdd);
    }

    private async Task ManageShoppingItemsAsync(SaveRecipeRequest request, Recipe recipe, CancellationToken cancellationToken)
    {
        var requestedItemIds = request.ShoppingItems
           .Select(x => x.Id)
           .ToArray();

        // Remove extra items.
        recipe.ShoppingItemRelations.RemoveAll(x => !requestedItemIds.Contains(x.ShoppingItem.Id));

        // Relate missing items.
        var missingItemIds = requestedItemIds
            .Where(x => !recipe.ShoppingItemRelations.Select(x => x.ShoppingItem.Id).Contains(x))
            .ToList();

        var specification = new ShoppingItemsSpecification(missingItemIds);

        var missingItems = await _data.ShoppingItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .ToListAsync(cancellationToken);

        var missingItemRelations = missingItems
            .Select(item =>
            {
                return new RecipeShoppingItemRelation
                {
                    ShoppingItem = item
                };
            });

        recipe.ShoppingItemRelations.AddRange(missingItemRelations);

        // Set properties
        foreach (var item in recipe.ShoppingItemRelations)
        {
            var requestedItem = request.ShoppingItems
                .Find(x => x.Id == item.ShoppingItem.Id);

            if (requestedItem == null)
            {
                continue;
            }

            item.Quantity = requestedItem.Quantity;
            item.Order = requestedItem.Order;
        }
    }
}
