using DataMigratorV13.NewData.Models;
using FoodStuffs.Web.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VoidCore.Model.Configuration;
using VoidCore.Model.Time;

// This app will migrate databases from v12 to v13.

Console.WriteLine("Starting data migration from v12 to v13. Be sure to have applied the current EF migration to create a new database next to your old one.");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var currentUserAccessor = new SingleUserAccessor();
var dateTimeService = new NowDateTimeService();

var oldData = new DataMigratorV13.OldData.EntityFramework.FoodStuffsContext(
    new DbContextOptionsBuilder<DataMigratorV13.OldData.EntityFramework.FoodStuffsContext>()
        .UseSqlServer(configuration.GetRequiredConnectionString("FoodStuffsOld"))
        .Options,
    dateTimeService,
    currentUserAccessor);

var newData = new DataMigratorV13.NewData.FoodStuffsContext(
    new DbContextOptionsBuilder<DataMigratorV13.NewData.FoodStuffsContext>()
        .UseSqlServer(configuration.GetRequiredConnectionString("FoodStuffsNew"))
        .Options,
    dateTimeService,
    currentUserAccessor);

// Start a transaction to roll it all back on failure.
using var dbContextTransaction = newData.Database.BeginTransaction();

// Categories
var oldCategories = oldData.Categories.ToList();

var categoryMap = new Dictionary<int, Category>();

foreach (var oldCat in oldCategories)
{
    var newCategory = new Category()
    {
        Name = oldCat.Name
    };

    newData.Categories.Add(newCategory);
    newData.SaveChanges();

    categoryMap.Add(oldCat.Id, newCategory);
}

// Meal sets
var oldMealSets = oldData.MealSets
    .Include(x => x.PantryIngredients)
    .ToList();

var mealSetDict = new Dictionary<int, MealSet>();

foreach (var oldMealSet in oldMealSets)
{
    var newMealSet = new MealSet()
    {
        Name = oldMealSet.Name
    };

    newMealSet.PantryIngredients
        .AddRange(oldMealSet.PantryIngredients
            .Select(pi => new MealSetPantryIngredient()
            {
                Name = pi.Name,
                Quantity = pi.Quantity,
            }));

    newData.MealSets.Add(newMealSet);
    newData.SaveChanges();

    mealSetDict.Add(oldMealSet.Id, newMealSet);
}

// Recipes, ingredients
var oldRecipes = oldData.Recipes
    .Include(x => x.Categories)
    .Include(x => x.Ingredients)
    .Include(x => x.Images)
    .Include(x => x.PinnedImage)
    .ToList();

var recipeDict = new Dictionary<int, Recipe>();

foreach (var oldRecipe in oldRecipes)
{
    var newRecipe = new Recipe()
    {
        Name = oldRecipe.Name,
        Directions = oldRecipe.Directions,
        PrepTimeMinutes = oldRecipe.PrepTimeMinutes,
        CookTimeMinutes = oldRecipe.CookTimeMinutes,
        IsForMealPlanning = oldRecipe.IsForMealPlanning,
    };

    newRecipe.Ingredients
        .AddRange(oldRecipe.Ingredients
            .Select(i => new RecipeIngredient()
            {
                Name = i.Name,
                Quantity = i.Quantity,
                IsCategory = i.IsCategory,
                Order = i.Order,
            }));

    newRecipe.Categories.AddRange(oldRecipe.Categories.Select(i => categoryMap[i.Id]));
    newRecipe.MealSets.AddRange(oldRecipe.MealSets.Select(i => mealSetDict[i.Id]));

    newData.Recipes.Add(newRecipe);
    newData.SaveChanges();

    recipeDict.Add(oldRecipe.Id, newRecipe);
}

// Images, blobs
var oldImages = oldData.Images
    .Include(x => x.Blob)
    .ToList();

var imageDict = new Dictionary<int, Image>();

foreach (var oldImage in oldImages)
{
    var newImage = new Image()
    {
        FileName = oldImage.FileName,
        Recipe = recipeDict[oldImage.RecipeId],
        ImageBlob = new ImageBlob()
        {
            Bytes = oldImage.Blob?.Bytes ?? Array.Empty<byte>()
        }
    };

    newData.Images.Add(newImage);
    newData.SaveChanges();

    imageDict.Add(oldImage.Id, newImage);
}

// Add images to recipes
foreach (var oldRecipe in oldRecipes)
{
    if (oldRecipe.PinnedImageId > 0)
    {
        var newRec = recipeDict[oldRecipe.Id];
        newRec.PinnedImage = imageDict[oldRecipe.PinnedImageId.Value];
    }

    newData.SaveChanges();
}

dbContextTransaction.Commit();

Console.WriteLine($"Migration complete. {oldRecipes.Count} recipes. {oldImages.Count} images. {oldMealSets.Count} meal sets. {oldCategories.Count} categories.");
