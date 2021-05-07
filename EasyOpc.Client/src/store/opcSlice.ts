import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { OpcItemData, OpcServerType } from "../types/opc";

export type OpcServer = {
    id: string;
    name: string;
    type: OpcServerType;
    host: string;
    opcGroups?: OpcGroup[] | null;
    jsonSettings: string;
}

export type OpcGroup = {
    id: string;
    name: string;
    reqUpdateRate: number;
    opcServerId: string;
    opcItems?: Map<string, OpcItemData> | null
}

export type OpcState = {
    opcServers: OpcServer[] | null,
    selectedOpcServer: OpcServer | null
    selectedOpcGroup: OpcGroup | null
};

const initialState: OpcState = {
    opcServers: null,
    selectedOpcServer: null,
    selectedOpcGroup: null,
};

const opcSlice = createSlice({
    name: 'OPC',
    initialState,
    reducers: {
        clear: () => initialState,
        setOpcServers: (state, action: PayloadAction<OpcServer[]>) => {
            state.opcServers = action.payload;
            return state;
        },
        addOpcServers: (state, action: PayloadAction<OpcServer[]>) => {
            state.opcServers =  state.opcServers.concat(action.payload);
            return state;
        },
        deleteOpcServer: (state, action: PayloadAction<string>) => {
            state.opcServers =  state.opcServers.filter(s => s.id !== action.payload);
            return state;
        },
        setOpcGroups: (state, action: PayloadAction<{ opcServerId: string, opcGroups: OpcGroup[] }>) => {
            const opcServer = state.opcServers.find(s => s.id === action.payload.opcServerId);
            opcServer.opcGroups = action.payload.opcGroups;
            return state;
        },           
        addOpcGroups: (state, action: PayloadAction<{ opcServerId: string, opcGroups: OpcGroup[] }>) => {
            const opcServer = state.opcServers.find(s => s.id === action.payload.opcServerId);
            opcServer.opcGroups = opcServer.opcGroups.concat(action.payload.opcGroups);
            return state;
        },   
        deleteOpcGroup: (state, action: PayloadAction<{opcServerId: string, opcGroupId: string}>) => {
            const opcServer = state.opcServers.find(s => s.id === action.payload.opcServerId);
            opcServer.opcGroups = opcServer.opcGroups.filter(g => g.id !== action.payload.opcGroupId);
            return state;
        },                
        setOpcItems: (state, action: PayloadAction<{opcServerId: string, opcGroupId: string, opcItems: OpcItemData[] }>) => {
            const opcServer = state.opcServers.find(s => s.id === action.payload.opcServerId);
            const opcGroup = opcServer.opcGroups.find(g => g.id === action.payload.opcGroupId);
            opcGroup.opcItems = new Map<string, OpcItemData>();
            action.payload.opcItems.forEach(item => opcGroup.opcItems.set(item.id, item));
            return state;
        },
        updateOpcItems: (state, action: PayloadAction<{opcServerId: string, opcGroupId: string, opcItems: OpcItemData[] }>) => {
            const group = state.opcServers.find(x => x.id === action.payload.opcServerId)?.opcGroups?.find(x => x.id === action.payload.opcGroupId);
            if(!group) return state;

            let currentItem: OpcItemData = null;
            action.payload.opcItems.forEach(newItem => {
                currentItem = group.opcItems.get(newItem.id);
                if(currentItem != null){
                    currentItem.value = newItem.value;
                    currentItem.quality = newItem.quality;
                    currentItem.timestampLocal = newItem.timestampLocal;
                    currentItem.timestampUtc = newItem.timestampUtc;
                }
            });

            return state;
        }
    }
});

export const {
    clear,
    setOpcServers,
    addOpcServers,
    deleteOpcServer,
    addOpcGroups,
    deleteOpcGroup,
    setOpcGroups,
    setOpcItems,
    updateOpcItems
} = opcSlice.actions;

const opcReducer = opcSlice.reducer;

export default opcReducer;