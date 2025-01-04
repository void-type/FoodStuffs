namespace FoodStuffs.Model.Events.Categories.Models;

public record GetCategoryResponse(
    int Id,
    string Name,
    string CreatedBy,
    DateTimeOffset CreatedOn,
    string ModifiedBy,
    DateTimeOffset ModifiedOn,
    List<GetCategoryResponseRecipe> Recipes);
