using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Auth;
using Microsoft.EntityFrameworkCore;
using VoidCore.Model.Auth;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Time;

namespace FoodStuffs.Test;

/// <summary>
/// Test Dependencies
/// </summary>
public static class Deps
{
    static Deps()
    {
        CurrentUserAccessor = new SingleUserAccessor();

        var early = new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc);
        var late = new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc);

        DateTimeServiceEarly = new DiscreteDateTimeService(early, new DateTimeOffset(early));
        DateTimeServiceLate = new DiscreteDateTimeService(late, new DateTimeOffset(late));
    }

    public static readonly IDateTimeService DateTimeServiceEarly;
    public static readonly IDateTimeService DateTimeServiceLate;
    public static readonly ICurrentUserAccessor CurrentUserAccessor;

    public const string PngBase64String = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQAAAAA3bvkkAAAACklEQVQI12NoAAAAggCB3UNq9AAAAABJRU5ErkJggg==";

    public static FoodStuffsContext FoodStuffsContext(string? dbName = null)
    {
        return new FoodStuffsContext(
            new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options,
            DateTimeServiceLate,
            CurrentUserAccessor
        );
    }

    public static FoodStuffsContext Seed(this FoodStuffsContext context)
    {
        var category1 = context.Categories.Add(new Category { Id = 1, Name = "Category1" }).Entity;
        var category2 = context.Categories.Add(new Category { Id = 2, Name = "Category2" }).Entity;
        var category3 = context.Categories.Add(new Category { Id = 3, Name = "Category3" }).Entity;
        var category4 = context.Categories.Add(new Category { Id = 4, Name = "Category4" }).Entity;

        var recipe1 = context.Recipes.Add(new Recipe
        {
            Name = "Cheeseburger",
            Directions = "dir",
            Sides = "side",
            CookTimeMinutes = 21,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "12"
        }).Entity;

        recipe1.Categories.Add(category1);
        recipe1.Categories.Add(category2);
        recipe1.Categories.Add(category4);

        var recipe2 = context.Recipes.Add(new Recipe
        {
            Name = "Hotdog",
            Directions = "some",
            Sides = "side",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            IsForMealPlanning = true,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        }).Entity;

        recipe2.Categories.Add(category3);

        var recipe3 = context.Recipes.Add(new Recipe
        {
            Name = "Sandwich",
            Directions = "some",
            Sides = "side",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        }).Entity;

        recipe3.Categories.Add(category4);

        var fileBytes = Convert.FromBase64String(PngBase64String);
        var file = new SimpleFile(fileBytes, "my-image.png");

        var image1 = new Image
        {
            RecipeId = recipe1.Id,
            FileName = "1.png",
            CreatedBy = "Long John Silver2",
            CreatedOn = new DateTime(2019, 11, 8),
            ModifiedBy = "Long John Silver2",
            ModifiedOn = new DateTime(2019, 11, 8),
            ImageBlob = new ImageBlob
            {
                Bytes = file.Content.AsBytes,
            }
        };

        var image2 = new Image
        {
            RecipeId = recipe1.Id,
            FileName = "2.png",
            CreatedBy = "Long John Silver2",
            CreatedOn = new DateTime(2019, 11, 8),
            ModifiedBy = "Long John Silver2",
            ModifiedOn = new DateTime(2019, 11, 8),
            ImageBlob = new ImageBlob
            {
                Bytes = file.Content.AsBytes,
            }
        };

        context.Images.AddRange(image1, image2);

        context.SaveChanges();
        return context;
    }
}
