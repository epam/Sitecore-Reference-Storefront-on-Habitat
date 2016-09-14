//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Sitecore Corporation">
//     Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>Defines the AccountController class.</summary>
//-----------------------------------------------------------------------
// Copyright 2016 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------

using Sitecore.Globalization;
using System.Globalization;

namespace Sitecore.Feature.Account.Controllers
{
    using Sitecore.Commerce.Connect.CommerceServer.Configuration;
    using Sitecore.Commerce.Entities.Customers;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Account.Interfaces;
    using Sitecore.Feature.Account.Models;
    using Sitecore.Feature.Account.Models.InputModels;
    using Sitecore.Feature.Account.Models.JsonResults;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Base.Filters;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Used to handle all account actions
    /// </summary>
    public class AccountController : CSBaseController
    {
        private readonly RenderingModel _model;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="orderManager">The order manager.</param>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="contactFactory">The contact factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="accountRepository"></param>
        public AccountController(
            [NotNull] IAccountManager accountManager,
            [NotNull] IContactFactory contactFactory,
            [NotNull] ILogger logger,
            [NotNull] IAccountRepository accountRepository)
            : base(accountManager, contactFactory)
        {
            _model = new RenderingModel();
            _accountRepository = accountRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current rendering model.
        /// </summary>
        /// <value>
        /// The current rendering model.
        /// </value>
        internal RenderingModel CurrentRenderingModel
        {
            get
            {
                _model.Initialize(this.CurrentRendering);
                return _model;
            }
        }

        #region Controller actions
        
        /// <summary>
        /// The default action for the main page for the account section
        /// </summary>
        /// <returns>The view for the section</returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public override ActionResult Index()
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            return View("Index");
        }

        /// <summary>
        /// Addressees this instance.
        /// </summary>
        /// <returns>The view to display address book</returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Addresses()
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            return View("Addresses");
        }

