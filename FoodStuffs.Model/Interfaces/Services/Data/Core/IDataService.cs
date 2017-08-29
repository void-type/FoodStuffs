using System;

namespace FoodStuffs.Model.Interfaces.Services.Data.Core
{
    public interface IDataService : IDisposable
    {
        void SaveChanges();
    }
}