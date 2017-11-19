using System.Collections.Generic;

namespace Core.Model.Actions.Responses.CountedItemSet
{
    public interface ICountedItemSet<TEntity> : ICountable
    {
        IEnumerable<TEntity> Items { get; set; }
    }
}