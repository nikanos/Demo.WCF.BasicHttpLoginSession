using Demo.WCF.BasicHttpLoginSession.Lib.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Common;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Demo.WCF.BasicHttpLoginSession.Services.Infrastructure
{
    public class AuthorizationMessageInspector : IDispatchMessageInspector
    {
        private readonly IRequestAuthorizer requestAuthorizer;
        private readonly IAuthorizationSynchronizer authorizationSynchronizer;
        private readonly IApplicationRequestContextManager applicationRequestContextManager;
        private readonly ILogger logger;

        public AuthorizationMessageInspector(
            IRequestAuthorizer requestAuthorizer,
            IAuthorizationSynchronizer authorizationSynchronizer,
            IApplicationRequestContextManager applicationRequestContextManager,
            ILogger logger
            )
        {
            Ensure.ArgumentNotNull(requestAuthorizer, nameof(requestAuthorizer));
            Ensure.ArgumentNotNull(authorizationSynchronizer, nameof(authorizationSynchronizer));
            Ensure.ArgumentNotNull(applicationRequestContextManager, nameof(applicationRequestContextManager));
            Ensure.ArgumentNotNull(logger, nameof(logger));

            this.requestAuthorizer = requestAuthorizer;
            this.authorizationSynchronizer = authorizationSynchronizer;
            this.applicationRequestContextManager = applicationRequestContextManager;
            this.logger = logger;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            try
            {
                if (!requestAuthorizer.IsAuthorizationNeeded(request))
                    // Skip authorization checks and return
                    return null;

                if (requestAuthorizer.Authorize(request, out string errorMessage, out UserContext userContext))
                {
                    // Authorizer result was valid
                    // acquire a lock for the current user
                    var authorizationLockObj = authorizationSynchronizer.LockAuthorization(userContext.UserID);
                    // and store the user context for later use by operations in case needed
                    applicationRequestContextManager.Store(userContext);
                    return new CorrelationState() { UserContext = userContext, ObjectLock = authorizationLockObj };
                }
                else
                {
                    request = null;
                    throw new FaultException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                throw;
            }
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            try
            {
                CorrelationState state = correlationState as CorrelationState;
                if (state != null)
                {
                    // state is not null, which means that we got the return value from AfterReceiveRequest()
                    // So we should do the necessary cleanup
                    applicationRequestContextManager.Remove();
                    authorizationSynchronizer.ReleaseLock(state.UserContext.UserID, state.ObjectLock);
                }
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                throw;
            }
        }
    }
}


