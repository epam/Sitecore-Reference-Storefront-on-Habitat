namespace Sitecore.Feature.Account.Tests.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Account.Controllers;
    using Data;
    using FakeDb.Sites;
    using FluentAssertions;
    using Foundation.CommerceServer.Infrastructure.Constants;
    using Foundation.CommerceServer.Interfaces;
    using Foundation.CommerceServer.Managers;
    using Foundation.CommerceServer.Models;
    using Foundation.CommerceServer.Models.InputModels;
    using Foundation.Testing.Attributes;
    using Foundation.Testing.Mocks.Contexts;
    using Foundation.Testing.Mocks.Managers;
    using Models;
    using Models.JsonResults;
    using Repositories;
    using Xunit;

    public class AccountControllerTests
    {
        private FakeSiteContext FakeSiteContext { get; }

        public AccountControllerTests()
        {
            FakeSiteContext = new FakeSiteContext(new Collections.StringDictionary { { "rootPath", "/sitecore/content/" + Templates.SiteName.Name } }); ;
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://local", null), new HttpResponse(null));
            Context.Site = FakeSiteContext;
        }

        [Theory]
        [AutoDbData]
        public void Login_Initialized_ShouldRedirectToHomePage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            RequestContext requestContext = new RequestContext(httpContextBase, new RouteData());
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            controller.Url = new UrlHelper(requestContext);
            var model = new LoginModel() { UserName = "fake", Password = "fake" };

            // act
            var result = controller.Login(model);

            // assert
            result.As<RedirectResult>().Should().NotBeNull();
            result.As<RedirectResult>().Url.Should().Be(StorefrontManager.StorefrontHome);
        }

        [Theory]
        [AutoDbData]
        public void Login_NotInitialized_ShouldNotRedirectToHomePage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            RequestContext requestContext = new RequestContext(httpContextBase, new RouteData());
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            controller.Url = new UrlHelper(requestContext);
            var model = new LoginModel() { UserName = "", Password = "" };

            // act
            var result = controller.Login(model);

            // assert
            result.As<RedirectResult>().Should().BeNull();
            result.As<ActionResult>().Should().NotBeNull();
        }

        [Theory]
        [AutoDbData]
        public void MyOrder_IsAuthenticated_ShouldRedirectToOrdersPage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.MyOrder("fake");

                // assert
                result.As<RedirectResult>().Should().BeNull();
                result.As<ActionResult>().Should().NotBeNull();
            }
        }

        [Theory]
        [AutoDbData]
        public void MyOrder_IsNotAuthenticated_ShouldRedirectToLoginPage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.MyOrder("fake");

            // assert
            result.As<RedirectResult>().Should().NotBeNull();
            result.As<RedirectResult>().Url.Should().Be("/login");
        }

        [Theory]
        [AutoDbData]
        public void MyOrders_IsAuthenticated_ShouldRedirectToOrdersPage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.MyOrders();

                // assert
                result.As<RedirectResult>().Should().BeNull();
                result.As<ActionResult>().Should().NotBeNull();
            }
        }

        [Theory]
        [AutoDbData]
        public void MyOrders_IsNotAuthenticated_ShouldRedirectToLoginPage(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            Context.Items["__visitorContext"] = new MockVisitorContext("1", "fake", "1");
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.MyOrders();

            // assert
            result.As<RedirectResult>().Should().NotBeNull();
            result.As<RedirectResult>().Url.Should().Be("/login");
        }

        [Theory]
        [AutoProfileDbData]
        public void RecentOrders_Initialized_ShouldReturnOk(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.RecentOrders();

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<OrdersBaseJsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<OrdersBaseJsonResult>().Orders.Count.Should().BeGreaterThan(0);
                result.As<JsonResult>().Data.As<OrdersBaseJsonResult>().Success.Should().BeTrue();
            }
        }

        [Theory]
        [AutoDbData]
        public void RecentOrders_NotInitialized_ShouldReturnErrors(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.RecentOrders();

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<OrdersBaseJsonResult>().Should().BeNull();
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void Register_Initialized_ShouldReturnOk(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            var model = new RegisterUserInputModel { UserName = "fake", Password = "fake", ConfirmPassword = "fake" };

            // act
            var result = controller.Register(model);

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void Register_ModelNotInitialized_ShouldReturnErrors(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            controller.ModelState.AddModelError("fake", "fake message");

            // act
            var result = controller.Register(new RegisterUserInputModel());

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void Register_ModelNotInitialized_ShouldNotReturnSuccess(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.Register(new RegisterUserInputModel());

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<RegisterBaseJsonResult>().Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void Register_ModelNotInitialized_ShouldReturnException(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.Register(null);

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
            result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_Initialized_ShouldReturnOk(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            var model = new ProfileModel { FirstName = "fake", LastName = "fake" };

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.UpdateProfile(model);

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<ProfileBaseJsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<ProfileBaseJsonResult>().Success.Should().BeTrue();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_ModelNotInitialized_ShouldReturnErrors(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());
            controller.ModelState.AddModelError("fake", "fake message");

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.UpdateProfile(new ProfileModel());

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_UserIsNotAuthenticated_ShouldReturnSuccess(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.UpdateProfile(new ProfileModel());

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_ModelNotInitialized_ShouldNotReturnSuccess(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.UpdateProfile(new ProfileModel());

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateProfile_ModelNotInitialized_ShouldReturnException(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.UpdateProfile(null);

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
            result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetCurrentUser_Initialized_ShouldReturnOk(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("fake", true))
            {
                // act
                var result = controller.GetCurrentUser();

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<UserBaseJsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<UserBaseJsonResult>().Success.Should().BeTrue();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void GetCurrentUser_UserNotIsAuthenticated_ShouldReturnOk(Database db, IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            FakeSiteContext.Database = db;
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.GetCurrentUser();

            // assert
            result.As<JsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<UserBaseJsonResult>().Should().NotBeNull();
            result.As<JsonResult>().Data.As<UserBaseJsonResult>().Success.Should().BeTrue();
        }

        [Theory]
        [AutoProfileDbData]
        public void GetCurrentUser_NotInitialized_ShouldReturnException(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            using (new Sitecore.Security.Accounts.UserSwitcher("exception", true))
            {
                // act
                var result = controller.GetCurrentUser();

                // assert
                result.As<JsonResult>().Should().NotBeNull();
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Errors.Count.Should().BeGreaterThan(0);
                result.As<JsonResult>().Data.As<Base.Models.JsonResults.BaseJsonResult>().Success.Should().BeFalse();
            }
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateUserName_NotInitialized_ShouldBeConcatedWithDomain(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var userName = "fake";
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.UpdateUserName(userName);

            // assert
            result.ShouldBeEquivalentTo(string.Concat(CommerceConstants.ProfilesStrings.CommerceUsersDomainName, @"\", userName));
        }

        [Theory]
        [AutoProfileDbData]
        public void UpdateUserName_Initialized_ShouldNotBeConcatedWithDomain(IOrderManager orderManager, IContactFactory contactFactory, ILogger logger, HttpContextBase httpContextBase)
        {
            // arrange
            var userName = string.Concat(CommerceConstants.ProfilesStrings.CommerceUsersDomainName, @"\", "fake");
            var controller = new AccountController(new MockAccountManager(), contactFactory, logger, new MockAccountRepository());

            // act
            var result = controller.UpdateUserName(userName);

            // assert
            result.ShouldBeEquivalentTo(userName);
        }
    }
}