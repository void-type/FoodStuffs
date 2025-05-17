namespace FoodStuffs.Model.Events.StorageLocations.Models;

public record GetStorageLocationResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetStorageLocationResponseGroceryItem> GroceryItems);
