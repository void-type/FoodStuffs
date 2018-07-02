using System;

namespace Core.Model.Logging
{
    /// <summary>
    /// Common interface for logging within the model.
    /// </summary>
    public interface ILoggingService
    {
        void Debug(params string[] messages);

        void Debug(Exception ex, params string[] messages);

        void Error(params string[] messages);

        void Error(Exception ex, params string[] messages);

        void Fatal(params string[] messages);

        void Fatal(Exception ex, params string[] messages);

        void Info(params string[] messages);

        void Info(Exception ex, params string[] messages);

        void Warn(params string[] messages);

        void Warn(Exception ex, params string[] messages);
    }
}
