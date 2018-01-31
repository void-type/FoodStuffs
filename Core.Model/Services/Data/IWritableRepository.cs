using System.Collections.Generic;

namespace Core.Model.Services.Data
{
    /// <summary>
    /// A persistent repository that can be modified.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IWritableRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns a new instance of the entity type.
        /// </summary>
        TEntity New { get; }

        /// <summary>
        /// Add a single entity.
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Add multiple entities at once.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove one entity.
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Remove multiple entities at once.
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}