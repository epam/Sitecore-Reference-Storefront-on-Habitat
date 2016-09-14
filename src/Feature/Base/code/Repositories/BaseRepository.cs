using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
using Sitecore.Foundation.CommerceServer.Models;
using Sitecore.Foundation.CommerceServer.Utils;

namespace Sitecore.Feature.Base.Repositories
{
    using Sitecore.Foundation.CommerceServer.Infrastructure.Contexts;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Defines the CSBaseController class.
    /// </summary>
    public class BaseRepository
    {
        private Catalog _currentCatalog;
        private ISiteContext _siteContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="contactFactory">The contact factory.</param>
        public BaseRepository(IAccountManager accountManager, IContactFactory contactFactory)
        {
            this.AccountManager = accountManager;
            this.ContactFactory = contactFactory;
        }

        /// <summary>
        /// Gets or sets the account manager.
        /// </summary>
        /// <value>
        /// The account manager.
        /// </value>
        public IAccountManager AccountManager { get; set; }

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
        /// Gets the contact factory.
        /// </summary>
        /// <value>
        /// The contact factory.
        /// </value>
        public IContactFactory ContactFactory { get; set; }

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
        /// Gets the Current Visitor Context
        /// </summary>
        public IVisitorContext CurrentVisitorContext
        {
            get
            {
                // Setup the visitor context only once per HttpRequest.
                var siteContext = this.CurrentSiteContext;
                var visitorContext = siteContext.Items["__visitorContext"] as IVisitorContext;

                if (visitorContext == null)
                {
                    visitorContext = new VisitorContext(this.ContactFactory);

                    if (Context.User.IsAuthenticated && !Context.User.Profile.IsAdministrator)
                    {
                        visitorContext.SetCommerceUser(this.AccountManager.ResolveCommerceUser().Result);
                    }

                    siteContext.Items["__visitorContext"] = visitorContext;
                }

                return visitorContext;
            }
        }
    }
}