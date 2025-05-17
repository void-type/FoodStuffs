namespace FoodStuffs.Model.Events.GroceryDepartments.Models;

public record GetGroceryDepartmentResponse(
    int Id,
    string Name,
    int Order,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetGroceryDepartmentResponseGroceryItem> GroceryItems);
