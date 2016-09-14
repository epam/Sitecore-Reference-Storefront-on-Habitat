namespace Sitecore.Feature.Navigation.Infrastructure
{
    using Repositories;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class NavigationDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INavigationRepository>().ImplementedBy<NavigationRepository>());
        }
    }
}