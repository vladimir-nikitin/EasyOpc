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
  OPC_DA_GROUP_TYPE,
  OPC_DA_SERVER_TYPE,
} from "../../../../constans/opc.da";
import { setSelectedItem } from "../../../../store/windowSlice";
import { OpcDaGroup } from "./OpcDaGroup";
import { loadOpcDaGroups } from "../../../../functions/opc.da";

type OpcDaServerProps = Omit<DrawerProps, "classes"> &
  WithStyles<any> & { opcDaServerId: string };

export const OpcDaServer = (props: OpcDaServerProps) => {
  console.log(`[App][Navigator][OpcDaServers][OpcDaServer][${props.opcDaServerId}] mount component`);
  const dispath = useDispatch();

  const itemPrimary = { primary: props.classes.itemPrimary };

  const opcDaServer = useSelector((state: AppState) => state.opcDa.opcDaServers.find(p => p.id === props.opcDaServerId));
  const { selectedItem } = useSelector((state: AppState) => state.window);

  const isSelected =
    selectedItem &&
    ((selectedItem.type === OPC_DA_SERVER_TYPE &&
      selectedItem.item.id === props.opcDaServerId) ||
      (selectedItem.type === OPC_DA_GROUP_TYPE &&
        selectedItem.item.opcDaServerId === props.opcDaServerId));

  const selectHandle = () => {
    dispath(setSelectedItem({ type: OPC_DA_SERVER_TYPE, item: opcDaServer }));
    if(opcDaServer.opcDaGroups == null){
        dispath(loadOpcDaGroups(opcDaServer.id));
    }
  }

  return (
    <>
      <ListItem
        button
        className={clsx(
          props.classes.item,
          isSelected &&
            selectedItem.type === OPC_DA_SERVER_TYPE &&
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
            {opcDaServer.name}
          </Box>
        </ListItemText>
        {isSelected ? <ExpandLess /> : <ExpandMore />}
      </ListItem>
      <Collapse in={isSelected} timeout="auto" unmountOnExit>
        {opcDaServer.opcDaGroups && 
        <List component="div" disablePadding>
            {opcDaServer.opcDaGroups.map(group => 
                <OpcDaGroup key={group.id} classes={props.classes} opcDaServerId={group.opcDaServerId} opcDaGroupId={group.id} />)}
        </List>}
      </Collapse>
    </>
  );
};