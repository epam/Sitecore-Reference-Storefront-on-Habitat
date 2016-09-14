$(function () {
    AJAXGet(StorefrontUri("api/sitecore/Account/GetRequiredFieldMessage"), null, function (data, success, sender) {
        if (success && data) {
            ko.validation.localize({ required: data.RequiredFieldMessage });
        }
    });
});