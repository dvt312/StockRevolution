// Copyright(c) Daniel Veintimilla 2016.

function UserLoginHandler() { }

UserLoginHandler.LoginUserFunction = function (loginURL) {
    var $myForm = $('#loginForm');
    if (!$myForm[0].checkValidity()) {
        $('<input type="submit">').hide().appendTo($myForm).click().remove();
    }
    else {
        CommonUtils.BlurLoadingScreen();
        setTimeout(function () {
            $('#loginForm').off('keydown');
            var username = $('#username').val();
            var userpass = $('#userpass').val();
            $.ajax(
                'http://localhost/CSharpAssignment/LoginWS.asmx/LogIn',
                {
                    cache: true,
                    async: true,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ userName: username, password: userpass }),
                    success: function (json) {
                        SuccessLoginFunction(json, loginURL);
                    },
                    error: function (error) {
                        FailureLoginFunction(error);
                    }
                });
        }, 1000);
        
    }
}

UserLoginHandler.RegisterUserFunction = function (registerURL) {
    var $myForm = $('#registerForm');
    if (!$myForm[0].checkValidity()) { 
        $('<input type="submit">').hide().appendTo($myForm).click().remove();
    }
    else {
        CommonUtils.BlurLoadingScreen();
        setTimeout(function () {
            $('#registerForm').off('keydown');
            var docID = $('#doc-id-field').val();
            if (CommonUtils.IsValid(docID) && (Number(docID) < 0 || Number(docID) > 2147483647)) {
                CommonUtils.RemoveLoadingScreen();
                Notifications.ShowWarningNotification(
                    'Doc ID overflow', 'Please insert an ID number between 0 and 2147483647');
            }
            else {
                var name = $('#name-field').val();
                var lastname = $('#lastname-field').val();
                var mail = $('#mail-field').val();
                var phone = $('#phone-field').val();
                var username = $('#username-field').val();
                var userpass = $('#userpass-field').val();

                $.ajax(
                    'http://localhost/CSharpAssignment/LoginWS.asmx/RegisterUser',
                    {
                        cache: true,
                        async: true,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            docId: Number(docID.trim()),
                            name: name.trim(),
                            lastName: lastname.trim(),
                            mail: mail.trim(),
                            phone: phone.trim(),
                            userName: username.trim(),
                            password: userpass
                        }),
                        success: function (json) {
                            UserLoginHandler.SuccessRegisterUserFunction(json, registerURL);
                        },
                        error: function (error) {
                            UserLoginHandler.FailureRegisterFunction(error);
                        }
                    });
            }
        }, 1000);
    }
}

UserLoginHandler.SuccessRegisterUserFunction = function (json, registerURL) {
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsValid(jsonObj.Messages)) {
        CommonUtils.RemoveLoadingScreen();
        MessageProcessor.ProcessMessages(jsonObj.Messages);
    }
    else {
        CommonUtils.RemoveLoadingScreen();
        var jsonModel = jsonObj.Model;
        if (CommonUtils.IsNotUndefinedAndNull(jsonModel)) {
            var userName = jsonModel.UserName;
            if (CommonUtils.IsNotUndefinedAndNull(userName)) {
                Notifications.ShowSuccessNotification('Success', 'User ' + userName + ' successfully created.');
                setTimeout(function () {
                    UserLoginHandler.RedirectToURL(registerURL);
                }, 3000);
            }
        }
    }
}

UserLoginHandler.FailureRegisterFunction = function (error) {
    CommonUtils.AddEnterListener('registerForm', UserLoginHandler.RegisterUserFunction);
    CommonUtils.RemoveLoadingScreen();
    MessageProcessor.ProcessFailureRequest(error);
}

var SuccessLoginFunction = function (result, loginURL) {
    var jsonObj = JSON.parse(result.d);
    if (CommonUtils.IsValid(jsonObj.Messages)) {
        CommonUtils.RemoveLoadingScreen();
        MessageProcessor.ProcessMessages(jsonObj.Messages);
    }
    else {
        var $myForm = $('#loginForm');
        $('<input type="submit">').hide().appendTo($myForm).click().remove();
        CommonUtils.AppendToStorage(UserLoginHandler.DefaultTokenStorageKey, jsonObj.Model.Token);

        $.ajax(
            loginURL.setTokenSessionURL,
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: jsonObj.Model.Token }),
                success: function (json) {
                    ProcessLoginRedirection(json, loginURL);
                },
                error: function (error) {
                    FailureLoginFunction(error);
                }
            });
    }
}


var ProcessLoginRedirection = function (result, loginURL) {
    if (result.wasSaved) {
        CommonUtils.RemoveLoadingScreen();
        window.open(loginURL.stockListURL, "_self");
    }
}

var FailureLoginFunction = function (error) {
    CommonUtils.AddEnterListener('loginForm', UserLoginHandler.LoginUserFunction);
    CommonUtils.RemoveLoadingScreen();
    MessageProcessor.ProcessFailureRequest(error);
}

UserLoginHandler.Logout = function () {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        CommonUtils.BlurLoadingScreen();
        setTimeout(function () {
            $.ajax(
           'http://localhost/CSharpAssignment/LoginWS.asmx/Logout',
           {
               cache: true,
               async: true,
               type: 'POST',
               contentType: 'application/json; charset=utf-8',
               data: JSON.stringify({ token: token }),
               success: function (json) { UserLoginHandler.ProcessLogoutResponse(json); },
               error: function (error) { UserLoginHandler.ProcessFailureRequest(error); }
           });
        }, 3000);
       
    }
}

UserLoginHandler.ProcessLogoutResponse = function (json) {
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsValid(jsonObj.Messages)) {
        MessageProcessor.ProcessMessages(jsonObj.Messages);
    }
    else {
        CommonUtils.RemoveItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
        var logoutA = $('#' + UserLoginHandler.LogoutLinkID);
        if (CommonUtils.IsValid(logoutA)) {
            var deleteTokenSession = logoutA.data('delete-cookie');
            if (CommonUtils.IsValid(deleteTokenSession)) {
                $.ajax(
                    deleteTokenSession,
                    {
                        cache: true,
                        async: true,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ }),
                        success: function (json) {
                            if (json.wasDeleted) {
                                CommonUtils.RemoveLoadingScreen();
                                window.open(logoutA.data('redirect-url'), "_self");
                            }
                        },
                        error: function (error) { UserLoginHandler.ProcessFailureRequest(error); }
                    });
            }
        }
    }
}

UserLoginHandler.RedirectToURL = function (obj) {
    if (CommonUtils.IsNotUndefinedAndNull(obj)) {
        window.open(obj.urlRedirect, "_self");
    }
}

UserLoginHandler.DefaultTokenStorageKey = 'local-storage-token-session';
UserLoginHandler.LogoutLinkID = 'logout-link-action';
UserLoginHandler.HomeIndexURL = ''