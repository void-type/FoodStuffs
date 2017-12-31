using Core.Model.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Data.EntityFramework.Repositories
{
    public class EfReadOnlyRepository<TInterface, TDbEntity> : IReadOnlyRepository<TInterface>
        where TDbEntity : class, TInterface, new() where TInterface : class

    {
        public void Dispose()
        {
            Context.Dispose();
        }

        protected readonly DbContext Context;

        public EfReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TInterface> Stored => Context.Set<TDbEntity>();
    }
}