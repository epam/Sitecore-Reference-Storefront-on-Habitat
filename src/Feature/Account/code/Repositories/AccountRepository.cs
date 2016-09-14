using System.Web;
using System.Web.Security;
using Sitecore.Commerce.Services;
using Sitecore.Diagnostics;
using Sitecore.Feature.Account.Utils;

namespace Sitecore.Feature.Account.Repositories
{
    using Models.JsonResults;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Commerce.Entities.Orders;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Feature.Account.Interfaces;
    using Sitecore.Feature.Account.Models.InputModels;
    using Sitecore.Feature.Base.Repositories;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public const int ReturnLastModifiedOrdersNum = 5;
        public const int ReturnLastModifiedOrdersDaysOld = -30;

        public AccountRepository(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            IOrderManager orderManager)
            : base(accountManager, contactFactory)
        {
            this.OrderManager = orderManager;
        }

        public IOrderManager OrderManager { get; protected set; }

        public ChangePasswordBaseJsonResult ChangePassword(ChangePasswordInputModel inputModel)
        {
            var response = this.AccountManager.UpdateUserPassword(this.CurrentStorefront, this.CurrentVisitorContext, inputModel);
            var result = new ChangePasswordBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success)
            {
                result.Initialize(this.CurrentVisitorContext.UserName);
            }

