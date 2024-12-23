mergeInto(LibraryManager.library, {
    p00ls_SaveUserData: function (data) {
        dispatchUnityEvent('saveuserdata', {payload: JSON.parse(UTF8ToString(data))})
    },
    p00ls_SavePartData: function (docKey, data) {
        dispatchUnityEvent('savepartdata', {docKey: UTF8ToString(docKey), payload: JSON.parse(UTF8ToString(data))})
    },
    p00ls_GetUserData: function (objectName, callback, fallback) {
        dispatchUnityEvent('getuserdata', {
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback),
            fallback: UTF8ToString(fallback)
        });
    },
    p00ls_ReadPartData: function (docKey, objectName, callback, fallback) {
        dispatchUnityEvent('readpartdata', {
            docKey: UTF8ToString(docKey),
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback),
            fallback: UTF8ToString(fallback)
        });
    },
    p00ls_GetIdToken: function (objectName, callback) {
        dispatchUnityEvent('getidtoken', {objectName: UTF8ToString(objectName), callback: UTF8ToString(callback)});
    },
    p00ls_GetUserProfile: function () {
        const returnValue = JSON.stringify(dispatchUnityEvent('getuserprofile'));
        const bufferSize = lengthBytesUTF8(returnValue) + 1;
        const buffer = _malloc(bufferSize);
        stringToUTF8(returnValue, buffer, bufferSize);
        return buffer;
    },
    p00ls_LogEvent: function (eventName, eventParams) {
        const parsedEventName = UTF8ToString(eventName);
        const parsedEventParams = JSON.parse(UTF8ToString(eventParams));
        dispatchUnityEvent('logevent', {eventName: parsedEventName, params: parsedEventParams});
    },
    p00ls_HapticFeedback: function (type, style) {
        dispatchUnityEvent('hapticfeedback', {type: UTF8ToString(type), style: UTF8ToString(style)});
    },
    p00ls_InitPurchase: function (purchaseParams) {
        const text = UTF8ToString(purchaseParams);
        const parsedPurchaseParams = JSON.parse(text);
        dispatchUnityEvent('initpurchase', {params: parsedPurchaseParams});
    },
    p00ls_ShowAd: function (adType, objectName, callback) {
        dispatchUnityEvent('showad', {
            adType: UTF8ToString(adType),
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    },
    p00ls_GetReferralLink: function (objectName, callback) {
        dispatchUnityEvent('getreferrallink', {objectName: UTF8ToString(objectName), callback: UTF8ToString(callback)});
    },
    p00ls_GetReferrer: function (objectName, callback) {
        dispatchUnityEvent('getreferrer', {objectName: UTF8ToString(objectName), callback: UTF8ToString(callback)});
    },
    p00ls_GetReferees: function (params, objectName, callback) {
        dispatchUnityEvent('getreferees', {
            params: JSON.parse(UTF8ToString(params)),
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    },
    p00ls_GetStatistics: function (objectName, callback) {
        dispatchUnityEvent('getstatistics', {
            callback: UTF8ToString(callback),
            objectName: UTF8ToString(objectName),
        });
    },
    p00ls_UpdateStatistics: function (params) {
        dispatchUnityEvent('updatestatistics', {
            params: JSON.parse(UTF8ToString(params)),
        });
    },
    p00ls_GetUserPosition: function (statistic, objectName, callback) {
        dispatchUnityEvent('getuserposition', {
            callback: UTF8ToString(callback),
            objectName: UTF8ToString(objectName),
            statistic: UTF8ToString(statistic)
        });
    },
    p00ls_GetLeaderboard: function (params, objectName, callback) {
        dispatchUnityEvent('getleaderboard', {
            params: JSON.parse(UTF8ToString(params)),
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    },
    p00ls_GetLeaderboardAround: function (params, objectName, callback) {
        dispatchUnityEvent('getleaderboardaround', {
            params: JSON.parse(UTF8ToString(params)),
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    },
    p00ls_ShareReferralLink: function (message) {
        const parsedMessage = UTF8ToString(message);
        dispatchUnityEvent('sharereferrallink', {
            message: parsedMessage === '' ? undefined : parsedMessage
        });
    },
    p00ls_GetServerTime: function (objectName, callback) {
        dispatchUnityEvent('getservertime', {
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    },
    p00ls_ShareURL: function (url, message) {
        const parsedMessage = UTF8ToString(message);
        dispatchUnityEvent('shareurl', {
            url: UTF8ToString(url),
            message: parsedMessage === '' ? undefined : parsedMessage
        });
    },
    p00ls_OpenURL: function (url) {
        dispatchUnityEvent('openurl', {
            url: UTF8ToString(url)
        });
    },
    p00ls_InitiateWalletChange: function () {
        dispatchUnityEvent('initiatewalletchange');
    },
    p00ls_GetUserWalletAddress: function (objectName, callback) {
        dispatchUnityEvent('getuserwalletaddress', {
            objectName: UTF8ToString(objectName),
            callback: UTF8ToString(callback)
        });
    }
});