import { Dispatch } from "redux";
import { api } from "../api/api";
import { addOpcUaGroups, addOpcUaServers, deleteOpcUaGroup, deleteOpcUaServer, setOpcUaGroups, setOpcUaItems, setOpcUaServers } from "../store/opcUaSlice";
import { setSelectedItem, showAppLoader } from "../store/windowSlice";
import { AddOpcUaGroupRequest, OpcUaDiscoveryItemData, OpcUaGroupWorkData, OpcUaServerData, OpcUaWorkType } from "../types/opc.ua";


export const loadOpcUaServers = () => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcUaServers = await api.opcUa.getOpcUaServers();
      dispatch(setOpcUaServers(opcUaServers));
      dispatch(showAppLoader(false));
    };
  };

  export const loadOpcUaGroups = (opcUaServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      const opcUaGroups = await api.opcUa.getOpcUaGroups(opcUaServerId);
      dispatch(setOpcUaGroups({opcUaServerId, opcUaGroups: opcUaGroups}));
      dispatch(showAppLoader(false));
    };
  };

  export const getOpcUaItems = async (dispatch: Dispatch<any>, opcUaGroupId: string) => {
    dispatch(showAppLoader(true));
    const opcUaItems = await api.opcUa.getOpcUaItemsAsync(opcUaGroupId);
    dispatch(showAppLoader(false));
    return opcUaItems;
  };

  export const getPageOpcUaItems = async (dispatch: Dispatch<any>, opcUaGroupId: string, pageNumber: number, countInPage: number) => {
    dispatch(showAppLoader(true));
    const opcUaItems = await api.opcUa.getPageOpcUaItemsAsync(opcUaGroupId, pageNumber, countInPage);
    dispatch(showAppLoader(false));
    return opcUaItems;
  };

  export const addOpcUaServer = (opcUaServer: OpcUaServerData) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcUa.addOpcUaServerAsync(opcUaServer);
      dispatch(addOpcUaServers([opcUaServer]));
      dispatch(showAppLoader(false));
    };
  };

  export const removeOpcUaServer = (opcUaServerId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcUa.removeOpcUaServerAsync(opcUaServerId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcUaServer(opcUaServerId));
      dispatch(showAppLoader(false));
    };
  };


  export const addOpcUaGroup = async (dispatch: Dispatch<any>, opcUaGroup: AddOpcUaGroupRequest) => {
    dispatch(showAppLoader(true));
      await api.opcUa.addOpcUaGroupAsync(opcUaGroup);
      dispatch(addOpcUaGroups({opcUaServerId: opcUaGroup.opcUaServerId, opcUaGroups: [opcUaGroup]}));
      dispatch(showAppLoader(false));
  }
 
  export const removeOpcUaGroup = (opcUaServerId: string, opcUaGroupId: string) => {
    return async (dispatch: Dispatch<any>) => {
      dispatch(showAppLoader(true));
      await api.opcUa.removeOpcUaGroupAsync(opcUaGroupId);
      dispatch(setSelectedItem(null));
      dispatch(deleteOpcUaGroup({opcUaServerId, opcUaGroupId}));
      dispatch(showAppLoader(false));
    };
  };

  export const browseOpcUaServer = async (dispatch: Dispatch<any>, item: OpcUaDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opcUa.browseOpcUaServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const browseAllOpcUaServer = async (dispatch: Dispatch<any>, item: OpcUaDiscoveryItemData) => {
    dispatch(showAppLoader(true));
    const items = await api.opcUa.browseAllOpcUaServerAsync(item);
    dispatch(showAppLoader(false));
    return items;
  }

  export const getOpcUaGroupWorks = async (dispatch: Dispatch<any>, opcUaGroupId: string, workTypes: OpcUaWorkType[]) => {
    dispatch(showAppLoader(true));
    const works = await api.opcUa.getOpcUaGroupWorksAsync(opcUaGroupId, workTypes);
    dispatch(showAppLoader(false));
    return works;
   
  }

  export const addOpcUaGroupWork = async (dispatch: Dispatch<any>, work: OpcUaGroupWorkData) => {
    dispatch(showAppLoader(true));
    await api.opcUa.addOpcUaGroupWorkAsync(work);
    dispatch(showAppLoader(false));
  }

  export const updateOpcUaGroupWork = async (dispatch: Dispatch<any>, work: OpcUaGroupWorkData) => {
    dispatch(showAppLoader(true));
    await api.opcUa.updateOpcUaGroupWorkAsync(work);
    dispatch(showAppLoader(false));
  }

  export const deleteOpcUaGroupWork = async (dispatch: Dispatch<any>, opcUaGroupWorkId: string) => {
    dispatch(showAppLoader(true));
    await api.opcUa.deleteOpcUaGroupWorkAsync(opcUaGroupWorkId);
    dispatch(showAppLoader(false));
  }