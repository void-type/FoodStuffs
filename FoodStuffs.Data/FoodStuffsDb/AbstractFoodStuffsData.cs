using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using FoodStuffs.Data.FoodStuffsDb.Repositories;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Data.FoodStuffsDb
{
    public abstract class AbstractFoodStuffsData : DatabaseService, IFoodStuffsData
    {
        public IRepository<ICategory> Categories { get; }

        public IRepository<ICategoryRecipe> CategoryRecipes { get; }

        public IRepository<IRecipe> Recipes { get; }

        public IRepository<IUser> Users { get; }

        protected AbstractFoodStuffsData(FoodStuffsContext context) : base(context)
        {
            Users = new UserRepository(context);
            Categories = new CategoryRepository(context);
            Recipes = new RecipeRepository(context);
            CategoryRecipes = new CategoryRecipeRepository(context);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}