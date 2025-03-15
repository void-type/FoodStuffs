using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class ListShoppingItemsHandler : CustomEventHandlerAbstract<ListShoppingItemsRequest, IItemSet<ListShoppingItemsResponse>>
{
    private readonly FoodStuffsContext _data;

    public ListShoppingItemsHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<IItemSet<ListShoppingItemsResponse>>> Handle(ListShoppingItemsRequest request, CancellationToken cancellationToken = default)
    {
        var paginationOptions = request.GetPaginationOptions();

        var specification = new ShoppingItemsSpecification(request);

        return await _data.ShoppingItems
            .TagWith(GetTag(specification))
            .ApplyEfSpecification(specification)
            .AsSplitQuery()
            .Select(si => new
            {
                ShoppingItem = si,
                RecipeCount = si.Recipes.Count
            })
            .ToItemSet(paginationOptions, cancellationToken)
            .SelectAsync(c => new ListShoppingItemsResponse(
                Id: c.ShoppingItem.Id,
                Name: c.ShoppingItem.Name,
                InventoryQuantity: c.ShoppingItem.InventoryQuantity,
                PantryLocations: [.. c.ShoppingItem.PantryLocations
                    .Select(pl => pl.Name)
                    .Order(StringComparer.Ordinal)],
                GroceryDepartmentId: c.ShoppingItem.GroceryDepartmentId,
                RecipeCount: c.RecipeCount))
            .MapAsync(Ok);
    }
}
