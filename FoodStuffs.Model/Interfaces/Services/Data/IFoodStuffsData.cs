using FoodStuffs.Model.Interfaces.Domain;
using FoodStuffs.Model.Interfaces.Services.Data.Core;

namespace FoodStuffs.Model.Interfaces.Services.Data
{
    /// <summary>
    /// Represents all the tables, views and functions of the database.
    /// </summary>
    public interface IFoodStuffsData : IDataService
    {
        IRepository<ICategory> Categories { get; }
        IRepository<ICategoryRecipe> CategoryRecipes { get; }
        IRepository<IRecipe> Recipes { get; }
        IRepository<IUser> Users { get; }
    }
}