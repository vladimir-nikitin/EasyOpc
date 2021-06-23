import { Dispatch } from "redux";
import { api } from "../api/api";
import { SERVICE_MODE_SETTING_NAME } from "../constans/settings";
import { setIsInit, setServiceMode, showAppLoader } from "../store/windowSlice";

export const initApp = () => {
    return async (dispatch: Dispatch<any>) => {
        dispatch(showAppLoader(true));

        while(true){
            try{
                const serviceModeSetting = await api.settings.getByNameAsync(SERVICE_MODE_SETTING_NAME);
                console.log(`serviceModeSetting:`)
                console.log(serviceModeSetting)

                dispatch(setIsInit(true));

                if(serviceModeSetting){
                    dispatch(setServiceMode(serviceModeSetting.value.toLowerCase() === 'true'));
                }

                break;
            }
            catch{
                console.log(`#start setTimeout(resolve, 1000)`)
                await new Promise((resolve) => setTimeout(resolve, 1000));
                console.log(`#end setTimeout(resolve, 1000)`)
            } 
        }

        dispatch(showAppLoader(false));
    };
};

export const changeServiceMode = (serviceMode: boolean) => {
    return async (dispatch: Dispatch<any>) => {
        dispatch(showAppLoader(true));

        if(serviceMode){
            await api.works.setWorkExecutionModeAsync("stop");
        }
        else{
            await api.works.setWorkExecutionModeAsync("start");
        }

        dispatch(setServiceMode(serviceMode));
        dispatch(showAppLoader(false));
    };
};