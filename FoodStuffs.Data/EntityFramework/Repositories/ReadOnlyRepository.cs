using FoodStuffs.Model.Interfaces.Services.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodStuffs.Data.EntityFramework.Repositories
{
    public class ReadOnlyRepository<TInterface, TDbEntity> : DatabaseService, IReadOnlyRepository<TInterface> where TDbEntity : class, TInterface, new() where TInterface : class

    {
        public virtual IQueryable<TInterface> Stored => Context.Set<TDbEntity>().AsQueryable();

        public ReadOnlyRepository(DbContext context) : base(context)
        {
        }
    }
}