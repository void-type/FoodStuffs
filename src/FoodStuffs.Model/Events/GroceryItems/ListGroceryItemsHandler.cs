using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.GroceryItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.GroceryItems;

public class ListGroceryItemsHandler : CustomEventHandlerAbstract<ListGroceryItemsRequest, IItemSet<ListGroceryItemsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListGroceryItemsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListGroceryItemsResponse>>> Handle(ListGroceryItemsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new GroceryItemsSpecification(request);

        return await _data.GroceryItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .AsSplitQuery()
            .Select(si => new
            {
                GroceryItem = si,
                RecipeCount = si.Recipes.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(c => new ListGroceryItemsResponse(
                Id: c.GroceryItem.Id,
                Name: c.GroceryItem.Name,
                InventoryQuantity: c.GroceryItem.InventoryQuantity,
                PantryLocations: [.. c.GroceryItem.PantryLocations
                    .Select(pl => pl.Name)
                    .Order(StringComparer.Ordinal)],
                GroceryDepartmentId: c.GroceryItem.GroceryDepartmentId,
                RecipeCount: c.RecipeCount))
            .MapAsync(Ok);
    }
}
