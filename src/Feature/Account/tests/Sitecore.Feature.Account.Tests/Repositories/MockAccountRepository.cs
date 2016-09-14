using Sitecore.Data.Items;

namespace Sitecore.Feature.Account.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using Castle.Core.Internal;
    using Commerce.Connect.CommerceServer.Orders.Models;
    using Commerce.Entities.Orders;
    using Commerce.Services;
    using Commerce.Services.Customers;
    using Foundation.CommerceServer.Models;
    using Foundation.CommerceServer.Models.InputModels;
    using Interfaces;
    using Models.InputModels;
    using Models.JsonResults;
    using StringExtensions;
    using Ploeh.AutoFixture;

    public class MockAccountRepository : IAccountRepository
    {
        public ChangePasswordBaseJsonResult ChangePassword(ChangePasswordInputModel inputModel)
        {
            throw new NotImplementedException();
        }

        public OrdersBaseJsonResult GetRecentOrders(string userName)
        {
            var fixture = new Fixture();
            fixture.RepeatCount = 10;
            if (!userName.IsNullOrEmpty() && userName.EndsWith("fake"))
            {
                var orders = fixture.Create<IEnumerable<CommerceOrderHeader>>();
                var res = fixture.Create<OrdersBaseJsonResult>();
                res.Success = true;
                res.Initialize(orders);
                return res;
            }

            return new OrdersBaseJsonResult(new ServiceProviderResult() { Success = false });
        }

        public RegisterBaseJsonResult RegisterUser(RegisterUserInputModel inputModel)
        {
            var fixture = new Fixture();
            var res = fixture.Create<RegisterBaseJsonResult>();

            if (!inputModel.UserName.IsNullOrEmpty() && inputModel.UserName.Equals("fake")
                && !inputModel.Password.IsNullOrEmpty() && !inputModel.ConfirmPassword.IsNullOrEmpty()
                && inputModel.Password.Equals(inputModel.ConfirmPassword)) res.Success = true;
            else res.Success = false;
            return res;
        }

        public bool Login(string userName, string password, bool rememberMe)
        {
            if (!userName.IsNullOrEmpty() && userName.EndsWith("fake") && !password.IsNullOrEmpty()) return true;
            return false;
        }

        public IEnumerable<OrderHeader> GetOrders(string userName)
        {
            var fixture = new Fixture();
            fixture.RepeatCount = 10;
            if (!userName.IsNullOrEmpty() && userName.EndsWith("fake"))
            {
                var odrers = fixture.Create<IEnumerable<CommerceOrderHeader>>();
                odrers.ForEach(r => r.ExternalId = "fake");
                return odrers;
            }

            return null;
        }

        public CommerceOrder GetOrder(string id)
        {
            if (!id.IsNullOrEmpty() && id.EndsWith("fake"))
                return new CommerceOrder() { ExternalId = "fake", Name = "fake" };
            return null;
        }

        public AddressListItemJsonResult GetAddressList()
        {
            throw new NotImplementedException();
        }

        public AddressListItemJsonResult ModifyAddress(PartyInputModelItem model)
        {
            throw new NotImplementedException();
        }

        public AddressListItemJsonResult DeleteAddresses(DeletePartyInputModelItem model)
        {
            throw new NotImplementedException();
        }

        public ManagerResponse<UpdatePasswordResult, bool> ResetPassword(ForgotPasswordInputModel model)
        {
            throw new NotImplementedException();
        }
        
        public ProfileBaseJsonResult UpdateProfile(ProfileModel model)
        {
            var fixture = new Fixture();
            var res = fixture.Create<ProfileBaseJsonResult>();

            if (!model.FirstName.IsNullOrEmpty() && model.FirstName.Equals("fake"))res.Success = true;
            else res.Success = false;
            return res;
        }

        public UserBaseJsonResult GetCurrentUser(string userName)
        {
            var fixture = new Fixture();
            var res = fixture.Create<UserBaseJsonResult>();

            if (!userName.IsNullOrEmpty() && userName.Equals("exception")) throw new Exception("fake"); 

            if (!userName.IsNullOrEmpty() && userName.Equals("fake"))res.Success = true;
            else res.Success = false;

            return res;
        }
    }
}