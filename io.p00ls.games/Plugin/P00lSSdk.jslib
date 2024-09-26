mergeInto(LibraryManager.library, {
    p00ls_SaveUserData: function (data) {
        var user = JSON.parse(UTF8ToString(data));
        p00ls.data.saveUserData(user);
    },
    p00ls_GetUserData: function (objectName, callback, fallback) {
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        var parsedObjectName = UTF8ToString(objectName);
        p00ls.data.getUserData().then(function (data) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback,
                data ? JSON.stringify(data) : "null");
        }).catch(function (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error));
        });
    },
    p00ls_GetIdToken: function (objectName, callback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        p00ls.auth.getIdToken().then(function (data) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, data);
        });
    },
    p00ls_GetUserProfile: function () {
        let returnValue = JSON.stringify(p00ls.auth.userProfile);
        var bufferSize = lengthBytesUTF8(returnValue) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnValue, buffer, bufferSize);
        return buffer;
    },
    p00ls_LogEvent: function (eventName, eventParams) {
        var parsedEventName = UTF8ToString(eventName);
        let text = UTF8ToString(eventParams);
        var parsedEventParams = JSON.parse(text);
        p00ls.analytics.logEvent(parsedEventName, parsedEventParams);
    },
    p00ls_HapticFeedback: function (type, style) {
        var parsedType = UTF8ToString(type);
        var parsedStyle = UTF8ToString(style);
        p00ls.tma.haptic(parsedType, parsedStyle);
    },
    p00ls_InitPurchase: function (purchaseParams) {
        let text = UTF8ToString(purchaseParams);
        var parsedPurchaseParams = JSON.parse(text);
        p00ls.purchase.initPurchase(parsedPurchaseParams);
    },
    p00ls_ShowAd: function (objectName, callback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        p00ls.ads.show().then(function (data) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, data ? 1 : 0);
        });
    }

});