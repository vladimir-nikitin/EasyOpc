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
import { AppState } from "../../../store/store";
import { loadOpcServers } from "../../../functions/opc";
import { OPC_DA_TYPE, OPC_UA_TYPE } from "../../../constans/opc";
import Settings  from "./Settings";
import Logs from "./Logs";
import OpcServers from "./OpcServers";
import AddOpcDaServerModal from "../../Opc/AddOpcDaServerModal/AddOpcDaServerModal";
import AddOpcUaServerModal from "../../Opc/AddOpcUaServerModal/AddOpcUaServerModal";
import { opcServersSelector } from "../../../store/opcSliceSelectors";

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

export interface NavigatorProps
  extends Omit<DrawerProps, "classes">,
    WithStyles<typeof styles> {}

function Navigator(props: NavigatorProps) {
  console.log(`[App][Navigator] mount component`);

  const { classes, ...other } = props;
  const dispath = useDispatch();

  const opcServers = opcServersSelector((state: AppState) => state.opc.opcServers);

  useEffect(() => {
    if (opcServers == null) {
      dispath(loadOpcServers());
    }
  }, []);

  const [showAddDaServerModal, setShowAddDaServerModal] = useState(false);
  const [showAddUaServerModal, setShowAddUaServerModal] = useState(false);

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
        {opcServers && (
          <OpcServers
            type={OPC_DA_TYPE}
            servers={opcServers.filter((s) => s.type === OPC_DA_TYPE)}
            addClickHandle={() => setShowAddDaServerModal(true)}
            {...props}
          />
        )}
        {opcServers && (
          <OpcServers
            type={OPC_UA_TYPE}
            servers={opcServers.filter((s) => s.type === OPC_UA_TYPE)}
            addClickHandle={() => setShowAddUaServerModal(true)}
            {...props}
          />
        )}
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

      {showAddDaServerModal && <AddOpcDaServerModal isOpen cancelHandle={() => setShowAddDaServerModal(false)} />}
      {showAddUaServerModal && <AddOpcUaServerModal isOpen cancelHandle={() => setShowAddUaServerModal(false)} />}
      
    </Drawer>
  );
}

export default withStyles(styles)(Navigator);
