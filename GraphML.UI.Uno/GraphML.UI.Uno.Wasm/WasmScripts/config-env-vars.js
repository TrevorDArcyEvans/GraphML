//  How\where to configure BaseURL for Wasm app that uses WasmHttpHandler
//    https://github.com/unoplatform/uno/issues/1481#issuecomment-531480543
//  [wasm] Store AppSettings externally in some sort of editable text file such as .config, .json or .xml so these values can be changed depending the on the deployment hosting target
//    https://github.com/unoplatform/uno/issues/1500
config.environmentVariables["IDENTITY_SERVER_CLIENT_ID"] = "GraphML.UI.Uno.Wasm";
config.environmentVariables["IDENTITY_SERVER_CLIENT_SECRET"] = "secret";
config.environmentVariables["API_URI"] = "https://localhost:5001";
