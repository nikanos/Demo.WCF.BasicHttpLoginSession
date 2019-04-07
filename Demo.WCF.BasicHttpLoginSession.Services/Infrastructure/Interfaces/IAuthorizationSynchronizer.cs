namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public interface IAuthorizationSynchronizer
    {
        void ReleaseLock(string authorizationKey, object lockObj);
        object LockAuthorization(string authorizationKey);
        bool RemoveAuthorizationLock(string authorizationKey);
    }
}