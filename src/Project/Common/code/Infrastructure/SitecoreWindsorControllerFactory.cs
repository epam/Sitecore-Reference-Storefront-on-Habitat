namespace Sitecore.Common.Website.Infrastructure
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Mvc.Controllers;
    using Sitecore.Mvc.Extensions;

    public class SitecoreWindsorControllerFactory : SitecoreControllerFactory
    {
        public SitecoreWindsorControllerFactory(IControllerFactory innerFactory) : base(innerFactory)
        {
        }

        protected override IController CreateControllerInstance(RequestContext requestContext, string controllerName)
        {
            if (controllerName.EqualsText(SitecoreControllerName))
            {
                return CreateSitecoreController(requestContext, controllerName);
            }

            return InnerFactory.CreateController(requestContext, controllerName);
        }
    }
}
