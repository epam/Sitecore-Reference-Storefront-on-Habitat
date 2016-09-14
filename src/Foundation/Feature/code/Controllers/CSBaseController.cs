namespace Sitecore.Foundation.Feature.Controllers
{
    using Sitecore.Commerce.Contacts;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Contexts;
    using Sitecore.Foundation.CommerceServer.Managers;

    /// <summary>
    /// Defines the CSBaseController class.
    /// </summary>
    public class CSBaseController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSBaseController"/> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="contactFactory">The contact factory.</param>
        public CSBaseController(AccountManager accountManager, ContactFactory contactFactory)
            : base(contactFactory)
        {
            // Assert.ArgumentNotNull(accountManager, "accountManager");

            this.AccountManager = accountManager;
        }

        /// <summary>
        /// Gets or sets the account manager.
        /// </summary>
        /// <value>
        /// The account manager.
        /// </value>
        public AccountManager AccountManager { get; set; }

        /// <summary>
        /// Gets the Current Visitor Context
        /// </summary>
        public override VisitorContext CurrentVisitorContext
        {
            get
            {
                // Setup the visitor context only once per HttpRequest.
                var siteContext = this.CurrentSiteContext;
                VisitorContext visitorContext = siteContext.Items["__visitorContext"] as VisitorContext;
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