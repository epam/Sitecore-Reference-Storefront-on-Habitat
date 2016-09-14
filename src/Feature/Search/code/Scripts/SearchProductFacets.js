function FireFacetApplied(facetName, facetValue, isApplied, url) {
    var facet = facetName + "=" + facetValue;
    AJAXPost(StorefrontUri("api/sitecore/catalog/facetapplied"), "{\"facetValue\":\"" + facet + "\", \"isApplied\":" + isApplied + "}", function (data, success, sender) {
        window.location.href = url;
    });
}