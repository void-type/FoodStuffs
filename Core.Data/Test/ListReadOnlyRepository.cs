using Core.Model.Services.Data;
using System.Collections.Generic;
using System.Linq;

namespace Core.Data.Test
{
    public class ListReadOnlyRepository<TInterface> : IReadOnlyRepository<TInterface> where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => ListStore.AsQueryable();
        protected readonly List<TInterface> ListStore = new List<TInterface>();
    }
}