using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Demo.WCF.BasicHttpLoginSession.Services.Common
{
    public class ApplicationRequestContextManager : IApplicationRequestContextManager
    {
        public ApplicationRequestContext Current
        {
            get
            {
                return OperationContext.Current?.Extensions.Find<ApplicationRequestContext>();
            }
        }

        public void Store(UserContext userContext)
        {
            Ensure.ArgumentNotNull(userContext, nameof(userContext));

            OperationContext.Current?.Extensions.Add(new ApplicationRequestContext(userContext));
        }

        public void Remove()
        {
            OperationContext.Current?.Extensions.Remove(Current);
        }
    }
}