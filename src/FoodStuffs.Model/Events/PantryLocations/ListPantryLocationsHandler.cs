using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.PantryLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.PantryLocations;

public class ListPantryLocationsHandler : CustomEventHandlerAbstract<ListPantryLocationsRequest, IItemSet<ListPantryLocationsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListPantryLocationsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListPantryLocationsResponse>>> Handle(ListPantryLocationsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new PantryLocationsSpecification(request);

        return await _data.PantryLocations
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .Select(pl => new
            {
                PantryLocation = pl,
                GroceryItemCount = pl.GroceryItems.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(x => new ListPantryLocationsResponse(
                Id: x.PantryLocation.Id,
                Name: x.PantryLocation.Name,
                GroceryItemCount: x.GroceryItemCount))
            .MapAsync(Ok);
    }
}
