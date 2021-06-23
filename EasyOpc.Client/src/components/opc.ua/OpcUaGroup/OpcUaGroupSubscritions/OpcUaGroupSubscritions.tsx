import React, { useEffect, useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import Paper from "@material-ui/core/Paper";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";
import StorageIcon from "@material-ui/icons/Storage";
import AddIcon from "@material-ui/icons/Add";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../../store/store";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Box from "@material-ui/core/Box";
import { OpcUaGroupWorkData, SubscritionToFileWorkSetting } from "../../../../types/opc.ua";
import { addOpcUaGroupWork, deleteOpcUaGroupWork, getOpcUaGroupWorks, updateOpcUaGroupWork } from "../../../../functions/opc.ua";
import { EditSubscritionModal } from "./EditSubscritionModal";
import { Guid } from "guid-typescript";

const useStyles = makeStyles({
  paper: {
    height: "100%",
    width: "100%",
    display: "flex",
    flexDirection: "column",
  },
  searchBar: {
    borderBottom: "1px solid rgba(0, 0, 0, 0.12)",
  },
  block: {
    display: "block",
  },
  input: {
    margin: "5px 0",
  },
  content: {
    display: "flex",
    flexDirection: "column",
    maxWidth: 400,
    padding: 20,
  },
  tableContainer: {
    display: "flex",
    flexDirection: "column",
    overflowY: "scroll",
    flexGrow: 1,
  },
  tableBody: {
    overflowY: "scroll",
  },
});

type OpcUaGroupSubscritionsProps = {
  opcUaServerId: string;
  opcUaGroupId: string;
};

export const OpcUaGroupSubscritions = (props: OpcUaGroupSubscritionsProps) => {
  const dispatch = useDispatch();
  const classes = useStyles();
  const { serviceMode } = useSelector((state: AppState) => state.window);
  const boxStyle = { maxWidth: "200px" };

  const [opcUaGroupWorks, setOpcUaGroupWorks] = useState<OpcUaGroupWorkData[] | null>(null);
  const [editOpcUaGroupWork, setEditOpcUaGroupWork] = useState<OpcUaGroupWorkData | null>(null);

  useEffect(() => {
    if(opcUaGroupWorks == null){
      getOpcUaGroupWorks(dispatch, props.opcUaGroupId, [ "SUBSCRITION_TO_FILE" , "SUBSCRITION_TO_SQL"])
      .then(result => setOpcUaGroupWorks(result));
    }
  }, []);

  const addHandle = () => {
    const setting: SubscritionToFileWorkSetting = {
      folderPath: "",
      fileTimespan: "00:01:00",
      historyRetentionTimespan: "24:00:00"
    };

    setEditOpcUaGroupWork({
      id: "",
      isEnabled: true,
      launchGroup: props.opcUaServerId,
      opcUaGroupId: props.opcUaGroupId,
      name: "Subscrition_1",
      type: "SUBSCRITION_TO_FILE",
      jsonSettings: JSON.stringify(setting)
    });
  }
  const closeHandle = () => setEditOpcUaGroupWork(null);
  const okHandle = async (work: OpcUaGroupWorkData) => {
    if(work.id){
      await updateOpcUaGroupWork(dispatch, work);
      setOpcUaGroupWorks(opcUaGroupWorks.map(p => p.id !== work.id ? p : work));
    }
    else {
      const addWork = {...work, id: Guid.create().toString()}
      await addOpcUaGroupWork(dispatch, addWork);
      setOpcUaGroupWorks(opcUaGroupWorks.concat(addWork));
    }
    closeHandle();
  };
  const deleteHandle = async(work: OpcUaGroupWorkData) => {
    await deleteOpcUaGroupWork(dispatch, work.id);
    setOpcUaGroupWorks(opcUaGroupWorks.filter(p => p.id !== work.id));
  };

  return (
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
                Subscritions
              </Typography>
            </Grid>
            <Grid item xs></Grid>
            <Grid item>
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                disabled={!serviceMode}
                onClick={addHandle}
              >
                Add subscrition
              </Button>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <div className={classes.tableContainer}>
        <Table size="small" aria-label="a dense table" stickyHeader>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Type</TableCell>
              <TableCell width={100}></TableCell>
              <TableCell width={100}></TableCell>
            </TableRow>
          </TableHead>
          <TableBody component={"tbody"} className={classes.tableBody}>
            {opcUaGroupWorks &&
              opcUaGroupWorks.map((work) => (
                <TableRow key={work.id}>
                  <TableCell>
                    <Box
                      component="div"
                      textOverflow="ellipsis"
                      overflow="hidden"
                      fontStyle="inherit"
                      style={boxStyle}
                    >
                      {work.name}
                    </Box>
                  </TableCell>
                  <TableCell>
                    <Box
                      component="div"
                      textOverflow="ellipsis"
                      overflow="hidden"
                      fontStyle="inherit"
                      style={boxStyle}
                    >
                      {work.type}
                    </Box>
                  </TableCell>
                  <TableCell width={100}>
                    <Button color="primary" disabled={!serviceMode} onClick={() => setEditOpcUaGroupWork(work)}>
                      Edit
                    </Button>
                  </TableCell>
                  <TableCell width={100}>
                    <Button color="primary" disabled={!serviceMode} onClick={() => deleteHandle(work)}>
                      Delete
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>
      </div>
      {editOpcUaGroupWork && <EditSubscritionModal work={editOpcUaGroupWork} okHandle={okHandle} closeHandle={closeHandle} />}
    </Paper>
  );
};
