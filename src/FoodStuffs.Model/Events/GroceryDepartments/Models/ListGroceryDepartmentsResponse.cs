namespace FoodStuffs.Model.Events.GroceryDepartments.Models;

public record ListGroceryDepartmentsResponse(
    int Id,
    string Name,
    int Order,
    int GroceryItemCount);
