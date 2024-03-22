using DataMigratorV13.OldData.Models;
using VoidCore.EntityFramework;
using VoidCore.Model.Data;

namespace DataMigratorV13.OldData.EntityFramework;

public class FoodStuffsEfData : IFoodStuffsData
{
    public FoodStuffsEfData(FoodStuffsContext context)
    {
        Blobs = new EfWritableRepository<Blob>(context);
        Categories = new EfWritableRepository<Category>(context);
        Images = new EfWritableRepository<Image>(context);
        MealSets = new EfWritableRepository<MealSet>(context);
        Recipes = new EfWritableRepository<Recipe>(context);
        Users = new EfWritableRepository<User>(context);
    }

    public IWritableRepository<Blob> Blobs { get; }
    public IWritableRepository<Category> Categories { get; }
    public IWritableRepository<Image> Images { get; }
    public IWritableRepository<MealSet> MealSets { get; }
    public IWritableRepository<Recipe> Recipes { get; }
    public IWritableRepository<User> Users { get; }
}
