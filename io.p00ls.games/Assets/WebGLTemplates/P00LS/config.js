export const sdkVersion = "v4";

const buildUrl = "Build";

export const unityConfiguration = {
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
}

export const loaderUrl = buildUrl + "/{{{ LOADER_FILENAME }}}";