using Demo.WCF.BasicHttpLoginSession.Lib;
using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Common;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class CacheTokenRetriever : ICacheTokenRetriever
    {
        private readonly ICache cache;

        public CacheTokenRetriever(ICache cache)
        {
            Ensure.ArgumentNotNull(cache, nameof(cache));

            this.cache = cache;
        }

        public string RetrieveToken(string userID)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userID, nameof(userID));

            ILoginSession loginSession = cache.GetAndRefreshTimeout(userID) as ILoginSession;
            return loginSession?.AuthenticationIdentifier;
        }
    }
}
