using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.StorageLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.StorageLocations;

public class ListStorageLocationsHandler : CustomEventHandlerAbstract<ListStorageLocationsRequest, IItemSet<ListStorageLocationsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListStorageLocationsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListStorageLocationsResponse>>> Handle(ListStorageLocationsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new StorageLocationsSpecification(request);

        return await _data.StorageLocations
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(pl => new
            {
                StorageLocation = pl,
                GroceryItemCount = pl.GroceryItems.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListStorageLocationsResponse(
                Id: x.StorageLocation.Id,
                Name: x.StorageLocation.Name,
                GroceryItemCount: x.GroceryItemCount))
            .MapAsync(Ok);
    }
}
