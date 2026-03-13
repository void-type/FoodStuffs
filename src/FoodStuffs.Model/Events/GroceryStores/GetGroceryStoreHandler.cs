using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryStores.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryStores;

public class GetGroceryStoreHandler : CustomEventHandlerAbstract<GetGroceryStoreRequest, GetGroceryStoreResponse>
{
    private readonly FoodStuffsContext _data;

    public GetGroceryStoreHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetGroceryStoreResponse>> Handle(GetGroceryStoreRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new GroceryStoresWithAllRelatedSpecification(request.Id);

        return await _data.GroceryStores
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryStoreNotFoundFailure())
            .SelectAsync(m => new GetGroceryStoreResponse(
                Id: m.Id,
                Name: m.Name,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                GroceryItems: [.. m.GroceryItems
                    .Select(r => new GetGroceryStoreResponseGroceryItem(
                        Id: r.Id,
                        Name: r.Name))
                    .OrderBy(r => r.Name)]));
    }
}
