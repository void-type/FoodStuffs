using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Data.Queries;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        var byId = new RecipesByIdWithCategoriesSpecification(request.Id);

        var maybeRecipe = await _data.Recipes.Get(byId, cancellationToken)
            .ConfigureAwait(false);

        if (maybeRecipe.HasValue)
        {
            return await maybeRecipe.Value
                .Tee(r => Transfer(request, r))
                .TeeAsync(r => _data.Recipes.Update(r, cancellationToken))
                .TeeAsync(r => ManageCategories(request, r, cancellationToken))
                .MapAsync(r => Ok(EntityMessage.Create("Recipe updated.", r.Id)))
                .ConfigureAwait(false);
        }

        return await new Recipe()
            .Tee(r => Transfer(request, r))
            .TeeAsync(r => _data.Recipes.Add(r, cancellationToken))
            .TeeAsync(r => ManageCategories(request, r, cancellationToken))
            .MapAsync(r => Ok(EntityMessage.Create("Recipe added.", r.Id)))
            .ConfigureAwait(false);
    }

    private static void Transfer(SaveRecipeRequest request, Recipe recipe)
    {
        recipe.Name = request.Name;
        recipe.Ingredients_Old = request.Ingredients;
        recipe.Directions = request.Directions;
        recipe.CookTimeMinutes = request.CookTimeMinutes;
        recipe.PrepTimeMinutes = request.PrepTimeMinutes;
    }

    private async Task ManageCategories(SaveRecipeRequest request, Recipe recipe, CancellationToken cancellationToken)
    {
        var requestedCategoryNames = request.Categories
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.ToLower().Trim())
            .ToArray();

        var categoriesThatMatchRequestedSpec = new CategoriesSpecification(
            c => requestedCategoryNames.Contains(c.Name.ToLower().Trim()));

        // Get categories that exist
        var existingCategories = await _data.Categories
            .List(categoriesThatMatchRequestedSpec, cancellationToken)
            .ConfigureAwait(false);

        var existingCategoryNames = existingCategories
            .Select(c => c.Name.ToLower().Trim());

        // Create categories that don't exist
        var newCategories = await requestedCategoryNames
            .Where(n => !existingCategoryNames.Contains(n))
            .Select(n => new Category { Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(n) })
            .TeeAsync(r => _data.Categories.AddRange(r, cancellationToken))
            .ConfigureAwait(false);

        var finalCategories = existingCategories.Concat(newCategories);

        var finalCategoryIds = finalCategories.Select(c => c.Id);
        var currentCategoryIds = recipe.Categories.Select(c => c.Id);

        // Remove relations that are no longer needed
        var categoriesToRemove = recipe.Categories
            .Where(c => !finalCategoryIds.Contains(c.Id));

        foreach (var category in categoriesToRemove)
        {
            recipe.Categories.Remove(category);
        }

        // Add relations that are missing
        var categoriesToAdd = finalCategories
            .Where(c => !currentCategoryIds.Contains(c.Id));

        foreach (var category in categoriesToAdd)
        {
            recipe.Categories.Add(category);
        }

        await _data.Recipes.Update(recipe, cancellationToken);
    }
}
