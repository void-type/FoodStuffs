using Core.Data.List;
using Core.Model.Services.Data;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Data.List
{
    public class FoodStuffsListData : IFoodStuffsData
    {
        public IWritableRepository<Category> Categories { get; } = new ListWritableRepository<Category>();
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; } = new ListWritableRepository<CategoryRecipe>();
        public IWritableRepository<Recipe> Recipes { get; } = new ListWritableRepository<Recipe>();
        public IWritableRepository<User> Users { get; } = new ListWritableRepository<User>();

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
        }
    }
}