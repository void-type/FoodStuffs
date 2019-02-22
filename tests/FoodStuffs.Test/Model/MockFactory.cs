using FoodStuffs.Model.Data.Models;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using VoidCore.Model.Time;

namespace FoodStuffs.Test.Model
{
    public static class MockFactory
    {
        public static IDateTimeService DateTimeServiceEarly
        {
            get
            {
                var when = new DateTime(2001, 1, 1, 11, 11, 11, DateTimeKind.Utc);
                return new DiscreteDateTimeService(when);
            }
        }

        public static IDateTimeService DateTimeServiceLate
        {
            get
            {
                var when = new DateTime(2002, 2, 2, 22, 22, 22, DateTimeKind.Utc);
                return new DiscreteDateTimeService(when);
            }
        }

        public static Category Category1 => new Category
        {
            Id = 11,
            Name = "Category1"
        };

        public static Category Category2 => new Category
        {
            Id = 12,
            Name = "Category2"
        };

        public static Category Category3 => new Category
        {
            Id = 13,
            Name = "Category3"
        };

        public static Recipe Recipe1 => new Recipe
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
        };

        public static Recipe Recipe2 => new Recipe
        {
            Id = 12,
            Name = "Recipe2",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        };

        public static Recipe Recipe3 => new Recipe
        {
            Id = 13,
            Name = "Recipe3",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = DateTimeServiceEarly.Moment,
            ModifiedOn = DateTimeServiceLate.Moment,
            CreatedBy = "11",
            ModifiedBy = "11"
        };

        public static User User1 => new User
        {
            Id = 11,
            FirstName = "First",
            LastName = "Last",
        };

        public static User User2 => new User
        {
            Id = 12,
            FirstName = "First",
            LastName = "Last",
        };

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

        public static void PopulateWithData(FoodStuffsEfData data)
        {
            data.Categories.Add(Category1);
            data.Categories.Add(Category2);
            data.Categories.Add(Category3);

            data.Recipes.Add(Recipe1);
            data.Recipes.Add(Recipe2);
            data.Recipes.Add(Recipe3);

            data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 11 });
            data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 11, CategoryId = 12 });
            data.CategoryRecipes.Add(new CategoryRecipe { RecipeId = 12, CategoryId = 11 });

            data.Users.Add(User1);
            data.Users.Add(User2);
        }
    }
}
