using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using VoidCore.EntityFramework;
using VoidCore.Model.Data;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context)
        {
            Blobs = new EfWritableRepository<Blob>(context);
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Images = new EfWritableRepository<Image>(context);
            Recipes = new EfWritableRepository<Recipe>(context);
            Users = new EfWritableRepository<User>(context);
        }

        public IWritableRepository<Blob> Blobs { get; }
        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Image> Images { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }
    }
}
