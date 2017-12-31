using FoodStuffs.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodStuffs.Data.Services
{
    public class FoodStuffsEfMemoryData : AbstractFoodStuffsEfData
    {
        public FoodStuffsEfMemoryData(string dbName = null) : base(BuildContext(dbName))
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