using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using VoidCore.Model.Auth;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Time;

namespace FoodStuffs.Test.Model;

/// <summary>
/// Test Dependencies
/// </summary>
public static class Deps
{
    static Deps()
    {
        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(a => a.User).Returns(new DomainUser("SingleUser", Array.Empty<string>()));
        CurrentUserAccessor = userAccessorMock.Object;

        var early = new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc);
        var late = new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc);

        DateTimeServiceEarly = new DiscreteDateTimeService(early, new DateTimeOffset(early));
        DateTimeServiceLate = new DiscreteDateTimeService(late, new DateTimeOffset(late));
    }

    public static readonly IDateTimeService DateTimeServiceEarly;
    public static readonly IDateTimeService DateTimeServiceLate;
    public static readonly ICurrentUserAccessor CurrentUserAccessor;

    public static FoodStuffsContext FoodStuffsContext(string dbName = null)
    {
        return new FoodStuffsContext(
            new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options,
            DateTimeServiceLate,
            CurrentUserAccessor
        );
    }

    public static FoodStuffsEfData FoodStuffsData(this FoodStuffsContext context)
    {
        return new FoodStuffsEfData(context);
    }

    public static FoodStuffsContext Seed(this FoodStuffsContext context)
    {
        var category1 = context.Categories.Add(new Category { Name = "Category1" }).Entity;
        var category2 = context.Categories.Add(new Category { Name = "Category2" }).Entity;
        var category3 = context.Categories.Add(new Category { Name = "Category3" }).Entity;

        var recipe1 = context.Recipes.Add(new Recipe
        {
            Name = "Recipe1",
            Directions = "dir",
            CookTimeMinutes = 21,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "12"
        }).Entity;

        recipe1.Ingredients.Add(new Ingredient { Name = "ing", Quantity = 1, Order = 1 });
        recipe1.Categories.Add(category1);
        recipe1.Categories.Add(category2);

        var recipe2 = context.Recipes.Add(new Recipe
        {
            Name = "Recipe2",
            Directions = "some",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        }).Entity;

        recipe1.Ingredients.Add(new Ingredient { Name = "ing", Quantity = 1, Order = 1 });
        recipe2.Categories.Add(category3);

        var recipe3 = context.Recipes.Add(new Recipe
        {
            Name = "Recipe3",
            Directions = "some",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        }).Entity;

        recipe3.Ingredients.Add(new Ingredient { Name = "some", Quantity = 1, Order = 1 });

        var file = new SimpleFile("seeded file content", "some file.txt");

        var image1 = new Image
        {
            RecipeId = recipe1.Id,
            CreatedBy = "Long John Silver2",
            CreatedOn = new DateTime(2019, 11, 8),
            ModifiedBy = "Long John Silver2",
            ModifiedOn = new DateTime(2019, 11, 8),
        };

        var image2 = new Image
        {
            RecipeId = recipe1.Id,
            CreatedBy = "Long John Silver2",
            CreatedOn = new DateTime(2019, 11, 8),
            ModifiedBy = "Long John Silver2",
            ModifiedOn = new DateTime(2019, 11, 8),
        };

        var blob1 = new Blob
        {
            Bytes = file.Content.AsBytes,
            Id = image1.Id,
            Image = image1,
        };

        var blob2 = new Blob
        {
            Bytes = file.Content.AsBytes,
            Id = image2.Id,
            Image = image2
        };

        image1.Blob = blob1;
        image2.Blob = blob2;

        context.Images.AddRange(image1, image2);
        context.Blobs.AddRange(blob1, blob2);

        context.SaveChanges();
        return context;
    }
}
