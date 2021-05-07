import { Guid } from "guid-typescript";
import { Dispatch } from "redux";
import { api } from "../api/api";
import { OPC_EXPORT_WORK_TYPE, OPC_HISTORY_WORK_TYPE } from "../constans/opc";
import { addOpcGroups, addOpcServers, deleteOpcGroup, deleteOpcServer, OpcGroup, OpcServer, setOpcGroups, setOpcItems, setOpcServers } from "../store/opcSlice";
import { setSelectedItem, showAppLoader } from "../store/windowSlice";
import { OpcDiscoveryItemData, OpcGroupData, OpcGroupExportSettingData, OpcGroupHistorySettingData, OpcItemData, OpcServerData } from "../types/opc";
import { WorkData } from "../types/work";

const mapToOpcGroup: (group: OpcGroupData) => OpcGroup = (group: OpcGroupData) => {
  const opcItemsMap = group.opcItems ? new Map<string, OpcItemData>() : null;
  if(opcItemsMap){
    group.opcItems.forEach(item => opcItemsMap.set(item.id, item));
  }

  return {
    id: group.id,
    opcServerId: group.opcServerId,
    name: group.name,
    reqUpdateRate: group.reqUpdateRate,
    opcItems: opcItemsMap
  }
}

const mapToOpcServer: (group: OpcServerData) => OpcServer = (server: OpcServerData) => {
  return {
    id: server.id,
    name: server.name,
    host: server.host,
    type: server.type,
    jsonSettings: server.jsonSettings,
    opcGroups: server.opcGroups?.map(g => mapToOpcGroup(g))
  }
}

export const loadOpcServers = () => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcServers = await api.opc.getOpcServers();
      dispatch(setOpcServers(opcServers.map(s => mapToOpcServer(s))));
      dispatch(showAppLoader(false));
    };
  };

  export const loadOpcGroups = (opcServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcGroups = await api.opc.getOpcGroups(opcServerId);
      dispatch(setOpcGroups({opcServerId, opcGroups: opcGroups.map(g => mapToOpcGroup(g))}));
      dispatch(showAppLoader(false));
    };
  };

  export const loadOpcItems = (opcServerId: string, opcGroupId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcItems = await api.opc.getOpcItems(opcGroupId);
      dispatch(setOpcItems({opcServerId, opcGroupId, opcItems}));
      dispatch(showAppLoader(false));
    };
  };

  export const getOpcItems = async (dispatch: Dispatch<any>, opcGroupId: string) => {
    dispatch(showAppLoader(true));
    const opcItems = await api.opc.getOpcItems(opcGroupId);
    dispatch(showAppLoader(false));
    return opcItems;
  };

  export const importOpcDaServers = (data: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opc.importOpcDaServersAsync(data);
      dispatch(showAppLoader(false));
      dispatch(loadOpcServers());
    };
  };

  export const addOpcServer = (opcServer: OpcServerData) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opc.addOpcServerAsync(opcServer);
      dispatch(addOpcServers([mapToOpcServer(opcServer)]));
      dispatch(showAppLoader(false));
    };
  };

  export const removeOpcServer = (opcServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opc.removeOpcServerAsync(opcServerId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcServer(opcServerId));
      dispatch(showAppLoader(false));
    };
  };

  export const addOpcGroup = (opcGroup: OpcGroupData) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opc.addOpcGroupAsync(opcGroup);
      dispatch(addOpcGroups({opcServerId: opcGroup.opcServerId, opcGroups: [mapToOpcGroup(opcGroup)]}));
      dispatch(showAppLoader(false));
    };
  };
  
  export const removeOpcGroup = (opcServerId: string, opcGroupId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opc.removeOpcGroupAsync(opcGroupId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcGroup({opcServerId, opcGroupId}));
      dispatch(showAppLoader(false));
    };
  };

  export const browseOpcServer = async (dispatch: Dispatch<any>, item: OpcDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opc.browseOpcServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const browseAllOpcServer = async (dispatch: Dispatch<any>, item: OpcDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opc.browseAllOpcServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const disconnectFromOpcServer = async (dispatch: Dispatch<any>, opcServerId: string) => {
    dispatch(showAppLoader(true));
    const result = await api.opc.disconnectFromOpcServer(opcServerId);
    dispatch(showAppLoader(false));
    return result;
  }

  export const getOpcGroupHistorySetting = async (dispatch: Dispatch<any>, opcGroupId: string) => {
    dispatch(showAppLoader(true));
    const work = await api.work.getWorkByTypeAndExternalIdAsync(OPC_HISTORY_WORK_TYPE, opcGroupId);
    const setting: OpcGroupHistorySettingData = !work ? {
      externalId: opcGroupId,
      id: '',
      type: OPC_HISTORY_WORK_TYPE,
      name: '',
      order: 1,
      isEnabled: false,
      fileTimespan: '00:30:00',
      historyRetentionTimespan: '24:00:00',
      folderPath: ''
    }: {
      ...work,
      ...JSON.parse(work.jsonSettings)
    }
    dispatch(showAppLoader(false));
    return setting;
  }

  export const saveOpcGroupHistorySetting = async (dispatch: Dispatch<any>, setting: OpcGroupHistorySettingData) => {
    dispatch(showAppLoader(true));

    const method = !setting.id ? "add" : "update";
    const work: WorkData = {
      externalId: setting.externalId,
      id: !setting.id ? Guid.create().toString() : setting.id,
      type: setting.type,
      name: setting.name,
      order: setting.order,
      isEnabled: setting.isEnabled,
      jsonSettings: JSON.stringify({
        fileTimespan: setting.fileTimespan,
        historyRetentionTimespan: setting.historyRetentionTimespan,
        folderPath: setting.folderPath
      })
    }

    await api.work.saveWorkAsync(work, method);
    dispatch(showAppLoader(false));

    return {...setting, id: work.id};
  }

  export const getOpcGroupExportSetting = async (dispatch: Dispatch<any>, opcGroupId: string) => {
    dispatch(showAppLoader(true));
    const work = await api.work.getWorkByTypeAndExternalIdAsync(OPC_EXPORT_WORK_TYPE, opcGroupId);
    const setting: OpcGroupExportSettingData = !work ? {
      externalId: opcGroupId,
      id: '',
      type: OPC_EXPORT_WORK_TYPE,
      name: '',
      order: 1,
      isEnabled: false,
      folderPath: '',
      timespan: '00:00:30',
      isWriteInOneFile: false
    }: {
      ...work,
      ...JSON.parse(work.jsonSettings)
    }
    dispatch(showAppLoader(false));
    return setting;
  }

  export const saveOpcGroupExportSetting = async (dispatch: Dispatch<any>, setting: OpcGroupExportSettingData) => {
    dispatch(showAppLoader(true));

    const method = !setting.id ? "add" : "update";
    const work: WorkData = {
      externalId: setting.externalId,
      id: !setting.id ? Guid.create().toString() : setting.id,
      type: setting.type,
      name: setting.name,
      order: setting.order,
      isEnabled: setting.isEnabled,
      jsonSettings: JSON.stringify({
        folderPath: setting.folderPath,
        timespan: setting.timespan,
        isWriteInOneFile: setting.isWriteInOneFile
        
      })
    }

    await api.work.saveWorkAsync(work, method);
    dispatch(showAppLoader(false));

    return {...setting, id: work.id};
  }