using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class ListGroceryAislesHandler : CustomEventHandlerAbstract<ListGroceryAislesRequest, IItemSet<ListGroceryAislesResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListGroceryAislesHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListGroceryAislesResponse>>> Handle(ListGroceryAislesRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new GroceryAislesSpecification(request);

        return await _data.GroceryAisles
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(gd => new
            {
                GroceryAisle = gd,
                GroceryItemCount = gd.GroceryItems.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListGroceryAislesResponse(
                Id: x.GroceryAisle.Id,
                Name: x.GroceryAisle.Name,
                Order: x.GroceryAisle.Order,
                GroceryItemCount: x.GroceryItemCount))
            .MapAsync(Ok);
    }
}
