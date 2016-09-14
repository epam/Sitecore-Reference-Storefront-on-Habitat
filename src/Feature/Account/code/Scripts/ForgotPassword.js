function SetForgotPasswordProcessingButton() {
    $(document).ready(function () {
        $("#forgotPasswordButton").button('loading');
    });
}

function ForgotPasswordSuccess(cntx) {
    ClearGlobalMessages();
    $("#forgotPasswordButton").button('reset');

    if (cntx.Success && !cntx.HasErrors) {
        window.location.href = StorefrontUri("ForgotPasswordConfirmation?username=" + cntx.UserName + "");
    }

    ShowGlobalMessages(cntx);
}

function ForgotPasswordFailure(cntx) {
    ClearGlobalMessages();
    ShowGlobalMessages(cntx);
    $("#forgotPasswordButton").button('reset');
}