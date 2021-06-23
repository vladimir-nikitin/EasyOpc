import { applyMiddleware, combineReducers, createStore, Store } from "redux";
import thunk from "redux-thunk";
import logReducer, { LogState } from "./logSlice";
import opcDaReducer, { OpcDaState } from "./opcDaSlice";
import opcUaReducer, { OpcUaState } from "./opcUaSlice";
import windowReducer, { WindowState } from "./windowSlice";

export type AppState = {
    window: WindowState,
    opcDa: OpcDaState,
    opcUa: OpcUaState,
    log: LogState
};

export const appReducer = combineReducers<AppState>({
    log: logReducer,
    window: windowReducer,
    opcDa: opcDaReducer,
    opcUa: opcUaReducer
});

export function configureStore(): Store<AppState> {
    const store = createStore(
        appReducer,
        applyMiddleware(thunk)
    );
    return store;
}