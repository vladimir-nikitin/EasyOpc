import { useSelector } from "react-redux";
import { OpcGroup, OpcServer } from "./opcSlice";
import { AppState } from "./store";

export function opcServersSelector(selector: (state: AppState) => OpcServer[]) {
    const equalityFn = (left: OpcServer[], right: OpcServer[]) => {
        
        if(left.length !== right.length){
            return false;
        } 

        let i = 0;
        let lServer: OpcServer | null = null;
        let rServer: OpcServer | null = null;

        for(i = 0; i < left.length; i++){
            lServer = left[i];
            rServer = right.find(x => x.id === lServer.id);

            if(!rServer){
                return false;
            } 

            if(lServer.opcGroups?.length !== rServer.opcGroups?.length){
                return false;
            }
        }

        return true;
    }
    return useSelector(selector, equalityFn);
}

export function opcGroupSelector(selector: (state: AppState) => OpcGroup) {
    return useSelector(selector, (left: OpcGroup, right: OpcGroup) => left.id === right.id);
}