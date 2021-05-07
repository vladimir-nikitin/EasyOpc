import { Dispatch } from "redux";
import { api } from "../api/api";
import { LOG_FILE_PATH_SETTING_NAME } from "../constans/common";
import { showAppLoader } from "../store/windowSlice";

export const getLogFilePath = async (dispatch: Dispatch<any>) => {
    dispatch(showAppLoader(true));
    const setting = await api.setting.getByNameAsync(LOG_FILE_PATH_SETTING_NAME);
    dispatch(showAppLoader(false));
    return setting.value;
};

export const setLogFilePath = async (dispatch: Dispatch<any>, path: string) => {
    dispatch(showAppLoader(true));
    await api.setting.updateByNameAsync(LOG_FILE_PATH_SETTING_NAME, path);
    dispatch(showAppLoader(false));
};