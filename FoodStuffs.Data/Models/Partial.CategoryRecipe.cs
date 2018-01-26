using FoodStuffs.Model.Interfaces.Services.Data.Models;

namespace FoodStuffs.Data.Models
{
    public partial class CategoryRecipe : ICategoryRecipe
    {
        ICategory ICategoryRecipe.Category
        {
            get => Category;
            set => Category = (Category)value;
        }

        IRecipe ICategoryRecipe.Recipe
        {
            get => Recipe;
            set => Recipe = (Recipe)value;
        }
    }
}