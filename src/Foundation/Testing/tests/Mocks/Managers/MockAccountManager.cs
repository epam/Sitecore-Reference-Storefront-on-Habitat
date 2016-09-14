using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Testing.Mocks.Managers
{
    using Ploeh.AutoFixture;
    using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
    using Sitecore.Commerce.Entities;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Commerce.Services.Customers;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using StringExtensions;

    public class MockAccountManager : IAccountManager
    {
        public bool IsCurrentCustomerPartiesInitialized { get; set; }
        public int NumOfCurrentCustomerPartiesToInitialize { get; set; }


        private readonly ICartManager _cartManager;
        private readonly CustomerServiceProvider _customerServiceProvider;
        public IContactFactory ContactFactory { get; set; }

        public MockAccountManager() { }

        public MockAccountManager(ICartManager cartManager, IContactFactory contactFactory, CustomerServiceProvider customerServiceProvider)
        {
            _customerServiceProvider = customerServiceProvider;
            _cartManager = cartManager;
            ContactFactory = contactFactory;
        }

        public bool Login(CommerceStorefront storefront, IVisitorContext visitorContext, string anonymousVisitorId, string userName, string password, bool persistent)
        {
            return false;
        }

        public void Logout()
        {
        }

        public ManagerResponse<GetUserResult, CommerceUser> GetUser(string userName)
        {
            if (userName.IsNullOrEmpty() || userName.Equals("null")) return new ManagerResponse<GetUserResult, CommerceUser>(new GetUserResult() { Success = false }, null);

            var fixture = new Fixture();
            var user = fixture.Create<CommerceUser>();
            user.UserName = userName;

            if (userName.Equals("fake")) user.ExternalId = userName;

            return new ManagerResponse<GetUserResult, CommerceUser>(new GetUserResult(), user);
        }

        public ManagerResponse<DeleteUserResult, bool> DeleteUser(CommerceStorefront storefront, IVisitorContext visitorContext)
        {
            return null;
        }

        public ManagerResponse<UpdateUserResult, CommerceUser> UpdateUser(CommerceStorefront storefront, IVisitorContext visitorContext, ProfileModel inputModel)
        {
            if(inputModel!=null && !inputModel.FirstName.IsNullOrEmpty() && inputModel.FirstName.Equals("fake")) 
                return new ManagerResponse<UpdateUserResult, CommerceUser>(new UpdateUserResult {Success = true}, new CommerceUser() {UserName = inputModel.FirstName } );

            return new ManagerResponse<UpdateUserResult, CommerceUser>(new UpdateUserResult { Success = false }, new CommerceUser() );
        }

        public ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetParties(CommerceStorefront storefront, CommerceCustomer customer)
        {
            return null;
        }

        public ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>> GetCurrentCustomerParties(CommerceStorefront storefront, IVisitorContext visitorContext)
        {
            if (IsCurrentCustomerPartiesInitialized == false)
                return new ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>>(new GetPartiesResult { Success = false }, null);

            var fixture = new Fixture();
            fixture.RepeatCount =
                NumOfCurrentCustomerPartiesToInitialize > 0 ? NumOfCurrentCustomerPartiesToInitialize : 5;
            var parties = fixture.Create<IEnumerable<CommerceParty>>();
            return new ManagerResponse<GetPartiesResult, IEnumerable<CommerceParty>>(new GetPartiesResult { Success = true }, parties);
        }

        public ManagerResponse<CustomerResult, bool> RemoveParties(CommerceStorefront storefront, CommerceCustomer user, List<CommerceParty> parties)
        {
            return null;
        }

        public ManagerResponse<CustomerResult, bool> RemovePartiesFromCurrentUser(CommerceStorefront storefront, IVisitorContext visitorContext, string addressExternalId)
        {
            if (!addressExternalId.IsNullOrEmpty() && addressExternalId.Equals("fake"))
                return new ManagerResponse<CustomerResult, bool>(new AddPartiesResult { Success = true }, true);
            return new ManagerResponse<CustomerResult, bool>(new AddPartiesResult { Success = false }, false);
        }

        public ManagerResponse<CustomerResult, bool> UpdateParties(CommerceStorefront storefront, CommerceCustomer user, List<Party> parties)
        {
            if (parties.Any() && parties.Count(r => r.ExternalId.Equals("fake")) > 0)
                return new ManagerResponse<CustomerResult, bool>(new AddPartiesResult { Success = true }, true);
            return new ManagerResponse<CustomerResult, bool>(new AddPartiesResult { Success = false }, false);
        }

        public ManagerResponse<AddPartiesResult, bool> AddParties(CommerceStorefront storefront, CommerceCustomer user, List<Party> parties)
        {
            return new ManagerResponse<AddPartiesResult, bool>(new AddPartiesResult { Success = true }, true);
        }

        public ManagerResponse<CreateUserResult, CommerceUser> RegisterUser(CommerceStorefront storefront, RegisterUserInputModel inputModel)
        {
            return null;
        }

        public ManagerResponse<UpdatePasswordResult, bool> UpdateUserPassword(CommerceStorefront storefront, IVisitorContext visitorContext, ChangePasswordInputModel inputModel)
        {
            if (inputModel.NewPassword.IsNullOrEmpty() || inputModel.ConfirmPassword.IsNullOrEmpty() ||
               !inputModel.NewPassword.Equals(inputModel.ConfirmPassword)) return new ManagerResponse<UpdatePasswordResult, bool>(new UpdatePasswordResult() { Success = false }, false);

            return new ManagerResponse<UpdatePasswordResult, bool>(new UpdatePasswordResult(), true);
        }
        
        public ManagerResponse<CustomerResult, bool> SetPrimaryAddress(CommerceStorefront storefront, IVisitorContext visitorContext, string addressExternalId)
        {
            return null;
        }

        public ManagerResponse<GetUserResult, CommerceUser> ResolveCommerceUser()
        {
            return null;
        }
    }
}
