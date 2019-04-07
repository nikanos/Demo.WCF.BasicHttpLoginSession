namespace Demo.WCF.BasicHttpLoginSession.Lib
{
    public interface ILoginSession
    {
        string ActionA();
        string ActionB();
        string AuthenticationIdentifier { get; }
    }
}