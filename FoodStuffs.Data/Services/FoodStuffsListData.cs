using Core.Data.Test.Repositories;
using Core.Model.Services.Data;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;

namespace FoodStuffs.Data.Services
{
    public class FoodStuffsListData : IFoodStuffsData
    {
        public IWritableRepository<ICategory> Categories { get; } = new ListWritableRepository<ICategory, Category>();

        public IWritableRepository<ICategoryRecipe> CategoryRecipes { get; } =
            new ListWritableRepository<ICategoryRecipe, CategoryRecipe>();

        public IWritableRepository<IRecipe> Recipes { get; } = new ListWritableRepository<IRecipe, Recipe>();

        public IWritableRepository<IUser> Users { get; } = new ListWritableRepository<IUser, User>();

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
        }
    }
}