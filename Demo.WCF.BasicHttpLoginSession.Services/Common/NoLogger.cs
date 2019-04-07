using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class NoLogger : ILogger
    {
        public void LogException(Exception ex)
        {
            //Log nothing
        }

        public void LogInformation(string message)
        {
            //Log nothing
        }

        public void LogWarning(string message)
        {
            //Log nothing
        }
    }
}