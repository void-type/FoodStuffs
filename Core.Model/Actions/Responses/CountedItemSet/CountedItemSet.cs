using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Responses.CountedItemSet
{
    public class CountedItemSet<TEntity> : ICountedItemSet<TEntity>
    {
        public virtual int Count => Items?.Count() ?? 0;

        public IEnumerable<TEntity> Items { get; set; }

        public CountedItemSet(IEnumerable<TEntity> items)
        {
            Items = items.ToList();
        }

        public CountedItemSet()
        {
        }
    }
}