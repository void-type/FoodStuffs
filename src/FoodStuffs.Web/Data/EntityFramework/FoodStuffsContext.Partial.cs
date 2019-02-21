using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public partial class FoodStuffsContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options) { }
    }
}
