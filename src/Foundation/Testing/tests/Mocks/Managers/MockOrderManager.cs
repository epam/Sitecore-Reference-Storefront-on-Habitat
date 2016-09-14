namespace Sitecore.Foundation.Testing.Mocks.Managers
{
    using System;
    using System.Collections.Generic;
    using Ploeh.AutoFixture;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Orders;
    using Sitecore.Commerce.Services.Orders;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public class MockOrderManager : IOrderManager
    {
        private readonly OrderServiceProvider _orderServiceProvider;
        private readonly ICartManager _cartManager;

        public bool GetOrdersLastModifyDateIsTooOld { get; set; }
        public bool IsAvailableCountriesInitialized { get; set; }
        public int GetOrdersDaysOld { get; set; }

        public MockOrderManager() { }

        public MockOrderManager(OrderServiceProvider orderServiceProvider, ICartManager cartManager)
        {
            _orderServiceProvider = orderServiceProvider;
            _cartManager = cartManager;
        }

        public ManagerResponse<SubmitVisitorOrderResult, CommerceOrder> SubmitVisitorOrder(CommerceStorefront storefront, IVisitorContext visitorContext, SubmitOrderInputModel inputModel)
        {
            return null;
        }

        public ManagerResponse<GetAvailableRegionsResult, Dictionary<string, string>> GetAvailableRegions(CommerceStorefront storefront, IVisitorContext visitorContext, string countryCode)
        {
            return null;
        }

        public ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>> GetOrders(string customerId, string shopName)
        {
            if (!customerId.Equals("fake")) return new ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>>
                     (new GetVisitorOrdersResult() { Success = false }, null);

            var fixture = new Fixture();
            fixture.RepeatCount = 10;
            var orders = fixture.Create<List<CommerceOrderHeader>>();

            if (GetOrdersLastModifyDateIsTooOld)
            {
                orders.ForEach(r => r.LastModified = DateTime.Now.AddDays(GetOrdersDaysOld - 1));
            }
            else
            {
                orders.ForEach(r => r.LastModified = DateTime.Now.AddDays(-1));
            }

            return new ManagerResponse<GetVisitorOrdersResult, IEnumerable<OrderHeader>>(new GetVisitorOrdersResult(), orders);
        }

        public ManagerResponse<GetVisitorOrderResult, CommerceOrder> GetOrderDetails(CommerceStorefront storefront, IVisitorContext visitorContext, string orderId)
        {
            var order = new CommerceOrder
            {
                LastModified = DateTime.Now
            };

            return new ManagerResponse<GetVisitorOrderResult, CommerceOrder>(new GetVisitorOrderResult(), order);
        }

        public ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>> GetAvailableCountries()
        {
            if (IsAvailableCountriesInitialized == false)
                return new ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>>(new GetAvailableCountriesResult { Success = false }, null);

            var fixture = new Fixture();
            var countries = fixture.Create<Dictionary<string, string>>();

            return new ManagerResponse<GetAvailableCountriesResult, Dictionary<string, string>>(new GetAvailableCountriesResult { Success = true }, countries);
        }
    }
}
