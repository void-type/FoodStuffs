using System.Collections.Generic;
using System.Linq;
using Core.Model.Services.Data;

namespace Core.Data.Test
{
    public class ListReadOnlyRepository<TInterface> : IReadOnlyRepository<TInterface> where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => _stored.AsQueryable();
        protected readonly List<TInterface> _stored = new List<TInterface>();
    }
}