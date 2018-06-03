using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Services.Data
{
    public partial class FoodStuffsContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options)
        {
        }
    }
}