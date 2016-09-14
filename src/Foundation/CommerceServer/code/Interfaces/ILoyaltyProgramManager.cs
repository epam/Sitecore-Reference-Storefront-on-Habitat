using System.Collections.Generic;
using Sitecore.Commerce.Entities.LoyaltyPrograms;
using Sitecore.Commerce.Services.LoyaltyPrograms;
using Sitecore.Foundation.CommerceServer.Managers;
using Sitecore.Foundation.CommerceServer.Models;

namespace Sitecore.Foundation.CommerceServer.Interfaces
{
    public interface ILoyaltyProgramManager
    {
        /// <summary>
        /// Activates the loyalty account for the current user cart.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="visitorContext">The visitor context.</param>
        /// <returns>
        /// The manager response where the loyalty card is returned in the Result.
        /// </returns>
        ManagerResponse<JoinLoyaltyProgramResult, LoyaltyCard> ActivateAccount([NotNull] CommerceStorefront storefront, [NotNull] IVisitorContext visitorContext);

        /// <summary>
        /// Gets the loyalty cards.
        /// </summary>
        /// <param name="storefront">The storefront.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The manager response where the enumerable list of loyalty cards is returned in the Result.</returns>
        ManagerResponse<GetLoyaltyCardsResult, IEnumerable<LoyaltyCard>> GetLoyaltyCards([NotNull] CommerceStorefront storefront, [NotNull] string userId);

        /// <summary>
        /// Gets the loyalty card transactions.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="rewardPointId">The reward point identifier.</param>
        /// <param name="rowsCount">The rows count.</param>
        /// <returns>The manager response where the enumarable list of loyalty card trabsactions are returned in the Result.</returns>
        ManagerResponse<GetLoyaltyCardTransactionsResult, IEnumerable<LoyaltyCardTransaction>> GetLoyaltyCardTransactions(string cardNumber, string rewardPointId, int rowsCount);
    }
}