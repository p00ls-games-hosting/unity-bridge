import {p00lsConfiguration, unityConfiguration, loaderUrl, sdkVersion} from './config.js';

const bufferedEvents = [];

window.dispatchUnityEvent = (eventName, param) => bufferedEvents.push([eventName, param]);

(async () => {
    const {p00lsGamesSdk} = await import(`https://assets.prod.p00ls.io/p00ls-games/p00lssdk/${sdkVersion}.js`);
    const sdk = await p00lsGamesSdk(p00lsConfiguration);
    console.log('P00ls SDK loaded');
    loadUnity();

    function loadUnity() {
        document.querySelector("#unity-init").style.display = "none";
        document.querySelector("#unity-progress-bar-empty").style.display = "block";
        var script = document.createElement("script");
        script.src = loaderUrl;
        script.onload = () => {
            sdk.tma.ready();
            createUnityInstance(
                document.querySelector("#unity-canvas"),
                unityConfiguration,
                progress,
            )
                .then(unityLoaded)
                .catch(console.error);
        };
        document.body.appendChild(script);
    }

    function progress(progress) {
        document.querySelector("#unity-progress-bar-full").style.width = `${100 * (progress + 0.1)}%`;
    }

    function unityLoaded(unityInstance) {
        bindEvents(unityInstance);
        document.querySelector("#unity-loading-bar").style.display = "none";
        sdk.purchase.onPurchase((params) => {
            unityInstance.SendMessage('P00lSGamesSDK', 'OnPurchaseCallback', JSON.stringify(params));
        });
        bufferedEvents.forEach(([eventName, params]) => {
            window.dispatchUnityEvent(eventName, params);
        });
        bufferedEvents.splice(0, bufferedEvents.length);
    }

    function bindEvents(unityInstance) {
        const handlers = createEventHandlers(unityInstance);
        window.dispatchUnityEvent = function (eventName, param) {
            if (handlers[eventName]) {
                return handlers[eventName](param);
            }
        }
    }

    function createEventHandlers(unityInstance) {
        return {
            'getuserdata': ({objectName, callback, fallback}) => {
                sdk.data.getUserData().then(function (data) {
                    unityInstance.SendMessage(objectName, callback, data ? JSON.stringify(data) : 'null');
                }).catch(function (error) {
                    unityInstance.SendMessage(objectName, fallback, JSON.stringify(error));
                });
            },
            'readPartData': ({docKey, objectName, callback, fallback}) => {
                sdk.data.readPartData(docKey).then(function (data) {
                    unityInstance.SendMessage(objectName, callback, data ? JSON.stringify(data) : 'null');
                }).catch(function (error) {
                    unityInstance.SendMessage(objectName, fallback, JSON.stringify(error));
                });
            },
            'saveuserdata': ({payload}) => {
                sdk.data.saveUserData(payload);
            },
            'savePartData': ({docKey, payload}) => {
                sdk.data.savePartData(docKey, payload);
            },
            'getidtoken': ({objectName, callback}) => {
                sdk.auth.getIdToken().then(function (data) {
                    unityInstance.SendMessage(objectName, callback, data);
                });
            },
            'getuserprofile': () => {
                return sdk.auth.userProfile;
            },
            'logevent': ({eventName, params}) => {
                sdk.analytics.logEvent(eventName, params);
            },
            'hapticfeedback': ({type, style}) => {
                sdk.tma.haptic(type, style);
            },
            'initpurchase': ({params}) => {
                sdk.purchase.initPurchase(params);
            },
            'showad': ({objectName, callback}) => {
                sdk.ads.show().then(function (data) {
                    unityInstance.SendMessage(objectName, callback, data ? 1 : 0);
                });
            },
            'getreferrallink': ({objectName, callback}) => {
                sdk.referral.getReferralLink().then((link) => unityInstance.SendMessage(objectName, callback, link));
            },
            'getreferrer': ({objectName, callback}) => {
                sdk.referral.getReferrer().then(referrer => unityInstance.SendMessage(objectName, callback, referrer ? JSON.stringify(referrer) : 'null'));
            },
            'getreferees': ({objectName, callback, params}) => {
                sdk.referral.getReferees(params).then(result => unityInstance.SendMessage(objectName, callback, JSON.stringify(result)));
            }
        }
    }
})();