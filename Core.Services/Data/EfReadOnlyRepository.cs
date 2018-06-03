using Core.Model.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Services.Data
{
    public class EfReadOnlyRepository<TDbEntity> : IReadOnlyRepository<TDbEntity> where TDbEntity : class, new()

    {
        public virtual IQueryable<TDbEntity> Stored => Context.Set<TDbEntity>();

        public EfReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        protected readonly DbContext Context;
    }
}