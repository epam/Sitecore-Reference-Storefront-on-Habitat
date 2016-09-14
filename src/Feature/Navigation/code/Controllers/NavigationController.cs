using Sitecore.Feature.Base.Controllers;
using Sitecore.Foundation.CommerceServer.Interfaces;
using CommerceTemplates = Sitecore.Foundation.CommerceServer.Templates;

namespace Sitecore.Feature.Navigation.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Feature.Navigation.Repositories;
    using Sitecore.Mvc.Presentation;

    public class NavigationController : CSBaseController
    {
        private readonly INavigationRepository _navigationRepository;

        public NavigationController(IAccountManager accountManager, IContactFactory contactFactory, INavigationRepository navigationRepository)
            : base(accountManager, contactFactory)
        {
            _navigationRepository = navigationRepository;
        }

        public ActionResult NavigationLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }

            var item = RenderingContext.Current.Rendering.Item;
            var items = _navigationRepository.GetLinkMenuItems(item);

            return View("NavigationLinks", items);
        }

        /// <summary>
        /// Childs the category navigation.
        /// </summary>
        /// <returns>The children category navigation view.</returns>
        public ActionResult ChildCategoryNavigation()
        {
            var viewModel = _navigationRepository.GetChildCategoryNavigationBar(Item, CurrentRendering);

            return View("ChildCategoryNavigation", viewModel);
        }

        /// <summary>
        /// Gets the Top bar links.
        /// </summary>
        /// <returns>TopBarLinks view</returns>
        public ActionResult TopBarLinks()
        {
            var renderingModel = new RenderingModel();
            renderingModel.Initialize(RenderingContext.Current.Rendering);

            return View("TopBarLinks", renderingModel);
        }

        /// <summary>
        /// The action for rendering the product list header view
        /// </summary>
        /// <returns>The navigation view.</returns>
        public ActionResult ProductBar()
        {
            var dataSourcePath = Item[CommerceTemplates.CommerceNavigationItem.Fields.CategoryDatasource.ToString()];

            if (string.IsNullOrEmpty(dataSourcePath))
            {
                return View("ProductBar", null);
            }

            var dataSource = Item.Database.GetItem(dataSourcePath);

            if (dataSource == null)
            {
                return View("ProductBar", null);
            }

            var viewModel = _navigationRepository.GetNavigationProductBar(dataSource, CurrentRendering);

            return View("ProductBar", viewModel);
        }

    }
}