import React, { useEffect, useState } from "react";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Paper from "@material-ui/core/Paper";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";
import { makeStyles } from "@material-ui/core/styles";
import StorageIcon from "@material-ui/icons/Storage";
import { OpcServer as OpcServerType } from "../../../store/opcSlice";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import DeleteIcon from "@material-ui/icons/Delete";
import AddIcon from "@material-ui/icons/Add";
import { removeOpcServer } from "../../../functions/opc";
import { SimpleConfirmDialog } from "../../controls/SimpleConfirmDialog";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import IconButton from "@material-ui/core/IconButton";
import { TabPanel } from "../../controls/TabPanel";
import { AddOpcGroupModal } from "../AddOpcGroupModal/AddOpcGroupModal";

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

export interface OpcServerProps {
  opcServer: OpcServerType;
}

export default function OpcServer(props: OpcServerProps) {

  console.log(`[App][Content][OpcServer] mount component`);

  const dispatch = useDispatch();
  const classes = useStyles();

  const serviceMode = useSelector((state: AppState) => state.window.serviceMode);
  const opcServer = useSelector((state: AppState) => state.opc.opcServers.find((s) => s.id === props.opcServer.id));

  const removeHandle = () => dispatch(removeOpcServer(props.opcServer.id));
  const [showRemoveConfirmDialog, setShowRemoveConfirmDialog] = useState(false);
  const [showAddOpcGroupDialog, setShowAddOpcGroupDialog] = useState(false);
  
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
                OPC server info
              </Typography>
            </Grid>
            <Grid item xs></Grid>
            <Grid item>
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                disabled={!serviceMode}
                onClick={() => setShowAddOpcGroupDialog(true)}
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
              {opcServer.name}
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Type:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcServer.type}
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Host:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcServer.host}
            </Typography>
          </Grid>
        </Grid>
        <Grid container spacing={2} alignItems="center">
          <Typography color="inherit" variant="subtitle1">
            Groups:
          </Typography>
          <Grid item>
            <Typography color="inherit" variant="subtitle1">
              {opcServer.opcGroups ? opcServer.opcGroups.length : 0}
            </Typography>
          </Grid>
        </Grid>
      </div>

      {showRemoveConfirmDialog && (
        <SimpleConfirmDialog
          isOpen
          header={"OPC server"}
          text={"Delete the OPC server?"}
          cancelHandle={() => setShowRemoveConfirmDialog(false)}
          okHandle={removeHandle}
        />
      )}

      {showAddOpcGroupDialog && (
        <AddOpcGroupModal
          opcServerId={props.opcServer.id}
          isOpen
          cancelHandle={() => setShowAddOpcGroupDialog(false)}
        />
      )}
    </Paper>
      </TabPanel>

    </div>
  );
}


