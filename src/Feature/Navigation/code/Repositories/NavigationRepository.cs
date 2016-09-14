namespace Sitecore.Feature.Navigation.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Navigation.Models;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Helpers;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.SitecoreItemModels;
    using Sitecore.Foundation.CommerceServer.Search.Models;
    using Sitecore.Foundation.CommerceServer.Utils;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Presentation;

    public class NavigationRepository : INavigationRepository
    {
        private ISiteContext _siteContext;
        private ICatalogManager _catalogManager;

        public NavigationRepository(ICatalogManager catalogManager)
        {
            _catalogManager = catalogManager;
        }

        private ISiteContext CurrentSiteContext
        {
            get
            {
                return _siteContext ?? (_siteContext = ContextTypeLoader.CreateInstance<ISiteContext>());
            }
        }

        public NavigationViewModel GetNavigationProductBar(Item dataSource, Rendering currentRendering)
        {
            Category currentCategory = _catalogManager.GetCategory(dataSource);

            if (currentCategory == null)
            {
                return null;
            }

            var viewModel = GetNavigationViewModel(currentCategory.InnerItem, dataSource, currentRendering);

            return viewModel;
        }

        public NavigationViewModel GetChildCategoryNavigationBar(Item datasource, Rendering currentRendering)
        {

            Item categoryItem = this.CurrentSiteContext.CurrentCatalogItem;
            Assert.IsTrue(categoryItem.ItemType() == StorefrontConstants.ItemTypes.Category, "Current item must be a Category.");

            var viewModel = GetNavigationViewModel(categoryItem, datasource, currentRendering);
            if (viewModel.ChildCategories.Count == 0)
            {
                viewModel = GetNavigationViewModel(categoryItem.Parent, datasource, currentRendering);
            }

            return viewModel;
        }

        public NavigationItems GetLinkMenuItems(Item menuRoot)
        {
            if (menuRoot == null)
            {
                throw new ArgumentNullException(nameof(menuRoot));
            }

            return GetChildNavigationItems(menuRoot, 0, 0);
        }

        private NavigationItem CreateNavigationItem(Item item, int level, int maxLevel = -1)
        {
            return new NavigationItem
            {
                Item = item,
                Url = (item.IsDerived(Templates.Link.ID) ? item.LinkFieldUrl(Templates.Link.Fields.Link) : item.Url()),
                Target = (item.IsDerived(Templates.Link.ID) ? item.LinkFieldTarget(Templates.Link.Fields.Link) : ""),
                Children = this.GetChildNavigationItems(item, level + 1, maxLevel)
            };
        }

        private NavigationItems GetChildNavigationItems(Item parentItem, int level, int maxLevel)
        {
            if (level > maxLevel || !parentItem.HasChildren)
            {
                return null;
            }

            var childItems = parentItem.Children.Where(item => this.IncludeInNavigation(item)).Select(i => this.CreateNavigationItem(i, level, maxLevel));

            return new NavigationItems
            {
                Items = childItems.ToList(),
                Title = parentItem.Name
            };
        }

        private bool IncludeInNavigation(Item item, bool forceShowInMenu = false)
        {
            return item.HasContextLanguage() && item.IsDerived(Templates.Navigable.ID) && (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
        }

        private NavigationViewModel GetNavigationViewModel(Item categoryItem, Item datasource, Rendering currentRendering)
        {
            var cacheKey = string.Format("Navigation/{0}", categoryItem.Name);
            
            if (this.CurrentSiteContext.Items[cacheKey] != null)
            {
                return (NavigationViewModel)this.CurrentSiteContext.Items[cacheKey];
            }

            var navigationViewModel = new NavigationViewModel();
            CategorySearchResults childCategories = GetChildCategories(datasource, new CommerceSearchOptions(), categoryItem);
            
            navigationViewModel.Initialize(currentRendering, childCategories, categoryItem.ID);
            this.CurrentSiteContext.Items[cacheKey] = navigationViewModel;

            return navigationViewModel;
        }

        private CategorySearchResults GetChildCategories(Item item, CommerceSearchOptions searchOptions, Item categoryItem)
        {
            var returnList = new List<Item>();
            var totalPageCount = 0;
            var totalCategoryCount = 0;

            if (item != null)
            {
                return SearchNavigation.GetCategoryChildCategories(categoryItem.ID, searchOptions);
            }

            return new CategorySearchResults(returnList, totalCategoryCount, totalPageCount, searchOptions.StartPageIndex, new List<FacetCategory>());
        }
    }
}