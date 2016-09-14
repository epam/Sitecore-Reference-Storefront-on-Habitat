using Sitecore.Data.Items;

namespace Sitecore.Foundation.CommerceServer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using Sitecore.Analytics;
    using Sitecore.Commerce.Connect.CommerceServer.Configuration;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Commerce.Services;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Extensions;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using Sitecore.Foundation.CommerceServer.Utils;
    using Sitecore.Security.Authentication;

    /// <summary>
    /// Defines the AccountManager class.
    /// </summary>
    public class AccountManager : IAccountManager
    {
        private readonly ICartManager _сartManager;
        private readonly CustomerServiceProvider _customerServiceProvider;
        private readonly IContactFactory _contactFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountManager" /> class.
        /// </summary>
        /// <param name="сartManager">The cart manager.</param>
        /// <param name="customerServiceProvider">The customer service provider.</param>
        /// <param name="contactFactory">The contact factory.</param>
        public AccountManager(
            [NotNull] ICartManager сartManager,
            [NotNull] CustomerServiceProvider customerServiceProvider,
            [NotNull] IContactFactory contactFactory)
        {
            Assert.ArgumentNotNull(customerServiceProvider, "customerServiceProvider");
            Assert.ArgumentNotNull(contactFactory, "contactFactory");

            this._сartManager = сartManager;
            this._customerServiceProvider = customerServiceProvider;
            this._contactFactory = contactFactory;
        }

        #region Methods (public, virtual)

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
        public virtual bool Login([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string anonymousVisitorId, string userName, string password, bool persistent)
        {
            Assert.ArgumentNotNullOrEmpty(userName, "userName");
            Assert.ArgumentNotNullOrEmpty(password, "password");

            var anonymousVisitorCart = this._сartManager.GetCurrentCart(storefront, anonymousVisitorId).Result;

            var isLoggedIn = AuthenticationManager.Login(userName, password, persistent);
            if (isLoggedIn)
            {
                Tracker.Current.CheckForNull().Session.Identify(userName);

                visitorContext.SetCommerceUser(this.ResolveCommerceUser().Result);

                this._сartManager.MergeCarts(storefront, visitorContext, anonymousVisitorId, anonymousVisitorCart);
            }

            return isLoggedIn;
        }

        /// <summary>
        /// Logouts the current user.
        /// </summary>
        public virtual void Logout()
        {
            Tracker.Current.CheckForNull().EndVisit(true);
            System.Web.HttpContext.Current.Session.Abandon();
            AuthenticationManager.Logout();
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>
        /// The manager response where the user is returned in the response.
        /// </returns>
        public virtual ManagerResponse<GetUserResult, CommerceUser> GetUser(string userName)
        {
            Assert.ArgumentNotNullOrEmpty(userName, "userName");

            var request = new GetUserRequest(userName);
            var result = this._customerServiceProvider.GetUser(request);
            if (!result.Success || result.CommerceUser == null)
            {
                var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserNotFoundError);
                result.SystemMessages.Add(new SystemMessage { Message = message });
            }

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetUserResult, CommerceUser>(result, result.CommerceUser);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>
        /// The manager response where the success flag is returned in the result.
        /// </returns>
        public virtual ManagerResponse<DeleteUserResult, bool> DeleteUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");

            var userName = visitorContext.UserName;
            var commerceUser = this.GetUser(userName).Result;

            if (commerceUser != null)
            {
                // NOTE: we do not need to call DeleteCustomer because this will delete the commerce server user under the covers
                var request = new DeleteUserRequest(new CommerceUser { UserName = userName });
                var result = this._customerServiceProvider.DeleteUser(request);

                //Helpers.LogSystemMessages(result.SystemMessages, result);
                return new ManagerResponse<DeleteUserResult, bool>(result, result.Success);
            }

            return new ManagerResponse<DeleteUserResult, bool>(new DeleteUserResult() { Success = false }, false);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager response where the user is returned.
        /// </returns>
        public virtual ManagerResponse<UpdateUserResult, CommerceUser> UpdateUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, ProfileModel inputModel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(inputModel, "inputModel");

            UpdateUserResult result;

            var userName = visitorContext.UserName;
            var commerceUser = this.GetUser(userName).Result;
            if (commerceUser != null)
            {
                commerceUser.FirstName = inputModel.FirstName;
                commerceUser.LastName = inputModel.LastName;
                commerceUser.Email = inputModel.Email;
                commerceUser.SetPropertyValue("Phone", inputModel.TelephoneNumber);

                try
                {
                    var request = new UpdateUserRequest(commerceUser);
                    result = this._customerServiceProvider.UpdateUser(request);
                }
                catch (Exception ex)
                {
                    result = new UpdateUserResult { Success = false };
                    result.SystemMessages.Add(new Sitecore.Commerce.Services.SystemMessage() { Message = ex.Message + "/" + ex.StackTrace });
                }
            }
            else
            {
                // user is authenticated, but not in the CommerceUsers domain - probably here because we are in edit or preview mode
                var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UpdateUserProfileError);
                message = string.Format(CultureInfo.InvariantCulture, message, Context.User.LocalName);
                result = new UpdateUserResult { Success = false };
                result.SystemMessages.Add(new Commerce.Services.SystemMessage { Message = message });
            }

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<UpdateUserResult, CommerceUser>(result, result.CommerceUser);
        }

        /// <summary>
        /// Gets the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="customer">The user.</param>
        /// <returns>The manager response where the list of parties is returned in the response.</returns>
        public virtual ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer customer)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(customer, "user");

            var request = new GetPartiesRequest(customer);
            var result = this._customerServiceProvider.GetParties(request);
            var partyList = result.Success && result.Parties != null ? (result.Parties).Cast<CommerceParty>() : new List<CommerceParty>();

            //Helpers.LogSystemMessages(result.SystemMessages, result);
            return new ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>>(result, partyList);
        }

        /// <summary>
        /// Gets the current user parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>The manager response where the list of parties is returned in the response.</returns>
        public virtual ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetCurrentCustomerParties([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");

            var result = new GetPartiesResult { Success = false };
            var getUserResponse = this.GetUser(visitorContext.UserName);
            if (!getUserResponse.ServiceProviderResult.Success || getUserResponse.Result == null)
            {
                result.SystemMessages.ToList().AddRange(getUserResponse.ServiceProviderResult.SystemMessages);
                return new ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>>(result, null);
            }

            return this.GetParties(storefront, new CommerceCustomer { ExternalId = getUserResponse.Result.ExternalId });
        }

        /// <summary>
        /// Removes the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        public virtual ManagerResponse<CustomerResult, bool> RemoveParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<CommerceParty> parties)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(user, "user");
            Assert.ArgumentNotNull(parties, "parties");

            var request = new RemovePartiesRequest(user, parties.Cast<Party>().ToList());
            var result = this._customerServiceProvider.RemoveParties(request);
            //  if (!result.Success)
            //  {
            //      Helpers.LogSystemMessages(result.SystemMessages, result);
            //  }

            return new ManagerResponse<CustomerResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Removes the parties from current user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="addressExternalId">The address external identifier.</param>
        /// <returns>
        /// The manager response with the successflag in the Result.
        /// </returns>
        public virtual ManagerResponse<CustomerResult, bool> RemovePartiesFromCurrentUser([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull] string addressExternalId)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(addressExternalId, "addresseExternalId");

            var getUserResponse = this.GetUser(Context.User.Name);
            if (getUserResponse.Result == null)
            {
                var customerResult = new CustomerResult { Success = false };
                customerResult.SystemMessages.ToList().AddRange(getUserResponse.ServiceProviderResult.SystemMessages);
                return new ManagerResponse<CustomerResult, bool>(customerResult, false);
            }

            var customer = new CommerceCustomer { ExternalId = getUserResponse.Result.ExternalId };
            var parties = new List<CommerceParty> { new CommerceParty { ExternalId = addressExternalId } };

            return this.RemoveParties(storefront, customer, parties);
        }

        /// <summary>
        /// Updates the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        public virtual ManagerResponse<CustomerResult, bool> UpdateParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<Party> parties)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(user, "user");
            Assert.ArgumentNotNull(parties, "parties");

            var request = new UpdatePartiesRequest(user, parties);
            var result = this._customerServiceProvider.UpdateParties(request);
            //            if (!result.Success)
            //            {
            //                Helpers.LogSystemMessages(result.SystemMessages, result);
            //            }

            return new ManagerResponse<CustomerResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Adds the parties.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="user">The user.</param>
        /// <param name="parties">The parties.</param>
        /// <returns>The manager result where the success flag is returned as the Result.</returns>
        public virtual ManagerResponse<AddPartiesResult, bool> AddParties([NotNull] CommerceStorefront storefront, [NotNull] CommerceCustomer user, List<Party> parties)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(user, "user");
            Assert.ArgumentNotNull(parties, "parties");

            var request = new AddPartiesRequest(user, parties);
            var result = this._customerServiceProvider.AddParties(request);
            //            if (!result.Success)
            //            {
            //                Helpers.LogSystemMessages(result.SystemMessages, result);
            //            }

            return new ManagerResponse<AddPartiesResult, bool>(result, result.Success);
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The manager result where the user is returned as the Result.
        /// </returns>
        public virtual ManagerResponse<CreateUserResult, CommerceUser> RegisterUser([NotNull] CommerceStorefront storefront, RegisterUserInputModel inputModel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(inputModel, "inputModel");
            Assert.ArgumentNotNullOrEmpty(inputModel.UserName, "inputModel.UserName");
            Assert.ArgumentNotNullOrEmpty(inputModel.Password, "inputModel.Password");

            CreateUserResult result;

            // Attempt to register the user
            try
            {
                var request = new CreateUserRequest(inputModel.UserName, inputModel.Password, inputModel.UserName, storefront.ShopName);
                result = this._customerServiceProvider.CreateUser(request);
                //                if (!result.Success)
                //                {
                //                    Helpers.LogSystemMessages(result.SystemMessages, result);
                //                }
                //                else 

                if (result.Success && result.CommerceUser == null && result.SystemMessages.Count() == 0)
                {
                    // Connect bug:  This is a work around to a Connect bug.  When the user already exists,connect simply aborts the pipeline but
                    // does not set the success flag nor does it return an error message.
                    result.Success = false;
                    result.SystemMessages.Add(new SystemMessage { Message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserAlreadyExists) });
                }
            }
            catch (MembershipCreateUserException e)
            {
                result = new CreateUserResult { Success = false };
                result.SystemMessages.Add(new Commerce.Services.SystemMessage { Message = ErrorCodeToString(e.StatusCode) });
            }

            return new ManagerResponse<CreateUserResult, CommerceUser>(result, result.CommerceUser);
        }

        /// <summary>
        /// Updates the user password.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The manager response.</returns>
        public virtual ManagerResponse<UpdatePasswordResult, bool> UpdateUserPassword([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, ChangePasswordInputModel inputModel)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNull(inputModel, "inputModel");
            Assert.ArgumentNotNullOrEmpty(inputModel.OldPassword, "inputModel.OldPassword");
            Assert.ArgumentNotNullOrEmpty(inputModel.NewPassword, "inputModel.NewPassword");

            var userName = visitorContext.UserName;

            var request = new UpdatePasswordRequest(userName, inputModel.OldPassword, inputModel.NewPassword);
            var result = this._customerServiceProvider.UpdatePassword(request);
            if (!result.Success && !result.SystemMessages.Any())
            {
                var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.PasswordCouldNotBeReset);
                result.SystemMessages.Add(new SystemMessage { Message = message });
            }

            //            if (!result.Success)
            //            {
            //                Helpers.LogSystemMessages(result.SystemMessages, result);
            //            }

            return new ManagerResponse<UpdatePasswordResult, bool>(result, result.Success);
        }
        
        /// <summary>
        /// Sets the primary address.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <param name="addressExternalId">The address external identifier.</param>
        /// <returns>The manager responsed with the success flag in the result.</returns>
        public virtual ManagerResponse<CustomerResult, bool> SetPrimaryAddress([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext, [NotNull]string addressExternalId)
        {
            Assert.ArgumentNotNull(storefront, "storefront");
            Assert.ArgumentNotNull(visitorContext, "visitorContext");
            Assert.ArgumentNotNullOrEmpty(addressExternalId, "addressExternalId");

            var userPartiesResponse = this.GetCurrentCustomerParties(storefront, visitorContext);
            if (userPartiesResponse.ServiceProviderResult.Success)
            {
                var customerResult = new CustomerResult { Success = false };
                customerResult.SystemMessages.ToList().AddRange(userPartiesResponse.ServiceProviderResult.SystemMessages);
                return new ManagerResponse<CustomerResult, bool>(customerResult, false);
            }

            var addressesToUpdate = new List<CommerceParty>();

            CommerceParty notPrimary = (CommerceParty)userPartiesResponse.Result.SingleOrDefault(address => ((CommerceParty)address).IsPrimary);
            if (notPrimary != null)
            {
                notPrimary.IsPrimary = false;
                addressesToUpdate.Add(notPrimary);
            }

            var primaryAddress = (CommerceParty)userPartiesResponse.Result.Single(address => address.PartyId == addressExternalId);

            //primaryAddress.IsPrimary = true;
            addressesToUpdate.Add(primaryAddress);

            var updatePartiesResponse = this.UpdateParties(storefront, new CommerceCustomer { ExternalId = visitorContext.UserId }, addressesToUpdate.Cast<Party>().ToList());

            return new ManagerResponse<CustomerResult, bool>(updatePartiesResponse.ServiceProviderResult, updatePartiesResponse.Result);
        }

        /// <summary>
        /// Resolve the CommerceUser from the Visitor
        /// </summary>
        /// <returns>
        /// A commerce user
        /// </returns>
        public ManagerResponse<GetUserResult, CommerceUser> ResolveCommerceUser()
        {
            if (Tracker.Current == null || Tracker.Current.Contact == null || Tracker.Current.Contact.ContactId == Guid.Empty)
            {
                // This only occurs if we are authenticated but there is no ExternalUser assigned.
                // This happens in preview mode we want to supply the default user to use in Preview mode
                // Tracker.Visitor.ExternalUser = "1";
                return new ManagerResponse<GetUserResult, CommerceUser>(new GetUserResult() { Success = true }, new CommerceUser { FirstName = "Test", LastName = "User" });
            }

            var userName = this._contactFactory.GetContact();
            var response = this.GetUser(userName);
            var commerceUser = response.Result;

            Assert.IsNotNull(commerceUser, "The user '{0}' could not be found.", userName);

            return new ManagerResponse<GetUserResult, CommerceUser>(response.ServiceProviderResult, commerceUser);
        }

        #endregion

        #region Methods (protected, virtual)

        /// <summary>
        /// Concats the user name with the current domain if it missing
        /// </summary>
        /// <param name="userName">The user's user name</param>
        /// <returns>The updated user name</returns>
        protected virtual string UpdateUserName(string userName)
        {
            var defaultDomain = CommerceServerSitecoreConfig.Current.DefaultCommerceUsersDomain;
            if (string.IsNullOrWhiteSpace(defaultDomain))
            {
                defaultDomain = CommerceConstants.ProfilesStrings.CommerceUsersDomainName;
            }

            return !userName.StartsWith(defaultDomain, StringComparison.OrdinalIgnoreCase) ? string.Concat(defaultDomain, @"\", userName) : userName;
        }

        /// <summary>
        /// Errors the code to string.
        /// </summary>
        /// <param name="createStatus">The create status.</param>
        /// <returns>The membership error status.</returns>
        protected virtual string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserAlreadyExists);

                case MembershipCreateStatus.DuplicateEmail:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserNameForEmailExists);

                case MembershipCreateStatus.InvalidPassword:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.InvalidPasswordError);

                case MembershipCreateStatus.InvalidEmail:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.InvalidEmailError);

                case MembershipCreateStatus.InvalidAnswer:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.PasswordRetrievalAnswerInvalid);

                case MembershipCreateStatus.InvalidQuestion:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.PasswordRetrievalQuestionInvalid);

                case MembershipCreateStatus.InvalidUserName:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserNameInvalid);

                case MembershipCreateStatus.ProviderError:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.AuthenticationProviderError);

                case MembershipCreateStatus.UserRejected:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UserRejectedError);

                default:
                    return StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.UnknownMembershipProviderError);
            }
        }

        #endregion
    }
}