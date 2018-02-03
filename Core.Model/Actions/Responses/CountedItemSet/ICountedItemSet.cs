using System.Collections.Generic;

namespace Core.Model.Actions.Responses.CountedItemSet
{
    public interface ICountedItemSet<TEntity>
    {
        int Count { get; }
        IEnumerable<TEntity> Items { get; set; }
    }
}