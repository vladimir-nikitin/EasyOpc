import React, { useEffect, useState } from "react";
import clsx from "clsx";
import {
  WithStyles,
} from "@material-ui/core/styles";
import Divider from "@material-ui/core/Divider";
import { DrawerProps } from "@material-ui/core/Drawer";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import StorageIcon from "@material-ui/icons/Storage";
import Collapse from "@material-ui/core/Collapse";
import { ExpandLess, ExpandMore } from "@material-ui/icons";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { loadOpcGroups } from "../../../functions/opc";
import { setSelectedItem } from "../../../store/windowSlice";
import {
  OPC_GROUP_TYPE,
  OPC_SERVER_TYPE,
} from "../../../constans/opc";
import IconButton from "@material-ui/core/IconButton";
import AddIcon from "@material-ui/icons/Add";
import Box from "@material-ui/core/Box";
import OpcGroups from "./OpcGroups";
import { OpcServer } from "../../../store/opcSlice";
import { OpcServerType } from "../../../types/opc";

export interface OpcServersProps
  extends Omit<DrawerProps, "classes">,
    WithStyles<any> {
  type: OpcServerType;
  servers: OpcServer[];
  addClickHandle?: (() => void) | null
}

export default function OpcServers(props: OpcServersProps) {
  const { classes, type, servers, addClickHandle } = props;
  const dispath = useDispatch();
  const { selectedItem, serviceMode } = useSelector((state: AppState) => state.window);

  useEffect(() => {
    if (
      selectedItem &&
      selectedItem.type === OPC_SERVER_TYPE &&
      selectedItem.item.type === type &&
      !selectedItem.item.opcGroups
    ) {
      dispath(loadOpcGroups(selectedItem.item.id));
    }
  }, [selectedItem]);

  const categoryHeaderPrimary = { primary: classes.categoryHeaderPrimary, }
  const iconButtonStyle = {padding: "8px 0"}
  const itemPrimary = { primary: classes.itemPrimary, }

  return (
    <React.Fragment key={type}>
      <ListItem className={classes.categoryHeader}>
        <ListItemText classes={categoryHeaderPrimary}>
          {type}
        </ListItemText>
        <IconButton className={classes.button} style={iconButtonStyle} onClick={addClickHandle} disabled={!serviceMode}>
          <AddIcon />
        </IconButton>
      </ListItem>
      {servers.map((server) => {
        const isSelected =
          selectedItem &&
          ((selectedItem.type === OPC_SERVER_TYPE &&
            selectedItem.item.id === server.id) ||
            (selectedItem.type === OPC_GROUP_TYPE &&
              selectedItem.item.opcServerId === server.id));
        return (
          <React.Fragment key={server.id}>
            <ListItem
              key={server.id}
              button
              className={clsx(
                classes.item,
                isSelected &&
                  selectedItem.type === OPC_SERVER_TYPE &&
                  classes.itemActiveItem
              )}
              onClick={() =>
                dispath(
                  setSelectedItem({ type: OPC_SERVER_TYPE, item: server })
                )
              }
            >
              <ListItemIcon className={classes.itemIcon}>
                <StorageIcon />
              </ListItemIcon>
              <ListItemText classes={itemPrimary}>
                <Box
                  component="div"
                  textOverflow="ellipsis"
                  overflow="hidden"
                  fontStyle="inherit"
                >
                  {server.name}
                </Box>
              </ListItemText>

              {isSelected ? <ExpandLess /> : <ExpandMore />}
            </ListItem>
            <Collapse in={isSelected} timeout="auto" unmountOnExit>
              {server.opcGroups && (
                <OpcGroups opcGroups={server.opcGroups} {...props} />
              )}
            </Collapse>
          </React.Fragment>
        );
      })}
      <Divider className={classes.divider} />
    </React.Fragment>
  );
}