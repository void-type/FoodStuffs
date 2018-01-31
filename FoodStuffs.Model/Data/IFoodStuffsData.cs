using Core.Model.Services.Data;
using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Data
{
    /// <summary>
    /// Represents all the tables, views and functions of the database.
    /// </summary>
    public interface IFoodStuffsData : IPersistable
    {
        IWritableRepository<ICategory> Categories { get; }
        IWritableRepository<ICategoryRecipe> CategoryRecipes { get; }
        IWritableRepository<IRecipe> Recipes { get; }
        IWritableRepository<IUser> Users { get; }
    }
}