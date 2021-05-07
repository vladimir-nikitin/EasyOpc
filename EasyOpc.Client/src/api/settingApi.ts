import { API_URL } from "../constans/common";
import { SettingData } from "../types/settings";

export const createSettingApi = (fetcher: typeof fetch) => (
{
    getByNameAsync: async (settingName: string): Promise<SettingData> => {
        const url = `${API_URL}/settings/getByName/${settingName}`;
        const options: RequestInit = {
            method: "GET",
        };
        const response = await fetcher(url, options);
        switch (response.status) {
            case 200: {
                return await response.json();
            }
            default:
                return null;
        }
    },
    updateByNameAsync: async (settingName: string, value: string): Promise<string> => {
        const url = `${API_URL}/settings/updateByName/${settingName}?value=${value}`;
        const options: RequestInit = {
            method: "PATCH",
        };
        const response = await fetcher(url, options);
        switch (response.status) {
            case 200: {
                return "OK";
            }
            default:
                return null;
        }
    },
});