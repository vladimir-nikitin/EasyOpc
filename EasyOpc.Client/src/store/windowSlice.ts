import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { OpcGroup, OpcServer } from "./opcSlice";


export type SelectedItem = {
    type: 'OpcServer',
    item: OpcServer
} | {
    type: 'OpcGroup',
    item: OpcGroup
} | {
    type: 'Settings',
    item?: null
} | {
    type: 'Logs',
    item?: null
}

export type WindowState = {
    showAppLoader: boolean,
    selectedItem?: SelectedItem | null,
    serviceMode: boolean,
    isInit: boolean
}

const initialState: WindowState = {
    showAppLoader: false,
    serviceMode: true,
    isInit: false
};

const windowSlice = createSlice({
    name: 'Window',
    initialState,
    reducers: {
        clear: () => initialState,
        setSelectedItem: (state, action: PayloadAction<SelectedItem | null>) => {
            state.selectedItem = action.payload;
            return state;
        },
        showAppLoader: (state, action: PayloadAction<boolean>) => {
            state.showAppLoader = action.payload;
            return state;
        },
        setServiceMode: (state, action: PayloadAction<boolean>)=> {
            state.serviceMode = action.payload;
            return state;
        },
        setIsInit: (state, action: PayloadAction<boolean>)=> {
            state.isInit = action.payload;
            return state;
        },
    }
});

export const {
    clear,
    setSelectedItem,
    showAppLoader,
    setServiceMode,
    setIsInit
} = windowSlice.actions;

const windowReducer = windowSlice.reducer;

export default windowReducer;