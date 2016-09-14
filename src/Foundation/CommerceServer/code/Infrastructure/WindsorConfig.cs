namespace Sitecore.Foundation.CommerceServer.Factories
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Sitecore.Commerce.Connect.CommerceServer.Orders;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Services.Catalog;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Commerce.Services.GiftCards;
    using Sitecore.Commerce.Services.Inventory;
    using Sitecore.Commerce.Services.Payments;
    using Sitecore.Commerce.Services.Prices;
    using Sitecore.Commerce.Services.Shipping;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Logging;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Configures Castle Windsor IoC container.
    /// </summary>
    public static class WindsorConfig
    {
        /// <summary>
        /// The container.
        /// </summary>
        public static readonly IWindsorContainer Container;

        /// <summary>
        /// Initializes static members of the <see cref="WindsorConfig"/> class. 
        /// </summary>
        static WindsorConfig()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());

            container.Register(Component.For<IInventoryManager>().ImplementedBy<InventoryManager>());
            container.Register(Component.For<ICatalogManager>().ImplementedBy<CatalogManager>());
            container.Register(Component.For<ILoyaltyProgramManager>().ImplementedBy<LoyaltyProgramManager>());
            container.Register(Component.For<ICartManager>().ImplementedBy<CartManager>());
            container.Register(Component.For<IOrderManager>().ImplementedBy<OrderManager>());
            container.Register(Component.For<IAccountManager>().ImplementedBy<AccountManager>());
            container.Register(Component.For<IPaymentManager>().ImplementedBy<PaymentManager>());
            container.Register(Component.For<IPricingManager>().ImplementedBy<PricingManager>());
            container.Register(Component.For<IShippingManager>().ImplementedBy<ShippingManager>());
            container.Register(Component.For<IEntityFactory>().ImplementedBy<EntityFactory>());
            container.Register(Component.For<IContactFactory>().ImplementedBy<CommerceContactFactory>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());

            container.Register(Component.For<CatalogServiceProvider>().ImplementedBy<CatalogServiceProvider>());
            container.Register(Component.For<GiftCardServiceProvider>().ImplementedBy<GiftCardServiceProvider>());
            container.Register(Component.For<PricingServiceProvider>().ImplementedBy<PricingServiceProvider>());
            container.Register(Component.For<CommerceCartServiceProvider>().ImplementedBy<CommerceCartServiceProvider>());
            container.Register(Component.For<InventoryServiceProvider>().ImplementedBy<InventoryServiceProvider>());
            container.Register(Component.For<CustomerServiceProvider>().ImplementedBy<CustomerServiceProvider>());
            container.Register(Component.For<PaymentServiceProvider>().ImplementedBy<PaymentServiceProvider>());
            container.Register(Component.For<ShippingServiceProvider>().ImplementedBy<ShippingServiceProvider>());
            container.Register(Component.For<CommerceOrderServiceProvider>().ImplementedBy<CommerceOrderServiceProvider>());




            Container = container;
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        public static void ConfigureContainer()
        {
        }

        /// <summary>
        /// Releases the container.
        /// </summary>
        public static void ReleaseContainer()
        {
            Container.Dispose();
        }
    }
}