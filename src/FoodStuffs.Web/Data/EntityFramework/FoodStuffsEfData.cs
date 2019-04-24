using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using VoidCore.AspNet.Data;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            Categories = new EfWritableRepository<Category>(context);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context);
            Recipes = new AuditableRepositoryDecorator<Recipe>(new EfWritableRepository<Recipe>(context), now, currentUserAccessor);
            Users = new EfWritableRepository<User>(context);
        }

        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }
    }
}
