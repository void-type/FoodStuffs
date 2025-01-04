namespace FoodStuffs.Model.Events.Images.Models;

public record SaveImageRequest(int RecipeId, Stream FileStream);
