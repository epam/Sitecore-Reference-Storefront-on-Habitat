$(function () {
    if ($("#catalogId").length) {
        var currentCategory = $("#catalogId").val();
        var item = $(".navbar-nav").find('a[itemid="' + currentCategory + '"]');
        if (item.length) {
            item.parent().addClass('active');
        }
    }
});