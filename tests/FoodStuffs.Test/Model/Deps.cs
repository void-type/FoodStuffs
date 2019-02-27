using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using VoidCore.Model.Time;

namespace FoodStuffs.Test.Model
{
    /// <summary>
    /// Test Dependencies
    /// </summary>
    public static class Deps
    {
        public static IDateTimeService DateTimeServiceEarly =>
            new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc));

        public static IDateTimeService DateTimeServiceLate =>
            new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc));

        public static FoodStuffsEfData FoodStuffsData(string dbName = null)
        {
            return new FoodStuffsEfData(
                new FoodStuffsContext(
                    new DbContextOptionsBuilder<FoodStuffsContext>()
                    .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                    .Options
                )
            );
        }

        public static async Task<FoodStuffsEfData> Seed(this FoodStuffsEfData data)
        {
            await data.Categories.Add(new Category { Id = 11, Name = "Category1" });
            await data.Categories.Add(new Category { Id = 12, Name = "Category2" });
            await data.Categories.Add(new Category { Id = 13, Name = "Category3" });

            await data.Recipes.Add(new Recipe
            {
                Id = 11,
                    Name = "Recipe1",
                    Ingredients = "ing",
                    Directions = "dir",
                    CookTimeMinutes = 21,
                    PrepTimeMinutes = 2,
                    CreatedOn = DateTimeServiceEarly.Moment,
                    ModifiedOn = DateTimeServiceLate.Moment,
                    CreatedBy = "11",
                    ModifiedBy = "12"
            });

            await data.Recipes.Add(new Recipe
            {
                Id = 12,
                    Name = "Recipe2",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2,
                    CreatedOn = DateTimeServiceEarly.Moment,
                    ModifiedOn = DateTimeServiceLate.Moment,
                    CreatedBy = "11",
                    ModifiedBy = "11"
            });

            await data.Recipes.Add(new Recipe
            {
                Id = 13,
                    Name = "Recipe3",
                    CookTimeMinutes = 2,
                    PrepTimeMinutes = 2,
                    CreatedOn = DateTimeServiceEarly.Moment,
                    ModifiedOn = DateTimeServiceLate.Moment,
                    CreatedBy = "11",
                    ModifiedBy = "11"
            });

            await data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 11 });
            await data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 12 });
            await data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 12, CategoryId = 11 });

            await data.Users.Add(new User { Id = 11, FirstName = "First", LastName = "Last", });
            await data.Users.Add(new User { Id = 12, FirstName = "First", LastName = "Last", });

            return data;
        }
    }
}
