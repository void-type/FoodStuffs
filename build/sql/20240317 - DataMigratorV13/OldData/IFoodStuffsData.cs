using DataMigratorV13.OldData.Models;
using VoidCore.Model.Data;

namespace DataMigratorV13.OldData;

/// <summary>
/// Represents all the tables, views and functions of the database.
/// </summary>
public interface IFoodStuffsData
{
    IWritableRepository<Blob> Blobs { get; }
    IWritableRepository<Category> Categories { get; }
    IWritableRepository<Image> Images { get; }
    IWritableRepository<MealSet> MealSets { get; }
    IWritableRepository<Recipe> Recipes { get; }
    IWritableRepository<User> Users { get; }
}
