using System.Collections.Generic;

namespace FoodStuffs.Model.Actions.Core.Responses.CountedItemSet
{
    public interface ICountedItemSet<TEntity> : ICountable
    {
        IEnumerable<TEntity> Items { get; set; }
    }
}