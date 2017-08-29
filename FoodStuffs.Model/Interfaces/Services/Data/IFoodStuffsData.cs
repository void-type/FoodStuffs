using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Model.Interfaces.Services.Data
{
    /// <summary>
    /// Represents all the tables, views and functions of the database.
    /// </summary>
    public interface IFoodStuffsData : IDataService
    {
        IRepository<IUser> Users { get; }
        IRepository<ICategory> Categories { get; }
        IRepository<IRecipe> Recipes { get; }
        IRepository<ICategoryRecipe> CategoryRecipes { get; }
    }
}