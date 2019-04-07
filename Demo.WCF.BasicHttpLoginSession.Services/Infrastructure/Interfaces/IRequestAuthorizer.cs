using Demo.WCF.BasicHttpLoginSession.Services.Common;
using System.ServiceModel.Channels;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public interface IRequestAuthorizer
    {
        bool IsAuthorizationNeeded(Message request);
        bool Authorize(Message request, out string errorMessage, out UserContext userContext);
    }
}