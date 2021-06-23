import { Guid } from "guid-typescript";
import { Dispatch } from "redux";
import { api } from "../api/api";
import { addOpcDaGroups, addOpcDaServers, deleteOpcDaGroup, deleteOpcDaServer, setOpcDaGroups, setOpcDaItems, setOpcDaServers } from "../store/opcDaSlice";
import { setSelectedItem, showAppLoader } from "../store/windowSlice";
import { AddOpcDaGroupRequest, OpcDaDiscoveryItemData, OpcDaGroupWorkData, OpcDaServerData, OpcDaWorkType } from "../types/opc.da";


export const loadOpcDaServers = () => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcDaServers = await api.opcDa.getOpcDaServers();
      dispatch(setOpcDaServers(opcDaServers));
      dispatch(showAppLoader(false));
    };
  };

  export const loadOpcDaGroups = (opcDaServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcDaGroups = await api.opcDa.getOpcDaGroups(opcDaServerId);
      dispatch(setOpcDaGroups({opcDaServerId, opcDaGroups: opcDaGroups}));
      dispatch(showAppLoader(false));
    };
  };

  export const getOpcDaItems = async (dispatch: Dispatch<any>, opcDaGroupId: string) => {
    dispatch(showAppLoader(true));
    const opcDaItems = await api.opcDa.getOpcDaItemsAsync(opcDaGroupId);
    dispatch(showAppLoader(false));
    return opcDaItems;
  };

  export const getPageOpcDaItems = async (dispatch: Dispatch<any>, opcDaGroupId: string, pageNumber: number, countInPage: number) => {
    dispatch(showAppLoader(true));
    const opcDaItems = await api.opcDa.getPageOpcDaItemsAsync(opcDaGroupId, pageNumber, countInPage);
    dispatch(showAppLoader(false));
    return opcDaItems;
  };

  export const importOpcDaServers = (data: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcDa.importOpcDaServersAsync(data);
      dispatch(showAppLoader(false));
      dispatch(loadOpcDaServers());
    };
  };

  export const addOpcDaServer = (opcDaServer: OpcDaServerData) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcDa.addOpcDaServerAsync(opcDaServer);
      dispatch(addOpcDaServers([opcDaServer]));
      dispatch(showAppLoader(false));
    };
  };

  export const removeOpcDaServer = (opcDaServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcDa.removeOpcDaServerAsync(opcDaServerId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcDaServer(opcDaServerId));
      dispatch(showAppLoader(false));
    };
  };

  export const addOpcDaGroup = async (dispatch: Dispatch<any>, opcDaGroup: AddOpcDaGroupRequest) => {
    dispatch(showAppLoader(true));
      await api.opcDa.addOpcDaGroupAsync(opcDaGroup);
      dispatch(addOpcDaGroups({opcDaServerId: opcDaGroup.opcDaServerId, opcDaGroups: [opcDaGroup]}));
      dispatch(showAppLoader(false));
  }
 
  export const removeOpcDaGroup = (opcDaServerId: string, opcDaGroupId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcDa.removeOpcDaGroupAsync(opcDaGroupId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcDaGroup({opcDaServerId, opcDaGroupId}));
      dispatch(showAppLoader(false));
    };
  };

  export const browseOpcDaServer = async (dispatch: Dispatch<any>, item: OpcDaDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opcDa.browseOpcDaServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const browseAllOpcDaServer = async (dispatch: Dispatch<any>, item: OpcDaDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opcDa.browseAllOpcDaServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const getOpcDaGroupWorks = async (dispatch: Dispatch<any>, opcDaGroupId: string, workTypes: OpcDaWorkType[]) => {
    dispatch(showAppLoader(true));
    const works = await api.opcDa.getOpcDaGroupWorksAsync(opcDaGroupId, workTypes);
    dispatch(showAppLoader(false));
    return works;
  }

  export const addOpcDaGroupWork = async (dispatch: Dispatch<any>, work: OpcDaGroupWorkData) => {
    dispatch(showAppLoader(true));
    await api.opcDa.addOpcDaGroupWorkAsync(work);
    dispatch(showAppLoader(false));
  }

  export const updateOpcDaGroupWork = async (dispatch: Dispatch<any>, work: OpcDaGroupWorkData) => {
    dispatch(showAppLoader(true));
    await api.opcDa.updateOpcDaGroupWorkAsync(work);
    dispatch(showAppLoader(false));
  }

  export const deleteOpcDaGroupWork = async (dispatch: Dispatch<any>, opcDaGroupWorkId: string) => {
    dispatch(showAppLoader(true));
    await api.opcDa.deleteOpcDaGroupWorkAsync(opcDaGroupWorkId);
    dispatch(showAppLoader(false));
  }
