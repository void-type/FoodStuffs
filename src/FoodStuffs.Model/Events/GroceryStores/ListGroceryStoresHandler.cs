using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryStores.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryStores;

public class ListGroceryStoresHandler : CustomEventHandlerAbstract<ListGroceryStoresRequest, IItemSet<ListGroceryStoresResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListGroceryStoresHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListGroceryStoresResponse>>> Handle(ListGroceryStoresRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new GroceryStoresSpecification(request);

        return await _data.GroceryStores
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(pl => new
            {
                GroceryStore = pl,
                GroceryItemCount = pl.GroceryItems.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListGroceryStoresResponse(
                Id: x.GroceryStore.Id,
                Name: x.GroceryStore.Name,
                GroceryItemCount: x.GroceryItemCount))
            .MapAsync(Ok);
    }
}
