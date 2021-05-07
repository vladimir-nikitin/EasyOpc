import React from "react";
import { SimpleDialog } from "../../controls/SimpleDialog";
import { useDispatch } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import { addOpcServer } from "../../../functions/opc";
import { Guid } from "guid-typescript";
import { OPC_UA_TYPE } from "../../../constans/opc";

const useStyles = makeStyles({
  container: {
    display: "flex",
    flexDirection: "column",
    width: 340,
    height: 240,
    margin: 10
  },
  input: {
    margin: "0 8px 8px 8px",
  },
});

export type AddOpcUaServerModalProps = {
  isOpen: boolean;
  cancelHandle: () => void;
};

export default function AddOpcUaServerModal(props: AddOpcUaServerModalProps) {
  const classes = useStyles();
  const dispatch = useDispatch();

  const [host, setHost] = React.useState<string>('opc.tcp://opcuaserver.com:48010');
  const hostChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setHost((event.target as HTMLInputElement).value);

  const [name, setName] = React.useState<string>('OpcUaServer');
  const nameChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setName((event.target as HTMLInputElement).value);

  const [user, setUser] = React.useState<string>('');
  const userChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setUser((event.target as HTMLInputElement).value);

  const [password, setPassword] = React.useState<string>('');
  const passwordChangedHandle = (event: React.ChangeEvent<HTMLInputElement>) =>
    setPassword((event.target as HTMLInputElement).value);

  const okHandle = () => {
    if (name.length < 1 || host.length < 1) return;
    
    const jsonSettings: string = user.length < 1 ? null : JSON.stringify({
      userIdentitySetting: {
        user: user,
        password: password
      }
    });
    
    dispatch(addOpcServer({host: host, name: name, type: OPC_UA_TYPE, id: Guid.create().toString(), jsonSettings: jsonSettings}));
    props.cancelHandle();
  };

  return (
    <SimpleDialog
      isOpen={props.isOpen}
      cancelHandle={props.cancelHandle}
      okHandle={okHandle}
      isOkButtonDisabled={!host || !name}
      header={"Add OPC.UA server"}
    >
      <div className={classes.container}>
        <TextField
          label="Host:"
          value={host}
          onChange={hostChangedHandle}
          className={classes.input}
        />
        <TextField
          label="Name:"
          value={name}
          onChange={nameChangedHandle}
          className={classes.input}
        />
        <TextField
          label="User:"
          value={user}
          onChange={userChangedHandle}
          className={classes.input}
        />
        <TextField
          label="Password:"
          value={password}
          onChange={passwordChangedHandle}
          className={classes.input}
        />
      </div>
    </SimpleDialog>
  );
}
