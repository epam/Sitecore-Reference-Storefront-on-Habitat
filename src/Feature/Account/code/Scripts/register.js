﻿//-----------------------------------------------------------------------
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

function RegisterSuccess(data) {
    if (data && data.Success) {
        window.location.href = StorefrontUri("AccountManagement");
    }

    ClearGlobalMessages();
    ShowGlobalMessages(data);
    $("#registerButton").button('reset');
}

function RegisterFailure(data) {
    ShowGlobalMessages(data);
    $("#registerButton").button('reset');
}

function SetLoadingButton(cntx) {
    $(document).ready(function () {
        $("#registerButton").button('loading');
    });
}