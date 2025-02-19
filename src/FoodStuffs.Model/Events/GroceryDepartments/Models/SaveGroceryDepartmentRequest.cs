namespace FoodStuffs.Model.Events.GroceryDepartments.Models;

public record SaveGroceryDepartmentRequest(
    int Id,
    string Name,
    int Order);
