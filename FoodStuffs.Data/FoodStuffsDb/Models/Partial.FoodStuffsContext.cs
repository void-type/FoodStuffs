using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Data.FoodStuffsDb.Models
{
    public partial class FoodStuffsContext
    {
        // Generate Model using Scaffold-DbContext "Server=SERVER;Database=FoodStuffs;User Id=FoodStuffsUser;Password=#####;"
        // Microsoft.EntityFrameworkCore.SqlServer -OutputDir Services/Data/Models -Force
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options)
        {
        }
    }
}