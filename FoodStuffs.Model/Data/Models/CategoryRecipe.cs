namespace FoodStuffs.Model.Data.Models
{
    public partial class CategoryRecipe
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}