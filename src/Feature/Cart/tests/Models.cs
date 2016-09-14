using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Entities.Prices;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Foundation.CommerceServer.Models;

namespace Sitecore.Feature.Cart.Tests
{
    public static class Models
    {
        public static CommerceCart CommerceCartStub { get; private set; }
        public const string TestUserId = "testUserId";
        public const string UserWithEmptyCache = "testEmptyCaheUser";
        public const string TestPromoCode = "testPromoCode";
        public const string HttpRequestUrl = "http://local";
        public static HashSet<ShippingMethod> ShippingMethods;

        static Models()
        {
            RefreshCommerceCart();
            ShippingMethods = new HashSet<ShippingMethod>
            {
                new ShippingMethod(),
                new ShippingMethod()
            };
        }

        public static void RefreshCommerceCart()
        {
            CommerceCartStub = new CommerceCart
            {
                Total = new CommerceTotal
                {
                    Amount = 0.0m,
                    TaxTotal = new TaxTotal
                    {
                        Amount = 0.0m
                    },
                    Subtotal = 0.0m,
                    OrderLevelDiscountAmount = 0.0m,
                    ShippingTotal = 0.0m,
                },

                Lines = new ReadOnlyCollection<CartLine>(new List<CartLine>
                {
                    new CustomCommerceCartLine
                    {
                        Product = new CommerceCartProduct
                        {
                            ProductId = "testProduct",
                            ProductCatalog = "tectCatalog",
                            Price = new Price
                            {
                                Amount = 0.0m
                            }
                        },
                        Total = new CommerceTotal
                        {
                            Amount = 0.0m,
                            LineItemDiscountAmount = 0.0m
                        },
                        ExternalCartLineId = "{testId}"
                    }
                }),

                OrderForms = new ReadOnlyCollection<CommerceOrderForm>(new List<CommerceOrderForm>
                {
                    new CommerceOrderForm
                    {
                        PromoCodes = new ReadOnlyCollection<string>(new List<string>{"test promo"}),
                        CartLines = new ReadOnlyCollection<CommerceCartLine>(new List<CommerceCartLine>
                        {
                            new CommerceCartLine()
                        })
                    }
                }),

                Parties = new ReadOnlyCollection<Party>(new List<Party>
                {
                    new CommerceParty(),
                    new CommerceParty()
                }),

                Payment = new ReadOnlyCollection<PaymentInfo>(new List<PaymentInfo>
                {
                    new CommerceCreditCardPaymentInfo(),
                    new CommerceCreditCardPaymentInfo()
                })
            };
        }

        public static void AddPromoToCommerceCart(string promoCode)
        {
            CommerceCartStub.OrderForms[0].PromoCodes = new ReadOnlyCollection<string>(
                new List<string>
                {
                    "test promo",
                    promoCode
                });
        }

        public static void RemoveCartLineFromCommerceCart()
        {
            CommerceCartStub.OrderForms[0].CartLines = new ReadOnlyCollection<CommerceCartLine>(
                new List<CommerceCartLine>());
            CommerceCartStub.Lines = new ReadOnlyCollection<CartLine>(new List<CartLine>());

        }
    }
}
