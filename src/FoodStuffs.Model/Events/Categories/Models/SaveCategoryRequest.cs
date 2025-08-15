namespace FoodStuffs.Model.Events.Categories.Models;

public record SaveCategoryRequest(
    int Id,
    string Name,
    bool ShowInMealPlan,
    string Color);
