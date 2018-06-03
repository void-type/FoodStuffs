using System;

namespace Core.Model.Data
{
    /// <summary>
    /// Can save changes through a disposable connection to some form of persistence.
    /// </summary>
    public interface IPersistable : IDisposable
    {
        void SaveChanges();
    }
}