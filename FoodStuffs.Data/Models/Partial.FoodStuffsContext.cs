using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Data.Models
{
    public partial class FoodStuffsContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options)
        {
        }
    }
}