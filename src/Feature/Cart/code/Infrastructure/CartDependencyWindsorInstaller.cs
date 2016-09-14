using Sitecore.Feature.Cart.Utils;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Utils;

namespace Sitecore.Feature.Cart.Infrastructure
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Repositories;

    public class CartDependencyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICartRepository>().ImplementedBy<CartRepository>());
            container.Register(Component.For<ICheckoutRepository>().ImplementedBy<CheckoutRepository>());
            container.Register(Component.For<ICartCacheService>().ImplementedBy<CartCacheService>());
            container.Register(Component.For<IProductResolver>().ImplementedBy<ProductResolver>());
        }
    }
}