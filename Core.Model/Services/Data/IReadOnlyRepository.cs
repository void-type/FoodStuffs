using System.Linq;

namespace Core.Model.Services.Data
{
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        IQueryable<TEntity> Stored { get; }
    }
}