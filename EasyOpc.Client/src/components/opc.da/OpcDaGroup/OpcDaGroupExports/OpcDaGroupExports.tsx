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
import { OpcDaGroupWorkData, ExportToFileWorkSetting } from "../../../../types/opc.da";
import { addOpcDaGroupWork, deleteOpcDaGroupWork, getOpcDaGroupWorks, updateOpcDaGroupWork } from "../../../../functions/opc.da";
import { Guid } from "guid-typescript";
import { EditExportModal } from "./EditExportModal";

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

type OpcDaGroupExportsProps = {
  opcDaServerId: string;
  opcDaGroupId: string;
};

export const OpcDaGroupExports = (props: OpcDaGroupExportsProps) => {
  const dispatch = useDispatch();
  const classes = useStyles();
  const { serviceMode } = useSelector((state: AppState) => state.window);
  const boxStyle = { maxWidth: "200px" };

  const [opcDaGroupWorks, setOpcDaGroupWorks] = useState<OpcDaGroupWorkData[] | null>(null);
  const [editOpcDaGroupWork, setEditOpcDaGroupWork] = useState<OpcDaGroupWorkData | null>(null);

  useEffect(() => {
    if(opcDaGroupWorks == null){
      getOpcDaGroupWorks(dispatch, props.opcDaGroupId, [ "EXPORT_TO_FILE" , "EXPORT_TO_SQL"])
      .then(result => setOpcDaGroupWorks(result));
    }
  }, []);

  const addHandle = () => {
    const setting: ExportToFileWorkSetting = {
      folderPath: "",
      writeInOneFile: false,
      timespan: "00:01:00"
    };

    setEditOpcDaGroupWork({
      id: "",
      isEnabled: true,
      launchGroup: props.opcDaServerId,
      opcDaGroupId: props.opcDaGroupId,
      name: "Export_1",
      type: "EXPORT_TO_FILE",
      jsonSettings: JSON.stringify(setting)
    });
  }
  const closeHandle = () => setEditOpcDaGroupWork(null);
  const okHandle = async (work: OpcDaGroupWorkData) => {
    if(work.id){
      await updateOpcDaGroupWork(dispatch, work);
      setOpcDaGroupWorks(opcDaGroupWorks.map(p => p.id !== work.id ? p : work));
    }
    else {
      const addWork = {...work, id: Guid.create().toString()}
      setOpcDaGroupWorks(opcDaGroupWorks.concat(addWork));
      await addOpcDaGroupWork(dispatch, addWork);
    }
    closeHandle();
  };
  const deleteHandle = async(work: OpcDaGroupWorkData) => {
    await deleteOpcDaGroupWork(dispatch, work.id);
    setOpcDaGroupWorks(opcDaGroupWorks.filter(p => p.id !== work.id));
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
                Exports
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
                Add export
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
            {opcDaGroupWorks &&
              opcDaGroupWorks.map((work) => (
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
                    <Button color="primary" disabled={!serviceMode} onClick={() => setEditOpcDaGroupWork(work)}>
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
      {editOpcDaGroupWork && <EditExportModal work={editOpcDaGroupWork} okHandle={okHandle} closeHandle={closeHandle} />}
    </Paper>
  );
};
