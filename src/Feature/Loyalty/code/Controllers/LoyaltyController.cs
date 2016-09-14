//-----------------------------------------------------------------------
// <copyright file="LoyaltyController.cs" company="Sitecore Corporation">
//     Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>Defines the LoyaltyController class.</summary>
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

namespace Sitecore.Feature.Loyalty.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sitecore.Commerce.Entities.LoyaltyPrograms;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Base.Controllers;
    using Sitecore.Feature.Base.Filters;
    using Sitecore.Feature.Base.Models.JsonResults;
    using Sitecore.Feature.Loyalty.Models.JsonResults;
    using Sitecore.Foundation.CommerceServer.Interfaces;
    using Sitecore.Mvc.Extensions;

    /// <summary>
    /// Controller for the Loyalty program
    /// </summary>
    public class LoyaltyController : CSBaseController
    {
        private readonly ILogger _logger;

        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyController" /> class.
        /// </summary>
        /// <param name="loyaltyProgramManager">The loyalty program manager.</param>
        /// <param name="cartManager">The cart manager.</param>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="contactFactory">The contact factory.</param>
        /// <param name="logger">The logger.</param>
        public LoyaltyController(
            [NotNull] ILoyaltyProgramManager loyaltyProgramManager,
            [NotNull] ICartManager cartManager,
            [NotNull] IAccountManager accountManager,
            [NotNull] IContactFactory contactFactory,
            [NotNull] ILogger logger)
            : base(accountManager, contactFactory)
        {
            Assert.ArgumentNotNull(loyaltyProgramManager, "loyaltyProgramManager");
            Assert.ArgumentNotNull(cartManager, "cartManager");
            Assert.ArgumentNotNull(contactFactory, "contactFactory");

            this._loyaltyProgramManager = loyaltyProgramManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the loyalty program manager.
        /// </summary>
        /// <value>
        /// The loyalty program manager.
        /// </value>
        private readonly ILoyaltyProgramManager _loyaltyProgramManager;


        #endregion

        #region Controller actions

        /// <summary>
        ///  Main controller action
        /// </summary>
        /// <returns>My loyalty cards view</returns>
        [HttpGet]
        public override ActionResult Index()
        {
            if (!Context.User.IsAuthenticated)
            {
                return Redirect("/login");
            }

            return View("Index");
        }

        /// <summary>
        /// Gets the loyalty cards.
        /// </summary>
        /// <returns>A list of loyalty cards</returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetLoyaltyCards()
        {
            try
            {
                var loyaltyCards = new List<LoyaltyCard>();
                var userResponse = this.AccountManager.GetUser(Context.User.Name);
                var result = new LoyaltyCardsJsonResult(userResponse.ServiceProviderResult);
                if (userResponse.ServiceProviderResult.Success && userResponse.Result != null)
                {
                    loyaltyCards = this.AllCards(result);
                    if (result.Success)
                    {
                        foreach (var card in loyaltyCards)
                        {
                            foreach (var loyaltyRewardPoint in card.RewardPoints)
                            {
                                var point = (LoyaltyRewardPoint)loyaltyRewardPoint;
                                // NOTE: the stab while we don't have 'point.RewardPointId' yet.
                                var transactionResponse = this._loyaltyProgramManager.GetLoyaltyCardTransactions(card.ExternalId, Guid.NewGuid().ToGuidString(), 50);
                                //var transactionResponse = this.LoyaltyProgramManager.GetLoyaltyCardTransactions(card.ExternalId, point.RewardPointId, 50);
                                if (transactionResponse.ServiceProviderResult.Success && transactionResponse.Result != null)
                                {
                                    point.SetPropertyValue("Transactions", transactionResponse.Result.ToList());
                                }

                                result.SetErrors(transactionResponse.ServiceProviderResult);
                            }
                        }
                    }
                }

                result.Initialize(loyaltyCards);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("GetLoyaltyCards", this, e);
                return Json(new BaseJsonResult("GetLoyaltyCards", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Retrieves the active Loyalty cards for the current user
        /// </summary>
        /// <returns>Returns json result for user's loyalty cards</returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        public JsonResult ActiveLoyaltyCards()
        {
            try
            {
                var loyaltyCards = new List<LoyaltyCard>();
                var userResponse = this.AccountManager.GetUser(Context.User.Name);
                var result = new LoyaltyCardsJsonResult(userResponse.ServiceProviderResult);
                if (userResponse.ServiceProviderResult.Success && userResponse.Result != null)
                {
                    loyaltyCards = this.AllCards(result).Take(5).ToList();
                }

                result.Initialize(loyaltyCards);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("ActiveLoyaltyCards", this, e);
                return Json(new BaseJsonResult("ActiveLoyaltyCards", e), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Activates the account.
        /// </summary>
        /// <returns>Status of acitvate action - success of failure</returns>
        [HttpPost]
        [Authorize]
        [ValidateJsonAntiForgeryToken]
        public JsonResult ActivateAccount()
        {
            try
            {
                var loyaltyCards = new List<LoyaltyCard>();
                var userResponse = this.AccountManager.GetUser(Context.User.Name);
                var result = new LoyaltyCardsJsonResult(userResponse.ServiceProviderResult);
                if (userResponse.ServiceProviderResult.Success && userResponse.Result != null)
                {
                    var response = this._loyaltyProgramManager.ActivateAccount(this.CurrentStorefront, this.CurrentVisitorContext);
                    result.SetErrors(response.ServiceProviderResult);

                    if (response.ServiceProviderResult.Success && response.Result != null && !string.IsNullOrEmpty(response.Result.CardNumber))
                    {
                        loyaltyCards = this.AllCards(result);
                    }
                }

                result.Initialize(loyaltyCards);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _logger.LogError("ActivateAccount", this, e);
                return Json(new BaseJsonResult("ActivateAccount", e), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Helpers

        private List<LoyaltyCard> AllCards(LoyaltyCardsJsonResult result)
        {
            var response = this._loyaltyProgramManager.GetLoyaltyCards(this.CurrentStorefront, this.CurrentVisitorContext.UserId);
            var cards = new List<LoyaltyCard>();
            if (response.ServiceProviderResult.Success && response.Result != null)
            {
                cards = response.Result.ToList();
            }

            result.SetErrors(response.ServiceProviderResult);
            return cards;
        }

        #endregion
    }
}