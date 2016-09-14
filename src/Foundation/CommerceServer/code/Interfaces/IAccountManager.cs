using Sitecore.Data.Items;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public interface IAccountManager
    {
        /// <summary>
        /// Gets or sets the contact factory.
        /// </summary>
        /// <value>
        /// The contact factory.
        /// </value>
        //IContactFactory ContactFactory { get; set; }

        /// <summary>
        /// Logins the specified storefront.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="anonymousVisitorId">The anonymous visitor identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        /// <returns>
        /// True if the user is logged in; Otherwise false.
        /// </returns>
        bool Login([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string anonymousVisitorId, string userName, string password, bool persistent);

        /// <summary>
        /// Logouts the current user.
        /// </summary>
        void Logout();

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>
        /// The manager response where the user is returned in the response.
        /// </returns>
        ManagerResponse<GetUserResult, CommerceUser> GetUser(string userName);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>
        /// The manager response where the success flag is returned in the result.
        /// </returns>
        ManagerResponse<DeleteUserResult, bool> DeleteUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the user is returned.
        /// </returns>
        ManagerResponse<UpdateUserResult, CommerceUser> UpdateUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, ProfileModel inputModel);

        /// <summary>
        /// Gets the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="customer">The user.</param>
        /// <returns>The manager response where the list of parties is returned in the response.</returns>
        ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer customer);

        /// <summary>
        /// Gets the current user parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>The manager response where the list of parties is returned in the response.</returns>
        ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetCurrentCustomerParties([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext);

        /// <summary>
        /// Removes the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        ManagerResponse<CustomerResult, bool> RemoveParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<CommerceParty> parties);

        /// <summary>
        /// Removes the parties from current user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="addressExternalId">The address external identifier.</param>
        /// <returns>
        /// The manager response with the successflag in the Result.
        /// </returns>
        ManagerResponse<CustomerResult, bool> RemovePartiesFromCurrentUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string addressExternalId);

        /// <summary>
        /// Updates the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        ManagerResponse<CustomerResult, bool> UpdateParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<Party> parties);

        /// <summary>
        /// Adds the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        ManagerResponse<AddPartiesResult, bool> AddParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<Party> parties);

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager result where the user is returned as the Result.
        /// </returns>
        ManagerResponse<CreateUserResult, CommerceUser> RegisterUser([NotNull] CommerceStorefront storefront, RegisterUserInputModel inputModel);

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The manager response.</returns>
        ManagerResponse<UpdatePasswordResult, bool> UpdateUserPassword([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, ChangePasswordInputModel inputModel);
        
        /// <summary>
        /// Sets the primary address.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="addressExternalId">The address external identifier.</param>
        /// <returns>The manager responsed with the success flag in the result.</returns>
        ManagerResponse<CustomerResult, bool> SetPrimaryAddress([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull]string addressExternalId);

        /// <summary>
        /// Resolve the CommerceUser from the Visitor
        /// </summary>
        /// <returns>
        /// A commerce user
        /// </returns>
        ManagerResponse<GetUserResult, CommerceUser> ResolveCommerceUser();
    }
}