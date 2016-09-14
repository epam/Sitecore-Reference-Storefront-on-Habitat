namespace Sitecore.Feature.Account.Tests.Repositories
{
    using System;
    using System.Web;
    using FluentAssertions;
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Foundation.CommerceServer.Managers;
    using Foundation.CommerceServer.Models;
    using Foundation.Testing.Mocks.Contexts;
    using Foundation.Testing.Mocks.Managers;
    using Models.InputModels;
    using Sitecore.Data;
    using Sitecore.FakeDb.Sites;
    using Sitecore.Feature.Account.Repositories;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using Sitecore.Foundation.Testing.Attributes;
    using Xunit;

    /* This section must be in config file
      <sitecore>
         <settings>
           <setting name="LicenseFile" value="c:\Websites\license.xml" />
         </settings>
         <commerceServer>
           <types>
             <type name="ISiteContext" type="Sitecore.Foundation.CommerceServer.Infrastructure.Contexts.SiteContext, Sitecore.Foundation.CommerceServer" lifetime="Singleton" />
             <type name="CommerceStorefront" type="Sitecore.Foundation.CommerceServer.Models.CommerceServerStorefront, Sitecore.Foundation.CommerceServer" lifetime="PerCall" />
             <type name="OrderHeaderItemBaseJsonResult" type="Sitecore.Feature.Account.Models.JsonResults.CSOrderHeaderItemBaseJsonResult, Sitecore.Feature.Account" lifetime="PerCall" />
           </types>
         </commerceServer>
       </sitecore>
   */
    public class AccountRepositoryTests
    {
        private FakeSiteContext FakeSiteContext { get; }

        public AccountRepositoryTests()
        {
            FakeSiteContext = new FakeSiteContext(new Collections.StringDictionary { { "rootPath", "/sitecore/content/" + Templates.SiteName.Name } }); ;
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://local", null), new HttpResponse(null));
            Context.Site = FakeSiteContext;
        }

        [Theory]
        [AutoDbData]
        public void ChangePassword_Initialized_ShouldReturnUserName(Database db, IOrderManager orderManager, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, orderManager);
            var model = new ChangePasswordInputModel { OldPassword = "fake", NewPassword = "fake", ConfirmPassword = "fake" };

            // act
            var result = rep.ChangePassword(model);

            // assert
            result.UserName.ShouldBeEquivalentTo("fake");
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoDbData]
        public void ChangePassword_NotInitialized_ShouldNotReturnUserName(Database db, IOrderManager orderManager, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, orderManager);

            // act
            var result = rep.ChangePassword(new ChangePasswordInputModel());

            // assert
            result.UserName.ShouldBeEquivalentTo(null);
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetRecentOrders_UserInitialized_ShouldReturnOrders(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetRecentOrders("fake");

            // assert
            result.Orders.Count.Should().BeGreaterThan(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetRecentOrders_UserNotInitialized_ShouldNotReturnOrders(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetRecentOrders("");

            // assert
            result.Orders.Count.Should().Be(0);
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetRecentOrders_OrdersInitialized_ShouldReturnOrders(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetRecentOrders("fake");

            // assert
            result.Orders.Count.Should().Be(AccountRepository.ReturnLastModifiedOrdersNum);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetRecentOrders_OrdersNotInitialized_ShouldNotReturnOrders(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetRecentOrders("not_fake");

            // assert
            result.Orders.Count.Should().Be(0);
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetRecentOrders_LastModifiedTooOld_ShouldNotReturnOrders(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var orderManager = new MockOrderManager { GetOrdersLastModifyDateIsTooOld = true, GetOrdersDaysOld = AccountRepository.ReturnLastModifiedOrdersDaysOld };
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, orderManager);

            // act
            var result = rep.GetRecentOrders("fake");

            // assert
            result.Orders.Count.Should().Be(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetOrder_Initialized_ShouldReturnOrderDetails(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetOrder("fake");

            // assert
            result.LastModified.Should().BeLessThan(DateTime.Now.TimeOfDay);
        }

        [Theory]
        [AutoProfileDbData]
        public void GetAddressList_Initialized_ShouldReturnAddressList(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var orderManager = new MockOrderManager { IsAvailableCountriesInitialized = true };
            var rep = new AccountRepository(accountManager, contactFactory, orderManager);

            // act
            var result = rep.GetAddressList();

            // assert
            result.Addresses.Count.Should().BeGreaterThan(0);
            result.Countries.Count.Should().BeGreaterThan(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetAddressList_CountriesNotInitialized_ShouldReturnAddressList(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var orderManager = new MockOrderManager { IsAvailableCountriesInitialized = false };
            var rep = new AccountRepository(accountManager, contactFactory, orderManager);

            // act
            var result = rep.GetAddressList();

            // assert
            result.Addresses.Count.Should().BeGreaterThan(0);
            result.Countries.Count.Should().Be(0);
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void ModifyAddress_AddressesInitializedExternalIdNotInitialized_ShouldReturnAllAddresses(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var rep = new AccountRepository(accountManager, contactFactory, new MockOrderManager());
            var model = new PartyInputModelItem { Name = "fake" };

            // act
            var result = rep.ModifyAddress(model);

            // assert
            result.Addresses.Count.Should().BeGreaterThan(0);
            result.Countries.Count.Should().Be(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void ModifyAddress_UserNameNotInitialized_ShouldNotReturnAddress(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var orderManager = new MockOrderManager { IsAvailableCountriesInitialized = false };
            var rep = new AccountRepository(accountManager, contactFactory, orderManager);
            var model = new PartyInputModelItem { Name = "fake" };

            using (new Sitecore.Security.Accounts.UserSwitcher("null", false))
            {
                // act
                var result = rep.ModifyAddress(model);

                // assert
                result.Addresses.Count.Should().Be(0);
                result.Countries.Count.Should().Be(0);
                result.Success.Should().BeFalse();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void ModifyAddress_AddressesAndExternalIdInitialized_ShouldUpdateAndNextReturnAllAddresses(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var rep = new AccountRepository(accountManager, contactFactory, new MockOrderManager());
            var model = new PartyInputModelItem { Name = "fake", ExternalId = "fake" };

            // act
            var result = rep.ModifyAddress(model);

            // assert
            result.Addresses.Count.Should().BeGreaterThan(0);
            result.Countries.Count.Should().Be(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void ModifyAddress_NumberOfAddressesExeedes_ShouldReturnError(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager()
            {
                IsCurrentCustomerPartiesInitialized = true,
                NumOfCurrentCustomerPartiesToInitialize = StorefrontManager.CurrentStorefront.MaxNumberOfAddresses + 1
            };
            var rep = new AccountRepository(accountManager, contactFactory, new MockOrderManager());
            var model = new PartyInputModelItem { Name = "fake" };

            // act
            var result = rep.ModifyAddress(model);

            // assert
            result.Addresses.Count.Should().Be(0);
            result.HasErrors.Should().BeTrue();
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.Contains($"[{StorefrontConstants.SystemMessages.MaxAddressLimitReached}]").Should().BeTrue();
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void DeleteAddresss_Initialized_ShouldReturnAllAdresses(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var rep = new AccountRepository(accountManager, contactFactory, new MockOrderManager());
            var model = new DeletePartyInputModelItem { ExternalId = "fake" };

            // act
            var result = rep.DeleteAddresses(model);

            // assert
            result.Addresses.Count.Should().BeGreaterThan(0);
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void DeleteAddresss_NotInitialized_ShouldNotReturnAllAdresses(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var accountManager = new MockAccountManager() { IsCurrentCustomerPartiesInitialized = true };
            var rep = new AccountRepository(accountManager, contactFactory, new MockOrderManager());
            var model = new DeletePartyInputModelItem { };

            // act
            var result = rep.DeleteAddresses(model);

            // assert
            result.Addresses.Count.Should().Be(0);
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_Initialized_ShouldReturnSuccess(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());
            var model = new ProfileModel { FirstName = "fake" };

            // act
            var result = rep.UpdateProfile(model);

            // assert
            //result.FirstName.ShouldBeEquivalentTo("fake");
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_PasswordInitialized_ShouldReturnSuccess(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());
            var model = new ProfileModel { FirstName = "fake", Password = "fake", PasswordRepeat = "fake" };

            // act
            var result = rep.UpdateProfile(model);

            // assert
            //result.FirstName.ShouldBeEquivalentTo("fake");
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_PasswordNotInitialized_ShouldNotReturnSuccess(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());
            var model = new ProfileModel { FirstName = "fake", Password = "fake", PasswordRepeat = "not same fake" };

            // act
            var result = rep.UpdateProfile(model);

            // assert
            result.Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetCurrentUser_Initialized_ShouldReturnUser(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetCurrentUser("fake");

            // assert
            result.Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetCurrentUser_NotInitialized_ShouldNotReturnUser(Database db, IContactFactory contactFactory)
        {
            // arrange
            FakeSiteContext.Database = db;
            var rep = new AccountRepository(new MockAccountManager(), contactFactory, new MockOrderManager());

            // act
            var result = rep.GetCurrentUser("");

            // assert
            result.Success.Should().BeFalse();
        }
    }
}