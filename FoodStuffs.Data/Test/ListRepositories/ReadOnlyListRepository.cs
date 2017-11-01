using FoodStuffs.Model.Interfaces.Services.Data.Core;
using System.Collections.Generic;
using System.Linq;

namespace FoodStuffs.Data.Test.ListRepositories
{
    public class ReadOnlyListRepository<TInterface> : IReadOnlyRepository<TInterface> where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => _stored.AsQueryable();

        protected readonly List<TInterface> _stored = new List<TInterface>();
    }
}