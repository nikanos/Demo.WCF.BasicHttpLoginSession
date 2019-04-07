namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    class Constants
    {
        public const string LOGIN_ACTION = "http://demo.wcfloginsession.net/IService/Login";
        public const string AUTHNS = "http://demo.wcfloginsession.net/auth/";
        public const string HEADER_USERID = "UserID";
        public const string HEADER_TOKEN = "Token";
        public const string ERROR_USERID_HEADER_MISSING = "UserID Header missing";
        public const string ERROR_TOKEN_HEADER_MISSING = "Token Header missing";
        public const string ERROR_TOKEN_NOT_IN_SESSION = "Token ({0}) was not in session for user ({1})";
        public const string ERROR_TOKEN_DOES_NOT_MATCH = "Token ({0}) does not match the one in session for user ({1})";
        public const int INDEX_NOT_FOUND = -1;
    }
}



