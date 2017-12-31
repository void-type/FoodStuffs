using System.Collections.Generic;

namespace Core.Model.Services.Data
{
    public interface IWritableRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        TEntity New { get; }

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);
    }
}