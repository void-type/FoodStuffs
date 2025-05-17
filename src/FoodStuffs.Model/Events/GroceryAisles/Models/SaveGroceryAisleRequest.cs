namespace FoodStuffs.Model.Events.GroceryAisles.Models;

public record SaveGroceryAisleRequest(
    int Id,
    string Name,
    int Order);
