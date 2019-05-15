using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using VoidCore.AspNet.Data;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public class FoodStuffsEfData : IFoodStuffsData
    {
        public FoodStuffsEfData(FoodStuffsContext context, ILoggingStrategy loggingStrategy, IDateTimeService now, ICurrentUserAccessor currentUserAccessor)
        {
            Categories = new EfWritableRepository<Category>(context, loggingStrategy);
            CategoryRecipes = new EfWritableRepository<CategoryRecipe>(context, loggingStrategy);
            Recipes = new EfWritableRepository<Recipe>(context, loggingStrategy).AddAuditability(now, currentUserAccessor);
            Users = new EfWritableRepository<User>(context, loggingStrategy);
        }

        public IWritableRepository<Category> Categories { get; }
        public IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        public IWritableRepository<Recipe> Recipes { get; }
        public IWritableRepository<User> Users { get; }
    }
}
