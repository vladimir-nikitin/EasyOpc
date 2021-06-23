import { makeStyles } from "@material-ui/core/styles";
import React, { useState } from "react";
import { useDispatch } from "react-redux";
import {
  OpcUaGroupWorkData,
  SubscritionToFileWorkSetting,
} from "../../../../types/opc.ua";
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

type EditSubscritionModalProps = {
  work: OpcUaGroupWorkData;
  okHandle: (work: OpcUaGroupWorkData) => void;
  closeHandle: () => void;
};

export const EditSubscritionModal = (props: EditSubscritionModalProps) => {
  const classes = useStyles();
  const dispatch = useDispatch();

  const [work, setWork] = useState<OpcUaGroupWorkData>(props.work);
  const [settings, setSettings] = useState<SubscritionToFileWorkSetting>(
    JSON.parse(props.work.jsonSettings)
  );

  const formatTimespan = (timespan: string) => timespan.substring(0, timespan.length - 3);

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

  const fileTimespanChangedHandle = (
    event: React.ChangeEvent<HTMLInputElement>
  ) =>
    setSettings({
      ...settings,
      fileTimespan: (event.target as HTMLInputElement).value + ":00",
    });

  const historyRetentionTimespanTimespanChangedHandle = (
    event: React.ChangeEvent<HTMLInputElement>
  ) =>
    setSettings({
      ...settings,
      historyRetentionTimespan: (event.target as HTMLInputElement).value + ":00",
    });

  const selectFolderPath = () => {
    const folderPaths = selectFolder();
    if (!folderPaths || folderPaths.length < 1) return;
    setSettings({ ...settings, folderPath: folderPaths[0] });
  };

  const applyHandler = () => props.okHandle({...work, jsonSettings: JSON.stringify(settings)});
  
  return (
    <SimpleDialog
      isOpen
      cancelHandle={props.closeHandle}
      okHandle={applyHandler}
      header={"Edit subscrition"}
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
        <TextField
          label="Folder:"
          className={classes.input}
          value={settings.folderPath}
          onClick={selectFolderPath}
          onChange={folderPathChangedHandle}
        />
        <TextField
          label="Timespan(hh:mm):"
          className={classes.input}
          value={formatTimespan(settings.fileTimespan)}
          onChange={fileTimespanChangedHandle}
        />
        <TextField
          label="History retention period(hh:mm):"
          className={classes.input}
          value={formatTimespan(settings.historyRetentionTimespan)}
          onChange={historyRetentionTimespanTimespanChangedHandle}
        />
      </div>
    </SimpleDialog>
  );
};
