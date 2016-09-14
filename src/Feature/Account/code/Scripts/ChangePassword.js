function ChangePasswordSuccess(cntx) {
    ClearGlobalMessages();
    $("#changePasswordButton").button('reset');

    if (cntx.Success && !cntx.HasErrors) {
        window.location.href = StorefrontUri("AccountManagement");
    }

    ShowGlobalMessages(cntx);
}

function ChangePasswordFailure(cntx) {
    ClearGlobalMessages();
    ShowGlobalMessages(cntx);
    $("#changePasswordButton").button('reset');
}

function SetChangePasswordProcessingButton() {
    $(document).ready(function () {
        $("#changePasswordButton").button('loading');
    });
}