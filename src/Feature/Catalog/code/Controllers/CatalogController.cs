namespace Sitecore.Feature.Catalog.Controllers
{
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Models.InputModels;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Base.Filters;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Web.Mvc;
    using System.Web.UI;
    using CommerceConstants = Sitecore.Foundation.CommerceServer.Infrastructure.Constants.CommerceConstants;

    /// <summary>
    /// Used to manage the data and view retrieval for catalog pages
    /// </summary>
    public class CatalogController : CSBaseController
    {
        #region Variables

        /// <summary>
        /// The commerce named search template
        /// </summary>
        public static ID CommerceNamedSearchTemplate = new ID("{9F7D719A-3A05-4A64-AA74-3C46D8D0D20D}");
        
        private readonly ILogger _logger;
        private readonly Repositories.ICatalogRepository _catalogRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogController" /> class.
        /// </summary>
        /// <param name="contactFactory">The contact factory.</param>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="catalogRepository">The catalog repository.</param> 
        /// <param name="logger">The logger.</param> 
        public CatalogController(
            [NotNull] IContactFactory contactFactory,
            [NotNull] IAccountManager accountManager,
            [NotNull] Repositories.ICatalogRepository catalogRepository,
            [NotNull] ILogger logger)
            : base(accountManager, contactFactory)
        {
            _catalogRepository = catalogRepository;
            _logger = logger;
        }

        #endregion

        #region Controller actions

        /// <summary>
        /// Currencies the menu.
        /// </summary>
        /// <returns>The currency menu.</returns>
        public ActionResult CurrencyMenu()
        {
            var currencyMenuModel = _catalogRepository.GetCurrencyMenu(CurrentRendering);

            return View("CurrencyMenu", currencyMenuModel);
        }

        /// <summary>
        /// Switches the currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>The Json result.</returns>
        [HttpPost]
        public JsonResult SwitchCurrency(string currency)
        {
            try
            {
                var json = _catalogRepository.SwitchCurrency(currency);

                if (json.Success == false)
                {
                    return json;
                }
            }
            catch (Exception e)
            {
                return Json(new BaseJsonResult("SwitchCurrency", e), JsonRequestBehavior.AllowGet);
            }

            return new JsonResult();
        }

        /// <summary>
        /// An action to manage data for the ProductList
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>
        /// The view that represents the ProductList
        /// </returns>
        public ActionResult MultipleProductLists(
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var productSearchOptions = new CommerceSearchOptions
            {
                NumberOfItemsToReturn = StorefrontConstants.Settings.DefaultItemsPerPage,
                StartPageIndex = 0,
                SortField = sortField
            };

            var currentRendering = RenderingContext.Current.Rendering;
            var datasource = currentRendering.Item;
            var viewModel = _catalogRepository.GetMultipleProductList(datasource, currentRendering, productSearchOptions);

            return View("ProductRecommendation", viewModel);
        }

        /// <summary>
        /// The action for rendering the category view
        /// </summary>
        /// <param name="pageNumber">The product page number</param>
        /// <param name="facetValues">A facet query string</param>
        /// <param name="sortField">The field to sort on</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="sortDirection">The direction to sort</param>
        /// <returns>The category view</returns>
        public ActionResult ProductList(
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var viewModel = _catalogRepository.GetProductList(CurrentRendering, pageNumber, facetValues, sortField, pageSize, sortDirection);

            return View("ProductList", viewModel);
        }

        /// <summary>
        /// The action for rendering the product list header view
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The product list header view.</returns>
        public ActionResult ProductListHeader(
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)] CommerceConstants.SortDirection? sortDirection)
        {
            var viewModel = _catalogRepository.GetProductListHeader(CurrentRendering, pageNumber, facetValues, sortField, pageSize, sortDirection);

            return View("ProductListHeader", viewModel);
        }

        /// <summary>
        /// The action for rendering the pagination view
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <returns>The pagination view.</returns>
        public ActionResult Pagination(
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues)
        {
            var viewModel = _catalogRepository.GetPagination(CurrentRendering, pageNumber, pageSize, facetValues);

            return View("Pagination", viewModel);
        }

        /// <summary>
        /// The action for rendering the product facets view
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="facetValues">The facet values.</param>
        /// <param name="sortField">The sort field.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The product facet view.</returns>
        public ActionResult ProductFacets(
            [Bind(Prefix = StorefrontConstants.QueryStrings.Paging)] int? pageNumber,
            [Bind(Prefix = StorefrontConstants.QueryStrings.PageSize)] int? pageSize,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Facets)] string facetValues,
            [Bind(Prefix = StorefrontConstants.QueryStrings.Sort)] string sortField,
            [Bind(Prefix = StorefrontConstants.QueryStrings.SortDirection)]
            CommerceConstants.SortDirection? sortDirection)
        {
            var viewModel = _catalogRepository.GetFacets(CurrentRendering, pageNumber, facetValues, sortField, pageSize, sortDirection);

            return View("ProductFacets", viewModel);
        }

        /// <summary>
        /// Facets the applied.
        /// </summary>
        /// <param name="facetValue">The facet value.</param>
        /// <param name="isApplied">The is applied.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [HttpPost]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult FacetApplied(string facetValue, bool? isApplied)
        {
            _catalogRepository.ApplyFacet(facetValue, isApplied);

            return new BaseJsonResult();
        }

        /// <summary>
        /// Sorts the order applied.
        /// </summary>
        /// <param name="sortField">The sort field.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [HttpPost]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult SortOrderApplied(string sortField, CommerceConstants.SortDirection? sortDirection)
        {
            _catalogRepository.ApplySortOrder(sortField, sortDirection);

            return new BaseJsonResult();
        }

        public ActionResult ProductImages()
        {
            var productViewModel = _catalogRepository.GetWildCardProductViewModel(Item, CurrentRendering);

            return View("ProductImages", productViewModel);
        }

        public ActionResult ProductRating()
        {
            var productViewModel = _catalogRepository.GetWildCardProductViewModel(Item, CurrentRendering);

            return View("ProductRating", productViewModel);
        }

        public ActionResult ProductInformation()
        {
            var productViewModel = _catalogRepository.GetWildCardProductViewModel(Item, CurrentRendering);

            return View("ProductInformation", productViewModel);
        }

        /// <summary>
        /// Gets the Product presentation.
        /// </summary>
        /// <returns>ProductPresentation view</returns>
        [HttpGet]
        public ActionResult ProductPresentation()
        {
            return View("ProductPresentation");
        }

        /// <summary>
        /// Gets the current product stock count.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A json result
        /// </returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetCurrentProductStockInfo(ProductStockInfoInputModel model)
        {
            try
            {
                Assert.ArgumentNotNull(model, "model");

                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _catalogRepository.GetCurrentProductStockInfo(model.ProductId);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetCurrentProductStockInfo", this, e);
                return Json(new BaseJsonResult("GetCurrentProductStockInfo", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// The action for rendering the related catalog items.
        /// </summary>
        /// <returns>The related catalog items view</returns>
        public ActionResult RelatedCatalogItems()
        {
            var relatedCatalogItemsModel = _catalogRepository.GetRelatedCatalogItems(this.Item, this.CurrentRendering);

            return View("RelatedCatalogItems", relatedCatalogItemsModel);
        }

        #endregion
    }
}