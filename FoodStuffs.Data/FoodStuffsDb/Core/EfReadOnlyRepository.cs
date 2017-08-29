using FoodStuffs.Model.Interfaces.Services.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.FoodStuffsDb.Core
{
    public class EfReadOnlyRepository<TInterface, TDbEntity> : EfDatabaseService, IReadOnlyRepository<TInterface> where TDbEntity : class, TInterface, new() where TInterface : class

    {
        public IQueryable<TInterface> Stored => Context.Set<TDbEntity>().AsQueryable();

        public EfReadOnlyRepository(DbContext context) : base(context)
        {
        }
    }
}