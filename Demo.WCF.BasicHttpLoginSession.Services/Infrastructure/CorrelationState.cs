using Demo.WCF.BasicHttpLoginSession.Services.Common;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    class CorrelationState
    {
        public UserContext UserContext { get; set; }
        public object ObjectLock { get; set; }
    }
}