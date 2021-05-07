import { applyMiddleware, combineReducers, createStore, Store } from "redux";
import thunk from "redux-thunk";
import logReducer, { LogState } from "./logSlice";
import opcReducer, { OpcState } from "./opcSlice";
import windowReducer, { WindowState } from "./windowSlice";

export type AppState = {
    window: WindowState,
    opc: OpcState,
    log: LogState
};

export const appReducer = combineReducers<AppState>({
    log: logReducer,
    opc: opcReducer,
    window: windowReducer,
});

export function configureStore(): Store<AppState> {
    const store = createStore(
        appReducer,
        applyMiddleware(thunk)
    );
    return store;
}