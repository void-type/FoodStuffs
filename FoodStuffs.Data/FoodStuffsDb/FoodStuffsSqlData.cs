using FoodStuffs.Data.FoodStuffsDb.Core;
using FoodStuffs.Data.FoodStuffsDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Data.FoodStuffsDb
{
    public class FoodStuffsSqlData : AbstractFoodStuffsData
    {
        public FoodStuffsSqlData(IConfiguration configuration) : base(BuildContext(configuration))
        {
        }

        private static FoodStuffsContext BuildContext(IConfiguration configuration)
        {
            var connectionString = configuration["FoodStuffsConnectionString"];

            var options = new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new FoodStuffsContext(options);
        }
    }
}