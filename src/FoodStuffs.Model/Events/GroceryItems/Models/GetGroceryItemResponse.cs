namespace FoodStuffs.Model.Events.GroceryItems.Models;

public record GetGroceryItemResponse(
    int Id,
    string Name,
    int InventoryQuantity,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetGroceryItemResponseRecipe> Recipes,
    GetGroceryItemResponseGroceryDepartment? GroceryDepartment,
    List<string> PantryLocations);
