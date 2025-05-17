namespace FoodStuffs.Model.Events.StorageLocations.Models;

public record ListStorageLocationsResponse(
    int Id,
    string Name,
    int GroceryItemCount);
