using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

/// <summary>
/// Permanently deletes a recipe that is in the recycle bin. Recipes that haven't been deleted first aren't found.
/// </summary>
public class PermanentlyDeleteRecipeHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<PermanentlyDeleteRecipeRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(PermanentlyDeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesSpecification(request.Id);

        return await data.Recipes
            .OnlySoftDeleted()
            .TagWith(GetTag(byId))
            .ApplyEfSpecification(byId)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                // Search index entries were removed when the recipe was deleted.
                data.Recipes.Remove(r);

                await data.SaveChangesAsync(cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Recipe permanently deleted.", r.Id));
    }
}
