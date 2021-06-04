namespace FoodStuffs.Model.Events.Images
{
    public record SaveImageRequest(int RecipeId, byte[] FileContent);
}
