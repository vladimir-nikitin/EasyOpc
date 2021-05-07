import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { makeStyles } from "@material-ui/core/styles";
import { OpcGroupExportSettingData } from "../../../types/opc";
import Paper from "@material-ui/core/Paper";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";
import StorageIcon from "@material-ui/icons/Storage";
import DeleteIcon from "@material-ui/icons/Delete";
import SaveIcon from "@material-ui/icons/Save";
import TextField from "@material-ui/core/TextField";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Switch from "@material-ui/core/Switch";

import { selectFolder } from "../../../functions/common";
import { getOpcGroupExportSetting, saveOpcGroupExportSetting } from "../../../functions/opc";

const useStyles = makeStyles({
  paper: {
    height: "300px",
    width: "100%",
  },
  searchBar: {
    borderBottom: "1px solid rgba(0, 0, 0, 0.12)",
  },
  block: {
    display: "block",
  },
  input: {
    margin: '5px 0'
  },
  content: {
    display: 'flex', 
    flexDirection: 'column', 
    maxWidth: 400, 
    padding: 20
  }
});

export type OpcGroupExportSettingsProps = {
  opcGroupId: string;
};

function OpcGroupExportSettings(props: OpcGroupExportSettingsProps) {
  const dispatch = useDispatch();
  const classes = useStyles();

  const { serviceMode } = useSelector((state: AppState) => state.window);

  const [setting, setSetting] = useState<OpcGroupExportSettingData | null>(null);

  const formatTimespan = (timespan: string) => timespan.substring(3);

  useEffect(() => {
    getOpcGroupExportSetting(dispatch, props.opcGroupId)
      .then(s => setSetting({...s, timespan: formatTimespan(s.timespan)}))
      .catch(error =>{
        alert(error);
        console.error(error);
      });
  }, [dispatch, props.opcGroupId]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSetting({ ...setting, isEnabled: event.target.checked });
  };

  const writeInOneFileHandleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSetting({ ...setting, isWriteInOneFile: event.target.checked });
  };

  const folderPathChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setSetting({ ...setting, folderPath: (event.target as HTMLInputElement).value });

  const timespanChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setSetting({ ...setting, timespan: (event.target as HTMLInputElement).value });


  const selectFolderPath = () => {
    const folderPaths = selectFolder();
    if(!folderPaths || folderPaths.length === 0) return;
    setSetting({ ...setting, folderPath: folderPaths[0]});
  }

  const saveHandle = () => 
    saveOpcGroupExportSetting(dispatch, { ...setting, timespan: `00:${setting.timespan}`})
    .then(result => {
      setSetting({...result, timespan: formatTimespan(result.timespan)});
    })
    .catch(error => {
      alert(error);
      console.error(error);
    });

  return !setting ? null : (
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
                Export to .CSV
              </Typography>
            </Grid>
            <Grid item xs></Grid>
            <Grid item>
              <Button
                variant="contained"
                color="primary"
                startIcon={<SaveIcon />}
                disabled={!serviceMode}
                onClick={saveHandle}
              >
                Save
              </Button>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <div className={classes.content}>
        <FormControlLabel
          control={
            <Switch
              name="checkedB"
              color="primary"
              checked={setting.isEnabled}
              onChange={handleChange}
              disabled={!serviceMode}
            />
          }
          label="Enable"
        />

        <FormControlLabel
          control={
            <Switch
              name="checkedB"
              color="primary"
              checked={setting.isWriteInOneFile}
              onChange={writeInOneFileHandleChange}
              disabled={!serviceMode}
            />
          }
          label="Write in one file"
        />

        <TextField label="Folder:" className={classes.input} value={setting.folderPath} 
          onClick={selectFolderPath} onChange={folderPathChangedHandle} disabled={!serviceMode} />
          
        <TextField label="Timespan(mm:ss):" className={classes.input} value={setting.timespan} 
          onChange={timespanChangedHandle} disabled={!serviceMode} />
      </div>
    </Paper>
  );
}

export default OpcGroupExportSettings;
