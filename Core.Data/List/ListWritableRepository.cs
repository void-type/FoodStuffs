using Core.Model.Services.Data;
using System.Collections.Generic;

namespace Core.Data.List
{
    public class ListWritableRepository<TDbEntity> : ListReadOnlyRepository<TDbEntity>, IWritableRepository<TDbEntity> where TDbEntity : class, new()
    {
        public TDbEntity New => new TDbEntity();

        public void Add(TDbEntity entity)
        {
            ListStore.Add(entity);
        }

        public void AddRange(IEnumerable<TDbEntity> entities)
        {
            ListStore.AddRange(entities);
        }

        public void Remove(TDbEntity entity)
        {
            ListStore.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TDbEntity> entities)
        {
            foreach (var entity in entities)
            {
                ListStore.Remove(entity);
            }
        }
    }
}