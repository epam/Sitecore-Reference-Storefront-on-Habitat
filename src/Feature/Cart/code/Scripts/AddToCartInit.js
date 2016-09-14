$(document).on("ProductCatalogInitialized", function(e) {
    if (window.variantCombinationsArray) {
        VariantSelectionChanged();
    }
});