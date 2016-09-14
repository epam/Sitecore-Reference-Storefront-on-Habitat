using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Entities.Payments;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Feature.Cart.Models.InputModels;
using Sitecore.Feature.Cart.Models.JsonResults;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models.InputModels;
using Sitecore.Security.Accounts;

namespace Sitecore.Feature.Cart.Repositories
{
    using Sitecore.Feature.Base.Repositories;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    public class CheckoutRepository : BaseRepository, ICheckoutRepository
    {
        private readonly ICartManager _cartManager;
        private readonly IOrderManager _orderManager;
        private readonly IPaymentManager _paymentManager;
        private readonly IShippingManager _shippingManager;
        private readonly IProductResolver _productResolver;

        public CheckoutRepository(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            ICartManager cartManager,
            IOrderManager orderManager,
            IPaymentManager paymentManager,
            IShippingManager shippingManager,
            IProductResolver productResolver)
            : base(accountManager, contactFactory)
        {
            this._cartManager = cartManager;
            this._orderManager = orderManager;
            this._paymentManager = paymentManager;
            this._shippingManager = shippingManager;
            _productResolver = productResolver;
        }

        public AvailableStatesBaseJsonResult GetAvailableStates(GetAvailableStatesInputModel model)
        {
            var response = this._orderManager.GetAvailableRegions(CurrentStorefront, CurrentVisitorContext, model.CountryCode);
            var result = new AvailableStatesBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result);
            }

