import { API_URL } from "../constans/common";

export const createWorksApi = (fetcher: typeof fetch) => (
    {
        setWorkExecutionModeAsync: async (method: "start" | "stop"): Promise<string> => {
            const url = `${API_URL}/works/${method}`;
            const options: RequestInit = {
                method: "PUT",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return 'Ok';
                }
                default:
                    return null;
            }

        },
    });