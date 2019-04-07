using System;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public interface ICacheWithEvents : ICache
    {
        event EventHandler<CacheItemRemovedEventArgs> CacheItemRemoved;
    }
}


