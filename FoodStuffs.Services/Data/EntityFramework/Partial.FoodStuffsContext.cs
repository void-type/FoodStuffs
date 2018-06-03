using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Services.Data.EntityFramework
{
    public partial class FoodStuffsContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options)
        {
        }
    }
}