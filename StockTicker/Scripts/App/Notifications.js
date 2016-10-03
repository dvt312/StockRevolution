// Copyright(c) Daniel Veintimilla 2016.

function Notifications() { }

Notifications.ShowWarningNotification = function (titleIN, messageIN) {
    notify({

        //alert | success | error | warning | info
        type: "warning",
        title: titleIN,
        message: messageIN,
        position: {
            x: "right",
            y: "top"
        },
        icon: '<span class="notification-icon-fa fa fa-exclamation-circle" aria-hidden="true"></span>',
        size: "normal",
        overlay: false,
        closeBtn: true,
        overflowHide: true,
        spacing: 20,
        theme: "default",
        autoHide: true,
        delay: 5000
    });
}

Notifications.ShowErrorNotification = function (titleIN, messageIN) {
    notify({

        //alert | success | error | warning | info
        type: "error",
        title: titleIN,
        message: messageIN,
        position: {
            x: "right",
            y: "top"
        },
        icon: '<span class="notification-icon-fa fa fa-times-circle" aria-hidden="true"></span>',
        size: "normal",
        overlay: false,
        closeBtn: true,
        overflowHide: true,
        spacing: 20,
        theme: "default",
        autoHide: true,
        delay: 5000
    });
}

Notifications.ShowSuccessNotification = function (titleIN, messageIN) {
    notify({

        //alert | success | error | warning | info
        type: "success",
        title: titleIN,
        message: messageIN,
        position: {
            x: "right",
            y: "top"
        },
        icon: '<span class="notification-icon-fa fa fa-check-circle" aria-hidden="true"></span>',
        size: "normal",
        overlay: false,
        closeBtn: true,
        overflowHide: true,
        spacing: 20,
        theme: "default",
        autoHide: true,
        delay: 5000
    });
}

Notifications.ShowInfoNotification = function (titleIN, messageIN) {
    notify({

        //alert | success | error | warning | info
        type: "info",
        title: titleIN,
        message: messageIN,
        position: {
            x: "right",
            y: "top"
        },
        icon: '<span class="notification-icon-fa fa fa-info-circle" aria-hidden="true"></span>',
        size: "normal",
        overlay: false,
        closeBtn: true,
        overflowHide: true,
        spacing: 20,
        theme: "default",
        autoHide: true,
        delay: 5000
    });
}

Notifications.ShowAlertNotification = function (titleIN, messageIN) {
    notify({

        //alert | success | error | warning | info
        type: "alert",
        title: titleIN,
        message: messageIN,
        position: {
            x: "right",
            y: "top"
        },
        icon: '<span class="notification-icon-fa fa fa-bell" aria-hidden="true"></span>',
        size: "normal",
        overlay: false,
        closeBtn: true,
        overflowHide: true,
        spacing: 20,
        theme: "default",
        autoHide: true,
        delay: 5000
    });
}

Notifications.ShowOverlayErrorNotification = function (title, message, cancelFunction, params) {
    var overlayNotification = $('#overlay-notification-div');
    if (CommonUtils.IsValid(overlayNotification)) {
        overlayNotification.tiktok({
            type: 'poptext',
            content: {
                'title': title,
                'content': message
            },
            status: 'error',
            buttonConfirm: 'Ok',
            themeColor: '#F77975',
            titleColor: '#F77975',
            contentAlign: 'center',
            headerAlign: 'center',
            onCancel: function () {
                if (CommonUtils.IsNotUndefinedAndNull(cancelFunction)) {
                    cancelFunction(params);
                }
            }
        });

        overlayNotification.tiktok('show');
    }
}

Notifications.ShowOverlayWarningNotification = function (title, message, cancelFunction, params) {
    var overlayNotification = $('#overlay-notification-div');
    if (CommonUtils.IsValid(overlayNotification)) {
        overlayNotification.tiktok({
            type: 'poptext',
            content: {
                'title': title,
                'content': message
            },
            status: 'warning',
            buttonConfirm: 'Ok',
            themeColor: '#ffeaa8',
            titleColor: '#ffeaa8',
            contentAlign: 'center',
            headerAlign: 'center',
            onCancel: function () {
                if (CommonUtils.IsNotUndefinedAndNull(cancelFunction)) {
                    cancelFunction(params);
                }
            }
        });

        overlayNotification.tiktok('show');
    }
}

Notifications.ShowOverlayConfirmation = function (title, message, confirmFunction, params) {
    var overlayNotification = $('#overlay-notification-div');
    if (CommonUtils.IsValid(overlayNotification)) {
        overlayNotification.tiktok({
            type: 'popbox',
            content: {
                'title': title,
                'content': message
            },
            buttonConfirm: 'Delete',
            buttonCancel: 'Cancel',
            contentAlign: 'center',
            headerAlign: 'center',
            onConfirm: function () {
                confirmFunction(params);
            }
        });

        overlayNotification.tiktok('show');
    }
}