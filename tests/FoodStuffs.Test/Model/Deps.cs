using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Time;

namespace FoodStuffs.Test.Model
{
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

            DateTimeServiceEarly = new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc));
            DateTimeServiceLate = new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc));
        }

        public static readonly IDateTimeService DateTimeServiceEarly;
        public static readonly IDateTimeService DateTimeServiceLate;
        public static readonly ICurrentUserAccessor CurrentUserAccessor;

        public static FoodStuffsContext FoodStuffsContext(string dbName = null)
        {
            return new FoodStuffsContext(
                new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options
            );
        }

        public static FoodStuffsEfData FoodStuffsData(this FoodStuffsContext context)
        {
            var loggingStrategyMock = new Mock<ILoggingStrategy>();
            loggingStrategyMock.Setup(x => x.Log(It.IsAny<string[]>())).Returns("test request");
            return new FoodStuffsEfData(context, loggingStrategyMock.Object, DateTimeServiceLate, CurrentUserAccessor);
        }

        public static FoodStuffsContext Seed(this FoodStuffsContext context)
        {
            var category1 = context.Categories.Add(new Category { Name = "Category1" }).Entity.Id;
            var category2 = context.Categories.Add(new Category { Name = "Category2" }).Entity.Id;
            var category3 = context.Categories.Add(new Category { Name = "Category3" }).Entity.Id;

            var recipe1 = context.Recipes.Add(new Recipe
            {
                Name = "Recipe1",
                Ingredients = "ing",
                Directions = "dir",
                CookTimeMinutes = 21,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "12"
            }).Entity.Id;

            var recipe2 = context.Recipes.Add(new Recipe
            {
                Name = "Recipe2",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            }).Entity.Id;

            context.Recipes.Add(new Recipe
            {
                Name = "Recipe3",
                CookTimeMinutes = 2,
                PrepTimeMinutes = 2,
                CreatedOn = DateTimeServiceEarly.Moment,
                ModifiedOn = DateTimeServiceLate.Moment,
                CreatedBy = "11",
                ModifiedBy = "11"
            });

            context.CategoryRecipes.Add(new CategoryRecipe { RecipeId = recipe1, CategoryId = category1 });
            context.CategoryRecipes.Add(new CategoryRecipe { RecipeId = recipe1, CategoryId = category2 });
            context.CategoryRecipes.Add(new CategoryRecipe { RecipeId = recipe2, CategoryId = category3 });

            var file = new SimpleFile("seeded file content", "some file.txt");

            var image1 = new Image
            {
                RecipeId = recipe1,
                CreatedBy = "Long John Silver2",
                CreatedOn = new DateTime(2019, 11, 8),
                ModifiedBy = "Long John Silver2",
                ModifiedOn = new DateTime(2019, 11, 8),
            };

            var image2 = new Image
            {
                RecipeId = recipe1,
                CreatedBy = "Long John Silver2",
                CreatedOn = new DateTime(2019, 11, 8),
                ModifiedBy = "Long John Silver2",
                ModifiedOn = new DateTime(2019, 11, 8),
            };

            context.Images.AddRange(image1, image2);

            context.Blobs.AddRange(
                new Blob
                {
                    Bytes = file.Content.AsBytes,
                    Id = image1.Id,
                },
                new Blob
                {
                    Bytes = file.Content.AsBytes,
                    Id = image2.Id,
                }
            );

            context.SaveChanges();
            return context;
        }
    }
}
