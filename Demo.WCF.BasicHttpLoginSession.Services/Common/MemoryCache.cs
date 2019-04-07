using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using System;


namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class MemoryCache : ICacheWithEvents
    {
        private System.Runtime.Caching.ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

        public event EventHandler<CacheItemRemovedEventArgs> CacheItemRemoved;

        public bool Add(string key, object value)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));
            Ensure.ArgumentNotNull(value, nameof(value));

            System.Runtime.Caching.CacheItem item = new System.Runtime.Caching.CacheItem(key, value);
            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy();
            policy.RemovedCallback = x => OnCacheItemRemoved(new CacheItemRemovedEventArgs(x.CacheItem.Key, x.CacheItem.Value));
            return cache.Add(item, policy);
        }

        public bool Add(string key, object value, TimeSpan timeout)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));
            Ensure.ArgumentNotNull(value, nameof(value));

            System.Runtime.Caching.CacheItem item = new System.Runtime.Caching.CacheItem(key, value);

            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy();
            policy.SlidingExpiration = timeout;
            // Set non-removable flag to prevent cache item removals due to other reasonse apart from the timeout. e.g. low memory
            policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
            policy.RemovedCallback = x => OnCacheItemRemoved(new CacheItemRemovedEventArgs(x.CacheItem.Key, x.CacheItem.Value));

            return cache.Add(item, policy);
        }

        public void AddOrReplace(string key, object value)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));
            Ensure.ArgumentNotNull(value, nameof(value));

            System.Runtime.Caching.CacheItem item = new System.Runtime.Caching.CacheItem(key, value);
            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy();
            policy.RemovedCallback = x => OnCacheItemRemoved(new CacheItemRemovedEventArgs(x.CacheItem.Key, x.CacheItem.Value));
            cache.Set(item, policy);
        }

        public void AddOrReplace(string key, object value, TimeSpan timeout)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));
            Ensure.ArgumentNotNull(value, nameof(value));

            System.Runtime.Caching.CacheItem item = new System.Runtime.Caching.CacheItem(key, value);

            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy();
            policy.SlidingExpiration = timeout;
            // Set non-removable flag to prevent cache item removals due to other reasonse apart from the timeout. e.g. low memory
            policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
            policy.RemovedCallback = x => OnCacheItemRemoved(new CacheItemRemovedEventArgs(x.CacheItem.Key, x.CacheItem.Value));

            cache.Set(item, policy);
        }

        public object GetAndRefreshTimeout(string key)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));

            // calling Get() on MemoryCache refreshes the timeout, so we don' need to do anything more
            return cache.Get(key);
        }

        public bool Remove(string key)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));

            object removedValue = cache.Remove(key);
            return removedValue != null;
        }

        protected virtual void OnCacheItemRemoved(CacheItemRemovedEventArgs args)
        {
            CacheItemRemoved?.Invoke(this, args);
        }
    }
}


