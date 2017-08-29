using FoodStuffs.Model.Interfaces.Domain;

namespace FoodStuffs.Data.FoodStuffsDb.Models
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