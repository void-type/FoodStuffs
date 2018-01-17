using Core.Model.Services.Data;
using System.Collections.Generic;

namespace Core.Data.Test
{
    public class ListWritableRepository<TInterface, TDbEntity> : ListReadOnlyRepository<TInterface>, IWritableRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class
    {
        public TInterface New => new TDbEntity();

        public void Add(TInterface entity)
        {
            ListStore.Add(entity);
        }

        public void AddRange(IEnumerable<TInterface> entities)
        {
            ListStore.AddRange(entities);
        }

        public void Remove(TInterface entity)
        {
            ListStore.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TInterface> entities)
        {
            foreach (var entity in entities)
            {
                ListStore.Remove(entity);
            }
        }
    }
}