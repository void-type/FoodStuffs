﻿using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Queries;
using FoodStuffs.Model.Events.ShoppingItems.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events.ShoppingItems;

public class GetShoppingItemHandler : CustomEventHandlerAbstract<GetShoppingItemRequest, GetShoppingItemResponse>
{
    private readonly FoodStuffsContext _data;

    public GetShoppingItemHandler(FoodStuffsContext data)
    {
        _data = data;
    }

    public override async Task<IResult<GetShoppingItemResponse>> Handle(GetShoppingItemRequest request, CancellationToken cancellationToken = default)
    {
        var byId = new ShoppingItemsWithAllRelatedSpecification(request.Id);

        return await _data.ShoppingItems
            .TagWith(GetTag(byId))
            .AsSplitQuery()
            .ApplyEfSpecification(byId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .MapAsync(Maybe.From)
            .ToResultAsync(new ShoppingItemNotFoundFailure())
            .SelectAsync(m => new GetShoppingItemResponse(
                Id: m.Id,
                Name: m.Name,
                InventoryQuantity: m.InventoryQuantity,
                CreatedBy: m.CreatedBy,
                CreatedOn: m.CreatedOn,
                ModifiedBy: m.ModifiedBy,
                ModifiedOn: m.ModifiedOn,
                Recipes: [.. m.Recipes
                    .Select(r => new GetShoppingItemResponseRecipe(
                        Id: r.Id,
                        Name: r.Name,
                        Slug: r.Slug,
                        Image: r.DefaultImage?.FileName))
                    .OrderBy(r => r.Name)],
                GroceryDepartment: m.GroceryDepartment is not null
                    ? new GetShoppingItemResponseGroceryDepartment(
                        Id: m.GroceryDepartment.Id,
                        Name: m.GroceryDepartment.Name,
                        Order: m.GroceryDepartment.Order)
                    : null,
                PantryLocations: [.. m.PantryLocations
                    .Select(c => c.Name)
                    .Order(StringComparer.Ordinal)]));
    }
}
