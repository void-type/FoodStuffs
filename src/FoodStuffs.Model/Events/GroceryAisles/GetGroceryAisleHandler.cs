using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryAisles.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryAisles;

public class GetGroceryAisleHandler(FoodStuffsContext data) : CustomEventHandlerAbstract<GetGroceryAisleRequest, GetGroceryAisleResponse>
{
    public override async Task<IResult<GetGroceryAisleResponse>> Handle(GetGroceryAisleRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new GroceryAislesWithAllRelatedSpecification(request.Id);

        return await data.GroceryAisles
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryAisleNotFoundFailure())
            .SelectAsync(m => new GetGroceryAisleResponse(
                Id: m.Id,
                Name: m.Name,
                Order: m.Order,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                GroceryItems: [.. m.GroceryItems
                    .OrderBy(r => r.Name)
                    .Select(r => new GetGroceryAisleResponseGroceryItem(
                        Id: r.Id,
                        Name: r.Name))]));
    }
}
