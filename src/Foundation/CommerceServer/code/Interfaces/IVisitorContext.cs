using System;
using System.Collections.Generic;
using System.Linq;
namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using Sitecore.Commerce.Contacts;
    using Sitecore.Commerce.Entities.Customers;

    public interface IVisitorContext
    {
        /// <summary>
        /// Gets or sets the contact factory.
        /// </summary>
        /// <value>
        /// The contact factory.
        /// </value>
        IContactFactory ContactFactory { get; }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        string UserId { get; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        /// Gets the commerce user.
        /// </summary>
        /// <value>
        /// The commerce user.
        /// </value>
        CommerceUser CommerceUser { get; }

        /// <summary>
        /// Gets the visitor id.
        /// </summary>
        /// <value>The visitor id.</value>
        string VisitorId { get; }

        /// <summary>
        /// Gets the current customer Id
        /// </summary>
        /// <returns>the id</returns>
        string GetCustomerId();

        /// <summary>
        /// Resolve the CommerceUser from the Visitor
        /// </summary>
        /// <param name="user">The user.</param>
        void SetCommerceUser(CommerceUser user);
    }
}