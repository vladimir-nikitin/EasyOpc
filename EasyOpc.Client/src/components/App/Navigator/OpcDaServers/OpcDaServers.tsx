import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { loadOpcDaServers } from "../../../../functions/opc.da";
import { opcDaServersSelector } from "../../../../store/opcDaSlice.selectors";
import { AppState } from "../../../../store/store";
import { DrawerProps } from "@material-ui/core/Drawer";
import { WithStyles } from "@material-ui/core/styles";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import IconButton from "@material-ui/core/IconButton";
import AddIcon from "@material-ui/icons/Add";
import Divider from "@material-ui/core/Divider";
import { OpcDaServer } from "./OpcDaServer";
import { AddOpcDaServerModal } from "./AddOpcDaServerModal";

type OpcDaServersProps = Omit<DrawerProps, "classes"> & WithStyles<any>;

export const OpcDaServers = (props: OpcDaServersProps) => {
  console.log(`[App][Navigator][OpcDaServers] mount component`);

  const dispath = useDispatch();

  const opcDaServers = opcDaServersSelector((state: AppState) => state.opcDa.opcDaServers);
  const { serviceMode } = useSelector((state: AppState) => state.window);

  const [showAddDaServerModal, setShowAddDaServerModal] = useState(false);

  useEffect(() => {
    if (opcDaServers == null) {
      dispath(loadOpcDaServers());
    }
  }, []);

  const addHandle = () => setShowAddDaServerModal(true);

  const categoryHeaderPrimary = { primary: props.classes.categoryHeaderPrimary };
  const iconButtonStyle = { padding: "8px 0" };

  return (
    <>
      <ListItem className={props.classes.categoryHeader}>
        <ListItemText classes={categoryHeaderPrimary}>OPC.DA</ListItemText>
        <IconButton
          className={props.classes.button}
          style={iconButtonStyle}
          onClick={addHandle}
          disabled={!serviceMode}
        >
          <AddIcon />
        </IconButton>
      </ListItem>
      {opcDaServers && opcDaServers.map((server) => <OpcDaServer key={server.id} classes={props.classes} opcDaServerId={server.id} /> )}
      <Divider className={props.classes.divider} />
      {showAddDaServerModal && <AddOpcDaServerModal isOpen cancelHandle={() => setShowAddDaServerModal(false)} />}
    </>
  );
};
