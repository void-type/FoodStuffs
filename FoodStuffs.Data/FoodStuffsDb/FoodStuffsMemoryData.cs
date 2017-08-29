using FoodStuffs.Data.FoodStuffsDb.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodStuffs.Data.FoodStuffsDb
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

            return new FoodStuffsContext(options);
        }
    }
}