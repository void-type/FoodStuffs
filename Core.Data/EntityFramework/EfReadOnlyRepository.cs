using Core.Model.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Data.EntityFramework
{
    public class EfReadOnlyRepository<TInterface, TDbEntity> : IReadOnlyRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => Context.Set<TDbEntity>();

        public EfReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        protected readonly DbContext Context;
    }
}