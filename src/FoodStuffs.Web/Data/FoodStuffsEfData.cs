using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data.EntityFramework;
using VoidCore.AspNet.Data;
using VoidCore.Model.Data;

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
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Recipes = new EfWritableRepository<Recipe>(context);
            Users = new EfWritableRepository<User>(context);
        }
    }
}
