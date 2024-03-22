using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Search;
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
        ManageIngredients(request, recipeToEdit);
        await ManageCategories(request, recipeToEdit, cancellationToken);

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
        recipe.Name = request.Name;
        recipe.Directions = request.Directions ?? string.Empty;
        recipe.CookTimeMinutes = request.CookTimeMinutes;
        recipe.PrepTimeMinutes = request.PrepTimeMinutes;
        recipe.IsForMealPlanning = request.IsForMealPlanning;
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

    private async Task ManageCategories(SaveRecipeRequest request, Recipe recipe, CancellationToken cancellationToken)
    {
        var requestedNames = request.Categories
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x).Trim())
            .ToArray();

        // Remove extra categories.
        recipe.Categories.RemoveAll(x => !requestedNames.Contains(x.Name));

        var missingNames = requestedNames
            .Where(n => !recipe.Categories.Select(x => x.Name).Contains(n))
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

        // Create missing categories.
        var createdCategories = missingNames
            .Where(x => !recipe.Categories.Select(x => x.Name).Contains(x))
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
}
