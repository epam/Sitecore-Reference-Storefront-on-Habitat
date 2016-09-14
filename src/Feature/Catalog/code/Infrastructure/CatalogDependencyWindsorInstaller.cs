namespace Sitecore.Feature.Catalog.Infrastructure
{
    using Repositories;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class CatalogDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICatalogRepository>().ImplementedBy<CatalogRepository>());
        }
    }
}
