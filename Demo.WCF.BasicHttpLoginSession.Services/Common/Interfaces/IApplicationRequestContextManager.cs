namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public interface IApplicationRequestContextManager
    {
        void Store(UserContext userContext);
        void Remove();
        ApplicationRequestContext Current { get; }
    }
}