namespace Sitecore.Feature.NavigationSearch.Controllers
{
    using Sitecore.Commerce.Connect.CommerceServer.Search.Models;
    using Sitecore.Commerce.Contacts;
    using Sitecore.Mvc.Presentation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Foundation.CommerceServer.Models.SitecoreItemModels;
    using Foundation.CommerceServer.Helpers;    /// <summary>
                                                /// Used to handle all search naviagtion actions
                                                /// </summary>
    public class NavigationSearchController : BaseController
    {
        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationSearchController"/> class.
        /// </summary>
        /// <param name="contactFactory">The contact factory.</param>
        public NavigationSearchController([NotNull] ContactFactory contactFactory)
            : base(contactFactory)
        {
        }

        /// <summary>
        /// Gets or sets the name of the index
        /// </summary>
        public string IndexName { get; set; }

        #endregion

        #region Controller actions

        /// <summary>
        /// Represents the index action
        /// </summary>
        /// <returns>An action result view</returns>
        [HttpGet]
        [AllowAnonymous]
        public override ActionResult Index()
        {
            var dataSourceQuery = RenderingContext.Current.Rendering.DataSource;

            var navigationCategories = new List<Category>();
            if (!string.IsNullOrWhiteSpace(dataSourceQuery))
            {
                var response = SearchNavigation.GetNavigationCategories(dataSourceQuery, new CommerceSearchOptions());
                var navigationResults = response.ResponseItems;
                navigationCategories.AddRange(navigationResults.Select(result => new Category(result)));
            }

            return View(navigationCategories);
        }

        #endregion
    }
}
