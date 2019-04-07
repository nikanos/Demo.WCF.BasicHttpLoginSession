namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public interface ICacheTokenRetriever
    {
        string RetrieveToken(string userID);
    }
}

