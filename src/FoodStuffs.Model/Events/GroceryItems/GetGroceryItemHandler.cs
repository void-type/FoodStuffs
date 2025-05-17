using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.GroceryItems;

public class GetGroceryItemHandler : CustomEventHandlerAbstract<GetGroceryItemRequest, GetGroceryItemResponse>
{
    private readonly FoodStuffsContext _data;

    public GetGroceryItemHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetGroceryItemResponse>> Handle(GetGroceryItemRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new GroceryItemsWithAllRelatedSpecification(request.Id);

        return await _data.GroceryItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new GroceryItemNotFoundFailure())
            .SelectAsync(m => new GetGroceryItemResponse(
                Id: m.Id,
                Name: m.Name,
                InventoryQuantity: m.InventoryQuantity,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                Recipes: [.. m.Recipes
                    .Select(r => new GetGroceryItemResponseRecipe(
                        Id: r.Id,
                        Name: r.Name,
                        Slug: r.Slug,
                        Image: r.DefaultImage?.FileName))
                    .OrderBy(r => r.Name)],
                GroceryDepartment: m.GroceryDepartment is not null
                    ? new GetGroceryItemResponseGroceryDepartment(
                        Id: m.GroceryDepartment.Id,
                        Name: m.GroceryDepartment.Name,
                        Order: m.GroceryDepartment.Order)
                    : null,
                PantryLocations: [.. m.PantryLocations
                    .Select(c => c.Name)
                    .Order(StringComparer.Ordinal)]));
    }
}
