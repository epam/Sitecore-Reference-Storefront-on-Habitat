$(function () {
    AJAXGet(StorefrontUri("api/sitecore/Checkout/GetStartCheckoutData"), null, function (data, success, sender) {
        if (success && data ) {
            ko.validation.localize({ required: data.RequiredFieldMessage });
            ko.validation.localize({ email: data.RequiredEmailMessage });
            ko.validation.localize({ number: data.RequiredNumberMessage });

            AddMessage('SelectDeliveryFirstMessage', data.SelectDeliveryFirstMessage);
            AddMessage('EmailsMustMatchMessage', data.EmailMustMatchMessage);

            initCheckoutData(data.MapKey);
        }
    });
});