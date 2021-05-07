import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { makeStyles } from "@material-ui/core/styles";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableContainer from "@material-ui/core/TableContainer";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
import { getOpcItems, loadOpcItems, removeOpcGroup } from "../../../functions/opc";
import AppBar from "@material-ui/core/AppBar";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import DeleteIcon from "@material-ui/icons/Delete";
import IconButton from "@material-ui/core/IconButton";
import { SimpleConfirmDialog } from "../../controls/SimpleConfirmDialog";
import OpcGroupHistorySettings from "./OpcGroupHistorySettings";
import OpcGroupExportSettings from "./OpcGroupExportSettings";
import Box from "@material-ui/core/Box";
import { Connection, hubConnection, Proxy } from "signalr-no-jquery";
import OpcItemRow from "./OpcItems/OpcItemRow";
import { setOpcItems, updateOpcItems } from "../../../store/opcSlice";
import { TabPanel } from "../../controls/TabPanel";
import { OpcItems } from "./OpcItems/OpcItems";

const useStyles = makeStyles({
  container: {
    height: "100%",
    display: "flex",
    flexDirection: "column",
  },
  bar: {
    display: "flex",
    flexDirection: "row",
  },
  deleteButton: {
    marginLeft: "auto",
    marginRight: "24px",
  },
  table: {
    minWidth: 650,
  },
});

type OpcGroupProps = {
  opcServerId: string;
  opcGroupId: string;
};

export default function OpcGroup(props: OpcGroupProps) {

  console.log(`[Opc][OpcGroup] mount component props:`);
  console.log(props);

  const dispatch = useDispatch();
  const classes = useStyles();

  const serviceMode  = useSelector((state: AppState) => state.window.serviceMode);
  
  const [value, setValue] = useState(0);
  const [showRemoveConfirmDialog, setShowRemoveConfirmDialog] = useState(false);

  const changeHandle = (event: React.ChangeEvent<{}>, newValue: number) => {
    setValue(newValue);
  };

  const deleteHandle = () => { 
    dispatch(removeOpcGroup(props.opcServerId, props.opcGroupId)); 
  }

  return (
    <div className={classes.container}>
      <AppBar
        component="div"
        color="primary"
        position="static"
        elevation={0}
        className={classes.bar}
      >
        <Tabs textColor="inherit" value={value} onChange={changeHandle}>
          <Tab textColor="inherit" label="Items" />
          <Tab textColor="inherit" label="Subscrition Mode" />
          <Tab textColor="inherit" label="Export" />
        </Tabs>

        <IconButton
          color="inherit"
          className={classes.deleteButton}
          disabled={!serviceMode}
          onClick={() => setShowRemoveConfirmDialog(true)}
        >
          <DeleteIcon />
        </IconButton>
      </AppBar>

      <TabPanel value={value} index={0}>
        <TableContainer component={Paper}>
          <OpcItems opcServerId={props.opcServerId} opcGroupId={props.opcGroupId}  />
        </TableContainer>
      </TabPanel>

      <TabPanel value={value} index={1}>
        <OpcGroupHistorySettings opcGroupId={props.opcGroupId} />
      </TabPanel>

      <TabPanel value={value} index={2}>
        <OpcGroupExportSettings opcGroupId={props.opcGroupId} />
      </TabPanel>

      <SimpleConfirmDialog
        isOpen={showRemoveConfirmDialog}
        header={"OPC group"}
        text={"Delete the OPC group?"}
        cancelHandle={() => setShowRemoveConfirmDialog(false)}
        okHandle={deleteHandle}
      />
    </div>
  );
}
