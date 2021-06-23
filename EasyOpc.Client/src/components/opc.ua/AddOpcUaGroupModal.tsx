import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import { Guid } from "guid-typescript";
import TreeView from "@material-ui/lab/TreeView";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import TreeItem from "@material-ui/lab/TreeItem";
import { Checkbox } from "@material-ui/core";
import Typography from "@material-ui/core/Typography";
import { Connection, hubConnection, Proxy } from "signalr-no-jquery";
import { OpcUaDiscoveryItemData } from "../../types/opc.ua";
import { AppState } from "../../store/store";
import { SIGNAL_R_URL } from "../../constans/common";
import { showAppLoader } from "../../store/windowSlice";
import { addOpcUaGroup, browseAllOpcUaServer, browseOpcUaServer } from "../../functions/opc.ua";
import { SimpleDialog } from "../controls/SimpleDialog";

const useStyles = makeStyles({
  container: {
    display: "flex",
    flexDirection: "column",
    width: 700,
    height: 500,
    padding: 20,
  },
  button: {
    marginLeft: 20,
  },
  text: {
    flexGrow: 1,
  },
  input: {
    margin: "0 8px 30px 8px",
    maxWidth: 200,
  },
  root: {
    height: 300,
    width: 400,
  },
  label: {
    display: "flex", 
    alignItems: "center"
  }
});

type AddOpcUaGroupModalProps = {
  opcUaServerId: string;
  isOpen: boolean;
  cancelHandle: () => void;
};

type LazyTreeItemProps = {
  item: OpcUaDiscoveryItemData,
  checkBoxClicked: (event: any, checked: boolean,item: OpcUaDiscoveryItemData) => void,
  selectedItems: Map<string, OpcUaDiscoveryItemData>,
}

const LazyTreeItem = (props: LazyTreeItemProps) => {
  //console.log(`LazyTreeItem mount component`)
  //console.log(props)

  const classes = useStyles();

  const [renderChild, setRenderChild] = useState(false);

  const label = (
    <div className={classes.label}>
      <Checkbox
        id={`checkbox-${props.item.id}`}
        checked={!!props.selectedItems.get(props.item.id)}
        onChange={(event, checked) => props.checkBoxClicked(event, checked, props.item)}
        onClick={(event) => event.stopPropagation()}
        color="primary"
      />
      <Typography variant="caption">{props.item.name}</Typography>
    </div>
  );

  return (
    <TreeItem key={props.item.id} nodeId={props.item.id} label={label} onClick={e => !renderChild && setRenderChild(true)}>
      {props.item.hasChildren &&
        (props.item.childs && renderChild ? props.item.childs.map((p, i) => 
          <LazyTreeItem 
            key={i} 
            item={p} 
            checkBoxClicked={props.checkBoxClicked} 
            selectedItems={props.selectedItems} 
          />) : "Loading...")}
    </TreeItem>
  );
}

