﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Data.EntityFramework
{
    public class FoodStuffsEfSqlData : AbstractFoodStuffsEfData
    {
        public FoodStuffsEfSqlData(IConfiguration configuration) : base(BuildContext(configuration))
        {
        }

        private static FoodStuffsContext BuildContext(IConfiguration configuration)
        {
            // Typcially we pull this from user envrionment variables.
            var connectionString = configuration["FoodStuffsConnectionString"];

            var options = new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new FoodStuffsContext(options);
        }
    }
}