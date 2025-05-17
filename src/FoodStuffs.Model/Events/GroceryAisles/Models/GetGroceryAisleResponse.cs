namespace FoodStuffs.Model.Events.GroceryAisles.Models;

public record GetGroceryAisleResponse(
    int Id,
    string Name,
    int Order,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetGroceryAisleResponseGroceryItem> GroceryItems);
