﻿//-----------------------------------------------------------------------
// <copyright file="LoyaltyRewardPointItemJsonResult.cs" company="Sitecore Corporation">
//     Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>Defines the LoyaltyRewardPointItemJsonResult class.</summary>
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

namespace Sitecore.Feature.Loyalty.Models.JsonResults
{
    using System;
    using Sitecore.Commerce.Entities.LoyaltyPrograms;
    using Sitecore.Commerce.Services;
    using Sitecore.Mvc.Extensions;

    /// <summary>
    /// Json result for loyalty reward point operations.
    /// </summary>
    public class LoyaltyRewardPointItemJsonResult : LoyaltyRewardPointItemBaseJsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyRewardPointItemJsonResult"/> class.
        /// </summary>
        public LoyaltyRewardPointItemJsonResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyRewardPointItemJsonResult"/> class.
        /// </summary>
        /// <param name="result">The service provider result.</param>
        public LoyaltyRewardPointItemJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }
        
        /// <summary>
        /// Initializes the specified reward point.
        /// </summary>
        /// <param name="rewardPoint">The reward point.</param>
        public override void Initialize(LoyaltyRewardPoint rewardPoint)
        {
            base.Initialize(rewardPoint);

            this.RewardPointId = Guid.NewGuid().ToGuidString();
            //((Sitecore.Commerce.Connect.DynamicsRetail.Entities.LoyaltyPrograms.LoyaltyRewardPoint)rewardPoint).RewardPointId;            
        }
    }
}