import React, { useEffect, useState } from "react";
import clsx from "clsx";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles,
} from "@material-ui/core/styles";
import Divider from "@material-ui/core/Divider";
import Drawer, { DrawerProps } from "@material-ui/core/Drawer";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import HomeIcon from "@material-ui/icons/Home";
import { Omit } from "@material-ui/types";
import { useDispatch, useSelector } from "react-redux";
import { Settings } from "./Settings";
import { Logs } from "./Logs";
import { AppState } from "../../../store/store";
import { opcDaServersSelector } from "../../../store/opcDaSlice.selectors";
import { opcUaServersSelector } from "../../../store/opcUaSlice.selectors";
import { OpcDaServers } from "./OpcDaServers/OpcDaServers";
import { OpcUaServers } from "./OpcUaServers/OpcUaServers";

const styles = (theme: Theme) =>
  createStyles({
    categoryHeader: {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    categoryHeaderPrimary: {
      color: theme.palette.common.white,
    },
    button: {
      color: theme.palette.common.white,
    },
    item: {
      paddingTop: 1,
      paddingBottom: 1,
      color: "rgba(255, 255, 255, 0.7)",
      "&:hover,&:focus": {
        backgroundColor: "rgba(255, 255, 255, 0.08)",
      },
    },
    itemCategory: {
      backgroundColor: "#232f3e",
      boxShadow: "0 -1px 0 #404854 inset",
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    firebase: {
      fontSize: 24,
      color: theme.palette.common.white,
    },
    itemActiveItem: {
      color: "#4fc3f7",
    },
    itemPrimary: {
      fontSize: "inherit",
    },
    itemIcon: {
      minWidth: "auto",
      marginRight: theme.spacing(2),
    },
    divider: {
      marginTop: theme.spacing(2),
    },
  });

type NavigatorProps = Omit<DrawerProps, "classes"> & WithStyles<typeof styles>;

export const Navigator = withStyles(styles)((props: NavigatorProps) => {
  console.log(`[App][Navigator] mount component`);

  const { classes, ...other } = props;
  const dispath = useDispatch();

  const categoryHeaderPrimary = { primary: classes.categoryHeaderPrimary, }
  const itemPrimary = { primary: classes.itemPrimary, }

  return (
    <Drawer variant="permanent" {...other}>
      <List disablePadding>
        <ListItem
          className={clsx(classes.firebase, classes.item, classes.itemCategory)}
        >
          Easy OPC
        </ListItem>
        <ListItem className={clsx(classes.item, classes.itemCategory)}>
          <ListItemIcon className={classes.itemIcon}>
            <HomeIcon />
          </ListItemIcon>
          <ListItemText classes={itemPrimary}>
            Home
          </ListItemText>
        </ListItem>
        
        <OpcDaServers classes={classes} />
        <OpcUaServers classes={classes} />
        
        <React.Fragment key={"Main"}>
          <ListItem className={classes.categoryHeader}>
            <ListItemText classes={categoryHeaderPrimary}>
              Main
            </ListItemText>
          </ListItem>
          <Settings {...props} />
          <Logs {...props} />
          <Divider className={classes.divider} />
        </React.Fragment>
      </List>     
    </Drawer>
  );
});
