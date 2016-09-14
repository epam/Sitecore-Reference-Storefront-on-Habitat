using Sitecore.Data.Items;

namespace Sitecore.Feature.Account.Interfaces
{
    using System.Collections.Generic;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities.Orders;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Feature.Account.Models.InputModels;
    using Sitecore.Feature.Account.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    public interface IAccountRepository
    {
        ChangePasswordBaseJsonResult ChangePassword(ChangePasswordInputModel inputModel);

        OrdersBaseJsonResult GetRecentOrders(string userName);

        RegisterBaseJsonResult RegisterUser(RegisterUserInputModel inputModel);

        bool Login(string userName, string password, bool rememberMe);

        IEnumerable<OrderHeader> GetOrders(string userName);

        CommerceOrder GetOrder(string id);

        AddressListItemJsonResult GetAddressList();

        AddressListItemJsonResult ModifyAddress(PartyInputModelItem model);

        AddressListItemJsonResult DeleteAddresses(DeletePartyInputModelItem model);

        ManagerResponse<UpdatePasswordResult, bool> ResetPassword(ForgotPasswordInputModel model);

        ProfileBaseJsonResult UpdateProfile(ProfileModel model);

        UserBaseJsonResult GetCurrentUser(string userName);
    }
}