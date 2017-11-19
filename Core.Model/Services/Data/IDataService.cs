using System;

namespace Core.Model.Services.Data
{
    public interface IDataService : IDisposable
    {
        void SaveChanges();
    }
}