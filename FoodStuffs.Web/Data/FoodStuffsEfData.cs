using Core.Model.Data;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using System;

namespace FoodStuffs.Web.Data
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }

        public FoodStuffsEfData(FoodStuffsContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            CategoryRecipes = new CategoryRecipeRepository(context);
            Recipes = new RecipeRepository(context);
            Users = new UserRepository(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        private readonly FoodStuffsContext _context;
    }
}
