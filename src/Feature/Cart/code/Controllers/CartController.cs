namespace Sitecore.Feature.Cart.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.UI;
    using Foundation.CommerceServer.Models.InputModels;
    using Models.InputModels;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Base.Filters;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Cart.Repositories;
    using Sitecore.Feature.Catalog.Repositories;
    using Sitecore.Feature.GiftCard.Repositories;
    using Sitecore.Foundation.CommerceServer.Interfaces;

    /// <summary>
    /// Defines the shopping cart controller type.
    /// </summary>
    public class CartController : CSBaseController
    {
        private readonly IGiftCardRepository _giftCardRepository;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger _logger;

        public CartController(
            IAccountManager accountManager,
            IContactFactory contactFactory,
            IGiftCardRepository gitCardRepository,
            ICatalogRepository catalogRepository,
            ICartRepository cartRepository,
            ILogger logger)
            : base(accountManager, contactFactory)
        {
            ContactFactory = contactFactory;
            _catalogRepository = catalogRepository;
            _giftCardRepository = gitCardRepository;
            _cartRepository = cartRepository;
            _logger = logger;
        }

        #region Controller actions

        /// <summary>
        ///     main cart controller action
        /// </summary>
        /// <returns>the cart view</returns>
        [HttpGet]
        public override ActionResult Index()
        {
            return View("ShoppingCart");
        }

        public ActionResult AddGiftCardToCart()
        {
            //this.Item = SearchNavigation.GetProduct(StorefrontManager.CurrentStorefront.GiftCardProductId, CurrentCatalog.Name);
            var productViewModel = _giftCardRepository.GetGiftCardViewModel(this.Item, this.CurrentRendering);

            return View("AddGiftCardToCart", productViewModel);
        }

        public ActionResult AddToCart()
        {
            var productViewModel = _catalogRepository.GetWildCardProductViewModel(Item, CurrentRendering);

            return View("AddToCart", productViewModel);
        }

        /// <summary>
        /// Applies the discount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The partial view of the updated cart
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult ApplyDiscount(DiscountInputModel model)
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

                var result = _cartRepository.ApplyDiscount(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("ApplyDiscount", this, e);
                return Json(new BaseJsonResult("ApplyDiscount", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// true if the product was added
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult AddCartLine(AddCartLineInputModel inputModel)
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

                var result = _cartRepository.AddCartLine(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("AddCartLine", this);
                return Json(new BaseJsonResult("AddCartLine", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Adds the items to cart.
        /// </summary>
        /// <param name="inputModels">The input model.</param>
        /// <returns>
        /// Returns json result with add items to cart operation status
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult AddCartLines(IEnumerable<AddCartLineInputModel> inputModels)
        {
            try
            {
                Assert.ArgumentNotNull(inputModels, "inputModels");
                var validationResult = new BaseJsonResult();
                this.ValidateModel(validationResult);

                if (validationResult.HasErrors)
                {
                    return Json(validationResult, JsonRequestBehavior.AllowGet);
                }

                var result = _cartRepository.AddCartLines(inputModels);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("AddCartLines", this);
                return Json(new BaseJsonResult("AddCartLines", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Deletes a line item from a cart
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// the partial view of the updated cart
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult DeleteLineItem(DeleteCartLineInputModel model)
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

                var result = _cartRepository.DeleteLineItem(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("DeleteLineItem", this, e);

                return Json(new BaseJsonResult("DeleteLineItem", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the current cart.
        /// </summary>
        /// <returns>
        /// Returns the Json cart result.
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult GetCurrentCart()
        {
            try
            {
                var cartResult = _cartRepository.GetCurrentCart();

                return Json(cartResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetCurrentCart", this, e);
                return Json(new BaseJsonResult("GetCurrentCart", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// The action for rendering the basket view
        /// </summary>
        /// <param name="updateCart">if set to <c>true</c> [update cart].</param>
        /// <returns>
        /// The MiniCart view.
        /// </returns>
        public ActionResult MiniCart(bool updateCart = false)
        {
            return PartialView("MiniCart");
        }

        /// <summary>
        /// Removes a discount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The partial view of the updated cart
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult RemoveDiscount(DiscountInputModel model)
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

                var result = _cartRepository.RemoveDiscount(model);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("RemoveDiscount", this, e);
                return Json(new BaseJsonResult("RemoveDiscount", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Baskets the update.
        /// </summary>
        /// <param name="updateCart">if set to <c>true</c> [update cart].</param>
        /// <returns>
        /// Returns the Json cart result.
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult UpdateMiniCart(bool updateCart = false)
        {
            try
            {
                var result = _cartRepository.UpdateMiniCart(updateCart);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("BasketUpdate", this, e);

                return Json(new BaseJsonResult("BasketUpdate", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Update a cart line item
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The partial view of the updated cart
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public JsonResult UpdateLineItem(UpdateCartLineInputModel inputModel)
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

                var result = _cartRepository.UpdateLineItem(inputModel);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("UpdateLineItem", this, e);
                return Json(new BaseJsonResult("UpdateLineItem", e), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}