using Core.Model.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Core.Services.Data
{
    public class EfWritableRepository<TDbEntity> : EfReadOnlyRepository<TDbEntity>, IWritableRepository<TDbEntity> where TDbEntity : class, new()
    {
        public TDbEntity New => new TDbEntity();

        public EfWritableRepository(DbContext context) : base(context)
        {
        }

        public void Add(TDbEntity entity)
        {
            Context.Set<TDbEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TDbEntity> entities)
        {
            Context.Set<TDbEntity>().AddRange(entities);
        }

        public void Remove(TDbEntity entity)
        {
            Context.Set<TDbEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TDbEntity> entities)
        {
            Context.Set<TDbEntity>().RemoveRange(entities);
        }
    }
}