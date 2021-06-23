export type OpcDaServerDataEx = OpcDaServerData & { 
    opcDaGroups?: OpcDaGroupData[] | null 
}

export type OpcDaServerData = {
    id: string,
    name: string,
    host: string,
    clsid: string,
}

export type OpcDaGroupData = {
    id: string,
    opcDaServerId: string,
    name: string,
    reqUpdateRate: number,
}

export type AddOpcDaGroupRequest = OpcDaGroupData & {
    opcDaItems: OpcDaItemData[]
}

export type OpcDaItemData = {
    id: string,
    opcDaGroupId: string,
    name: string,
    canonicalDataType?: string | null,
    accessPath?: string | null,
    reqDataType?: string | null,
    value?: string | null,
    quality?: string | null,
    timestampUtc?: string | null, 
    timestampLocal?: string | null,
}

export type OpcDaDiscoveryItemData = {
    id: string,
    opcDaServerId: string,
    name: string,
    accessPath: string,
    hasChildren: boolean,
    hasValue: boolean,
    childs?: OpcDaDiscoveryItemData[] | null,
}

export type OpcDaWorkType = "SUBSCRITION_TO_FILE" | "SUBSCRITION_TO_SQL" | "EXPORT_TO_FILE" | "EXPORT_TO_SQL";

export type OpcDaGroupWorkData = {
    id: string,
    opcDaGroupId: string,
    name: string,
    type: OpcDaWorkType,
    launchGroup: string,
    isEnabled: boolean,
    jsonSettings: string
}

export type SubscritionToFileWorkSetting = {
    folderPath: string,
    fileTimespan: string,
    historyRetentionTimespan: string,
}

export type ExportToFileWorkSetting = {
    folderPath: string,
    timespan: string,
    writeInOneFile: boolean,
}
