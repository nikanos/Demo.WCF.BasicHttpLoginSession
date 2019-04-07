using System;
using System.Runtime.Serialization;

namespace Demo.WCF.BasicHttpLoginSession.Lib
{
    [Serializable]
    internal class LoginException : Exception
    {
        public LoginException()
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}