export const AddOpcUaGroupModal = (props: AddOpcUaGroupModalProps) => {

  //console.log(`AddOpcGroupModal mount component`)

  const classes = useStyles();
  const dispatch = useDispatch();

  const serviceMode  = useSelector((state: AppState) => state.window.serviceMode);

  const [name, setName] = useState<string>('OpcUaGroup_1');
  const [items, setItems] = useState<OpcUaDiscoveryItemData[] | null>(null);
  const [selectedItems, setSelectedItems] = useState<Map<string, OpcUaDiscoveryItemData>>(new Map<string, OpcUaDiscoveryItemData>());

  const nameChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setName((event.target as HTMLInputElement).value);


  const [signalrConnection, setSignalrConnection] = useState<{connection: Connection;hub: Proxy;} | null>(null);

  const connect = () => {
    const connection = hubConnection(SIGNAL_R_URL);
    const hub = connection.createHubProxy("OpcUaServerHub");

    hub.on("OnConnected", (opcUaGroupId: string, error: string | null): void => {
      dispatch(showAppLoader(false));
      if(error){
        error && alert(error);
      }
      else {
        console.log(`##OnConnected`)
        setSignalrConnection({ connection: connection, hub: hub });

        browseOpcUaServer(dispatch, {
          opcUaServerId: props.opcUaServerId,
          name: null,
          id: null,
          nodeId: null,
          hasChildren: false,
          hasValue: false,
        }).then((result) => setItems(result));
      }
    });

    hub.on(
      "OnDisconnected", (opcUaGroupId: string, error: string | null): void => {
        console.log('OnDisconnected');
        dispatch(showAppLoader(false));
        props.cancelHandle();
      }
    );

    hub.on("Ping", (): void => {
      hub.invoke("Pong", props.opcUaServerId);
    });

    connection.start().done(() => {
      hub.invoke("Connect", props.opcUaServerId);
      dispatch(showAppLoader(true));
    });

    return { connection: connection, hub: hub };
  };

  useEffect(() => {
    if(serviceMode && !signalrConnection){
      connect();
    }
    else if(serviceMode && signalrConnection){
      return () => {
        if(signalrConnection == null) 
          return;

        try{
          signalrConnection?.connection?.stop();
          setSignalrConnection(null);
        }
        catch(ex){
        }
      }
    }

    return null;
  }, [serviceMode, signalrConnection, props.opcUaServerId]);

  const find = (
    id: string,
    items: OpcUaDiscoveryItemData[]
  ): OpcUaDiscoveryItemData | null => {
    if (!items || items.length < 1) return null;
    const item = items.find((x) => x.id === id);
    if (item) return item;
    return find(
      id,
      [].concat(...items.filter((x) => x.childs).map((x) => x.childs))
    );
  };

  const getAllExistChilds = (item: OpcUaDiscoveryItemData): OpcUaDiscoveryItemData[] => {
    return item.childs
      ? item.childs.concat(...item.childs.map((p) => getAllExistChilds(p)))
      : [];
  };

  const getAllChildsAsync = async(item: OpcUaDiscoveryItemData): Promise<OpcUaDiscoveryItemData[]> => {
    if(!item.hasChildren){
      return [];
    }

    if(item.childs){
      let result = [...item.childs];
      let i = 0;
      for(i; i < item.childs.length; i++)
      {
        result = result.concat(await getAllChildsAsync(item.childs[i]));
      }
      return result;
    }
    else{
      item.childs = await browseAllOpcUaServer(dispatch, item);
      item.hasChildren = item.childs && item.childs.length > 0;
      return item.childs.concat(...item.childs.map(ch => getAllExistChilds(ch)));
    }
  };

  const handleToggle = useCallback((event: React.ChangeEvent<{}>, nodeIds: string[]) => {
    if (nodeIds && nodeIds.length > 0) {
      const item = find(nodeIds[0], items);
      if (item && item.hasChildren && !item.childs) {
        browseOpcUaServer(dispatch, item).then((result) => {
          item.childs = result;
          item.hasChildren = result && result.length > 0;
          setItems([...items]);
        });
      }
    }
  }, [items]);

  const checkBoxClicked = useCallback(async(event: any, checked: boolean, item: OpcUaDiscoveryItemData) => {
    event.stopPropagation();
    if (checked) {
      const allChilds = await getAllChildsAsync(item);
      setItems([...items]);

      const newSelectedItems = new Map<string, OpcUaDiscoveryItemData>(selectedItems);
      !newSelectedItems.has(item.id) && newSelectedItems.set(item.id, item);
      allChilds.forEach(p => !newSelectedItems.has(p.id) && newSelectedItems.set(p.id, p));
      
      setSelectedItems(newSelectedItems);
    } else {
      const unselectItems = [item, ...getAllExistChilds(item)];

      const newSelectedItems = new Map<string, OpcUaDiscoveryItemData>(selectedItems);
      unselectItems.forEach(p => newSelectedItems.delete(p.id));

      setSelectedItems(newSelectedItems);
    }
  }, [items, selectedItems]);

  const cancel = useCallback(() => { 
    if(signalrConnection){
      dispatch(showAppLoader(true));
      signalrConnection.hub.invoke("Disconnect", props.opcUaServerId);
    }
    else{
      props.cancelHandle();
    }
    //await disconnectFromOpcServer(dispatch, props.opcServerId); 
    //props.cancelHandle();
  }, [signalrConnection])

  const okHandle = useCallback(async () => {
    const opcUaGroupId = Guid.create().toString();

    const getOpcDiscoveryItemId = (item: OpcUaDiscoveryItemData) => `${item.name};nodeId=${item.nodeId}`;
    const addingItemsMap = new Map<string, OpcUaDiscoveryItemData>([]);
    Array.from(selectedItems.values()).forEach(p => p.hasValue && !addingItemsMap.has(getOpcDiscoveryItemId(p)) && addingItemsMap.set(getOpcDiscoveryItemId(p), p));

    await addOpcUaGroup(dispatch, {
      id: opcUaGroupId,
      name: name,
      opcUaServerId: props.opcUaServerId,
      reqUpdateRate: 1000,
      opcUaItems: Array.from(addingItemsMap.values())
      .map((item) => ({
          id: Guid.create().toString(),
          name: item.name,
          opcUaGroupId: opcUaGroupId,
          nodeId: item.nodeId
        }))
    });
    cancel();
  }, [selectedItems]);

  return (
    <SimpleDialog
      isOpen={props.isOpen}
      cancelHandle={cancel}
      okHandle={okHandle}
      isOkButtonDisabled={!name}
      header={"Add group"}
    >
      <div className={classes.container}>
        <TextField
          label="Name:"
          value={name}
          onChange={nameChangedHandle}
          className={classes.input}
        />
        <TreeView
          className={classes.root}
          defaultCollapseIcon={<ExpandMoreIcon />}
          defaultExpandIcon={<ChevronRightIcon />}
          onNodeToggle={handleToggle}
          multiSelect
        >
          {items && items.map((item, i) => 
            <LazyTreeItem 
              key={i} 
              item={item} 
              checkBoxClicked={checkBoxClicked} 
              selectedItems={selectedItems} />)}
        </TreeView>
      </div>
    </SimpleDialog>
  );
}
