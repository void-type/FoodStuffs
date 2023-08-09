using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Globalization;
using VoidCore.Model.Events;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class SaveRecipeHandler : EventHandlerAbstract<SaveRecipeRequest, EntityMessage<int>>
{
    private readonly IFoodStuffsData _data;

    public SaveRecipeHandler(IFoodStuffsData data)
    {
        _data = data;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(SaveRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesByIdWithCategoriesAndIngredientsSpecification(request.Id);

        var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken);

        var recipeToEdit = maybeRecipe.Unwrap(() => new Recipe());

        Transfer(request, recipeToEdit);
        ManageIngredients(request, recipeToEdit);
        await ManageCategories(request, recipeToEdit, cancellationToken);

        if (maybeRecipe.HasValue)
        {
            await _data.Recipes.Update(recipeToEdit, cancellationToken);
        }
        else
        {
            await _data.Recipes.Add(recipeToEdit, cancellationToken);
        }

        return Ok(EntityMessage.Create($"Recipe {(maybeRecipe.HasValue ? "updated" : "added")}.", recipeToEdit.Id));
    }

    private void Transfer(SaveRecipeRequest request, Recipe recipe)
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
            .Select(x => new Ingredient
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
        var missingCategoriesSpec = new CategoriesSpecification(x => missingNames.Contains(x.Name));

        var existingCategories = (await _data.Categories.List(missingCategoriesSpec, cancellationToken))
            // In case there are duplicates, add only the first.
            .OrderBy(x => x.Id)
            .GroupBy(x => x.Name)
            .Select(g => g.First());

        recipe.Categories.AddRange(existingCategories);

        // Create missing categories.
        var createdCategories = missingNames
            .Where(x => !recipe.Categories.Select(x => x.Name).Contains(x))
            .Select(x => new Category { Name = x });

        recipe.Categories.AddRange(createdCategories);

        // Remove categories that are no longer used.
        var unusedCategoriesSpec = new CategoriesUnusedSpecification();
        var categories = await _data.Categories.List(unusedCategoriesSpec, cancellationToken);
        await _data.Categories.RemoveRange(categories, cancellationToken);
    }
}
