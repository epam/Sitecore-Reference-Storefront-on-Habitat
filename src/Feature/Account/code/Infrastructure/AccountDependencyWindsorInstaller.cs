namespace Sitecore.Feature.Account.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Sitecore.Feature.Account.Interfaces;
    using Sitecore.Feature.Account.Repositories;

    public class AccountDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAccountRepository>().ImplementedBy<AccountRepository>());
        }
    }
}
