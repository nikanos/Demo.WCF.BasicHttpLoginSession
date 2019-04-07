using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using System.Collections.Concurrent;
using System.Threading;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class AuthorizationSynchronizer : IAuthorizationSynchronizer
    {
        private static ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public object LockAuthorization(string authorizationKey)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(authorizationKey, nameof(authorizationKey));

            var lockObj = locks.GetOrAdd(authorizationKey, new object());
            Monitor.Enter(lockObj);
            return lockObj;
        }

        public void ReleaseLock(string authorizationKey, object lockObj)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(authorizationKey, nameof(authorizationKey));
            Ensure.ArgumentNotNull(lockObj, nameof(lockObj));

            Monitor.Exit(lockObj);
        }

        public bool RemoveAuthorizationLock(string authorizationKey)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(authorizationKey, nameof(authorizationKey));

            bool result = locks.TryRemove(authorizationKey, out object lockObj);
            return result;
        }
    }
}


