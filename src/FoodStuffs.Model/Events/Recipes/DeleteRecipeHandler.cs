using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.Recipes.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.Recipes;

public class DeleteRecipeHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteRecipeRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new RecipesSpecification(request.Id);

        return await data.Recipes
            .TagWith(GetTag(byId))
            .ApplyEfSpecification(byId)
            .Include(r => r.GroceryItemRelations)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new RecipeNotFoundFailure())
            .TeeOnSuccessAsync(async r =>
            {
                var groceryItemIds = r.GroceryItemRelations.Select(gr => gr.GroceryItemId).ToList();

                data.Recipes.Remove(r);

                await data.SaveChangesAsync(cancellationToken);

                await searchIndex.RemoveAsync(SearchIndex.Recipes, r.Id, cancellationToken);

                searchIndex.EnqueueUpdate(SearchIndex.GroceryItems, groceryItemIds);
            })
            .SelectAsync(r => EntityMessage.Create("Recipe deleted.", r.Id));
    }
}
