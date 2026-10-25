using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.GroceryItems.Models;
using FoodStuffs.Model.Search;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.GroceryItems;

public class DeleteGroceryItemHandler(FoodStuffsContext data, ISearchIndexService searchIndex) : CustomEventHandlerAbstract<DeleteGroceryItemRequest, EntityMessage<int>>
{
    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteGroceryItemRequest request, CancellationToken cancellationToken = default)
    {
        return await data.GroceryItems
            .TagWith(GetTag())
            .Include(si => si.Recipes)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryItemNotFoundFailure())
            .TeeOnSuccessAsync(async si =>
            {
                var recipeIds = si.Recipes.ConvertAll(r => r.Id);

                data.GroceryItems.Remove(si);

                await data.SaveChangesAsync(cancellationToken);

                searchIndex.EnqueueUpdate(SearchIndex.Recipes, recipeIds);

                await searchIndex.RemoveAsync(SearchIndex.GroceryItems, si.Id, cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Grocery item deleted.", r.Id));
    }
}
