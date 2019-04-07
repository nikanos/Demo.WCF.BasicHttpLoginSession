using System;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogException(Exception ex);
    }
}