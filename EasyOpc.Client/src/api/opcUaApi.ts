import { OPC_UA_GROUPS_API_URL, OPC_UA_GROUP_WORKS_API_URL, OPC_UA_ITEMS_API_URL, OPC_UA_SERVERS_API_URL } from "../constans/opc.ua";
import { Page } from "../types/common";
import { AddOpcUaGroupRequest, OpcUaDiscoveryItemData, OpcUaGroupData, OpcUaGroupWorkData, OpcUaItemData, OpcUaServerData } from "../types/opc.ua";

export const createOpcUaApi = (fetcher: typeof fetch) => (
    {
        browseAllOpcUaServerAsync: async (item: OpcUaDiscoveryItemData): Promise<OpcUaDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.nodeId && params.push(`nodeId=${item.nodeId}`);

            const url = `${OPC_UA_SERVERS_API_URL}/browseAll/${item.opcUaServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
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
        browseOpcUaServerAsync: async (item: OpcUaDiscoveryItemData): Promise<OpcUaDiscoveryItemData[]> => {
            const params: string[] = [];
            !!item.nodeId && params.push(`nodeId=${item.nodeId}`);

            const url = `${OPC_UA_SERVERS_API_URL}/browse/${item.opcUaServerId}${params.length > 0 ? '?' + params.join('&') : ''}`;
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
        addOpcUaServerAsync: async (opcUaServer: OpcUaServerData): Promise<string | null> => {
            const url = `${OPC_UA_SERVERS_API_URL}/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcUaServer)
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
        removeOpcUaServerAsync: async (opcUaServerId: string): Promise<string | null> => {
            const url = `${OPC_UA_SERVERS_API_URL}/${opcUaServerId}`;
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
        getOpcUaServers: async (): Promise<OpcUaServerData[]> => {
            const url = `${OPC_UA_SERVERS_API_URL}/getAll`;
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
        getOpcUaGroups: async (opcUaServerId: string): Promise<OpcUaGroupData[]> => {
            const url = `${OPC_UA_GROUPS_API_URL}/getByOpcUaServerId/${opcUaServerId}`;
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
        addOpcUaGroupAsync: async (opcUaGroup: AddOpcUaGroupRequest): Promise<string> => {
            const url = `${OPC_UA_GROUPS_API_URL}/add`;
            const options: RequestInit = {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(opcUaGroup)
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
        removeOpcUaGroupAsync: async (opcUaGroupId: string): Promise<string> => {
            const url = `${OPC_UA_GROUPS_API_URL}/${opcUaGroupId}`;
            const options: RequestInit = {
                method: "DELETE",
                headers: { "Content-Type": "application/json" },
            };
            const response = await fetcher(url, options);
            switch (response.status) {
                case 200: {
                    return opcUaGroupId;
                }
                default:
                    return null;
            }
        },
        getOpcUaItemsAsync: async (opcUaGroupId: string): Promise<OpcUaItemData[]> => {
            const url = `${OPC_UA_ITEMS_API_URL}/getByOpcUaGroupId/${opcUaGroupId}`;
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
        getPageOpcUaItemsAsync: async (opcUaGroupId: string, pageNumber: number, countInPage: number): Promise<Page<OpcUaItemData>> => {
            const url = `${OPC_UA_ITEMS_API_URL}/getPage/opcUaGroup/${opcUaGroupId}/page/${pageNumber}/count/${countInPage}`;
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
        getOpcUaGroupWorksAsync: async (opcUaGroupId: string, workTypes: string[]): Promise<OpcUaGroupWorkData[]> => {
            const url = `${OPC_UA_GROUP_WORKS_API_URL}/getByOpcUaGroupIdAndType/${opcUaGroupId}?${workTypes.map(p => `workTypes=${p}`).join('&')}`;
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
        addOpcUaGroupWorkAsync: async (work: OpcUaGroupWorkData): Promise<string> => {
            const url = `${OPC_UA_GROUP_WORKS_API_URL}/add`;
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
        updateOpcUaGroupWorkAsync: async (work: OpcUaGroupWorkData): Promise<string> => {
            const url = `${OPC_UA_GROUP_WORKS_API_URL}/update`;
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
        deleteOpcUaGroupWorkAsync: async (opcUaGroupWorkId: string): Promise<string> => {
            const url = `${OPC_UA_GROUP_WORKS_API_URL}/delete/${opcUaGroupWorkId}`;
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