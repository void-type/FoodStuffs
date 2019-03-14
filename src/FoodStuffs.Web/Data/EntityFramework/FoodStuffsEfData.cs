using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using VoidCore.AspNet.Data;
using VoidCore.Model.Data;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context)
        {
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Recipes = new EfWritableRepository<Recipe>(context);
            Users = new EfWritableRepository<User>(context);
        }

        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }
    }
}
