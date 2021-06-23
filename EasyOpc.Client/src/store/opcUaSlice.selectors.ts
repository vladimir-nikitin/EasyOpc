import { useSelector } from "react-redux";
import { OpcUaGroupData, OpcUaServerDataEx } from "../types/opc.ua";

import { AppState } from "./store";

export function opcUaServersSelector(selector: (state: AppState) => OpcUaServerDataEx[]) {
    const equalityFn = (left: OpcUaServerDataEx[], right: OpcUaServerDataEx[]) => {
        
        if(left.length !== right.length){
            return false;
        } 

        let i = 0;
        let lServer: OpcUaServerDataEx | null = null;
        let rServer: OpcUaServerDataEx | null = null;

        for(i = 0; i < left.length; i++){
            lServer = left[i];
            rServer = right.find(x => x.id === lServer.id);

            if(!rServer){
                return false;
            } 

            if(lServer.opcUaGroups?.length !== rServer.opcUaGroups?.length){
                return false;
            }
        }

        return true;
    }
    return useSelector(selector, equalityFn);
}

export function opcUaGroupSelector(selector: (state: AppState) => OpcUaGroupData) {
    return useSelector(selector, (left: OpcUaGroupData, right: OpcUaGroupData) => left.id === right.id);
}