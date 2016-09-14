namespace Sitecore.Foundation.Testing.Mocks.Managers
{
    using System.Collections.Generic;
    using Commerce.Entities.Inventory;
    using Commerce.Services.Inventory;
    using CommerceServer.Interfaces;
    using CommerceServer.Models;
    using CommerceServer.Models.InputModels;

    public class MockInventoryManager : IInventoryManager
    {
        private readonly InventoryServiceProvider _inventoryServiceProvider;

        private readonly IContactFactory _contactFactory;

        public MockInventoryManager() { }

        public MockInventoryManager(InventoryServiceProvider inventoryServiceProvider, IContactFactory contactFactory)
        {
            _inventoryServiceProvider = inventoryServiceProvider;
            _contactFactory = contactFactory;
        }

        public void GetProductsStockStatusForList(CommerceStorefront storefront, List<ProductViewModel> productViewModels)
        {
        }

        public void GetProductsStockStatus(CommerceStorefront storefront, List<ProductViewModel> productViewModels)
        {
        }

        public ManagerResponse<GetStockInformationResult, IEnumerable<StockInformation>> GetStockInformation(CommerceStorefront storefront, IEnumerable<InventoryProduct> products, StockDetailsLevel detailsLevel)
        {
            return null;
        }

        public ManagerResponse<GetPreOrderableInformationResult, IEnumerable<OrderableInformation>> GetPreOrderableInformation(CommerceStorefront storefront, IEnumerable<InventoryProduct> products)
        {
            return null;
        }

        public ManagerResponse<GetBackOrderableInformationResult, IEnumerable<OrderableInformation>> GetBackOrderableInformation(CommerceStorefront storefront, IEnumerable<InventoryProduct> products)
        {
            return null;
        }

        public ManagerResponse<VisitedProductStockStatusResult, bool> VisitedProductStockStatus(CommerceStorefront storefront, StockInformation stockInformation, string location)
        {
            return null;
        }

        public ManagerResponse<VisitorSignUpForStockNotificationResult, bool> VisitorSignupForStockNotification(CommerceStorefront storefront, SignUpForNotificationInputModel model, string location)
        {
            return null;
        }
    }
}
