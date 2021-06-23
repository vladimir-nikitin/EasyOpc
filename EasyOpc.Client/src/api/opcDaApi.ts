import { API_URL } from "../constans/common";
import { OPC_DA_GROUPS_API_URL, OPC_DA_GROUP_WORKS_API_URL, OPC_DA_ITEMS_API_URL, OPC_DA_SERVERS_API_URL } from "../constans/opc.da";
import { Page } from "../types/common";
import { AddOpcDaGroupRequest, OpcDaDiscoveryItemData, OpcDaGroupData, OpcDaGroupWorkData, OpcDaItemData, OpcDaServerData } from "../types/opc.da";

export const createOpcDaApi = (fetcher: typeof fetch) => (
    {
        browseAllOpcDaServerAsync: async (item: OpcDaDiscoveryItemData): Promise<OpcDaDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.name && params.push(`itemName=${item.name}`);

            const url = `${OPC_DA_SERVERS_API_URL}/browseAll/${item.opcDaServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
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
        browseOpcDaServerAsync: async (item: OpcDaDiscoveryItemData): Promise<OpcDaDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.name && params.push(`itemName=${item.name}`);

            const url = `${OPC_DA_SERVERS_API_URL}/browse/${item.opcDaServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
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
        importOpcDaServersAsync: async (data: string): Promise<OpcDaServerData[]> => {
            const url = `${OPC_DA_SERVERS_API_URL}/import`;
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
        addOpcDaServerAsync: async (opcDaServer: OpcDaServerData): Promise<string | null> => {
            const url = `${OPC_DA_SERVERS_API_URL}/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcDaServer)
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
        removeOpcDaServerAsync: async (opcDaServerId: string): Promise<string | null> => {
            const url = `${OPC_DA_SERVERS_API_URL}/${opcDaServerId}`;
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
        getOpcDaServers: async (): Promise<OpcDaServerData[]> => {
            const url = `${OPC_DA_SERVERS_API_URL}/getAll`;
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
        getOpcDaGroups: async (opcDaServerId: string): Promise<OpcDaGroupData[]> => {
            const url = `${OPC_DA_GROUPS_API_URL}/getByOpcDaServerId/${opcDaServerId}`;
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

        getOpcDaGroupWork: async (opcDaServerId: string, workType: string): Promise<OpcDaGroupData> => {
            const url = `${OPC_DA_GROUP_WORKS_API_URL}/getByOpcDaGroupIdAndType/${opcDaServerId}?workType=${workType}`;
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
        addOpcDaGroupAsync: async (opcDaGroup: AddOpcDaGroupRequest): Promise<string> => {
            const url = `${OPC_DA_GROUPS_API_URL}/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcDaGroup)
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
        removeOpcDaGroupAsync: async (opcDaGroupId: string): Promise<string> => {
            const url = `${OPC_DA_GROUPS_API_URL}/${opcDaGroupId}`;
            const options: RequestInit = {
                method: "DELETE",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return opcDaGroupId;
                }
                default:
                    return null;
            }
        },
        getOpcDaItemsAsync: async (opcDaGroupId: string): Promise<OpcDaItemData[]> => {
            const url = `${OPC_DA_ITEMS_API_URL}/getByOpcDaGroupId/${opcDaGroupId}`;
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
        getPageOpcDaItemsAsync: async (opcDaGroupId: string, pageNumber: number, countInPage: number): Promise<Page<OpcDaItemData>> => {
            const url = `${OPC_DA_ITEMS_API_URL}/getPage/opcDaGroup/${opcDaGroupId}/page/${pageNumber}/count/${countInPage}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return {
                        countInPage: countInPage,
                        pageNumber: pageNumber,
                        totalCount: 0,
                        items: []
                    };
            }
        },
        getOpcDaGroupWorksAsync: async (opcDaGroupId: string, workTypes: string[]): Promise<OpcDaGroupWorkData[]> => {
            const url = `${OPC_DA_GROUP_WORKS_API_URL}/getByOpcDaGroupIdAndType/${opcDaGroupId}?${workTypes.map(p => `workTypes=${p}`).join('&')}`;
            const options: RequestInit = {
                method: "GET",
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return await response.json();
                }
                default:
                    return []
            }
        },
        addOpcDaGroupWorkAsync: async (work: OpcDaGroupWorkData): Promise<string> => {
            const url = `${OPC_DA_GROUP_WORKS_API_URL}/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(work)
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return 'OK';
                }
                default:
                    return null;
            }
        },
        updateOpcDaGroupWorkAsync: async (work: OpcDaGroupWorkData): Promise<string> => {
            const url = `${OPC_DA_GROUP_WORKS_API_URL}/update`;
            const options: RequestInit = {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(work)
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
        deleteOpcDaGroupWorkAsync: async (opcDaGroupWorkId: string): Promise<string> => {
            const url = `${OPC_DA_GROUP_WORKS_API_URL}/delete/${opcDaGroupWorkId}`;
            const options: RequestInit = {
                method: "DELETE",
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