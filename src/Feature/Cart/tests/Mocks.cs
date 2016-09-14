using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using NSubstitute;
using Sitecore.Collections;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Pipelines;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Entities.Customers;
using Sitecore.Commerce.Entities.Payments;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Services.Customers;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Commerce.Services.Payments;
using Sitecore.Commerce.Services.Shipping;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb.Sites;
using Sitecore.Feature.Cart.Utils;
using Sitecore.Foundation.CommerceServer.Interfaces;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Models.InputModels;
using Sitecore.Globalization;
using AddPartiesResult = Sitecore.Commerce.Services.Carts.AddPartiesResult;
using Version = Sitecore.Data.Version;

namespace Sitecore.Feature.Cart.Tests
{
    public class Mocks
    {
        public IAccountManager AccountManager { get; }
        public IContactFactory ContactFactory { get; }

        public ICartManager CartManager { get; }

        public IVisitorContext VisitorContext { get; }

        public ICartCacheService CartCacheService { get; }

        public IOrderManager OrderManager { get; }

        public IPaymentManager PaymentManager { get; }

        public IShippingManager ShippingManager { get; }

        public IProductResolver ProductResolver { get; }

        public Mocks()
        {
            AccountManager = Substitute.For<IAccountManager>();
            AccountManager.ResolveCommerceUser()
                .Returns(new ManagerResponse<GetUserResult, CommerceUser>(new GetUserResult(), new CommerceUser
                {
                    Email = "testMail"
                }));
            AccountManager.GetCurrentCustomerParties(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>())
                .Returns(new ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>>(
                    new GetPartiesResult(),
                    new List<CommerceParty>()));

            ContactFactory = Substitute.For<IContactFactory>();

            CartManager = Substitute.For<ICartManager>();
            CartManager.AddLineItemsToCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<List<AddCartLineInputModel>>())
                    .Returns(new ManagerResponse<CartResult, bool>(new CartResult(), true));

            CartManager.AddPromoCodeToCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<string>())
                    .Returns(
                        new ManagerResponse<AddPromoCodeResult, CommerceCart>(new AddPromoCodeResult(), Models.CommerceCartStub));

