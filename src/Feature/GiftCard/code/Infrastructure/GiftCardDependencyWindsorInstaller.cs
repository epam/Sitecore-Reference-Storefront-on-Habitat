namespace Sitecore.Feature.GiftCard.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Repositories;

    public class GiftCardDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IGiftCardRepository>().ImplementedBy<GiftCardRepository>());
        }
    }
}
