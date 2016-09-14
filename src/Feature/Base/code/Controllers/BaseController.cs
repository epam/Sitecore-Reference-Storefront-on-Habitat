namespace Sitecore.Feature.Base.Controllers
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Utils;
    using Sitecore.Mvc.Controllers;
    using Sitecore.Mvc.Presentation;
    using System.Linq;

    /// <summary>
    /// Base class for Controllers
    /// </summary>
    public class BaseController : SitecoreController
    {
        private Catalog _currentCatalog;
        private ISiteContext _siteContext;
        private IVisitorContext _currentVisitorContext;
        private IContactFactory _contactFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="contactFactory">The contact factory.</param>
        public BaseController(IContactFactory contactFactory)
        {
            this._contactFactory = contactFactory;
        }

        /// <summary>
        /// Gets the contact factory.
        /// </summary>
        /// <value>
        /// The contact factory.
        /// </value>
        public IContactFactory ContactFactory
        {
            get
            {
                return this._contactFactory;
            }
            set
            {
                this._contactFactory = value;
            }
        }

        /// <summary>
        /// Gets the current sitecontext
        /// </summary>
        /// <value>
        /// The current site context.
        /// </value>
        public ISiteContext CurrentSiteContext
        {
            get
            {
                return _siteContext ?? (_siteContext = ContextTypeLoader.CreateInstance<ISiteContext>());
            }
        }

        /// <summary>
        /// Gets the Current Visitor Context
        /// </summary>
        public virtual IVisitorContext CurrentVisitorContext
        {
            get
            {
                return _currentVisitorContext ?? (_currentVisitorContext = ContextTypeLoader.CreateInstance<IVisitorContext>(this.ContactFactory));
            }
        }

        /// <summary>
        /// Gets the Current Storefront being accessed
        /// </summary>
        public CommerceStorefront CurrentStorefront
        {
            get
            {
                return StorefrontManager.CurrentStorefront;
            }
        }

        /// <summary>
        /// Gets the current catalog being accessed
        /// </summary>
        public Catalog CurrentCatalog
        {
            get
            {
                if (_currentCatalog == null)
                {
                    _currentCatalog = CurrentStorefront.DefaultCatalog;
                    if (_currentCatalog == null)
                    {
                        //There was no catalog associated with the storefront or we are not using multi-storefront
                        //So we use the default catalog
                        _currentCatalog = new Catalog(Context.Database.GetItem(CommerceConstants.KnownItemIds.DefaultCatalog));
                    }
                }

                return _currentCatalog;
            }
        }

        /// <summary>
        /// Gets the current rendering under which the controller action is running
        /// </summary>
        protected Rendering CurrentRendering
        {
            get
            {
                return RenderingContext.Current.Rendering;
            }
        }

        /// <summary>
        /// Gets or sets the item under which the current controller action is running
        /// </summary>
        protected Item Item
        {
            get
            {
                return RenderingContext.Current.Rendering.Item;
            }

            set
            {
                RenderingContext.Current.Rendering.Item = value;
            }
        }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="result">The result.</param>
        public virtual void ValidateModel(BaseJsonResult result)
        {
            if (ModelState.IsValid)
            {
                return;
            }

            var errors = (from modelValue in ModelState.Values.Where(modelValue => modelValue.Errors.Any()) from error in modelValue.Errors select error.ErrorMessage).ToList();
            result.SetErrors(errors);
        }
    }
}