            CartManager.RemoveLineItemFromCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<string>())
                    .Returns(
                        new ManagerResponse<CartResult, CommerceCart>(new CartResult(), Models.CommerceCartStub));

            CartManager.GetCurrentCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<bool>())
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(new CartResult
                    {
                        Cart = Models.CommerceCartStub,
                    }, Models.CommerceCartStub));

            CartManager.RemovePromoCodeFromCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<string>())
                    .Returns(new ManagerResponse<RemovePromoCodeResult, CommerceCart>(new RemovePromoCodeResult(), Models.CommerceCartStub));

            CartManager.ChangeLineQuantity(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<UpdateCartLineInputModel>())
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(new CartResult(), Models.CommerceCartStub));


            CartManager.AddLineItemsToCart(
               Arg.Any<CommerceStorefront>(),
               Arg.Any<IVisitorContext>(),
               Arg.Is<List<AddCartLineInputModel>>(x => x.Single() == null))
                    .Returns(new ManagerResponse<CartResult, bool>(new CartResult { Success = false }, false));

            CartManager.AddPromoCodeToCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<string>(x => x == string.Empty))
                    .Returns(new ManagerResponse<AddPromoCodeResult, CommerceCart>(
                        new AddPromoCodeResult { Success = false },
                        Models.CommerceCartStub));

            CartManager.RemoveLineItemFromCart(
               Arg.Any<CommerceStorefront>(),
               Arg.Any<IVisitorContext>(),
               Arg.Is<string>(x => x == null))
                   .Returns(new ManagerResponse<CartResult, CommerceCart>(
                       new CartResult { Success = false },
                       Models.CommerceCartStub));

            CartManager.RemovePromoCodeFromCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<string>(x => x == string.Empty))
                    .Returns(new ManagerResponse<RemovePromoCodeResult, CommerceCart>(
                       new RemovePromoCodeResult { Success = false },
                       Models.CommerceCartStub));

            CartManager.ChangeLineQuantity(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<UpdateCartLineInputModel>(x => x == null))
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(
                        new CartResult { Success = false },
                        Models.CommerceCartStub));

            CartManager.RemovePartiesFromCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<CommerceCart>(),
                Arg.Any<List<Party>>())
                .Returns(new ManagerResponse<CartResult, CommerceCart>(new AddPartiesResult(), new CommerceCart
                {
                    Parties = new ReadOnlyCollection<Party>(new List<Party>())
                }));

            CartManager.RemoveAllPaymentMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<CommerceCart>())
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(new AddPaymentInfoResult(), new CommerceCart
                    {
                        Payment = new ReadOnlyCollection<PaymentInfo>(new List<PaymentInfo>())
                    }));

            CartManager.RemoveAllShippingMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<CommerceCart>())
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(
                        new AddPartiesResult(),
                        new CommerceCart
                        {
                            Shipping = new ReadOnlyCollection<ShippingInfo>(new List<ShippingInfo>())
                        }));

            CartManager.SetPaymentMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<PaymentInputModel>(x => x != null))
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(
                        new AddPaymentInfoResult(),
                        Models.CommerceCartStub));

            CartManager.SetPaymentMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<PaymentInputModel>(x => x == null))
                    .Returns(new ManagerResponse<CartResult, CommerceCart>(
                        new AddPaymentInfoResult { Success = false },
                        Models.CommerceCartStub));

            CartManager.SetShippingMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<SetShippingMethodsInputModel>(x => x != null))
                    .Returns(new ManagerResponse<AddShippingInfoResult, CommerceCart>(
                        new AddShippingInfoResult(),
                        Models.CommerceCartStub));

            CartManager.SetShippingMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<SetShippingMethodsInputModel>(x => x == null))
                    .Returns(new ManagerResponse<AddShippingInfoResult, CommerceCart>(
                        new AddShippingInfoResult { Success = false },
                        Models.CommerceCartStub));

            VisitorContext = Substitute.For<IVisitorContext>();
            VisitorContext.GetCustomerId().Returns(Models.TestUserId);

            CommerceCart nullCart = null;
            CartCacheService = Substitute.For<ICartCacheService>();
            CartCacheService.GetCart(Arg.Any<string>()).Returns(Models.CommerceCartStub);
            CartCacheService.GetCart(Arg.Is<string>(x => x.Equals(Models.UserWithEmptyCache))).Returns(nullCart);

            OrderManager = Substitute.For<IOrderManager>();
            OrderManager.GetAvailableRegions(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<string>())
                    .Returns(
                        new ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>>(
                            new GetAvailableRegionsResult(), new Dictionary<string, string>
                            {
                                 {"test", "test"}
                            }));

            OrderManager.GetOrderDetails(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<string>())
                .Returns(new ManagerResponse<GetVisitorOrderResult, CommerceOrder>(new GetVisitorOrderResult(),
                    new CommerceOrder()));

            OrderManager.GetAvailableRegions(
               Arg.Any<CommerceStorefront>(),
               Arg.Any<IVisitorContext>(),
               Arg.Is<string>(x => x == null))
                   .Returns(
                       new ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>>(
                           new GetAvailableRegionsResult { Success = false }, new Dictionary<string, string>()));
            OrderManager.GetAvailableCountries()
                .Returns(new ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>>(
                    new GetAvailableCountriesResult(),
                    new Dictionary<string, string>()));

            OrderManager.SubmitVisitorOrder(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<SubmitOrderInputModel>(x => x != null))
                    .Returns(new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(
                        new SubmitVisitorOrderResult(),
                        new CommerceOrder()));

            OrderManager.SubmitVisitorOrder(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Is<SubmitOrderInputModel>(x => x == null))
                    .Returns(new ManagerResponse<SubmitVisitorOrderResult, CommerceOrder>(
                        new SubmitVisitorOrderResult { Success = false },
                        new CommerceOrder()));

            ShippingManager = Substitute.For<IShippingManager>();
            ShippingManager.GetShippingPreferences(
                Arg.Any<CommerceCart>())
                    .Returns(new ManagerResponse<GetShippingOptionsResult, List<ShippingOption>>(
                        new GetShippingOptionsResult
                        {
                            ShippingOptions = new ReadOnlyCollection<ShippingOption>(new List<ShippingOption>()),
                            LineShippingPreferences = new ReadOnlyCollection<LineShippingOption>(new List<LineShippingOption>())
                        },
                        new List<ShippingOption>()));

            ShippingManager.GetShippingMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<ShippingOption>())
                    .Returns(new ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>>(
                        new GetShippingMethodsResult(),
                        new HashSet<ShippingMethod>()));

            ShippingManager.GetShippingMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<GetShippingMethodsInputModel>())
                    .Returns(new ManagerResponse<GetShippingMethodsResult, IReadOnlyCollection<ShippingMethod>>(
                        new GetShippingMethodsResult
                        {
                            ShippingMethods = new ReadOnlyCollection<ShippingMethod>(Models.ShippingMethods.ToList())
                        },
                        Models.ShippingMethods));

            PaymentManager = Substitute.For<IPaymentManager>();
            PaymentManager.GetPaymentOptions(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>())
                    .Returns(new ManagerResponse<GetPaymentOptionsResult, IEnumerable<PaymentOption>>(
                        new GetPaymentOptionsResult(),
                        new List<PaymentOption>()));

            PaymentManager.GetPaymentMethods(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<PaymentOption>())
                    .Returns(
                        new ManagerResponse<GetPaymentMethodsResult, IEnumerable<PaymentMethod>>(
                            new GetPaymentMethodsResult(),
                            new List<PaymentMethod>()));

            ProductResolver = Substitute.For<IProductResolver>();
            ProductResolver.ResolveCatalogItem(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<bool>())
                .Returns(new Item(ID.NewID,
                    new ItemData(ItemDefinition.Empty, Language.Invariant, Version.First, new FieldList()),
                    Database.GetDatabase("master")));
        }

        public void MockCartManagerGetCurrentCartToFalseSuccess()
        {
            var response = new ManagerResponse<CartResult, CommerceCart>(
                new CartResult { Success = false },
                Models.CommerceCartStub);
            CartManager.GetCurrentCart(
                Arg.Any<CommerceStorefront>(),
                Arg.Any<IVisitorContext>(),
                Arg.Any<bool>())
                    .Returns(response);
        }

        public void MockContexts(Database db)
        {
            var fakeSiteContext = new FakeSiteContext(new StringDictionary
            {
                {"rootPath", "/sitecore/Content/Habitat" },
                {"startItem", "/Home" }
            });
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, Models.HttpRequestUrl, null),
                new HttpResponse(null));
            fakeSiteContext.Database = db;
            Context.Site = fakeSiteContext;
            Context.Items["__visitorContext"] = VisitorContext;
            Context.Database = db;
        }
    }
}