using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Restores a recipe from the recycle bin. It returns to the meal plans it was on.
/// </summary>
public class RestoreRecipeHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<RestoreRecipeRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(RestoreRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesSpecification(request.Id);

        var maybeRecipe = await data.Recipes
            .OnlySoftDeleted()
            .TagWith(GetTag(byId))
            .ApplyEfSpecification(byId)
            .Include(r => r.GroceryItemRelations)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From);

        if (!maybeRecipe.HasValue)
        {
            return Fail(new RecipeNotFoundFailure());
        }

        var recipe = maybeRecipe.Value;

        // Names must be unique, so another recipe may have taken the name while this one was in the bin.
        var byName = new RecipesSpecification(recipe.Name, recipe.Id);

        var conflictingRecipeExists = await data.Recipes
            .TagWith(GetTag(byName))
            .ApplyEfSpecification(byName)
            .AnyAsync(cancellationToken);

        if (conflictingRecipeExists)
        {
            return Fail(new Failure("Recipe name already exists. Rename or delete the other recipe to restore this one.", "recipeId"));
        }

        recipe.IsDeleted = false;

        await data.SaveChangesAsync(cancellationToken);

        await searchIndex.UpdateAsync(SearchIndex.Recipes, recipe.Id, cancellationToken);

        searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, recipe.GroceryItemRelations.Select(gr => gr.GroceryItemId));

        return Ok(EntityMessage.Create("Recipe restored.", recipe.Id));
    }
}
