using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using System;

namespace Demo.WCF.BasicHttpLoginSession.Lib
{
    public class LoginSession : ILoginSession
    {
        private readonly string userID;
        private readonly string authenticationIdentifier;

        public string AuthenticationIdentifier
        {
            get { return authenticationIdentifier; }
        }

        public LoginSession(string userID, string password)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userID, nameof(userID));
            Ensure.StringArgumentNotNullAndNotEmpty(password, nameof(password));

            if (userID != password)//Dummy password check
                throw new LoginException("wrong password");

            this.userID = userID;
            this.authenticationIdentifier = Guid.NewGuid().ToString("N");
        }

        public string ActionA()
        {
            System.Threading.Thread.Sleep(1000);
            return $"Doing work in ActionA for user ({userID})";
        }

        public string ActionB()
        {
            System.Threading.Thread.Sleep(5000);
            return $"Doing heavy work in ActionB for user ({userID})";
        }
    }
}
