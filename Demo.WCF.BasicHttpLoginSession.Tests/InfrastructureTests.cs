using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Demo.WCF.BasicHttpLoginSession.Services.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Demo.WCF.BasicHttpLoginSession.Tests
{
    [TestClass]
    public class InfrastructureTests
    {
        #region AuthorizationMessageInspector

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizationMessageInspector_Construct___With_null_RequestAuthorizer___Throws()
        {
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>();
            var logger = new Mock<ILogger>();
            var applicationRequestContextManager = new Mock<IApplicationRequestContextManager>();

            var inspector = new AuthorizationMessageInspector(
                requestAuthorizer: null,
                authorizationSynchronizer: authorizationSynchronizer.Object,
                applicationRequestContextManager: applicationRequestContextManager.Object,
                logger: logger.Object);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizationMessageInspector_Construct___With_null_AuthorizationSynchronizer___Throws()        
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            var logger = new Mock<ILogger>();
            var applicationRequestContextManager = new Mock<IApplicationRequestContextManager>();

            var inspector = new AuthorizationMessageInspector(
                requestAuthorizer: requestAuthorizer.Object,
                authorizationSynchronizer: null,
                applicationRequestContextManager: applicationRequestContextManager.Object,
                logger: logger.Object);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizationMessageInspector_Construct___With_null_ApplicationRequestContextManager___Throws()        
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>();
            var logger = new Mock<ILogger>();

            var inspector = new AuthorizationMessageInspector(
                requestAuthorizer: requestAuthorizer.Object,
                authorizationSynchronizer: authorizationSynchronizer.Object,
                applicationRequestContextManager: null,
                logger: logger.Object);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizationMessageInspector_Construct___With_null_Logger___Throws()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>();
            var applicationRequestContextManager = new Mock<IApplicationRequestContextManager>();

            var inspector = new AuthorizationMessageInspector(
                requestAuthorizer: requestAuthorizer.Object,
                authorizationSynchronizer: authorizationSynchronizer.Object,
                applicationRequestContextManager: applicationRequestContextManager.Object,
                logger: null);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_AfterReceiveRequest___With_request_that_does_not_need_authorization___Does_not_try_to_Authorize()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            requestAuthorizer.Setup(x => x.IsAuthorizationNeeded(It.IsAny<Message>())).
                Returns(false);

            var inspector = Create_AuthorizationMessageInspector(requestAuthorizer: requestAuthorizer.Object);
            Message request = null;
            object ret = inspector.AfterReceiveRequest(request: ref request, channel: null, instanceContext: null);

            Assert.IsNull(ret);
            requestAuthorizer.Verify(x => x.Authorize(It.IsAny<Message>(), out It.Ref<string>.IsAny, out It.Ref<UserContext>.IsAny), Times.Never);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        [ExpectedException(typeof(FaultException))]
        public void AuthorizationMessageInspector_AfterReceiveRequest___With_call_to_Authorize_that_fails___Throws()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();

            requestAuthorizer.Setup(x => x.IsAuthorizationNeeded(It.IsAny<Message>())).
                Returns(true);

            string errorMessage = "authorization error";
            UserContext userContext = null;
            requestAuthorizer.Setup(x => x.Authorize(It.IsAny<Message>(), out errorMessage, out userContext))
                .Returns(false);

            var inspector = Create_AuthorizationMessageInspector(requestAuthorizer: requestAuthorizer.Object);

            Message request = null;
            object ret = inspector.AfterReceiveRequest(request: ref request, channel: null, instanceContext: null);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_AfterReceiveRequest___With_call_to_Authorize_that_fails___LogsException()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            var logger = new Mock<ILogger>();

            requestAuthorizer.Setup(x => x.IsAuthorizationNeeded(It.IsAny<Message>())).
                Returns(true);

            string errorMessage = "authorization error";
            UserContext userContext = null;
            requestAuthorizer.Setup(x => x.Authorize(It.IsAny<Message>(), out errorMessage, out userContext))
                .Returns(false);

            var inspector = Create_AuthorizationMessageInspector(requestAuthorizer: requestAuthorizer.Object, logger: logger.Object);

            Exception thrownException = null;
            try
            {
                Message request = null;
                object ret = inspector.AfterReceiveRequest(request: ref request, channel: null, instanceContext: null);
            }
            catch (FaultException ex)
            {
                thrownException = ex;
            }
            logger.Verify(x => x.LogException(It.Is<Exception>(y => object.ReferenceEquals(y, thrownException))), Times.Once);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_AfterReceiveRequest___With_call_to_Authorize_that_throws___LogsException()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            var logger = new Mock<ILogger>();

            requestAuthorizer.Setup(x => x.IsAuthorizationNeeded(It.IsAny<Message>())).
                Returns(true);

            Exception authorizeException = new Exception("exception during authorization");
            string errorMessage = null;
            UserContext userContext = null;
            requestAuthorizer.Setup(x => x.Authorize(It.IsAny<Message>(), out errorMessage, out userContext))
                .Throws(authorizeException);

            var inspector = Create_AuthorizationMessageInspector(requestAuthorizer: requestAuthorizer.Object, logger: logger.Object);

            Exception thrownException = null;
            try
            {
                Message request = null;
                object ret = inspector.AfterReceiveRequest(request: ref request, channel: null, instanceContext: null);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
            logger.Verify(x => x.LogException(It.Is<Exception>(y => object.ReferenceEquals(y, thrownException))), Times.Once);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_AfterReceiveRequest___With_call_to_Authorize_that_succeeds___SucceedsAndReturnsState()
        {
            var requestAuthorizer = new Mock<IRequestAuthorizer>();
            requestAuthorizer.Setup(x => x.IsAuthorizationNeeded(It.IsAny<Message>())).Returns(true);

            const string USERID = "myuserid";
            const string TOKEN = "mytoken";

            string errorMessage = null;
            UserContext userContext = new UserContext(USERID, TOKEN);
            requestAuthorizer.Setup(x => x.Authorize(It.IsAny<Message>(), out errorMessage, out userContext))
                .Returns(true);

            var inspector = Create_AuthorizationMessageInspector(requestAuthorizer: requestAuthorizer.Object);

            Message request = null;
            object ret = inspector.AfterReceiveRequest(request: ref request, channel: null, instanceContext: null);

            Assert.IsNotNull(ret);
            Assert.IsInstanceOfType(ret, typeof(CorrelationState));
            CorrelationState correlationState = (CorrelationState)ret;
            Assert.IsNotNull(correlationState.UserContext);
            Assert.AreEqual(USERID, correlationState.UserContext.UserID);
            Assert.AreEqual(TOKEN, correlationState.UserContext.Token);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_BeforeSendReply___When_CorrelationState_is_not_passed_as_argument___Does_not_call_Synchronizer()
        {
            // Strict mode of Mock will throw if a method is called without a setup
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>(MockBehavior.Strict);

            var inspector = Create_AuthorizationMessageInspector(authorizationSynchronizer: authorizationSynchronizer.Object);

            Message reply = null;
            inspector.BeforeSendReply(reply: ref reply, correlationState: null);
        }


        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_BeforeSendReply___When_CorrelationState_is_passed_as_argument___Calls_Synchronizer_to_release_lock_with_correct_arguments()
        {
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>();

            const string USERID = "myuserid";
            object OBJECTLOCK = new object();

            CorrelationState correlationState = new CorrelationState()
            {
                ObjectLock = OBJECTLOCK,
                UserContext = new UserContext(USERID, "dummytoken")
            };
            var inspector = Create_AuthorizationMessageInspector(authorizationSynchronizer: authorizationSynchronizer.Object);
            Message reply = null;
            inspector.BeforeSendReply(reply: ref reply, correlationState: correlationState);

            authorizationSynchronizer.Verify(x => x.ReleaseLock(It.Is<string>(y => y == USERID), It.Is<object>(y => object.ReferenceEquals(y, OBJECTLOCK))), Times.Once);
        }

        [TestMethod]
        [TestCategory("Infrastructure.AuthorizationMessageInspector")]
        public void AuthorizationMessageInspector_BeforeSendReply___When_Synchronizer_throws___Logs_Exception()
        {
            var authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>();
            var logger = new Mock<ILogger>();

            Exception synchronizationException = new Exception("exception during synchronization");

            authorizationSynchronizer.Setup(x => x.ReleaseLock(It.IsAny<string>(), It.IsAny<object>()))
                .Throws(synchronizationException);

            CorrelationState correlationState = new CorrelationState()
            {
                ObjectLock = new object(),
                UserContext = new UserContext("myuserid", "mytoken")
            };
            var inspector = Create_AuthorizationMessageInspector(authorizationSynchronizer: authorizationSynchronizer.Object, logger: logger.Object);
            Exception thrownException = null;
            try
            {
                Message reply = null;
                inspector.BeforeSendReply(reply: ref reply, correlationState: correlationState);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
            logger.Verify(x => x.LogException(It.Is<Exception>(y => object.ReferenceEquals(y, thrownException))), Times.Once);
        }

        #endregion

        #region RequestAuthorizer

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequestAuthorizer_Construct___With_null_CacheTokenRetriever___Throws()
        {
            var authorizer = new RequestAuthorizer(cacheTokenRetriever: null);
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_IsAuthorizationNeeded___For_login_method___Authorization_is_NOT_needed()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);

            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_LOGIN = NS + "/Login";

            var messageForLogin = Create_Message(ACTION_LOGIN);
            Assert.IsFalse(authorizer.IsAuthorizationNeeded(messageForLogin));
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_IsAuthorizationNeeded___For_non_login_methods___Authorization_is_needed()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);

            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_LOGOUT = NS + "/Logout";
            const string ACTION_A = NS + "/ActionA";
            const string ACTION_DUMMY = NS + "/ActionDummy";

            var messageForLogout = Create_Message(ACTION_LOGOUT);
            var messageForActionA = Create_Message(ACTION_A);
            var messageForActionDummy = Create_Message(ACTION_DUMMY);
            
            Assert.IsTrue(authorizer.IsAuthorizationNeeded(messageForLogout));
            Assert.IsTrue(authorizer.IsAuthorizationNeeded(messageForActionA));
            Assert.IsTrue(authorizer.IsAuthorizationNeeded(messageForActionDummy));
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_Authorize___With_no_userid_in_headers___Fails_authorization()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);

            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_DUMMY = NS + "/ActionDummy";
            var message = Create_Message(ACTION_DUMMY);
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_TOKEN, Constants.AUTHNS, "mytoken"));

            string errorMessage = null;
            UserContext userContext = null;

            bool result = authorizer.Authorize(message, out errorMessage, out userContext);
            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(Constants.ERROR_USERID_HEADER_MISSING, errorMessage);
            Assert.IsNull(userContext);
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_Authorize___With_no_token_in_headers___Fails_authorization()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);

            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_DUMMY = NS + "/ActionDummy";
            var message = Create_Message(ACTION_DUMMY);
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_USERID, Constants.AUTHNS, "myuserid"));

            string errorMessage = null;
            UserContext userContext = null;

            bool result = authorizer.Authorize(message, out errorMessage, out userContext);
            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            Assert.AreEqual(Constants.ERROR_TOKEN_HEADER_MISSING, errorMessage);
            Assert.IsNull(userContext);
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_Authorize___With_no_token_in_cache___Fails_authorization()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            const string TOKEN = "mytoken";
            const string USERID = "myuserid";

            cacheTokenRetriever.Setup(x => x.RetrieveToken(It.IsAny<string>())).
                Returns<string>(null);

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);
            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_DUMMY = NS + "/ActionDummy";
            var message = Create_Message(ACTION_DUMMY);
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_USERID, Constants.AUTHNS, USERID));
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_TOKEN, Constants.AUTHNS, TOKEN));

            bool result = authorizer.Authorize(message, out string errorMessage, out UserContext userContext);

            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            var expectedErrorMessage = $"Token ({TOKEN}) was not in session for user ({USERID})";
            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.IsNull(userContext);
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_Authorize___With_different_token_in_cache___Fails_authorization()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            const string TOKEN = "mytoken";
            const string USERID = "myuserid";

            cacheTokenRetriever.Setup(x => x.RetrieveToken(It.IsAny<string>())).
                Returns("token_in_cache");

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);
            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_DUMMY = NS + "/ActionDummy";
            var message = Create_Message(ACTION_DUMMY);
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_USERID, Constants.AUTHNS, USERID));
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_TOKEN, Constants.AUTHNS, TOKEN));

            bool result = authorizer.Authorize(message, out string errorMessage, out UserContext userContext);

            Assert.IsFalse(result);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
            var expectedErrorMessage = $"Token ({TOKEN}) does not match the one in session for user ({USERID})";
            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.IsNull(userContext);
        }

        [TestMethod]
        [TestCategory("Infrastructure.RequestAuthorizer")]
        public void RequestAuthorizer_Authorize___With_correct_values_in_cache___Succeeds_authorization_and_returns_context()
        {
            var cacheTokenRetriever = new Mock<ICacheTokenRetriever>();

            const string TOKEN = "mytoken";
            const string USERID = "myuserid";

            cacheTokenRetriever.Setup(x => x.RetrieveToken(It.IsAny<string>())).
                Returns(TOKEN);

            var authorizer = Create_RequestAuthorizer(cacheTokenRetriever: cacheTokenRetriever.Object);
            const string NS = "http://demo.wcfloginsession.net/IService";
            const string ACTION_DUMMY = NS + "/ActionDummy";
            var message = Create_Message(ACTION_DUMMY);
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_USERID, Constants.AUTHNS, USERID));
            message.Headers.Add(MessageHeader.CreateHeader(Constants.HEADER_TOKEN, Constants.AUTHNS, TOKEN));

            bool result = authorizer.Authorize(message, out string errorMessage, out UserContext userContext);

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
            Assert.IsNotNull(userContext);
            Assert.AreEqual(USERID, userContext.UserID);
            Assert.AreEqual(TOKEN, userContext.Token);
        }

        #endregion

        #region Helpers

        private static AuthorizationMessageInspector Create_AuthorizationMessageInspector(
            IRequestAuthorizer requestAuthorizer = null,
            IAuthorizationSynchronizer authorizationSynchronizer = null,
            IApplicationRequestContextManager applicationRequestContextManager = null,
            ILogger logger = null)
        {
            if (requestAuthorizer == null)
                requestAuthorizer = new Mock<IRequestAuthorizer>().Object;

            if (authorizationSynchronizer == null)
                authorizationSynchronizer = new Mock<IAuthorizationSynchronizer>().Object;

            if (applicationRequestContextManager == null)
                applicationRequestContextManager = new Mock<IApplicationRequestContextManager>().Object;

            if (logger == null)
                logger = new Mock<ILogger>().Object;

            return new AuthorizationMessageInspector(requestAuthorizer, authorizationSynchronizer, applicationRequestContextManager, logger);
        }

        private static RequestAuthorizer Create_RequestAuthorizer(ICacheTokenRetriever cacheTokenRetriever = null)
        {
            if (cacheTokenRetriever == null)
                cacheTokenRetriever = new Mock<ICacheTokenRetriever>().Object;

            return new RequestAuthorizer(cacheTokenRetriever);
        }

        private static Message Create_Message(string action)
        {
            return Message.CreateMessage(MessageVersion.Soap11, action);
        }

        #endregion
    }
}