            return result;
        }

        public OrdersBaseJsonResult GetRecentOrders(string userName)
        {
            var recentOrders = new List<OrderHeader>();
            var userResponse = this.AccountManager.GetUser(userName);
            var result = new OrdersBaseJsonResult(userResponse.ServiceProviderResult);

            if (userResponse.ServiceProviderResult.Success && userResponse.Result != null)
            {
                var commerceUser = userResponse.Result;
                var response = this.OrderManager.GetOrders(commerceUser.ExternalId, Context.Site.Name);
                result.SetErrors(response.ServiceProviderResult);

                if (response.ServiceProviderResult.Success && response.Result != null)
                {
                    var orders = response.Result.ToList();
                    recentOrders = orders.Where(order => (order as CommerceOrderHeader).LastModified > DateTime.Today.AddDays(ReturnLastModifiedOrdersDaysOld))
                        .Take(ReturnLastModifiedOrdersNum).ToList();
                }
            }

            result.Initialize(recentOrders);

            return result;
        }

        public RegisterBaseJsonResult RegisterUser(RegisterUserInputModel inputModel)
        {
            RegisterBaseJsonResult result = new RegisterBaseJsonResult();
            var anonymousVisitorId = this.CurrentVisitorContext.UserId;
            var response = this.AccountManager.RegisterUser(this.CurrentStorefront, inputModel);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result);
                this.AccountManager.Login(CurrentStorefront, CurrentVisitorContext, anonymousVisitorId, response.Result.UserName, inputModel.Password, false);
            }
            else
            {
                result.SetErrors(response.ServiceProviderResult);
            }

            return result;
        }

        public bool Login(string userName, string password, bool rememberMe)
        {
            var anonymousVisitorId = this.CurrentVisitorContext.UserId;

            return this.AccountManager.Login(CurrentStorefront, CurrentVisitorContext,
                anonymousVisitorId, userName, password, rememberMe);
        }

        public IEnumerable<OrderHeader> GetOrders(string userName)
        {
            var commerceUser = this.AccountManager.GetUser(userName).Result;
            var orders = this.OrderManager.GetOrders(commerceUser.ExternalId, Context.Site.Name).Result;

            return orders.ToList();
        }

        public CommerceOrder GetOrder(string id)
        {
            var response = this.OrderManager.GetOrderDetails(CurrentStorefront, CurrentVisitorContext, id);

            return response.Result;
        }

        public AddressListItemJsonResult GetAddressList()
        {
            var result = new AddressListItemJsonResult();
            var addresses = this.AllAddresses(result);
            var countries = this.GetAvailableCountries(result);

            result.Initialize(addresses, countries);

            return result;
        }

        public AddressListItemJsonResult ModifyAddress(PartyInputModelItem model)
        {
            var addresses = new List<CommerceParty>();
            var userResponse = this.AccountManager.GetUser(Context.User.Name);
            var result = new AddressListItemJsonResult(userResponse.ServiceProviderResult);

            if (userResponse.ServiceProviderResult.Success && userResponse.Result != null)
            {
                var commerceUser = userResponse.Result;
                var customer = new CommerceCustomer { ExternalId = commerceUser.ExternalId };
                var party = new CommerceParty
                {
                    ExternalId = model.ExternalId,
                    Name = model.Name,
                    Address1 = model.Address1,
                    City = model.City,
                    Country = model.Country,
                    State = model.State,
                    ZipPostalCode = model.ZipPostalCode,
                    PartyId = model.PartyId,
                    IsPrimary = model.IsPrimary
                };

                if (string.IsNullOrEmpty(party.ExternalId))
                {
                    // Verify we have not reached the maximum number of addresses supported.
                    var numberOfAddresses = this.AllAddresses(result).Count;

                    if (numberOfAddresses >= StorefrontManager.CurrentStorefront.MaxNumberOfAddresses)
                    {
                        var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.MaxAddressLimitReached);
                        result.Errors.Add(string.Format(CultureInfo.InvariantCulture, message, numberOfAddresses));
                        result.Success = false;
                    }
                    else
                    {
                        party.ExternalId = Guid.NewGuid().ToString("B");

                        var response = this.AccountManager.AddParties(this.CurrentStorefront, customer, new List<Sitecore.Commerce.Entities.Party> { party });
                        result.SetErrors(response.ServiceProviderResult);
                        if (response.ServiceProviderResult.Success)
                        {
                            addresses = this.AllAddresses(result);
                        }

                        result.Initialize(addresses, null);
                    }
                }
                else
                {
                    var response = this.AccountManager.UpdateParties(this.CurrentStorefront, customer, new List<Sitecore.Commerce.Entities.Party> { party });
                    result.SetErrors(response.ServiceProviderResult);

                    if (response.ServiceProviderResult.Success)
                    {
                        addresses = this.AllAddresses(result);
                    }

                    result.Initialize(addresses, null);
                }
            }

            return result;
        }

        public AddressListItemJsonResult DeleteAddresses(DeletePartyInputModelItem model)
        {
            var addresses = new List<CommerceParty>();
            var response = this.AccountManager.RemovePartiesFromCurrentUser(this.CurrentStorefront, this.CurrentVisitorContext, model.ExternalId);
            var result = new AddressListItemJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success)
            {
                addresses = this.AllAddresses(result);
            }

            result.Initialize(addresses, null);

            return result;
        }

        public ManagerResponse<UpdatePasswordResult, bool> ResetPassword(ForgotPasswordInputModel model)
        {
            Assert.ArgumentNotNull(this.CurrentStorefront, "storefront");
            Assert.ArgumentNotNull(model, "inputModel");

            var result = new UpdatePasswordResult { Success = true };
            var getUserResponse = AccountManager.GetUser(model.Email);

            if (!getUserResponse.ServiceProviderResult.Success || getUserResponse.Result == null)
            {
                result.Success = false;
                foreach (var systemMessage in getUserResponse.ServiceProviderResult.SystemMessages)
                {
                    result.SystemMessages.Add(systemMessage);
                }
            }
            else
            {
                try
                {
                    var userIpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;
                    var provisionalPassword = Membership.Provider.ResetPassword(getUserResponse.Result.UserName, string.Empty);

                    var mailUtil = new MailUtil();
                    var wasEmailSent = mailUtil.SendMail(model.Subject, model.Body, model.Email, this.CurrentStorefront.SenderEmailAddress, new object(), new object[] { userIpAddress, provisionalPassword });

                    if (!wasEmailSent)
                    {
                        var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.CouldNotSentEmailError);
                        result.Success = false;
                        result.SystemMessages.Add(new SystemMessage { Message = message });
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.SystemMessages.Add(new SystemMessage { Message = e.Message });
                }
            }

            return new ManagerResponse<UpdatePasswordResult, bool>(result, result.Success);
        }

        public ProfileBaseJsonResult UpdateProfile(ProfileModel model)
        {
            var result = new ProfileBaseJsonResult();
            var response = this.AccountManager.UpdateUser(this.CurrentStorefront, this.CurrentVisitorContext, model);
            result.SetErrors(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && !string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.PasswordRepeat))
            {
                var changePasswordModel = new ChangePasswordInputModel { NewPassword = model.Password, ConfirmPassword = model.PasswordRepeat };
                var passwordChangeResponse = this.AccountManager.UpdateUserPassword(this.CurrentStorefront, this.CurrentVisitorContext, changePasswordModel);
                result.SetErrors(passwordChangeResponse.ServiceProviderResult);

                if (passwordChangeResponse.ServiceProviderResult.Success)
                {
                    result.Initialize(response.ServiceProviderResult);
                }
            }

            return result;
        }

        public UserBaseJsonResult GetCurrentUser(string userName)
        {
            var response = this.AccountManager.GetUser(userName);
            var result = new UserBaseJsonResult(response.ServiceProviderResult);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                result.Initialize(response.Result);
            }

            return result;
        }

        private Dictionary<string, string> GetAvailableCountries(AddressListItemJsonResult result)
        {
            var countries = new Dictionary<string, string>();
            var response = OrderManager.GetAvailableCountries();

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                countries = response.Result;
            }

            result.SetErrors(response.ServiceProviderResult);

            return countries;
        }

        private List<CommerceParty> AllAddresses(AddressListItemJsonResult result)
        {
            var addresses = new List<CommerceParty>();
            var response = this.AccountManager.GetCurrentCustomerParties(this.CurrentStorefront, this.CurrentVisitorContext);

            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                addresses = response.Result.ToList();
            }

            result.SetErrors(response.ServiceProviderResult);

            return addresses;
        }
    }
}