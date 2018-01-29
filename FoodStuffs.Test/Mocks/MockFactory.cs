using Core.Model.Actions.Responder;
using Core.Model.Services.DateTime;
using FoodStuffs.Data.Models;
using FoodStuffs.Model.Interfaces.Data.Models;
using System;

namespace FoodStuffs.Test.Mocks
{
    public static class MockFactory
    {
        public static ICategory Category1 => new Category
        {
            Id = 1,
            Name = "Category1"
        };

        public static ICategory Category2 => new Category
        {
            Id = 2,
            Name = "Category2"
        };

        public static ICategory Category3 => new Category
        {
            Id = 3,
            Name = "Category3"
        };

        public static IDateTimeService EarlyDateTimeService =>
                                            new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11, 11, 11));

        public static SimpleActionResponder GetResponder => new SimpleActionResponder();

        public static IDateTimeService LateDateTimeService =>
                    new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22, 22, 22));

        public static IRecipe Recipe1 => new Recipe
        {
            Id = 1,
            Name = "Recipe1",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 1,
            ModifiedByUserId = 1
        };

        public static IRecipe Recipe2 => new Recipe
        {
            Id = 2,
            Name = "Recipe2",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 1,
            ModifiedByUserId = 1
        };

        public static IRecipe Recipe3 => new Recipe
        {
            Id = 3,
            Name = "Recipe3",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 1,
            ModifiedByUserId = 1
        };

        public static IUser User1 => new User
        {
            Id = 1,
            FirstName = "First",
            LastName = "Last",
        };

        public static IUser User2 => new User
        {
            Id = 2,
            FirstName = "First",
            LastName = "Last",
        };
    }
}