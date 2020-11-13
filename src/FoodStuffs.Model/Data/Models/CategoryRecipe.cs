#nullable disable

namespace FoodStuffs.Model.Data.Models
{
    public partial class CategoryRecipe
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
