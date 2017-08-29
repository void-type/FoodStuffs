using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Data.FoodStuffsDb
{
    public abstract class AbstractFoodStuffsData : EfDatabaseService, IFoodStuffsData
    {
        public IRepository<ICategory> Categories { get; }

        public IRepository<ICategoryRecipe> CategoryRecipes { get; }

        public IRepository<IRecipe> Recipes { get; }

        public IRepository<IUser> Users { get; }

        protected AbstractFoodStuffsData(FoodStuffsContext context) : base(context)
        {
            Users = new EfRepository<IUser, User>(context);
            Categories = new EfRepository<ICategory, Category>(context);
            Recipes = new EfRepository<IRecipe, Recipe>(context);
            CategoryRecipes = new EfRepository<ICategoryRecipe, CategoryRecipe>(context);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}