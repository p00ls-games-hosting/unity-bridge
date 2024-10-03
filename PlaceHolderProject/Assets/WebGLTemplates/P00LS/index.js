import {p00lsGamesSdk} from 'https://assets.prod.p00ls.io/p00ls-games/p00lssdk/{{{ P00LS_SDK_VERSION }}}.js';
const firebaseConfig = {
    apiKey: "{{{ P00LS_API_KEY }}}",
    authDomain: "{{{ P00LS_AUTH_DOMAIN }}}",
    projectId: "{{{ P00LS_PROJECT_ID }}}",
    storageBucket: "{{{ P00LS_STORAGE_BUCKET }}}",
    messagingSenderId: "{{{ P00LS_MESSAGING_SENDER_ID }}}",
    appId: "{{{ P00LS_APP_ID }}}",
    env: "{{{ P00LS_ENV }}}",
    gameId: "{{{ P00LS_GAME_ID }}}",
    #if P00LS_BLOCK_ID
    blockId: "{{{ P00LS_BLOCK_ID }}}",
    #endif
};

var buildUrl = "Build";
var loaderUrl = buildUrl + "/{{{ LOADER_FILENAME }}}";
var config = {
    arguments: [],
    dataUrl: buildUrl + "/{{{ DATA_FILENAME }}}",
    frameworkUrl: buildUrl + "/{{{ FRAMEWORK_FILENAME }}}",
    #if USE_THREADS
    workerUrl: buildUrl + "/{{{ WORKER_FILENAME }}}",
    #endif
    #if USE_WASM
    codeUrl: buildUrl + "/{{{ CODE_FILENAME }}}",
    #endif
    #if SYMBOLS_FILENAME
    symbolsUrl: buildUrl + "/{{{ SYMBOLS_FILENAME }}}",
    #endif
    streamingAssetsUrl: "StreamingAssets",
    companyName: {{{ JSON.stringify(COMPANY_NAME) }}},
    productName: {{{ JSON.stringify(PRODUCT_NAME) }}},
    productVersion: {{{ JSON.stringify(PRODUCT_VERSION) }}}
};


p00lsGamesSdk(firebaseConfig).then(sdk => {
    window.p00ls = sdk;
    console.log('P00ls SDK loaded');
    loadUnity();
});

function loadUnity() {
    document.querySelector("#unity-loading-bar").style.display = "block";
    var script = document.createElement("script");
    script.src = loaderUrl;
    script.onload = () => {
        window.p00ls.tma.ready();
        createUnityInstance(
            document.querySelector("#unity-canvas"),
            config,
            (progress) => {
                document.querySelector("#unity-progress-bar-full").style.width = 100 * progress + "%";
            }
        )
        .then(function (unityInstance) {
            document.querySelector("#unity-loading-bar").style.display = "none";
            window.p00ls.purchase.onPurchase((params) => {
                unityInstance.SendMessage('P00lSGamesSDK', 'OnPurchaseCallback', JSON.stringify(params));
                });
            window.unityInstance = unityInstance;
        })
        .catch(console.error);
    };
    document.body.appendChild(script);   
}