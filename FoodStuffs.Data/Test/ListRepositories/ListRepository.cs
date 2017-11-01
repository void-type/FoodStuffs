using FoodStuffs.Model.Interfaces.Services.Data.Core;
using System.Collections.Generic;

namespace FoodStuffs.Data.Test.ListRepositories
{
    public class ListRepository<TInterface, TDbEntity> : ReadOnlyListRepository<TInterface>, IRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class
    {
        public TInterface New => new TDbEntity();

        public void Add(TInterface entity)
        {
            _stored.Add(entity);
        }

        public void AddRange(IEnumerable<TInterface> entities)
        {
            _stored.AddRange(entities);
        }

        public void Remove(TInterface entity)
        {
            _stored.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TInterface> entities)
        {
            foreach (var entity in entities)
            {
                _stored.Remove(entity);
            }
        }
    }
}