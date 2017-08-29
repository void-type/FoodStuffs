namespace FoodStuffs.Model.Interfaces.Domain
{
    public interface ICategoryRecipe
    {
        ICategory Category { get; set; }
        int CategoryId { get; set; }
        IRecipe Recipe { get; set; }
        int RecipeId { get; set; }
    }
}