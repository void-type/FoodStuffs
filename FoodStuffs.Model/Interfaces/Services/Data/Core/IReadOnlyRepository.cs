using System.Linq;

namespace FoodStuffs.Model.Interfaces.Services.Data.Core
{
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        IQueryable<TEntity> Stored { get; }
    }
}