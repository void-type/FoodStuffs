using FoodStuffs.Model.Data.Models;
using VoidCore.Model.Data;

namespace FoodStuffs.Model.Data
{
    /// <summary>
    /// Represents all the tables, views and functions of the database.
    /// </summary>
    public interface IFoodStuffsData
    {
        // TODO: private constructors for data entities.
        IWritableRepository<Category> Categories { get; }
        IWritableRepository<CategoryRecipe> CategoryRecipes { get; }
        IWritableRepository<Recipe> Recipes { get; }
        IWritableRepository<User> Users { get; }
    }
}
