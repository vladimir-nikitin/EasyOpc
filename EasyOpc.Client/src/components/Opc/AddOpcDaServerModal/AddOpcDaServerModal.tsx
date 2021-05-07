import React, { useState } from "react";
import Button from "@material-ui/core/Button";
import { openFiles, readFile } from "../../../functions/common";
import { SimpleDialog } from "../../controls/SimpleDialog";
import { useDispatch } from "react-redux";
import { addOpcServer, importOpcDaServers } from "../../../functions/opc";
import { makeStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import RadioGroup from "@material-ui/core/RadioGroup";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Radio from "@material-ui/core/Radio";
import { OPC_DA_TYPE } from "../../../constans/opc";
import { Guid } from "guid-typescript";

const useStyles = makeStyles({
  container: {
    display: "flex",
    flexDirection: "column",
    width: 400,
    height: 280,
    margin: 10
  },
  button: {
    marginLeft: 20,
  },
  text: {
    flexGrow: 1,
  },
  input: {
    margin: '0 8px 8px 8px',
  },
  file: {
     display: "flex", 
     flexDirection: "row", 
     marginTop: "10px"
  },
  add: {
    display: "flex",
    flexDirection: "column",
    marginTop: "10px",
  }
});

export type AddOpcUaServerModalProps = {
  isOpen: boolean;
  cancelHandle: () => void;
};

export default function AddOpcUaServerModal(props: AddOpcUaServerModalProps) {
  const classes = useStyles();
  const dispatch = useDispatch();
  const [selectedFile, setSelectedFile] = useState<string | null>(null);

  const [mode, setMode] = React.useState("file");
  const modeChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setMode((event.target as HTMLInputElement).value);

  const [host, setHost] = React.useState<string | null>('127.0.0.1');
  const hostChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setHost((event.target as HTMLInputElement).value);

  const [name, setName] = React.useState<string | null>('Matrikon.OPC.Simulation.1');
  const nameChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) => 
    setName((event.target as HTMLInputElement).value);

  const [clsid, setClsid] = React.useState<string | null>('F8582CF2-88FB-11D0-B850-00C0F0104305');
  const clsidChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) => 
    setClsid((event.target as HTMLInputElement).value);

  const openFilesHandle = () => {
    const files = openFiles();
    if (files.length > 0) {
      setSelectedFile(files[0]);
    }
  };

  const okHandle = () => {
    if(mode === 'file'){
      if (!selectedFile) return;
      var data = readFile(selectedFile);
      dispatch(importOpcDaServers(data));
    }
    else {
      if (!name || !host) return;
      dispatch(addOpcServer({host: host, name: name, type: OPC_DA_TYPE, id: Guid.create().toString(), jsonSettings: clsid}));
    }
    props.cancelHandle();
  };

  return (
    <SimpleDialog
      isOpen={props.isOpen}
      cancelHandle={props.cancelHandle}
      okHandle={okHandle}
      isOkButtonDisabled={mode==="file" ? !selectedFile : !host || !name}
      header={"Add OPC.DA server"}
    >
      <div className={classes.container}>
        <RadioGroup aria-label="gender" value={mode} onChange={modeChangedHandle}>
          <FormControlLabel
            value="file"
            control={<Radio />}
            label="Import from config file"
          />
          <FormControlLabel
            value="add"
            control={<Radio />}
            label="Add server"
          />
        </RadioGroup>
        {mode === "file" && (
          <div className={classes.file}>
            <TextField
              label="Path"
              className={classes.text}
              value={selectedFile ?? ""}
            />
            <Button
              variant="contained"
              component="label"
              className={classes.button}
              onClick={openFilesHandle}
            >
              Select Config File
            </Button>
          </div>
        )}
        {mode === "add" && (
          <div className={classes.add}>
            <TextField label="Host:" value={host} onChange={hostChangedHandle} className={classes.input} />
            <TextField label="Name:" value={name} onChange={nameChangedHandle} className={classes.input}  />
            <TextField label="CLSID:" value={clsid} onChange={clsidChangedHandle} className={classes.input} />
          </div>
        )}
      </div>
    </SimpleDialog>
  );
}
