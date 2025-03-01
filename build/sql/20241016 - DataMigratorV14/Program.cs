using DataMigratorV14;
using DataMigratorV14.OldData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VoidCore.Model.Configuration;
using VoidCore.Model.Time;

// This app will migrate databases from v13 to v14.

Console.WriteLine("Starting data migration from v13 to v14.");
Console.WriteLine("This will migrate recipe ingredients from the old format to the new one.");
Console.WriteLine("Note that Meal Plans are completely redesigned and need to be re-entered.");
Console.WriteLine("Take a backup of your V13 database, restore it (source) next to your current database (target).");
Console.WriteLine("Then run the migration against the current database. This will delete current recipe ingredients, but they will be restored with this migration.");

Console.WriteLine("Press any key to continue. Or press Ctrl+C to exit.");
Console.ReadKey();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var currentUserAccessor = new SingleUserAccessor();
var dateTimeService = new NowDateTimeService();

var sourceData = new DataMigratorV14.OldData.FoodStuffsContext(
    new DbContextOptionsBuilder<DataMigratorV14.OldData.FoodStuffsContext>()
        .UseSqlServer(configuration.GetRequiredConnectionString("FoodStuffsSource"))
        .Options,
    dateTimeService,
    currentUserAccessor);

var targetData = new DataMigratorV14.NewData.FoodStuffsContext(
    new DbContextOptionsBuilder<DataMigratorV14.NewData.FoodStuffsContext>()
        .UseSqlServer(configuration.GetRequiredConnectionString("FoodStuffsTarget"))
        .Options,
    dateTimeService,
    currentUserAccessor);

// Start a transaction to roll it all back on failure.
using var dbContextTransaction = targetData.Database.BeginTransaction();

// Recipes, ingredients
var sourceRecipes = sourceData.Recipes
    .Include(x => x.Ingredients)
    .ToList();

var recipeDict = new Dictionary<int, Recipe>();

foreach (var sourceRecipe in sourceRecipes)
{
    var targetRecipe = await targetData.Recipes
        .FirstOrDefaultAsync(x => x.Id == sourceRecipe.Id);

    if (targetRecipe == null)
    {
        Console.WriteLine($"Recipe {sourceRecipe.Id} not found in target database. Skipping.");
        continue;
    }

    targetRecipe.Ingredients
        .AddRange(sourceRecipe.Ingredients
            .Select(i => new DataMigratorV14.NewData.Models.RecipeIngredient()
            {
                Name = i.Name,
                Quantity = i.Quantity,
                IsCategory = i.IsCategory,
                Order = i.Order,
            }));

    targetData.SaveChanges();
}

dbContextTransaction.Commit();

Console.WriteLine($"Migration complete. {sourceRecipes.Count} recipes.");
