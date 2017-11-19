using Microsoft.EntityFrameworkCore;

namespace Core.Data.EntityFramework
{
    /// <summary>
    /// A Database service wraps a dbcontext, allows it be disposed of, and can be extended to match any model interface needed to provide data or services.
    /// </summary>
    public abstract class DatabaseService
    {
        public void Dispose()
        {
            Context.Dispose();
        }

        protected readonly DbContext Context;

        protected DatabaseService(DbContext context)
        {
            Context = context;
        }
    }
}