import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { OpcDaGroupData, OpcDaItemData, OpcDaServerData, OpcDaServerDataEx } from "../types/opc.da";

export type OpcDaState = {
    opcDaServers: OpcDaServerDataEx[] | null,
    opcDaItems: Map<string, OpcDaItemData> | null
};

const initialState: OpcDaState = {
    opcDaServers: null,
    opcDaItems: null,
};

const opcDaSlice = createSlice({
    name: 'OPC.DA',
    initialState,
    reducers: {
        clear: () => initialState,
        setOpcDaServers: (state, action: PayloadAction<OpcDaServerData[]>) => {
            state.opcDaServers = action.payload;
            return state;
        },
        addOpcDaServers: (state, action: PayloadAction<OpcDaServerData[]>) => {
            state.opcDaServers =  state.opcDaServers.concat(action.payload);
            return state;
        },
        deleteOpcDaServer: (state, action: PayloadAction<string>) => {
            state.opcDaServers =  state.opcDaServers.filter(s => s.id !== action.payload);
            return state;
        },
        setOpcDaGroups: (state, action: PayloadAction<{ opcDaServerId: string, opcDaGroups: OpcDaGroupData[] }>) => {
            const opcServer = state.opcDaServers.find(s => s.id === action.payload.opcDaServerId);
            opcServer.opcDaGroups = action.payload.opcDaGroups;
            return state;
        },           
        addOpcDaGroups: (state, action: PayloadAction<{ opcDaServerId: string, opcDaGroups: OpcDaGroupData[] }>) => {
            const opcServer = state.opcDaServers.find(s => s.id === action.payload.opcDaServerId);
            opcServer.opcDaGroups = opcServer.opcDaGroups.concat(action.payload.opcDaGroups);
            return state;
        },   
        deleteOpcDaGroup: (state, action: PayloadAction<{opcDaServerId: string, opcDaGroupId: string}>) => {
            const opcServer = state.opcDaServers.find(s => s.id === action.payload.opcDaServerId);
            opcServer.opcDaGroups = opcServer.opcDaGroups.filter(g => g.id !== action.payload.opcDaGroupId);
            return state;
        },                
        setOpcDaItems: (state, action: PayloadAction<{opcDaItems: OpcDaItemData[] }>) => {
            const map = new Map<string, OpcDaItemData>();
            action.payload.opcDaItems.forEach(p => !map.has(p.id) && map.set(p.id, p));
            state.opcDaItems = map;
            return state;
        },
        updateOpcDaItems: (state, action: PayloadAction<{opcDaItems: OpcDaItemData[] }>) => {
            let currentItem: OpcDaItemData = null;
            action.payload.opcDaItems.forEach(newItem => {
                currentItem = state.opcDaItems.get(newItem.id);
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
    setOpcDaServers,
    addOpcDaServers,
    deleteOpcDaServer,
    addOpcDaGroups,
    deleteOpcDaGroup,
    setOpcDaGroups,
    setOpcDaItems,
    updateOpcDaItems
} = opcDaSlice.actions;

const opcDaReducer = opcDaSlice.reducer;

export default opcDaReducer;