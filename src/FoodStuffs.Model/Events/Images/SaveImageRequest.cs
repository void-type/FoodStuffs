namespace FoodStuffs.Model.Events.Images;

public record SaveImageRequest(int RecipeId, Stream FileStream);
