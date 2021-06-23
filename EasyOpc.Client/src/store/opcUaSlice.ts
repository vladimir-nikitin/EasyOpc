import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { OpcUaGroupData, OpcUaItemData, OpcUaServerData, OpcUaServerDataEx } from "../types/opc.ua";

export type OpcUaState = {
    opcUaServers: OpcUaServerDataEx[] | null,
    opcUaItems: Map<string, OpcUaItemData> | null
};

const initialState: OpcUaState = {
    opcUaServers: null,
    opcUaItems: null,
};

const opcUaSlice = createSlice({
    name: 'OPC.UA',
    initialState,
    reducers: {
        clear: () => initialState,
        setOpcUaServers: (state, action: PayloadAction<OpcUaServerData[]>) => {
            state.opcUaServers = action.payload;
            return state;
        },
        addOpcUaServers: (state, action: PayloadAction<OpcUaServerData[]>) => {
            state.opcUaServers =  state.opcUaServers.concat(action.payload);
            return state;
        },
        deleteOpcUaServer: (state, action: PayloadAction<string>) => {
            state.opcUaServers =  state.opcUaServers.filter(s => s.id !== action.payload);
            return state;
        },
        setOpcUaGroups: (state, action: PayloadAction<{ opcUaServerId: string, opcUaGroups: OpcUaGroupData[] }>) => {
            const opcServer = state.opcUaServers.find(s => s.id === action.payload.opcUaServerId);
            opcServer.opcUaGroups = action.payload.opcUaGroups;
            return state;
        },           
        addOpcUaGroups: (state, action: PayloadAction<{ opcUaServerId: string, opcUaGroups: OpcUaGroupData[] }>) => {
            const opcServer = state.opcUaServers.find(s => s.id === action.payload.opcUaServerId);
            opcServer.opcUaGroups = opcServer.opcUaGroups.concat(action.payload.opcUaGroups);
            return state;
        },   
        deleteOpcUaGroup: (state, action: PayloadAction<{opcUaServerId: string, opcUaGroupId: string}>) => {
            const opcServer = state.opcUaServers.find(s => s.id === action.payload.opcUaServerId);
            opcServer.opcUaGroups = opcServer.opcUaGroups.filter(g => g.id !== action.payload.opcUaGroupId);
            return state;
        },                
        setOpcUaItems: (state, action: PayloadAction<{opcUaItems: OpcUaItemData[] }>) => {
            const map = new Map<string, OpcUaItemData>();
            action.payload.opcUaItems.forEach(p => !map.has(p.id) && map.set(p.id, p));
            state.opcUaItems = map;
            return state;
        },
        updateOpcUaItems: (state, action: PayloadAction<{opcUaItems: OpcUaItemData[] }>) => {
            let currentItem: OpcUaItemData = null;
            action.payload.opcUaItems.forEach(newItem => {
                currentItem = state.opcUaItems.get(newItem.id);
                if(currentItem != null){
                    currentItem.value = newItem.value;
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
    setOpcUaServers,
    addOpcUaServers,
    deleteOpcUaServer,
    addOpcUaGroups,
    deleteOpcUaGroup,
    setOpcUaGroups,
    setOpcUaItems,
    updateOpcUaItems
} = opcUaSlice.actions;

const opcUaReducer = opcUaSlice.reducer;

export default opcUaReducer;