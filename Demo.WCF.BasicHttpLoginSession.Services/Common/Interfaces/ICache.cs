using System;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public interface ICache
    {
        bool Add(string key, object value);
        bool Add(string key, object value, TimeSpan timeout);
        void AddOrReplace(string key, object value);
        void AddOrReplace(string key, object value, TimeSpan timeout);
        object GetAndRefreshTimeout(string key);
        bool Remove(string key);
    }
}