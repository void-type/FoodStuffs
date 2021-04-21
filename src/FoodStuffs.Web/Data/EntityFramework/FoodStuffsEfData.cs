using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            Blobs = new EfWritableRepository<Blob>(context);
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Images = new EfWritableRepository<Image>(context).AddAuditability(now, currentUserAccessor);
            Recipes = new EfWritableRepository<Recipe>(context).AddAuditability(now, currentUserAccessor);
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
