using Core.Model.Services.Data;
using System.Collections.Generic;
using System.Linq;

namespace Core.Data.List
{
    public class ListReadOnlyRepository<TDbEntity> : IReadOnlyRepository<TDbEntity> where TDbEntity : class

    {
        public virtual IQueryable<TDbEntity> Stored => ListStore.AsQueryable();
        protected readonly List<TDbEntity> ListStore = new List<TDbEntity>();
    }
}