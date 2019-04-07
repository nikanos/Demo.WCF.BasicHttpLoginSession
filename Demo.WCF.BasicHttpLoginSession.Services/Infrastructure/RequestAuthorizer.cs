using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Common;
using System.ServiceModel.Channels;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class RequestAuthorizer : IRequestAuthorizer
    {
        private readonly ICacheTokenRetriever cacheTokenRetriever;

        public RequestAuthorizer(ICacheTokenRetriever cacheTokenRetriever)
        {
            Ensure.ArgumentNotNull(cacheTokenRetriever, nameof(cacheTokenRetriever));

            this.cacheTokenRetriever = cacheTokenRetriever;
        }

        public bool IsAuthorizationNeeded(Message request)
        {
            Ensure.ArgumentNotNull(request, nameof(request));

            if (request.Headers?.Action == Constants.LOGIN_ACTION)
                return false;// Skip authorization for login action

            return true;
        }

        public bool Authorize(Message request, out string errorMessage, out UserContext userContext)
        {
            Ensure.ArgumentNotNull(request, nameof(request));

            int userIDHeaderIndex = request.Headers.FindHeader(Constants.HEADER_USERID, Constants.AUTHNS);
            int tokenHeaderIndex = request.Headers.FindHeader(Constants.HEADER_TOKEN, Constants.AUTHNS);

            if (userIDHeaderIndex == Constants.INDEX_NOT_FOUND)
            {
                errorMessage = Constants.ERROR_USERID_HEADER_MISSING;
                userContext = null;
                return false;
            }

            if (tokenHeaderIndex == Constants.INDEX_NOT_FOUND)
            {
                errorMessage = Constants.ERROR_TOKEN_HEADER_MISSING;
                userContext = null;
                return false;
            }

            string userID = request.Headers.GetHeader<string>(userIDHeaderIndex);
            string token = request.Headers.GetHeader<string>(tokenHeaderIndex);

            string tokenInCache = cacheTokenRetriever.RetrieveToken(userID);
            if (tokenInCache == null)
            {
                errorMessage = string.Format(Constants.ERROR_TOKEN_NOT_IN_SESSION, token, userID);
                userContext = null;
                return false;
            }

            if (token != tokenInCache)
            {
                errorMessage = string.Format(Constants.ERROR_TOKEN_DOES_NOT_MATCH, token, userID);
                userContext = null;
                return false;
            }

            errorMessage = string.Empty;
            userContext = new UserContext(userID, token);
            return true;
        }
    }
}


