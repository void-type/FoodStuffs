namespace FoodStuffs.Model.Events.Recipes;

public record ListRecipesResponse(
    int Id,
    string Name,
    IEnumerable<string> Categories,
    int? ImageId);
