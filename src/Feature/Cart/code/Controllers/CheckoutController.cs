
using Sitecore.Feature.Cart.Repositories;

namespace Sitecore.Feature.Cart.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Base.Filters;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Cart.Models;
    using Sitecore.Feature.Cart.Models.InputModels;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Foundation.CommerceServer.Models.InputModels;

    /// <summary>
    /// Handles all calls to checkout
    /// </summary>
    public class CheckoutController : CSBaseController
    {
        private readonly ILogger _logger;
        private readonly ICheckoutRepository _checkoutRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController" /> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="contactFactory">The contact factory.</param>
        /// <param name="checkoutRepository">The checkout repository.</param>
        /// <param name="logger">The logger.</param>
        public CheckoutController(
            [NotNull] IAccountManager accountManager,
            [NotNull] IContactFactory contactFactory,
            [NotNull] ICheckoutRepository checkoutRepository,
            [NotNull] ILogger logger)
            : base(accountManager, contactFactory)
        {
            _logger = logger;
            _checkoutRepository = checkoutRepository;
        }

        #region Controller actions

        /// <summary>
        /// Handles the index view of the controller
        /// </summary>
        /// <returns>The action for this view</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult StartCheckout()
        {
            var cart = _checkoutRepository.GetCommerceCart();

            if (cart.Lines == null || !cart.Lines.Any())
            {
                var cartPageUrl = StorefrontManager.StorefrontUri("/shoppingcart");
                return Redirect(cartPageUrl);
            }

            return View("Checkout", new CartRenderingModel(cart));
        }

        /// <summary>
        /// Gets the Orders confirmation.
        /// </summary>
        /// <param name="confirmationId">The confirmation identifier.</param>
        /// <returns>
        /// Order Confirmation view
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult OrderConfirmation([Bind(Prefix = StorefrontConstants.QueryStrings.ConfirmationId)] string confirmationId)
        {
            var viewModel = new OrderConfirmationViewModel();
            var order = _checkoutRepository.GetCommerceOrder(confirmationId);
            viewModel.Initialize(this.CurrentRendering, confirmationId, order);

            return View("OrderConfirmation", viewModel);
        }

        /// <summary>
        /// Retrieves data required to start the checkout process.
        /// </summary>
        /// <returns>Data required to start the checkout process.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetCheckoutData()
        {
            try
            {
                var result = _checkoutRepository.GetCheckoutData(Sitecore.Context.User);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetCheckoutData", this, e);
                return Json(new BaseJsonResult("GetCheckoutData", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Submits the order in json.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The result in Json format.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult SubmitOrder(SubmitOrderInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "inputModel");

                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _checkoutRepository.SubmitOrder(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("SubmitOrder", this, e);
                return Json(new BaseJsonResult("SubmitOrder", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the available shipping methods.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The available shipping methods.
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetShippingMethods(GetShippingMethodsInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "inputModel");

                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _checkoutRepository.GetShippingMethods(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetShippingMethods", this, e);
                return Json(new BaseJsonResult("GetShippingMethods", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Sets the shipping methods.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The action for this view
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult SetShippingMethods(SetShippingMethodsInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "inputModel");

                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _checkoutRepository.SetShippingMethods(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("SetShippingMethods", this, e);
                return Json(new BaseJsonResult("SetShippingMethods", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Sets the payment methods.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The CartJsonResult.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult SetPaymentMethods(PaymentInputModel inputModel)
        {
            try
            {
                Assert.ArgumentNotNull(inputModel, "inputModel");

                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _checkoutRepository.SetPaymentMethods(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("SetPaymentMethods", this, e);
                return Json(new BaseJsonResult("SetPaymentMethods", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the nearby stores.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// A list of stores
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetNearbyStoresJson(GetNearbyStoresInputModel inputModel)
        {
            Assert.ArgumentNotNull(inputModel, "inputModel");

            return Json(new { success = false, errors = new List<string> { "Not supported in CS Connect" } });
        }

        /// <summary>
        /// Gets the data which is required by the javascript of partial view "StartCheckout.cshtml".
        /// It includes some messages and a map key.
        /// This action method is intended to be used in an ajax manner.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetStartCheckoutData()
        {
            return Json(new
            {
                RequiredFieldMessage = StorefrontManager.GetHtmlSystemMessage("RequiredFieldMessage").ToHtmlString(),
                RequiredEmailMessage = StorefrontManager.GetHtmlSystemMessage("RequiredEmailMessage").ToHtmlString(),
                RequiredNumberMessage = StorefrontManager.GetHtmlSystemMessage("RequiredNumberMessage").ToHtmlString(),
                SelectDeliveryFirstMessage = StorefrontManager.GetHtmlSystemMessage("SelectDeliveryFirstMessage").ToHtmlString(),
                EmailMustMatchMessage = StorefrontManager.GetHtmlSystemMessage("EmailsMustMatchMessage").ToHtmlString(),
                MapKey = StorefrontManager.CurrentStorefront.GetMapKey()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the available states.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of states based on the country
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetAvailableStates(GetAvailableStatesInputModel model)
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

                var result = _checkoutRepository.GetAvailableStates(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetAvailableStates", this, e);
                return Json(new BaseJsonResult("GetAvailableStates", e), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
