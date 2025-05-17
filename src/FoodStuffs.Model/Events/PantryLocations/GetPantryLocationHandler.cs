using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.PantryLocations.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.PantryLocations;

public class GetPantryLocationHandler : CustomEventHandlerAbstract<GetPantryLocationRequest, GetPantryLocationResponse>
{
    private readonly FoodStuffsContext _data;

    public GetPantryLocationHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetPantryLocationResponse>> Handle(GetPantryLocationRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new PantryLocationsWithAllRelatedSpecification(request.Id);

        return await _data.PantryLocations
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new PantryLocationNotFoundFailure())
            .SelectAsync(m => new GetPantryLocationResponse(
                Id: m.Id,
                Name: m.Name,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                GroceryItems: [.. m.GroceryItems
                    .Select(r => new GetPantryLocationResponseGroceryItem(
                        Id: r.Id,
                        Name: r.Name))
                    .OrderBy(r => r.Name)]));
    }
}
