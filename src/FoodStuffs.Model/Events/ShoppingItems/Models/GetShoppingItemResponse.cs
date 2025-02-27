﻿namespace FoodStuffs.Model.Events.ShoppingItems.Models;

public record GetShoppingItemResponse(
    int Id,
    string Name,
    int InventoryQuantity,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetShoppingItemResponseRecipe> Recipes,
    GetShoppingItemResponseGroceryDepartment? GroceryDepartment,
    List<string> PantryLocations);
