namespace FoodStuffs.Model.Events.MealPlans.Models;

public record ListMealPlansResponse(
    int Id,
    string Name,
    DateTimeOffset CreatedOn,
    DateTimeOffset ModifiedOn);
