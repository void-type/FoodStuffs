using FoodStuffs.Model.Interfaces.Services.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.EntityFramework.Repositories
{
    public class Repository<TInterface, TDbEntity> : ReadOnlyRepository<TInterface, TDbEntity>, IRepository<TInterface> where TDbEntity : class, TInterface, new() where TInterface : class
    {
        public TInterface New => new TDbEntity();

        public Repository(DbContext context) : base(context)
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