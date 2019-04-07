using System.ServiceModel;
using System;
using Demo.WCF.BasicHttpLoginSession.Lib.Common;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class ApplicationRequestContext : IExtension<OperationContext>
    {
        public UserContext UserContext { get; }

        public ApplicationRequestContext(UserContext userContext)
        {
            this.UserContext = userContext;
        }

        public void Attach(OperationContext owner)
        {
        }

        public void Detach(OperationContext owner)
        {
        }
    }
}


