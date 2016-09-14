function SwitchToCurrency(currency) {
    var payload = '{' +
            '"currency":' + '"' + currency + '"' +
          '}';
    AJAXPost(StorefrontUri("api/sitecore/catalog/switchcurrency"), payload, function (data, success, sender) {
        ShowGlobalMessages(data);
        window.location.reload();
    });
}