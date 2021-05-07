import { API_URL } from "../constans/common";
import { WorkData } from "../types/work";

export const createWorkApi = (fetcher: typeof fetch) => (
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
        saveWorkAsync: async (work: WorkData, method: "add" | "update"): Promise<string> => {
            const url = `${API_URL}/works/${method}`;
            const options: RequestInit = {
                method: method === "add" ? "POST" : "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(work)
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
        getWorkByTypeAndExternalIdAsync: async (type: string, externalId: string): Promise<WorkData> => {
            const url = `${API_URL}/works/getByTypeAndExternalId?type=${type}&externalId=${externalId}`;
            const options: RequestInit = {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
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
    });