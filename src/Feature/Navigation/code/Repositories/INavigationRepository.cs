using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.Navigation.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Navigation.Models;

    public interface INavigationRepository
    {
        NavigationViewModel GetNavigationProductBar(Item dataSource, Rendering currentRendering);

        NavigationViewModel GetChildCategoryNavigationBar(Item datasource, Rendering currentRendering);
        
        NavigationItems GetLinkMenuItems(Item menuItem);
    }
}