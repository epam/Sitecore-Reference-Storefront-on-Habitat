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


function AddToCartSuccess(data) {
    if (data.Success) {
        $("#addToCartSuccess").show().fadeOut(4000);
        UpdateMiniCart();
    }

    ShowGlobalMessages(data);
    // Update Button State
    $('#AddToCartButton').button('reset');
}

function AddToCartFailure(data) {
    ShowGlobalMessages(data);
    // Update Button State
    $('#AddToCartButton').button('reset');
}

function CheckGiftCardBalance() {
    var giftCardId = $("#GiftCardId").val();
    $("#balance-value").html('');
    if (giftCardId.length === 0) {
        return;
    }

    states('CheckGiftCardBalanceButton', 'loading');
    var data = {};
    data.GiftCardId = giftCardId;
    ClearGlobalMessages();
    AJAXPost("/api/sitecore/giftcard/checkgiftcardbalance", JSON.stringify(data), function (data, success, sender) {
        if (success && data.Success) {
            $("#balance-value").html(data.FormattedBalance);
        }

        $('#CheckGiftCardBalanceButton').button('reset');
        ShowGlobalMessages(data);
    }, this);
}

function SetAddButton() {
    $(document).ready(function () {
        ClearGlobalMessages();
        $("#AddToCartButton").button('loading');
    });
}

//-----------------------------------------------------------------//
//          SIGN UP FOR NOTIFICATION AND STOCK INFO                //
//-----------------------------------------------------------------//
var StockInfoViewModel = function (info) {
    var populate = info != null;
    var self = this;

    self.productId = populate ? ko.observable(info.ProductId) : ko.observable();
    self.variantId = populate ? ko.observable(info.VariantId) : ko.observable();
    self.status = populate ? ko.observable(info.Status) : ko.observable();
    self.count = populate ? ko.observable(info.Count) : ko.observable();
    self.availabilityDate = populate ? ko.observable(info.AvailabilityDate) : ko.observable();
    self.showSingleLabel = populate ? ko.observable(info.Count === 1) : ko.observable(false);
    self.isOutOfStock = populate ? ko.observable(info.Status === "Out-Of-Stock") : ko.observable(false);
    self.canShowSignupForNotification = populate ? ko.observable(info.CanShowSignupForNotification) : ko.observable(false);

    self.showSignUpForNotification = self.canShowSignupForNotification() && self.isOutOfStock();
}