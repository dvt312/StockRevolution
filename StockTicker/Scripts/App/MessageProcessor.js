// Copyright(c) Daniel Veintimilla 2016.

function MessageProcessor() { }

MessageProcessor.ProcessMessages = function (messages) {
    for (var index = 0; index < messages.length; index++) {
        var message = messages[index];
        if (CommonUtils.IsNotUndefinedAndNull(message)) {
            switch (message.MessageType) {
                case 0: // informative
                    Notifications.ShowInfoNotification(message.Title, message.Message);
                    break;
                case 1: // warning
                    Notifications.ShowWarningNotification(message.Title, message.Message);
                    break;
                case 2: // error
                    Notifications.ShowErrorNotification(message.Title, message.Message);
                    break;
                default: // default
                    Notifications.ShowAlertNotification(message.Title, message.Message);
                    break;
            }
        }
    }
}

MessageProcessor.ProcessFailureRequest = function (error) {
    CommonUtils.RemoveLoadingScreen();
    if (CommonUtils.IsNotUndefinedAndNull(error)) {
        if (error.status === 503) {
            Notifications.ShowOverlayErrorNotification(
                'Service Unavailable',
                'Service is unavailable, please contact system administrator.',
                UserLoginHandler.RedirectToURL,
                { 'urlRedirect': UserLoginHandler.HomeIndexURL });
        }
        else if (error.status === 500) {
            var objException = JSON.parse(error.responseText);
            if (CommonUtils.IsNotUndefinedAndNull(objException)) {
                Notifications.ShowErrorNotification("Exception Error",
                    objException.Message + '.<br><br>StackTrace: ' + objException.StackTrace);
                if (error.responseText.indexOf('Lifetime validation failed') !== -1) {
                    Notifications.ShowOverlayErrorNotification(
                        'Session not valid',
                        'Your session has been terminated, please login again and continue using the app.',
                        UserLoginHandler.RedirectToURL,
                        { 'urlRedirect': UserLoginHandler.HomeIndexURL });
                }
            }
        }
    }
}