            return result;
        }

        public CheckoutDataBaseJsonResult GetCheckoutData(User user)
        {
            var result = new CheckoutDataBaseJsonResult();
            var response = this._cartManager.GetCurrentCart(CurrentStorefront, CurrentVisitorContext, true);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                var cart = (CommerceCart)response.ServiceProviderResult.Cart;

                if (cart.Lines != null && cart.Lines.Any())
                {
                    result.Cart = new CSCartBaseJsonResult(response.ServiceProviderResult);
                    result.Cart.Initialize(response.ServiceProviderResult.Cart, _productResolver);
                    result.ShippingMethods = new List<ShippingMethod>();
                    result.CartLoyaltyCardNumber = cart.LoyaltyCardID;
                    result.CurrencyCode = StorefrontManager.GetCustomerCurrency();
                    this.AddShippingOptionsToResult(result, cart);

                    if (result.Success)
                    {
                        this.AddShippingMethodsToResult(result);

                        if (result.Success)
                        {
                            this.GetAvailableCountries(result);

                            if (result.Success)
                            {
                                this.GetPaymentOptions(result);

                                if (result.Success)
                                {
                                    this.GetPaymentMethods(result);

                                    if (result.Success)
                                    {
                                        this.GetUserInfo(result, user);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            result.SetErrors(response.ServiceProviderResult);

            return result;
        }

        public CommerceCart GetCommerceCart()
        {
            var response = this._cartManager.GetCurrentCart(this.CurrentStorefront, this.CurrentVisitorContext, true);
            var cart = (CommerceCart)response.ServiceProviderResult.Cart;

            if (cart.Lines == null || !cart.Lines.Any())
            {
                return cart;
            }

            cart = this.CheckForPartyInfoOnCart(cart);
            cart = this.CheckForPaymentInCart(cart);
            cart = this.CheckForShipmentInCart(cart);

            return cart;
        }

        public CommerceOrder GetCommerceOrder(string confirmationId)
        {
            CommerceOrder order = null;

            if (!string.IsNullOrWhiteSpace(confirmationId))
            {
                var response = this._orderManager.GetOrderDetails(this.CurrentStorefront, this.CurrentVisitorContext, confirmationId);

                if (response.ServiceProviderResult.Success)
                {
                    order = response.Result;
                }
            }

            return order;
        }

        public ShippingMethodsBaseJsonResult GetShippingMethods(GetShippingMethodsInputModel inputModel)
        {
            var response = this._shippingManager.GetShippingMethods(CurrentStorefront, CurrentVisitorContext, inputModel);
            var result = new ShippingMethodsJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.ServiceProviderResult.ShippingMethods, response.ServiceProviderResult.ShippingMethodsPerItem);
            }

            return result;
        }

        public CSCartBaseJsonResult SetPaymentMethods(PaymentInputModel inputModel)
        {
            var response = this._cartManager.SetPaymentMethods(this.CurrentStorefront, this.CurrentVisitorContext, inputModel);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);
            }

            return result;
        }

        public CSCartBaseJsonResult SetShippingMethods(SetShippingMethodsInputModel inputModel)
        {
            var response = this._cartManager.SetShippingMethods(CurrentStorefront, CurrentVisitorContext, inputModel);
            var result = new CSCartBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result, _productResolver);
            }

            return result;
        }

        public SubmitOrderBaseJsonResult SubmitOrder(SubmitOrderInputModel inputModel)
        {

            var response = this._orderManager.SubmitVisitorOrder(CurrentStorefront, CurrentVisitorContext, inputModel);
            var result = new SubmitOrderBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null && response.ServiceProviderResult.CartWithErrors == null)
            {
                result.Initialize(string.Concat(StorefrontManager.StorefrontUri("checkout/OrderConfirmation"), "?confirmationId=", (response.Result.TrackingNumber)));
            }

            return result;
        }

        private void AddShippingMethodsToResult(Models.JsonResults.CheckoutDataBaseJsonResult result)
        {
            var response = this._shippingManager.GetShippingMethods(this.CurrentStorefront, this.CurrentVisitorContext, new ShippingOption { ShippingOptionType = ShippingOptionType.ElectronicDelivery });

            if (response.ServiceProviderResult.Success && response.Result.Count > 0)
            {
                result.EmailDeliveryMethod = response.Result.ElementAt(0);

                return;
            }

            result.EmailDeliveryMethod = new ShippingMethod();
            result.ShipToStoreDeliveryMethod = new ShippingMethod();
            result.SetErrors(response.ServiceProviderResult);
        }

        private void AddShippingOptionsToResult(CheckoutDataBaseJsonResult result, CommerceCart cart)
        {
            var response = this._shippingManager.GetShippingPreferences(cart);
            var orderShippingOptions = new List<ShippingOption>();
            var lineShippingOptions = new List<LineShippingOption>();

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                orderShippingOptions = response.ServiceProviderResult.ShippingOptions.ToList();
                lineShippingOptions = response.ServiceProviderResult.LineShippingPreferences.ToList();
            }

            result.OrderShippingOptions = orderShippingOptions;
            result.LineShippingOptions = lineShippingOptions;

            if (result.LineShippingOptions != null && result.LineShippingOptions.Any())
            {
                foreach (var line in result.Cart.Lines)
                {
                    var lineShippingOption = result.LineShippingOptions.FirstOrDefault(l => l.LineId.Equals(line.ExternalCartLineId, StringComparison.OrdinalIgnoreCase));

                    if (lineShippingOption != null)
                    {
                        line.ShippingOptions = lineShippingOption.ShippingOptions;
                    }
                }
            }

            result.SetErrors(response.ServiceProviderResult);
        }

        private CommerceCart CheckForPartyInfoOnCart(CommerceCart cart)
        {
            if (cart.Parties.Any())
            {
                var response = this._cartManager.RemovePartiesFromCart(this.CurrentStorefront, this.CurrentVisitorContext, cart, cart.Parties.ToList());

                if (response.ServiceProviderResult.Success)
                {
                    cart = response.Result;
                }
            }

            return cart;
        }

        private CommerceCart CheckForPaymentInCart(CommerceCart cart)
        {
            if (cart.Payment != null && cart.Payment.Any())
            {
                var response = this._cartManager.RemoveAllPaymentMethods(this.CurrentStorefront, this.CurrentVisitorContext, cart);

                if (response.ServiceProviderResult.Success)
                {
                    cart = response.Result;
                }
            }

            return cart;
        }

        private CommerceCart CheckForShipmentInCart(CommerceCart cart)
        {
            if (cart.Shipping != null && cart.Shipping.Any())
            {
                var response = this._cartManager.RemoveAllShippingMethods(this.CurrentStorefront, this.CurrentVisitorContext, cart);

                if (response.ServiceProviderResult.Success)
                {
                    cart = response.Result;
                }
            }

            return cart;
        }

        private void GetAvailableCountries(CheckoutDataBaseJsonResult result)
        {
            var response = this._orderManager.GetAvailableCountries();
            var countries = new Dictionary<string, string>();

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                countries = response.Result;
            }

            result.Countries = countries;
            result.SetErrors(response.ServiceProviderResult);
        }
        private void GetPaymentMethods(CheckoutDataBaseJsonResult result)
        {
            List<PaymentMethod> paymentMethodList = new List<PaymentMethod>();

            var response = this._paymentManager.GetPaymentMethods(this.CurrentStorefront, this.CurrentVisitorContext, new PaymentOption { PaymentOptionType = PaymentOptionType.PayCard });

            if (response.ServiceProviderResult.Success)
            {
                paymentMethodList.AddRange(response.Result);
            }

            result.SetErrors(response.ServiceProviderResult);
            result.PaymentMethods = paymentMethodList;
        }

        private void GetPaymentOptions(CheckoutDataBaseJsonResult result)
        {
            var response = this._paymentManager.GetPaymentOptions(this.CurrentStorefront, this.CurrentVisitorContext);
            var paymentOptions = new List<PaymentOption>();

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                paymentOptions = response.Result.ToList();
            }

            result.PaymentOptions = paymentOptions;
            result.SetErrors(response.ServiceProviderResult);
        }

        private void GetUserInfo(CheckoutDataBaseJsonResult result, User user)
        {
            var isUserAuthenticated = user.IsAuthenticated;
            result.IsUserAuthenticated = isUserAuthenticated;
            result.UserEmail = isUserAuthenticated && !user.Profile.IsAdministrator ? this.AccountManager.ResolveCommerceUser().Result.Email : string.Empty;

            if (!isUserAuthenticated)
            {
                return;
            }

            var addresses = new List<CommerceParty>();
            var response = this.AccountManager.GetCurrentCustomerParties(this.CurrentStorefront, this.CurrentVisitorContext);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                addresses = response.Result.ToList();
            }

            var addressesResult = new AddressListItemJsonResult();
            addressesResult.Initialize(addresses, null);
            result.UserAddresses = addressesResult;
            result.SetErrors(response.ServiceProviderResult);
        }
    }
}