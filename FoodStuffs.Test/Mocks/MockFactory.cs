using Core.Model.Actions.Responder;
using Core.Model.Services.DateTime;
using FoodStuffs.Data.Models;
using FoodStuffs.Data.Service;
using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodStuffs.Test.Mocks
{
    public static class MockFactory
    {
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

        public static IDateTimeService EarlyDateTimeService =>
                                            new DiscreteDateTimeService(new DateTime(2001, 1, 1, 11, 11, 11));

        public static SimpleActionResponder GetResponder => new SimpleActionResponder();

        public static IDateTimeService LateDateTimeService =>
                    new DiscreteDateTimeService(new DateTime(2002, 2, 2, 22, 22, 22));

        public static Recipe Recipe1 => new Recipe
        {
            Id = 11,
            Name = "Recipe1",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 11,
            ModifiedByUserId = 11
        };

        public static Recipe Recipe2 => new Recipe
        {
            Id = 12,
            Name = "Recipe2",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 11,
            ModifiedByUserId = 11
        };

        public static Recipe Recipe3 => new Recipe
        {
            Id = 13,
            Name = "Recipe3",
            CookTimeMinutes = 2,
            PrepTimeMinutes = 2,
            CreatedOn = EarlyDateTimeService.Moment,
            ModifiedOn = EarlyDateTimeService.Moment,
            CreatedByUserId = 11,
            ModifiedByUserId = 11
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

        public static FoodStuffsEfData MemoryData(string dbName = null)
        {
            return new FoodStuffsEfData(new FoodStuffsContext(new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options));
        }
    }
}