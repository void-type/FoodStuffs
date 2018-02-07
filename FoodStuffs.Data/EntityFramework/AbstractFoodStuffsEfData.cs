using Core.Model.Services.Data;
using FoodStuffs.Data.EntityFramework.CustomRepositories;
using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using System;

namespace FoodStuffs.Data.EntityFramework
{
    public abstract class AbstractFoodStuffsEfData : IFoodStuffsData
    {
        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected AbstractFoodStuffsEfData(FoodStuffsContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            CategoryRecipes = new CategoryRecipeRepository(context);
            Recipes = new RecipeRepository(context);
            Users = new UserRepository(context);
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