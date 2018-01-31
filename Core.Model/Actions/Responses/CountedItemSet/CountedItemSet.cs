using System.Collections.Generic;
using System.Linq;

namespace Core.Model.Actions.Responses.CountedItemSet
{
    /// <summary>
    /// A DTO for sending a collection to the UI. Predictably brings items into memory from the database and counts them before sending them to the UI.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
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