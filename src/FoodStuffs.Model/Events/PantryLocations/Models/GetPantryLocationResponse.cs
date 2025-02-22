namespace FoodStuffs.Model.Events.PantryLocations.Models;

public record GetPantryLocationResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetPantryLocationResponseShoppingItem> ShoppingItems);
