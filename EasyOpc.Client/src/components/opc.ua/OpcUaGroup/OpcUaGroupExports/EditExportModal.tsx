import { makeStyles } from "@material-ui/core/styles";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { ExportToFileWorkSetting, OpcUaGroupWorkData } from "../../../../types/opc.ua";
import { SimpleDialog } from "../../../controls/SimpleDialog";
import TextField from "@material-ui/core/TextField";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Switch from "@material-ui/core/Switch";
import { selectFolder } from "../../../../functions/common";

const useStyles = makeStyles({
  container: {
    display: "flex",
    flexDirection: "column",
    padding: 20,
  },
  input: {
    margin: "0 8px 30px 8px",
    width: "300px",
  },
});

type EditExportModalProps = {
  work: OpcUaGroupWorkData;
  okHandle: (work: OpcUaGroupWorkData) => void;
  closeHandle: () => void;
};

export const EditExportModal = (props: EditExportModalProps) => {
  const classes = useStyles();
  const dispatch = useDispatch();

  const [work, setWork] = useState<OpcUaGroupWorkData>(props.work);
  const [settings, setSettings] = useState<ExportToFileWorkSetting>(
    JSON.parse(props.work.jsonSettings)
  );

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setWork({ ...work, isEnabled: event.target.checked });
  };

  const nameChangedHandle = (
    event: React.ChangeEvent<HTMLInputElement>
  ) =>
    setWork({
      ...work,
      name: (event.target as HTMLInputElement).value,
    });

  const folderPathChangedHandle = (
    event: React.ChangeEvent<HTMLInputElement>
  ) =>
    setSettings({
      ...settings,
      folderPath: (event.target as HTMLInputElement).value,
    });

  const selectFolderPath = () => {
    const folderPaths = selectFolder();
    if (!folderPaths || folderPaths.length < 1) return;
    setSettings({ ...settings, folderPath: folderPaths[0] });
  };

  const writeInOneFileHandleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSettings({ ...settings, writeInOneFile: event.target.checked });
  };

  const timespanChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setSettings({ ...settings, timespan: "00:" + (event.target as HTMLInputElement).value });

  const applyHandler = () => props.okHandle({...work, jsonSettings: JSON.stringify(settings)});

  const formatTimespan = (timespan: string) => timespan.substring(3);
  
  return (
    <SimpleDialog
      isOpen
      cancelHandle={props.closeHandle}
      okHandle={applyHandler}
      header={"Edit export"}
    >
      <div className={classes.container}>
      <FormControlLabel
          control={
            <Switch
              name="checkedB"
              color="primary"
              checked={work.isEnabled}
              onChange={handleChange}
            />
          }
          label="Enable"
        />

        <TextField
          label="Name:"
          className={classes.input}
          value={work.name}
          onChange={nameChangedHandle}
        />

        {/*<FormControlLabel
          control={
            <Switch
              name="checkedB"
              color="primary"
              checked={settings.writeInOneFile}
              onChange={writeInOneFileHandleChange}
            />
          }
          label="Write in one file"
        />*/}

        <TextField label="Folder:" className={classes.input} value={settings.folderPath} 
          onClick={selectFolderPath} onChange={folderPathChangedHandle} />
          
        <TextField label="Timespan(mm:ss):" className={classes.input} value={formatTimespan(settings.timespan)} 
          onChange={timespanChangedHandle} />
        
      </div>
    </SimpleDialog>
  );
};
