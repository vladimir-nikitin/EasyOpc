import { useSelector } from "react-redux";
import { OpcDaGroupData, OpcDaServerDataEx } from "../types/opc.da";

import { AppState } from "./store";

export function opcDaServersSelector(selector: (state: AppState) => OpcDaServerDataEx[]) {
    const equalityFn = (left: OpcDaServerDataEx[], right: OpcDaServerDataEx[]) => {
        
        if(left.length !== right.length){
            return false;
        } 

        let i = 0;
        let lServer: OpcDaServerDataEx | null = null;
        let rServer: OpcDaServerDataEx | null = null;

        for(i = 0; i < left.length; i++){
            lServer = left[i];
            rServer = right.find(x => x.id === lServer.id);

            if(!rServer){
                return false;
            } 

            if(lServer.opcDaGroups?.length !== rServer.opcDaGroups?.length){
                return false;
            }
        }

        return true;
    }
    return useSelector(selector, equalityFn);
}

export function opcDaGroupSelector(selector: (state: AppState) => OpcDaGroupData) {
    return useSelector(selector, (left: OpcDaGroupData, right: OpcDaGroupData) => left.id === right.id);
}