        /// <summary>
        /// Gets the data which is required by the javascript of view "Addresses.cshtml", sitecore rendering "AddressBook".
        /// The data includes the required field validation message.
        /// This action method is intended to be used in an ajax manner.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetRequiredFieldMessage()
        {
            return Json(
                new
                {
                    RequiredFieldMessage = StorefrontManager.GetHtmlSystemMessage("RequiredFieldMessage").ToHtmlString()
                },
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Handles deleting of an address and removing it from a user's profile
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The JsonResult with deleting operation status
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult AddressDelete(DeletePartyInputModelItem model)
        {
            try
            {
                Assert.ArgumentNotNull(model, "model");
                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _accountRepository.DeleteAddresses(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("AddressDelete", this, e);

                return Json(new BaseJsonResult("AddressDelete", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Handles updates to an address
        /// </summary>
        /// <param name="model">Any changes to the address</param>
        /// <returns>
        /// The view to display the updated address
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult AddressModify(PartyInputModelItem model)
        {
            try
            {
                Assert.ArgumentNotNull(model, "model");
                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _accountRepository.ModifyAddress(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("AddressModify", this, e);

                return Json(new BaseJsonResult("AddressModify", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Address Book in the Home Page
        /// </summary>
        /// <returns>The list of addresses</returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult AddressList()
        {
            try
            {
                var result = _accountRepository.GetAddressList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("AddressList", this, e);

                return Json(new BaseJsonResult("AddressList", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <returns>Chagne password view</returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult ChangePassword()
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            return View("ChangePassword");
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The result in Json format.
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult ChangePassword(ChangePasswordInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "ChangePasswordInputModel");
                ChangePasswordBaseJsonResult result = new ChangePasswordBaseJsonResult();
                this.ValidateModel(result);

                if (result.HasErrors)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result = _accountRepository.ChangePassword(inputModel);

                return Json(result);
            }

            catch (Exception e)
            {
                _logger.LogError("ChangePassword", this, e);

                return Json(new BaseJsonResult("ChangePassword", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Displays the Profile Edit Page.
        /// </summary>
        /// <returns>
        /// Profile Edit Page
        /// </returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult EditProfile()
        {
            var model = new ProfileModel();

            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            var commerceUser = this.AccountManager.GetUser(Context.User.Name).Result;

            if (commerceUser == null)
            {
                return View("EditProfile", model);
            }

            model.FirstName = commerceUser.FirstName;
            model.Email = commerceUser.Email;
            model.EmailRepeat = commerceUser.Email;
            model.LastName = commerceUser.LastName;
            model.TelephoneNumber = commerceUser.GetPropertyValue("Phone") as string;

            return View("EditProfile", model);
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <returns>The view to display.</returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult ForgotPassword()
        {
            var datasource = this.Item;
            var subjectField = datasource.Fields[Templates.EmailSender.Fields.Subject.ToString()];

            if (subjectField == null)
            {
                var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.CouldNotFindEmailSubjectMessageError);
                Log.Error(Translate.Text(string.Format(CultureInfo.InvariantCulture, message, datasource.TemplateName)), this);
            }

            var bodyField = datasource.Fields[Templates.EmailSender.Fields.Body.ToString()];

            if (bodyField == null)
            {
                var message = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.CouldNotFindEmailBodyMessageError);
                Log.Error(Translate.Text(string.Format(CultureInfo.InvariantCulture, message, datasource.TemplateName)), this);
            }

            return View("ForgotPassword");
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The result in json format</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult ForgotPassword(ForgotPasswordInputModel model)
        {
            try
            {
                Assert.ArgumentNotNull(model, "model");
                ForgotPasswordBaseJsonResult result = new ForgotPasswordBaseJsonResult();
                this.ValidateModel(result);

                if (result.HasErrors)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var resetResponse = _accountRepository.ResetPassword(model);

                if (!resetResponse.ServiceProviderResult.Success)
                {
                    return Json(new ForgotPasswordBaseJsonResult(resetResponse.ServiceProviderResult));
                }

                result.Initialize(model.Email);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("ForgotPassword", this, e);

                return Json(new BaseJsonResult("ForgotPassword", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Changes the password confirmation.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The forgot password confirmation view.</returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult ForgotPasswordConfirmation(string userName)
        {
            ViewBag.UserName = userName;

            return View("ForgotPasswordConfirmation");
        }

        /// <summary>
        /// An action to handle displaying the login form
        /// </summary>
        /// <param name="returnUrl">A location to redirect the user to</param>
        /// <returns>The view to display to the user</returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View("Login");
        }

        /// <summary>
        /// Handles a user trying to login
        /// </summary>
        /// <param name="model">The user's login details</param>
        /// <returns>The view to display to the user</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Login(LoginModel model)
        {
            var anonymousVisitorId = this.CurrentVisitorContext.UserId;

            if (ModelState.IsValid && _accountRepository.Login(UpdateUserName(model.UserName), model.Password, model.RememberMe))
            {
                return RedirectToLocal(StorefrontManager.StorefrontHome);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");

            return View("Login");
        }

        /// <summary>
        /// Handles a user trying to log off
        /// </summary>
        /// <returns>The view to display to the user after logging off</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            this.AccountManager.Logout();

            return RedirectToLocal(StorefrontManager.StorefrontHome);
        }

        /// <summary>
        /// Orders the detail.
        /// </summary>
        /// <param name="id">The order confirmation Id.</param>
        /// <returns>
        /// The view to display order details
        /// </returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult MyOrder(string id)
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            var response = _accountRepository.GetOrder(id);
            ViewBag.IsItemShipping = response.Shipping != null && response.Shipping.Count > 1 && response.Lines.Count > 1;

            return View("MyOrder", response);
        }

        /// <summary>
        /// Orderses the history.
        /// </summary>
        /// <returns>
        /// The view to display all orders for current user
        /// </returns>
        [HttpGet]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult MyOrders()
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            var userName = Context.User.Name;
            var orders = _accountRepository.GetOrders(userName);

            return View("MyOrders", orders);
        }

        /// <summary>
        /// Recent Orders PlugIn for Account Management Home Page
        /// </summary>
        /// <returns>The view to display recent orders</returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult RecentOrders()
        {
            try
            {
                var userName = Context.User.Name;
                var result = _accountRepository.GetRecentOrders(userName);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("RecentOrders", this, e);

                return Json(new BaseJsonResult("RecentOrders", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Handles displaying a form for the user to login
        /// </summary>
        /// <returns>The view to display to the user</returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// Handles a user trying to register
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The view to display to the user after they register
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult Register(RegisterUserInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "RegisterInputModel");
                RegisterBaseJsonResult result = new RegisterBaseJsonResult();
                this.ValidateModel(result);

                if (result.HasErrors)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                result = _accountRepository.RegisterUser(inputModel);

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogError("Register", this, e);

                return Json(new BaseJsonResult("Register", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The result in Json format.</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult UpdateProfile(ProfileModel model)
        {
            try
            {
                Assert.ArgumentNotNull(model, "UpdateProfileInputModel");
                var result = new ProfileBaseJsonResult();
                this.ValidateModel(result);

                if (result.HasErrors)
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!Context.User.IsAuthenticated || Context.User.Profile.IsAdministrator)
                {
                    return Json(result);
                }

                result = _accountRepository.UpdateProfile(model);

                return Json(result);
            }
            catch (Exception e)
            {
                _logger.LogError("UpdateProfile", this, e);

                return Json(new BaseJsonResult("UpdateProfile", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>The current authenticated user info</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetCurrentUser()
        {
            try
            {
                if (!Context.User.IsAuthenticated || Context.User.Profile.IsAdministrator)
                {
                    var anonymousResult = new UserBaseJsonResult();
                    anonymousResult.Initialize(new CommerceUser());
                    return Json(anonymousResult, JsonRequestBehavior.AllowGet);
                }

                var result = _accountRepository.GetCurrentUser(Context.User.Name);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetCurrentUser", this, e);

                return Json(new BaseJsonResult("GetCurrentUser", e), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Concats the user name with the current domain if it missing
        /// </summary>
        /// <param name="userName">The user's user name</param>
        /// <returns>The updated user name</returns>
        public virtual string UpdateUserName(string userName)
        {
            var defaultDomain = CommerceServerSitecoreConfig.Current.DefaultCommerceUsersDomain;

            if (string.IsNullOrWhiteSpace(defaultDomain))
            {
                defaultDomain = CommerceConstants.ProfilesStrings.CommerceUsersDomainName;
            }

            return !userName.StartsWith(defaultDomain, StringComparison.OrdinalIgnoreCase) ? string.Concat(defaultDomain, @"\", userName) : userName;
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }

        #endregion
    }
}
