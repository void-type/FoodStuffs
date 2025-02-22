using FoodStuffs.Model.Data;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using FoodStuffs.Model.Search.Recipes;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class DeleteShoppingItemHandler : CustomEventHandlerAbstract<DeleteShoppingItemRequest, EntityMessage<int>>
{
    private readonly FoodStuffsContext _data;
    private readonly IRecipeIndexService _index;

    public DeleteShoppingItemHandler(FoodStuffsContext data, IRecipeIndexService index)
    {
        _data = data;
        _index = index;
    }

    public override async Task<IResult<EntityMessage<int>>> Handle(DeleteShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        return await _data.ShoppingItems
            .TagWith(GetTag())
            .Include(si => si.Recipes)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ShoppingItemNotFoundFailure())
            .TeeOnSuccessAsync(async si =>
            {
                var recipeIds = si.Recipes.ConvertAll(r => r.Id);

                _data.ShoppingItems.Remove(si);

                await _data.SaveChangesAsync(cancellationToken);

                await _index.AddOrUpdateAsync(recipeIds, cancellationToken);
            })
            .SelectAsync(r => EntityMessage.Create("Shopping item deleted.", r.Id));
    }
}
