import React, { useEffect, useState } from "react";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core/styles";
import StorageIcon from "@material-ui/icons/Storage";
import { useDispatch, useSelector } from "react-redux";
import DeleteIcon from "@material-ui/icons/Delete";
import AddIcon from "@material-ui/icons/Add";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import IconButton from "@material-ui/core/IconButton";
import { AppState } from "../../store/store";
import { removeOpcUaServer } from "../../functions/opc.ua";
import { TabPanel } from "../controls/TabPanel";
import { SimpleConfirmDialog } from "../controls/SimpleConfirmDialog";
import { AddOpcUaGroupModal } from "./AddOpcUaGroupModal";


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
  paper: {
    //margin: "40px",
    overflow: "hidden",
    width: "100%",
    height: "300px"
  },
  searchBar: {
    borderBottom: "1px solid rgba(0, 0, 0, 0.12)",
  },
  block: {
    display: "block",
  },
  content: {
    paddingLeft: 74, 
    paddingTop: 24, 
    paddingBottom: 24
  }
});

type OpcUaServerProps = {
  opcUaServerId: string;
}

export const OpcUaServer = (props: OpcUaServerProps) => {

  console.log(`[App][Content][OpcUaServer] mount component`);

  const dispatch = useDispatch();
  const classes = useStyles();

  const serviceMode = useSelector((state: AppState) => state.window.serviceMode);
  const opcUaServer = useSelector((state: AppState) => state.opcUa.opcUaServers.find((s) => s.id === props.opcUaServerId));

  const removeHandle = () => dispatch(removeOpcUaServer(props.opcUaServerId));
  const [showRemoveConfirmDialog, setShowRemoveConfirmDialog] = useState(false);
  const [showAddOpcUaGroupDialog, setShowAddOpcUaGroupDialog] = useState(false);
  
  const [value, setValue] = useState(0);
  const changeHandle = (event: React.ChangeEvent<{}>, newValue: number) => {
    setValue(newValue);
  };

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
          <Tab textColor="inherit" label="Info" />
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
      <Paper className={classes.paper}>
      <AppBar
        className={classes.searchBar}
        position="static"
        color="default"
        elevation={0}
      >
        <Toolbar>
          <Grid container spacing={2} alignItems="center">
            <Grid item>
              <StorageIcon className={classes.block} color="inherit" />
            </Grid>
            <Grid item>
              <Typography color="inherit" variant="h6">
                OPC.UA server info
              </Typography>
            </Grid>
            <Grid item xs></Grid>
            <Grid item>
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                disabled={!serviceMode}
                onClick={() => setShowAddOpcUaGroupDialog(true)}
              >
                Add group
              </Button>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <div className={classes.content}>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Name:
          </Typography>

          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcUaServer.name}
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Type:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              OPC.UA
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Host:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcUaServer.host}
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Groups:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcUaServer.opcUaGroups ? opcUaServer.opcUaGroups.length : 0}
            </Typography>
          </Grid>
        </Grid>
      </div>

      {showRemoveConfirmDialog && (
        <SimpleConfirmDialog
          isOpen
          header={"OPC.DA server"}
          text={"Delete the OPC.DA server?"}
          cancelHandle={() => setShowRemoveConfirmDialog(false)}
          okHandle={removeHandle}
        />
      )}

      {showAddOpcUaGroupDialog && (
        <AddOpcUaGroupModal
          opcUaServerId={props.opcUaServerId}
          isOpen
          cancelHandle={() => setShowAddOpcUaGroupDialog(false)}
        />
      )}
    </Paper>
      </TabPanel>

    </div>
  );
}


