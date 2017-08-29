using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Data.FoodStuffsDb.Core
{
    /// <summary>
    /// A Database service wraps a dbcontext, allows it be disposed of, and can be extended to match any model interface needed to provide data or services.
    /// </summary>
    public abstract class EfDatabaseService
    {
        public void Dispose()
        {
            Context.Dispose();
        }

        protected readonly DbContext Context;

        protected EfDatabaseService(DbContext context)
        {
            Context = context;
        }
    }
}