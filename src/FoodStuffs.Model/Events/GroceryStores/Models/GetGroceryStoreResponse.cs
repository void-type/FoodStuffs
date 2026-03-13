namespace FoodStuffs.Model.Events.GroceryStores.Models;

public record GetGroceryStoreResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetGroceryStoreResponseGroceryItem> GroceryItems);
