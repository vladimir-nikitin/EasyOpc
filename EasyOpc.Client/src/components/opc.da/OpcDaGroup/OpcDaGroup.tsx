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
import { removeOpcDaGroup } from "../../../functions/opc.da";
import { TabPanel } from "../../controls/TabPanel";
import { SimpleConfirmDialog } from "../../controls/SimpleConfirmDialog";
import { OpcDaItems } from "./OpcDaItems/OpcDaItems";
import { OpcDaGroupSubscritions } from "./OpcDaGroupSubscritions/OpcDaGroupSubscritions";
import { OpcDaGroupExports } from "./OpcDaGroupExports/OpcDaGroupExports";

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

type OpcDaGroupProps = {
  opcDaServerId: string;
  opcDaGroupId: string;
};

export const OpcDaGroup = (props: OpcDaGroupProps) => {

  console.log(`[Opc][OpcDaGroup] mount component props:`);
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
    dispatch(removeOpcDaGroup(props.opcDaServerId, props.opcDaGroupId)); 
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
          <OpcDaItems opcDaServerId={props.opcDaServerId} opcDaGroupId={props.opcDaGroupId}  />
        </TableContainer>
      </TabPanel>

      <TabPanel value={value} index={1}>
        <OpcDaGroupSubscritions opcDaServerId={props.opcDaServerId} opcDaGroupId={props.opcDaGroupId} />
      </TabPanel>

      <TabPanel value={value} index={2}>
        <OpcDaGroupExports opcDaServerId={props.opcDaServerId} opcDaGroupId={props.opcDaGroupId} />
      </TabPanel>

      <SimpleConfirmDialog
        isOpen={showRemoveConfirmDialog}
        header={"OPC.DA group"}
        text={"Delete the OPC.DA group?"}
        cancelHandle={() => setShowRemoveConfirmDialog(false)}
        okHandle={deleteHandle}
      />
    </div>
  );
}
