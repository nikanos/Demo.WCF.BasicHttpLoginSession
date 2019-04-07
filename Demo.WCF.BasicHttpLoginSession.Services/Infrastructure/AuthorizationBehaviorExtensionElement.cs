using System;
using System.ServiceModel.Configuration;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class AuthorizationBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(AuthorizationBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new AuthorizationBehavior();
        }
    }
}


