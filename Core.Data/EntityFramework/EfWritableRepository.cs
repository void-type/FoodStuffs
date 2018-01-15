using System.Collections.Generic;
using System.Linq;
using Core.Model.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.EntityFramework
{
    public class EfWritableRepository<TInterface, TDbEntity> : EfReadOnlyRepository<TInterface, TDbEntity>, IWritableRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class
    {
        public TInterface New => new TDbEntity();

        public EfWritableRepository(DbContext context) : base(context)
        {
        }

        public void Add(TInterface entity)
        {
            Context.Set<TDbEntity>().Add((TDbEntity)entity);
        }

        public void AddRange(IEnumerable<TInterface> entities)
        {
            Context.Set<TDbEntity>().AddRange(entities.Select(entity => (TDbEntity)entity));
        }

        public void Remove(TInterface entity)
        {
            Context.Set<TDbEntity>().Remove((TDbEntity)entity);
        }

        public void RemoveRange(IEnumerable<TInterface> entities)
        {
            Context.Set<TDbEntity>().RemoveRange(entities.Select(entity => (TDbEntity)entity));
        }
    }
}