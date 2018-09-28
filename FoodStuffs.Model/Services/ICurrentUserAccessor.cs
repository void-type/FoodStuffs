using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Services
{
    public interface ICurrentUserAccessor
    {
        User User { get; }
    }
}
