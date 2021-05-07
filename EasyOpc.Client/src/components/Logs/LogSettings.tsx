import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import { AppState } from "../../store/store";
import { getLogFilePath, setLogFilePath } from "../../functions/log";
import { selectFolder } from "../../functions/common";

const useStyles = makeStyles({
  container: {
    display: 'flex',
    flexDirection: 'column'
  },
  input: {
    margin: '6px 6px 0 6px',
    width: '400px'
  },
});

export const LogSettings = () => {
  console.log(`[App][Content][Logs][LogSettings] mount component`);

  const classes = useStyles();
  const dispatch = useDispatch();
  
  const serviceMode = useSelector((state: AppState) => state.window.serviceMode);
  const [logFilePath, setLogFilePathState] = useState<string>("");
  
  useEffect(() => {
    if(logFilePath) return;
    getLogFilePath(dispatch).then(path => setLogFilePathState(path));
  }, []);

  const editLogFilePath = (path: string) => {
    setLogFilePathState(path);
    setLogFilePath(dispatch, path);
  }

  const selectLogFilePath = () => {
    const folders = selectFolder();
    folders.length > 0 && editLogFilePath(folders[0]+"\\Log.txt");
  };

  return (
    <div className={classes.container}>
      <TextField label="Logs file path:" 
          className={classes.input} 
          disabled={!serviceMode}
          value={logFilePath} 
          onClick={serviceMode ? selectLogFilePath : null} 
          onChange={(event) => serviceMode && editLogFilePath(event.target.value)} 
          />
    </div>
  );
};
