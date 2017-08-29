using FoodStuffs.Data.FoodStuffsDb.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodStuffs.Data.FoodStuffsDb
{
    public static class TestDataFactory
    {
        public static FoodStuffsDbData FoodStuffsDb(string dbName = null)
        {
            return new FoodStuffsDbData(new FoodStuffsContext(MemoryDbContextOptions(dbName)));
        }

        private static DbContextOptions<FoodStuffsContext> MemoryDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options;
        }
    }
}