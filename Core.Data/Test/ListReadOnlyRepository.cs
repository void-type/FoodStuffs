using Core.Model.Services.Data;
using System.Collections.Generic;
using System.Linq;

namespace Core.Data.Test.Repositories
{
    public class ListReadOnlyRepository<TInterface> : IReadOnlyRepository<TInterface> where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => _stored.AsQueryable();
        protected readonly List<TInterface> _stored = new List<TInterface>();
    }
}