using Microsoft.EntityFrameworkCore;
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
            var connectionString = configuration["FoodStuffsConnectionString"];

            var options = new DbContextOptionsBuilder<FoodStuffsContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new FoodStuffsContext(options);
        }
    }
}