namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class UserContext
    {
        public string UserID { get; private set; }
        public string Token { get; private set; }

        public UserContext(string userID, string token)
        {
            this.UserID = userID;
            this.Token = token;
        }
    }
}
