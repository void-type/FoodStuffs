using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Data.Test.ListRepositories;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Data.Test
{
    public class FoodStuffsListData : IFoodStuffsData
    {
        public IRepository<ICategory> Categories { get; } = new ListRepository<ICategory, Category>();
        public IRepository<ICategoryRecipe> CategoryRecipes { get; } = new ListRepository<ICategoryRecipe, CategoryRecipe>();
        public IRepository<IRecipe> Recipes { get; } = new ListRepository<IRecipe, Recipe>();
        public IRepository<IUser> Users { get; } = new ListRepository<IUser, User>();

        public FoodStuffsListData()
        {
        }

        public void Dispose()
        {
            return;
        }

        public void SaveChanges()
        {
            return;
        }
    }
}