namespace FoodStuffs.Model.Events.MealPlans;

public record ListMealPlansResponse(
    int Id,
    string Name,
    DateTimeOffset CreatedOn,
    DateTimeOffset ModifiedOn);
