using Core.Model.Services.Data;
using FoodStuffs.Data.CustomRepositories;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data;
using System;

namespace FoodStuffs.Data.Services
{
    public abstract class AbstractFoodStuffsEfData : IFoodStuffsData
    {
        private readonly FoodStuffsContext _context;

        public IWritableRepository<ICategory> Categories { get; }

        public IWritableRepository<ICategoryRecipe> CategoryRecipes { get; }

        public IWritableRepository<IRecipe> Recipes { get; }

        public IWritableRepository<IUser> Users { get; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected AbstractFoodStuffsEfData(FoodStuffsContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Categories = new CategoryRepository(context);
            Recipes = new RecipeRepository(context);
            CategoryRecipes = new CategoryRecipeRepository(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }
    }
}