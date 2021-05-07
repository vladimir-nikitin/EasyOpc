export type OpcServerType = 'OPC.DA' | 'OPC.UA';

export type OpcServerData = {
    id: string;
    name: string;
    type: OpcServerType;
    host: string;
    opcGroups?: OpcGroupData[] | null;
    jsonSettings: string;
}

export type OpcGroupData = {
    id: string;
    name: string;
    reqUpdateRate: number;
    opcServerId: string;
    opcItems?: OpcItemData[] | null
}

export type OpcItemData = {
    id: string;
    name: string;
    opcGroupId: string;
    canonicalDataType?: string | null;
    accessPath?: string | null;
    reqDataType?: string | null;
    value?: string | null;
    quality?: string | null;
    timestampUtc?: string | null; 
    timestampLocal?: string | null;
}

export type OpcDiscoveryItemData = {
    opcServerId: string;
    id: string;
    name: string;
    accessPath: string;
    hasChildren: boolean;
    childs?: OpcDiscoveryItemData[] | null;
}

export type OpcGroupHistorySettingData = {
    id: string;
    type: string;
    name: string;
    externalId: string;
    order: number;
    isEnabled: boolean;

    folderPath: string;
    fileTimespan: string;
    historyRetentionTimespan: string;
}

export type OpcGroupExportSettingData = {
    id: string;
    type: string;
    name: string;
    externalId: string;
    order: number;
    isEnabled: boolean;

    folderPath: string;
    timespan: string;
    isWriteInOneFile: boolean;
}

