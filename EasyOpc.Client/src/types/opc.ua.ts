export type OpcUaServerDataEx = OpcUaServerData & { 
    opcUaGroups?: OpcUaGroupData[] | null 
}

export type OpcUaServerData = {
    id: string,
    name: string,
    host: string,
    userName: string,
    password: string,
}

export type OpcUaGroupData = {
    id: string,
    opcUaServerId: string,
    name: string,
    reqUpdateRate: number,
}

export type AddOpcUaGroupRequest = OpcUaGroupData & {
    opcUaItems: OpcUaItemData[]
}

export type OpcUaItemData = {
    id: string,
    opcUaGroupId: string,
    name: string,
    nodeId?: string | null,
    value?: string | null,
    timestampUtc?: string | null, 
    timestampLocal?: string | null,
}

export type OpcUaDiscoveryItemData = {
    id: string,
    opcUaServerId: string,
    name: string,
    nodeId: string,
    hasChildren: boolean,
    hasValue: boolean,
    childs?: OpcUaDiscoveryItemData[] | null,
}

export type OpcUaWorkType = "SUBSCRITION_TO_FILE" | "SUBSCRITION_TO_SQL" | "EXPORT_TO_FILE" | "EXPORT_TO_SQL";

export type OpcUaGroupWorkData = {
    id: string,
    opcUaGroupId: string,
    name: string,
    type: OpcUaWorkType,
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