import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { loadOpcUaServers } from "../../../../functions/opc.ua";
import { opcUaServersSelector } from "../../../../store/opcUaSlice.selectors";
import { AppState } from "../../../../store/store";
import { DrawerProps } from "@material-ui/core/Drawer";
import { WithStyles } from "@material-ui/core/styles";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import IconButton from "@material-ui/core/IconButton";
import AddIcon from "@material-ui/icons/Add";
import Divider from "@material-ui/core/Divider";
import { OpcUaServer } from "./OpcUaServer";
import { AddOpcUaServerModal } from "./AddOpcUaServerModal";

type OpcUaServersProps = Omit<DrawerProps, "classes"> & WithStyles<any>;

export const OpcUaServers = (props: OpcUaServersProps) => {
  console.log(`[App][Navigator][OpcUaServers] mount component`);

  const dispath = useDispatch();

  const opcUaServers = opcUaServersSelector((state: AppState) => state.opcUa.opcUaServers);
  const { serviceMode } = useSelector((state: AppState) => state.window);

  const [showAddUaServerModal, setShowAddUaServerModal] = useState(false);

  useEffect(() => {
    if (opcUaServers == null) {
      dispath(loadOpcUaServers());
    }
  }, []);

  const addHandle = () => setShowAddUaServerModal(true);

  const categoryHeaderPrimary = { primary: props.classes.categoryHeaderPrimary };
  const iconButtonStyle = { padding: "8px 0" };

  return (
    <>
      <ListItem className={props.classes.categoryHeader}>
        <ListItemText classes={categoryHeaderPrimary}>OPC.UA</ListItemText>
        <IconButton
          className={props.classes.button}
          style={iconButtonStyle}
          onClick={addHandle}
          disabled={!serviceMode}
        >
          <AddIcon />
        </IconButton>
      </ListItem>
      {opcUaServers && opcUaServers.map((server) => <OpcUaServer key={server.id} classes={props.classes} opcUaServerId={server.id} /> )}
      <Divider className={props.classes.divider} />
      {showAddUaServerModal && <AddOpcUaServerModal isOpen cancelHandle={() => setShowAddUaServerModal(false)} />}
    </>
  );
};
