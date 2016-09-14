using System.Collections.Generic;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.GiftCard.Repositories
{
    using System;
    using System.Linq;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Base.Repositories;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;

    public class GiftCardRepository : BaseRepository, IGiftCardRepository
    {
        private const string CurrentProductViewModelKeyName = "CurrentProductViewModel";

        public GiftCardRepository(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            ICatalogManager catalogManager)
            : base(accountManager, contactFactory)
        {
            this.CatalogManager = catalogManager;
        }

        public ICatalogManager CatalogManager { get; set; }

        public ProductViewModel GetGiftCardViewModel(Item productItem, Rendering currentRendering)
        {
            if (this.CurrentSiteContext.Items[CurrentProductViewModelKeyName] != null)
            {
                return (ProductViewModel)this.CurrentSiteContext.Items[CurrentProductViewModelKeyName];
            }

            var variants = new List<VariantViewModel>();

            if (productItem != null && productItem.HasChildren)
            {
                foreach (Item item in productItem.Children)
                {
                    var v = new VariantViewModel(item);
                    variants.Add(v);
                }
            }

            var productViewModel = new ProductViewModel(productItem);
            productViewModel.Initialize(currentRendering, variants);
            productViewModel.ProductName = productViewModel.DisplayName;

            if (this.CurrentSiteContext.UrlContainsCategory)
            {
                productViewModel.ParentCategoryId = CatalogUrlManager.ExtractCategoryNameFromCurrentUrl();

                var category = this.CatalogManager.GetCategory(productViewModel.ParentCategoryId);
                if (category != null)
                {
                    productViewModel.ParentCategoryName = category.DisplayName;
                }
            }

            //Special handling for gift card
            if (productViewModel.ProductId == StorefrontManager.CurrentStorefront.GiftCardProductId)
            {
                productViewModel.GiftCardAmountOptions = GetGiftCardAmountOptions(productViewModel);
            }
            else
            {
                this.CatalogManager.GetProductPrice(this.CurrentVisitorContext, productViewModel);
                productViewModel.CustomerAverageRating = this.CatalogManager.GetProductRating(productItem);
            }

            this.CurrentSiteContext.Items[CurrentProductViewModelKeyName] = productViewModel;

            return productViewModel;
        }

        private List<KeyValuePair<string, decimal?>> GetGiftCardAmountOptions(ProductViewModel productViewModel)
        {
            var giftCardAmountOptions = new Dictionary<string, decimal?>();

            if (productViewModel != null && productViewModel.Variants != null)
            {
                this.CatalogManager.GetProductPrice(this.CurrentVisitorContext, productViewModel);
                foreach (var variant in productViewModel.Variants)
                {
                    giftCardAmountOptions.Add(variant.VariantId, Math.Round(variant.AdjustedPrice.Value, 2));
                }

                return giftCardAmountOptions.ToList();
            }

            return null;
        }
    }
}
