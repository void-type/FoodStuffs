using Core.Model.Services.Logging;
using System;

namespace FoodStuffs.Test.Mocks.Services.Logging
{
    public class NullLoggingService : ILoggingService
    {
        public void Debug(params string[] messages)
        {
        }

        public void Debug(Exception ex, params string[] messages)
        {
        }

        public void Error(params string[] messages)
        {
        }

        public void Error(Exception ex, params string[] messages)
        {
        }

        public void Fatal(params string[] messages)
        {
        }

        public void Fatal(Exception ex, params string[] messages)
        {
        }

        public void Info(params string[] messages)
        {
        }

        public void Info(Exception ex, params string[] messages)
        {
        }

        public void Warn(params string[] messages)
        {
        }

        public void Warn(Exception ex, params string[] messages)
        {
        }
    }
}