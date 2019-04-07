using Demo.WCF.BasicHttpLoginSession.Services.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Infrastructure;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Autofac;
using System;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class AuthorizationBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //make an exception and resolve directly from the cotainer without creating a lifetime scope. This inspector resolution should happen once for the lifetime of the application
            AuthorizationMessageInspector inspector = DependencyManager.CompositionRoot.Resolve<AuthorizationMessageInspector>();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}


