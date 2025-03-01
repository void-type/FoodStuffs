using DataMigratorV15;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using VoidCore.Model.Configuration;
using VoidCore.Model.Text;
using VoidCore.Model.Time;

Console.WriteLine("Starting data migration from v14 to v15.");
Console.WriteLine("Run this data migration against your v14 database before applying the v15 EF schema migration. The EF schema migration will delete the ingredients column from recipes.");
Console.WriteLine("This will migrate recipe ingredients to be merged with directions. It will also format directions to be rich text.");
Console.WriteLine("Be sure you have backups of your data before running this migration!");

Console.WriteLine("Press any key to continue. Or press Ctrl+C to exit.");
Console.ReadKey();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var currentUserAccessor = new SingleUserAccessor();
var dateTimeService = new NowDateTimeService();

var targetData = new DataMigratorV15.NewData.FoodStuffsContext(
    new DbContextOptionsBuilder<DataMigratorV15.NewData.FoodStuffsContext>()
        .UseSqlServer(configuration.GetRequiredConnectionString("FoodStuffsTarget"))
        .Options,
    dateTimeService,
    currentUserAccessor);

// Start a transaction to roll it all back on failure.
using var dbContextTransaction = targetData.Database.BeginTransaction();

// Recipes, ingredients
var recipes = targetData.Recipes
    .Include(x => x.Ingredients)
    .ToList();

foreach (var recipe in recipes)
{
    var ingredientsString = recipe.Ingredients
        .OrderBy(i => i.Order)
        // Accumulate ingredients and categories as strings.
        .Aggregate(new List<IngredientCategory>() { new() }, (acc, i) =>
        {
            if (i.IsCategory)
            {
                acc.Add(new IngredientCategory { Name = i.Name });
            }
            else
            {
                acc.Last().Items.Add(i);
            }

            return acc;
        })
        .Aggregate(string.Empty, (acc, ic) =>
        {
            if (!ic.Name.IsNullOrWhiteSpace())
            {
                acc += $"<p><strong>{HtmlEncoder.Default.Encode(ic.Name)}</strong></p>";
            }

            if (ic.Items.Count != 0)
            {
                acc += $"<ul>{string.Join("", ic.Items.Select(i => $"<li>{HtmlEncoder.Default.Encode($"{i.Quantity}x {i.Name}")}</li>"))}</ul>";
            }

            return acc;
        });

    if (!ingredientsString.IsNullOrWhiteSpace())
    {
        ingredientsString = $"<h5>Ingredients</h5>{ingredientsString}";
    }

    var directionsString = recipe.Directions
        .SplitOnNewLine()
        .Where(x => !x.IsNullOrWhiteSpace())
        .Select(x => x.Trim())
        .Aggregate(string.Empty, (acc, d) => acc += $"<p>{HtmlEncoder.Default.Encode(d)}</p>");

    recipe.Directions = string.Join("", ingredientsString, directionsString);

    targetData.SaveChanges();
}

dbContextTransaction.Commit();

Console.WriteLine($"Migration complete. {recipes.Count} recipes.");
