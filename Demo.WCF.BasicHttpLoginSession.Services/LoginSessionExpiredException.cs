using System;
using System.Runtime.Serialization;

namespace Demo.WCF.BasicHttpLoginSession.Services
{
    [Serializable]
    internal class LoginSessionExpiredException : Exception
    {
        public LoginSessionExpiredException()
        {
        }

        public LoginSessionExpiredException(string message) : base(message)
        {
        }

        public LoginSessionExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginSessionExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}