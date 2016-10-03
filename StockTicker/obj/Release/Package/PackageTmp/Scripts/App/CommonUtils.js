// Copyright(c) Daniel Veintimilla 2016.


function CommonUtils() { }

/**
 * Avoid to write unwanted characters in some input fields (especially used with the User Name).
 *
 * @param event To know which is the current character
 *
 * @return false if the character is no valid
 */
CommonUtils.AvoidSpecialChars = function (event) {
    var specialChars = '@@¡!#$^&%*()+=-[]\\/{}|:<>¿?,·~ºª¨´"`;';
    var chr = String.fromCharCode(event.which);
    if (specialChars.indexOf(chr) >= 0) {
        return false;
    }
}

/**
 * Adds a function to the passed component to executed when the user press the enter key.
 *
 * @param component     The component who will have the event.
 * @param eventFunction The function to execute.
 */
CommonUtils.AddEnterListener = function (component, eventFunction) {
    $('#' + component).keydown(function (e) {
        if (e.keyCode == 13) {
            eventFunction();
        }
    });
}

CommonUtils.IsValid = function (element) {
    return CommonUtils.IsNotUndefinedAndNull(element) && element.length > 0;
}

CommonUtils.IsNotUndefinedAndNull = function (element) {
    return element !== undefined && element != null;
}

CommonUtils.AppendToStorage = function(name, data) {
    var old = localStorage.getItem(name);
    if (old === null) old = "";
    localStorage.setItem(name, data);
}

CommonUtils.GetItemFromLocalStorage = function (name) {
    var item = localStorage.getItem(name);
    if (item === null) return undefined;

    return item;
}

CommonUtils.RemoveItemFromLocalStorage = function (name) {
    var item = localStorage.getItem(name);
    if (item != null) localStorage.removeItem(name);

    return item;
}

CommonUtils.EnableHtmlComponentByID = function (elementId, isEnable) {
    if (CommonUtils.IsValid(elementId)) {
        var component = $('#' + elementId);
        if (CommonUtils.IsValid(component)) {
            if (CommonUtils.IsNotUndefinedAndNull(isEnable) && isEnable) {
                component.removeClass('disabled');
            }
            else {
                component.addClass('disabled');
            }
        }
    }
}

CommonUtils.BlurLoadingScreen = function (div) {
    $.LoadingOverlay("show");
    CommonUtils.AddLogoToOverlay(div);
}

CommonUtils.AddLogoToOverlay = function (div) {
    var overlayDiv = $('#' + div);
    if (!CommonUtils.IsValid(overlayDiv)) {
        overlayDiv = $('#loading-notification-div');
    }

    if (overlayDiv.find('sk-fading-circle').length < 1) {
        overlayDiv.html(CommonUtils.CubesLoadingDiv);
    }
}

CommonUtils.RemoveLoadingScreen = function (div) {
    $.LoadingOverlay("hide", true);

    var overlayDiv = $('#' + div);
    if (!CommonUtils.IsValid(overlayDiv)) {
        overlayDiv = $('#loading-notification-div');
    }
    overlayDiv.html('');
}

CommonUtils.CubesLoadingDiv =
    '<div class="sk-cube-grid"><div class="sk-cube sk-cube1"></div><div class="sk-cube sk-cube2"></div>'
        + '<div class="sk-cube sk-cube3"></div><div class="sk-cube sk-cube4"></div>'
        + '<div class="sk-cube sk-cube5"></div><div class="sk-cube sk-cube6"></div>'
        + '<div class="sk-cube sk-cube7"></div><div class="sk-cube sk-cube8"></div>'
        + '<div class="sk-cube sk-cube9"></div></div>';