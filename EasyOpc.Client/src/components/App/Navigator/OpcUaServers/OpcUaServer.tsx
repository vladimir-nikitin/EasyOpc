import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../../store/store";
import { DrawerProps } from "@material-ui/core/Drawer";
import { WithStyles } from "@material-ui/core/styles";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import List from "@material-ui/core/List";
import Box from "@material-ui/core/Box";
import StorageIcon from "@material-ui/icons/Storage";
import Collapse from "@material-ui/core/Collapse";
import { ExpandLess, ExpandMore } from "@material-ui/icons";
import clsx from "clsx";
import {
  OPC_UA_GROUP_TYPE,
  OPC_UA_SERVER_TYPE,
} from "../../../../constans/opc.ua";
import { setSelectedItem } from "../../../../store/windowSlice";
import { OpcUaGroup } from "./OpcUaGroup";
import { loadOpcUaGroups } from "../../../../functions/opc.ua";

type OpcUaServerProps = Omit<DrawerProps, "classes"> &
  WithStyles<any> & { opcUaServerId: string };

export const OpcUaServer = (props: OpcUaServerProps) => {
  console.log(`[App][Navigator][OpcUaServers][OpcUaServer][${props.opcUaServerId}] mount component`);
  const dispath = useDispatch();

  const itemPrimary = { primary: props.classes.itemPrimary };

  const opcUaServer = useSelector((state: AppState) => state.opcUa.opcUaServers.find(p => p.id === props.opcUaServerId));
  const { selectedItem } = useSelector((state: AppState) => state.window);

  const isSelected =
    selectedItem &&
    ((selectedItem.type === OPC_UA_SERVER_TYPE &&
      selectedItem.item.id === props.opcUaServerId) ||
      (selectedItem.type === OPC_UA_GROUP_TYPE &&
        selectedItem.item.opcUaServerId === props.opcUaServerId));

  const selectHandle = () => {
    dispath(setSelectedItem({ type: OPC_UA_SERVER_TYPE, item: opcUaServer }));
    if(opcUaServer.opcUaGroups == null){
        dispath(loadOpcUaGroups(opcUaServer.id));
    }
  }

  return (
    <>
      <ListItem
        button
        className={clsx(
          props.classes.item,
          isSelected &&
            selectedItem.type === OPC_UA_SERVER_TYPE &&
            props.classes.itemActiveItem
        )}
        onClick={selectHandle}
      >
        <ListItemIcon className={props.classes.itemIcon}>
          <StorageIcon />
        </ListItemIcon>
        <ListItemText classes={itemPrimary}>
          <Box
            component="div"
            textOverflow="ellipsis"
            overflow="hidden"
            fontStyle="inherit"
          >
            {opcUaServer.name}
          </Box>
        </ListItemText>
        {isSelected ? <ExpandLess /> : <ExpandMore />}
      </ListItem>
      <Collapse in={isSelected} timeout="auto" unmountOnExit>
        {opcUaServer.opcUaGroups && 
        <List component="div" disablePadding>
            {opcUaServer.opcUaGroups.map(group => 
                <OpcUaGroup key={group.id} classes={props.classes} opcUaServerId={group.opcUaServerId} opcUaGroupId={group.id} />)}
        </List>}
      </Collapse>
    </>
  );
};