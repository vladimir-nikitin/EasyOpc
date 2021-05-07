import { API_URL } from "../constans/common";
import { OpcDiscoveryItemData, OpcGroupData, OpcItemData, OpcServerData } from "../types/opc";

export const createOpcApi = (fetcher: typeof fetch) => (
    {
        disconnectFromOpcServer: async (opcServerId: string): Promise<boolean> => {
            const url = `${API_URL}/opcservers/tryDisconnect/${opcServerId}`;
            const options: RequestInit = {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return false;
            }
        },
        browseAllOpcServerAsync: async (item: OpcDiscoveryItemData): Promise<OpcDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.name && params.push(`itemName=${item.name}`);
            !!item.accessPath && params.push(`accessPath=${item.accessPath}`);

            const url = `${API_URL}/opcservers/browseAll/${item.opcServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
        browseOpcServerAsync: async (item: OpcDiscoveryItemData): Promise<OpcDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.name && params.push(`itemName=${item.name}`);
            !!item.accessPath && params.push(`accessPath=${item.accessPath}`);

            const url = `${API_URL}/opcservers/browse/${item.opcServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
        importOpcDaServersAsync: async (data: string): Promise<OpcServerData[]> => {
            const url = `${API_URL}/opcservers/import`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({content: data}) 
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
        addOpcServerAsync: async (opcServer: OpcServerData): Promise<string | null> => {
            const url = `${API_URL}/opcservers/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcServer)
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
        removeOpcServerAsync: async (opcServerId: string): Promise<string | null> => {
            const url = `${API_URL}/opcservers/${opcServerId}`;
            const options: RequestInit = {
                method: "DELETE",
                headers: { "Content-Type": "application/json" },
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
        getOpcServers: async (): Promise<OpcServerData[]> => {
            const url = `${API_URL}/opcservers/getAll`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
        getOpcGroups: async (opcServerId: string): Promise<OpcGroupData[]> => {
            const url = `${API_URL}/opcgroups/getByOpcServerId/${opcServerId}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
        addOpcGroupAsync: async (opcGroup: OpcGroupData): Promise<string> => {
            const url = `${API_URL}/opcgroups/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcGroup)
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
        removeOpcGroupAsync: async (opcGroupId: string): Promise<string> => {
            const url = `${API_URL}/opcgroups/${opcGroupId}`;
            const options: RequestInit = {
                method: "DELETE",
                headers: { "Content-Type": "application/json" },
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return opcGroupId;
                }
                default:
                    return null;
            }
        },
        getOpcItems: async (opcGroupId: string): Promise<OpcItemData[]> => {
            const url = `${API_URL}/opcitems/getByOpcGroupId/${opcGroupId}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return [];
            }
        },
    });