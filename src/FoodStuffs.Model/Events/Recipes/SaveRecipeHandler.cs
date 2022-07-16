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

        var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken)
            .ConfigureAwait(false);

        var recipeToEdit = maybeRecipe.Unwrap(() => new Recipe());

        Transfer(request, recipeToEdit);

        if (maybeRecipe.HasValue)
        {
            await _data.Recipes.Update(recipeToEdit, cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await _data.Recipes.Add(recipeToEdit, cancellationToken)
                .ConfigureAwait(false);
        }

        return Ok(EntityMessage.Create($"Recipe {(maybeRecipe.HasValue ? "updated" : "added")}.", recipeToEdit.Id));
    }

    private static void Transfer(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Name = request.Name;
        recipe.Directions = request.Directions;
        recipe.CookTimeMinutes = request.CookTimeMinutes;
        recipe.PrepTimeMinutes = request.PrepTimeMinutes;
        recipe.IsForMealPlanning = request.IsForMealPlanning;

        ManageIngredients(request, recipe);
        ManageCategories(request, recipe);
    }

    private static void ManageIngredients(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Ingredients.Clear();

        foreach (var ingredient in request.Ingredients)
        {
            recipe.Ingredients.Add(new Ingredient
            {
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                Order = ingredient.Order,
                IsCategory = ingredient.IsCategory,
            });
        }
    }

    private static void ManageCategories(SaveRecipeRequest request, Recipe recipe)
    {
        var requested = request.Categories
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.ToLower().Trim())
            .ToArray();

        var existingCategories = recipe.Categories
            .Where(c => requested.Contains(c.Name.ToLower().Trim()));

        var existingCategoryNames = existingCategories
            .Select(c => c.Name.ToLower().Trim());

        // Create categories that don't exist
        var newCategories = requested
            .Where(n => !existingCategoryNames.Contains(n))
            .Select(n => new Category { Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n) });

        var finalCategories = existingCategories.Concat(newCategories);

        recipe.Categories.Clear();

        foreach (var category in finalCategories)
        {
            recipe.Categories.Add(category);
        }
    }
}
