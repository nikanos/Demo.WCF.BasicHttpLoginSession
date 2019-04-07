using Autofac;
using Autofac.Integration.Wcf;
using Demo.WCF.BasicHttpLoginSession.Services.Common;
using Demo.WCF.BasicHttpLoginSession.Services.Infrastructure;

namespace Demo.WCF.BasicHttpLoginSession.Services
{
    public static class DependencyManager
    {
        public static IContainer CompositionRoot
        {
            get;
            private set;
        }

        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MemoryCache>().As<ICache>();
            builder.RegisterType<CacheTokenRetriever>().As<ICacheTokenRetriever>();
            builder.RegisterType<RequestAuthorizer>().As<IRequestAuthorizer>();
            builder.RegisterType<AuthorizationSynchronizer>().As<IAuthorizationSynchronizer>();
            builder.RegisterType<ApplicationRequestContextManager>().As<IApplicationRequestContextManager>();
            builder.RegisterType<NoLogger>().As<ILogger>();
            builder.RegisterType<AuthorizationMessageInspector>().AsSelf();
            builder.RegisterType<Service>().AsSelf();

            IContainer container = builder.Build();
            CompositionRoot = container;

            AutofacHostFactory.Container = container;//Set container to WCF AutofacHostFactory
        }
    }
}