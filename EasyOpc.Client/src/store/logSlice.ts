import { createSlice, PayloadAction } from "@reduxjs/toolkit";


export type LogState = {
    logs: string[],
}

const initialState: LogState = {
    logs: [],
};

const logSlice = createSlice({
    name: 'Logs',
    initialState,
    reducers: {
        clear: () => initialState,
        setLogs: (state, action: PayloadAction<string[]>) => ({ ...state, logs: action.payload }),
        addLogs: (state, action: PayloadAction<string[]>) => ({ ...state, logs: state.logs.concat(action.payload) }),
    }
});

export const {
    clear,
    setLogs,
    addLogs
} = logSlice.actions;

const logReducer = logSlice.reducer;

export default logReducer;