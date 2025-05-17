using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.StorageLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.StorageLocations;

public class GetStorageLocationHandler : CustomEventHandlerAbstract<GetStorageLocationRequest, GetStorageLocationResponse>
{
    private readonly FoodStuffsContext _data;

    public GetStorageLocationHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetStorageLocationResponse>> Handle(GetStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new StorageLocationsWithAllRelatedSpecification(request.Id);

        return await _data.StorageLocations
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new StorageLocationNotFoundFailure())
            .SelectAsync(m => new GetStorageLocationResponse(
                Id: m.Id,
                Name: m.Name,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                GroceryItems: [.. m.GroceryItems
                    .Select(r => new GetStorageLocationResponseGroceryItem(
                        Id: r.Id,
                        Name: r.Name))
                    .OrderBy(r => r.Name)]));
    }
}
