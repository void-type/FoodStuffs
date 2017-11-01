using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodStuffs.Data.Test
{
    public class FoodStuffsMemoryData : AbstractFoodStuffsData
    {
        public FoodStuffsMemoryData(string dbName = null) : base(BuildContext(dbName))
        {
        }

        private static FoodStuffsContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options;

            var context = new FoodStuffsContext(options);

            context.Database.EnsureDeleted();

            return context;
        }
    }
}