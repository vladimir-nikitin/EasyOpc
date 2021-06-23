import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import TableContainer from "@material-ui/core/TableContainer";
import Paper from "@material-ui/core/Paper";
import AppBar from "@material-ui/core/AppBar";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import DeleteIcon from "@material-ui/icons/Delete";
import IconButton from "@material-ui/core/IconButton";
import { AppState } from "../../../store/store";
import { removeOpcUaGroup } from "../../../functions/opc.ua";
import { TabPanel } from "../../controls/TabPanel";
import { SimpleConfirmDialog } from "../../controls/SimpleConfirmDialog";
import { OpcUaItems } from "./OpcUaItems/OpcUaItems";
import { OpcUaGroupSubscritions } from "./OpcUaGroupSubscritions/OpcUaGroupSubscritions";
import { OpcUaGroupExports } from "./OpcUaGroupExports/OpcUaGroupExports";

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
});

type OpcUaGroupProps = {
  opcUaServerId: string;
  opcUaGroupId: string;
};

export const OpcUaGroup = (props: OpcUaGroupProps) => {

  console.log(`[Opc][OpcUaGroup] mount component props:`);
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
    dispatch(removeOpcUaGroup(props.opcUaServerId, props.opcUaGroupId)); 
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
          <Tab textColor="inherit" label="Export Mode" />
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
          <OpcUaItems opcUaServerId={props.opcUaServerId} opcUaGroupId={props.opcUaGroupId}  />
        </TableContainer>
      </TabPanel>

      <TabPanel value={value} index={1}>
        <OpcUaGroupSubscritions opcUaServerId={props.opcUaServerId} opcUaGroupId={props.opcUaGroupId} />
      </TabPanel>

      <TabPanel value={value} index={2}>
        <OpcUaGroupExports opcUaServerId={props.opcUaServerId} opcUaGroupId={props.opcUaGroupId} />
      </TabPanel>

      <SimpleConfirmDialog
        isOpen={showRemoveConfirmDialog}
        header={"OPC.UA group"}
        text={"Delete the OPC.UA group?"}
        cancelHandle={() => setShowRemoveConfirmDialog(false)}
        okHandle={deleteHandle}
      />
    </div>
  );
}
