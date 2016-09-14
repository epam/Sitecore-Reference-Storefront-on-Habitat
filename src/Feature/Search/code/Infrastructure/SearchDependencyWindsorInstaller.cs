using Sitecore.Feature.Search.Repositories;

namespace Sitecore.Feature.Search.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class SearchDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISearchRepository>().ImplementedBy<SearchRepository>());
        }
    }
}
