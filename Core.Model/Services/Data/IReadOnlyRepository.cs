using System.Linq;

namespace Core.Model.Services.Data
{
    /// <summary>
    /// A persistent repository of objects that can be queried.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IReadOnlyRepository<out TEntity> where TEntity : class
    {
        IQueryable<TEntity> Stored { get; }
    }
}