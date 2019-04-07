using Demo.WCF.BasicHttpLoginSession.Lib;
using Demo.WCF.BasicHttpLoginSession.Services.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Infrastructure;
using System;
using System.ServiceModel;

namespace Demo.WCF.BasicHttpLoginSession.Services
{
    [ServiceBehavior(Namespace = "http://demo.wcfloginsession.net")]
    public class Service : IService
    {
        private readonly ICache cache;
        private readonly IAuthorizationSynchronizer authorizationSynchronizer;
        private readonly IApplicationRequestContextManager applicationRequestContextManager;

        public Service(
            ICache cache,
            IAuthorizationSynchronizer authorizationSynchronizer,
            IApplicationRequestContextManager applicationRequestContextManager)
        {
            this.cache = cache;
            this.authorizationSynchronizer = authorizationSynchronizer;
            this.applicationRequestContextManager = applicationRequestContextManager;

            ICacheWithEvents cacheWithEvents = cache as ICacheWithEvents;
            if (cacheWithEvents != null)
            {
                cacheWithEvents.CacheItemRemoved += (sender, args) =>
                {
                    authorizationSynchronizer.RemoveAuthorizationLock(args.Key);
                };
            }
        }

        private ILoginSession LoginSession
        {
            get
            {
                string cacheKey = applicationRequestContextManager.Current?.UserContext.UserID;
                if (cacheKey == null)
                    return null;
                return cache.GetAndRefreshTimeout(cacheKey) as ILoginSession;
            }
        }

        public string Login(string userID, string password)
        {
            try
            {
                //Note:
                //the cache key for this demo is set to be the userid - without the token -. This means that each login of the same user will overwrite his/her previous session (if any)

                var loginSession = new LoginSession(userID, password); //TODO: makde LoginSession am injectable dependency
                cache.AddOrReplace(userID, loginSession, TimeSpan.FromMinutes(1)); //TODO: make timeout configurable
                return loginSession.AuthenticationIdentifier;
            }
            catch(Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public void Logout()
        {
            try
            {
                if (LoginSession == null)
                    throw new LoginSessionExpiredException("session expired");

                cache.Remove(applicationRequestContextManager.Current.UserContext.UserID);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public string ActionA()
        {
            try
            {
                if (LoginSession == null)
                    throw new LoginSessionExpiredException("session expired");

                return LoginSession.ActionA();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public string ActionB()
        {
            try
            {
                if (LoginSession == null)
                    throw new LoginSessionExpiredException("session expired");

                return LoginSession.ActionB();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}