using System.Collections.Generic;
using Sitecore.Commerce.Connect.CommerceServer.Catalog.Fields;
using Sitecore.Commerce.Services.Catalog;
using Sitecore.Data.Items;
using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Models.SitecoreItemModels;
using Sitecore.Foundation.CommerceServer.Search.Models;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface ICatalogManager
    {
        /// <summary>
        /// Gets or sets the inventory manager.
        /// </summary>
        /// <value>
        /// The inventory manager.
        /// </value>
        IInventoryManager InventoryManager { get; }

        SearchResults GetChildProducts(CommerceSearchOptions searchOptions, Item categoryItem);

        /// <summary>
        /// The <see cref="RelatedCatalogItemsViewModel" /> representing the related catalog items.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="catalogItem">The catalog item.</param>
        /// <param name="rendering">The target renering.</param>
        /// <returns>
        /// The related catalog item view model.
        /// </returns>
        RelatedCatalogItemsViewModel GetRelationshipsFromItem([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, Item catalogItem, Rendering rendering);

        /// <summary>
        /// Gets a lists of target items from a relationship field
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="field">the relationship field</param>
        /// <param name="rendering">The target renering.</param>
        /// <returns>
        /// a list of relationship targets or null if no items found
        /// </returns>
        RelatedCatalogItemsViewModel GetRelationshipsFromField([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, RelationshipField field, Rendering rendering);

        /// <summary>
        /// Gets a lists of target items from a relationship field
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="field">the relationship field</param>
        /// <param name="rendering">The target renering.</param>
        /// <param name="relationshipNames">the names of the relationships, to retrieve (for example upsell).</param>
        /// <returns>
        /// a list of relationship targets or null if no items found
        /// </returns>
        RelatedCatalogItemsViewModel GetRelationshipsFromField([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, RelationshipField field, Rendering rendering, IEnumerable<string> relationshipNames);

        /// <summary>
        /// Registers an event specifying that the category page has been visited.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryName">The category name.</param>
        /// <returns>
        /// A <see cref="CatalogResult" /> specifying the result of the service request.
        /// </returns>
        CatalogResult VisitedCategoryPage([NotNull] CommerceStorefront storefront, [NotNull] string categoryId, string categoryName);

        /// <summary>
        /// Registers an event specifying that the product details page has been visited.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="productId">the product id.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="parentCategoryId">The parent category identifier.</param>
        /// <param name="parentCategoryName">the parent category name.</param>
        /// <returns>
        /// A <see cref="CatalogResult" /> specifying the result of the service request.
        /// </returns>
        CatalogResult VisitedProductDetailsPage([NotNull] CommerceStorefront storefront, [NotNull] string productId, string productName, string parentCategoryId, string parentCategoryName);

        /// <summary>
        /// This method returns the ProductSearchResults for a datasource
        /// </summary>
        /// <param name="dataSource">The datasource to perform the search with</param>
        /// <param name="productSearchOptions">The search options.</param>
        /// <returns>A ProductSearchResults</returns>     
        SearchResults GetProductSearchResults(Item dataSource, CommerceSearchOptions productSearchOptions);
        
        /// <summary>
        /// This method returns the current category by URL
        /// </summary>
        /// <returns>The category.</returns>
        Category GetCurrentCategoryByUrl();

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The category.</returns>
        Category GetCategory(string categoryId);

        /// <summary>
        /// Get category by item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The catgory.</returns>
        Category GetCategory(Item item);

        /// <summary>
        /// Gets the product price.
        /// </summary>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="productViewModel">The product view model.</param>
        void GetProductPrice([NotNull] IVisitorContext visitorContext, ProductViewModel productViewModel);

        /// <summary>
        /// Gets the product price.
        /// </summary>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="productViewModels">The product view models.</param>
        void GetProductBulkPrices([NotNull] IVisitorContext visitorContext, List<ProductViewModel> productViewModels);

        /// <summary>
        /// Gets the product rating.
        /// </summary>
        /// <param name="productItem">The product item.</param>
        /// <returns>The product rating</returns>
        decimal GetProductRating(Item productItem);

        /// <summary>
        /// Visiteds the product details page.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <returns>
        /// The manager response
        /// </returns>
        ManagerResponse<CatalogResult, bool> VisitedProductDetailsPage([NotNull] CommerceStorefront storefront);

        /// <summary>
        /// Facets the applied.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="facet">The facet.</param>
        /// <param name="isApplied">if set to <c>true</c> [is applied].</param>
        /// <returns>The manager response.</returns>
        ManagerResponse<CatalogResult, bool> FacetApplied([NotNull] CommerceStorefront storefront, string facet, bool isApplied);

        /// <summary>
        /// Sorts the order applied.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The manager response.</returns>
        ManagerResponse<CatalogResult, bool> SortOrderApplied([NotNull] CommerceStorefront storefront, string sortKey, CommerceConstants.SortDirection? sortDirection);

        /// <summary>
        /// Registers the search event.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="searchKeyword">The search keyword.</param>
        /// <param name="numberOfHits">The number of hits.</param>
        /// <returns>
        /// The manager response
        /// </returns>
        ManagerResponse<CatalogResult, bool> RegisterSearchEvent([NotNull] CommerceStorefront storefront, string searchKeyword, int numberOfHits);
    }
}