import React, { useEffect, useMemo, useState } from "react";
import { SimpleDialog } from "../../controls/SimpleDialog";
import { useDispatch } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import {
  addOpcGroup,
  browseAllOpcServer,
  browseOpcServer,
  disconnectFromOpcServer,
} from "../../../functions/opc";
import { Guid } from "guid-typescript";
import { OpcDiscoveryItemData } from "../../../types/opc";
import TreeView from "@material-ui/lab/TreeView";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import TreeItem from "@material-ui/lab/TreeItem";
import { Checkbox } from "@material-ui/core";
import Typography from "@material-ui/core/Typography";

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

type AddOpcGroupModalProps = {
  opcServerId: string;
  isOpen: boolean;
  cancelHandle: () => void;
};

type LazyTreeItemProps = {
  item: OpcDiscoveryItemData,
  checkBoxClicked: (event: any, checked: boolean,item: OpcDiscoveryItemData) => void,
  selectedItems: Map<string, OpcDiscoveryItemData>,
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
        checked={!!props.selectedItems.get(props.item.name)}
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

export const AddOpcGroupModal = (props: AddOpcGroupModalProps) => {

  //console.log(`AddOpcGroupModal mount component`)

  const classes = useStyles();
  const dispatch = useDispatch();

  const [name, setName] = useState<string>('Group_1');
  const [items, setItems] = useState<OpcDiscoveryItemData[] | null>(null);
  const [selectedItems, setSelectedItems] = useState<Map<string, OpcDiscoveryItemData>>(new Map<string, OpcDiscoveryItemData>());

  const nameChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setName((event.target as HTMLInputElement).value);

  useEffect(() => {
    if (!items) {
      browseOpcServer(dispatch, {
        opcServerId: props.opcServerId,
        name: null,
        id: null,
        accessPath: null,
        hasChildren: false,
      }).then((result) => setItems(result));
    }
  }, [items]);

  const find = (
    id: string,
    items: OpcDiscoveryItemData[]
  ): OpcDiscoveryItemData | null => {
    if (!items || items.length < 1) return null;
    const item = items.find((x) => x.id === id);
    if (item) return item;
    return find(
      id,
      [].concat(...items.filter((x) => x.childs).map((x) => x.childs))
    );
  };

  const getAllExistChilds = (item: OpcDiscoveryItemData): OpcDiscoveryItemData[] => {
    return item.childs
      ? item.childs.concat(...item.childs.map((p) => getAllExistChilds(p)))
      : [];
  };

  const getAllChildsAsync = async(item: OpcDiscoveryItemData): Promise<OpcDiscoveryItemData[]> => {
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
      item.childs = await browseAllOpcServer(dispatch, item);
      item.hasChildren = item.childs && item.childs.length > 0;
      return item.childs.concat(...item.childs.map(ch => getAllExistChilds(ch)));
    }
  };

  const handleToggle = (event: React.ChangeEvent<{}>, nodeIds: string[]) => {
    if (nodeIds && nodeIds.length > 0) {
      const item = find(nodeIds[0], items);
      if (item && item.hasChildren && !item.childs) {
        browseOpcServer(dispatch, item).then((result) => {
          item.childs = result;
          item.hasChildren = result && result.length > 0;
          setItems([...items]);
        });
      }
    }
  };

  const checkBoxClicked = async(event: any, checked: boolean, item: OpcDiscoveryItemData) => {
    event.stopPropagation();
    if (checked) {
      const allChilds = await getAllChildsAsync(item);
      setItems([...items]);

      const newSelectedItems = new Map<string, OpcDiscoveryItemData>(selectedItems);
      !newSelectedItems.has(item.name) && newSelectedItems.set(item.name, item);
      allChilds.forEach(p => !newSelectedItems.has(p.name) && newSelectedItems.set(p.name, p));
      
      setSelectedItems(newSelectedItems);
    } else {
      const unselectItems = [item, ...getAllExistChilds(item)];

      const newSelectedItems = new Map<string, OpcDiscoveryItemData>(selectedItems);
      unselectItems.forEach(p => newSelectedItems.delete(p.name));

      setSelectedItems(newSelectedItems);
    }
  }

  const cancel = async () => { 
    await disconnectFromOpcServer(dispatch, props.opcServerId); 
    props.cancelHandle();
  }

  /*const renderTreeItem = (item: OpcDiscoveryItemData) => {
    const label = (
      <div className={classes.label}>
        <Checkbox
          id={`checkbox-${item.id}`}
          checked={!!selectedItems.find((p) => p.id === item.id)}
          onChange={(event, checked) => checkBoxClicked(event, checked, item)}
          onClick={(event) => event.stopPropagation()}
          color="primary"
        />
        <Typography variant="caption">{item.name}</Typography>
      </div>
    );

    return (
      <TreeItem key={item.id} nodeId={item.id} label={label}>
        {item.hasChildren &&
          (item.childs ? item.childs.map((p, i) => renderTreeItem(p)) : "Loading...")}
      </TreeItem>
    );
  };*/

  const okHandle = () => {
    const opcGroupId = Guid.create().toString();
    //const selectedItemsMap = new Map<string, OpcDiscoveryItemData>();
    /*selectedItems.filter((item) => !item.childs || item.childs.length < 1)
                 .forEach(item => selectedItemsMap.get(item.name) == null && selectedItemsMap.set(item.name, item));*/
    dispatch(
      addOpcGroup({
        id: opcGroupId,
        name: name,
        opcServerId: props.opcServerId,
        reqUpdateRate: 1000,
        opcItems: Array.from(selectedItems.values()).filter((item) => !item.childs || item.childs.length < 1)
          .map((item) => ({
            id: Guid.create().toString(),
            name: item.name,
            opcGroupId: opcGroupId,
            accessPath: item.accessPath,
          })),
      })
    );

    cancel();
  };

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
              selectedItems={selectedItems} /> /*renderTreeItem(item)*/)}
        </TreeView>
      </div>
    </SimpleDialog>
  );
}
