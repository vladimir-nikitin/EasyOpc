import React, { useEffect, useState } from "react";
import { Connection, hubConnection, Proxy } from "signalr-no-jquery";
import { SIGNAL_R_URL } from "../../constans/common";
import { showAppLoader } from "../../store/windowSlice";
import { useDispatch } from "react-redux";
import { addLogs, setLogs } from "../../store/logSlice";
import { ListLogs } from "./ListLogs";
import { makeStyles } from "@material-ui/core/styles";
import { LogSettings } from "./LogSettings";

const useStyles = makeStyles({
  container: {
    display: 'flex',
    flexDirection: 'column',
    height: '100%',
    overflow: 'hidden'
  }
});

export const Logs = () => {
  console.log(`[App][Content][Logs] mount component`);

  const classes = useStyles();
  const dispatch = useDispatch();
  
  const [signalrConnection, setSignalrConnection] = useState<{
    connection: Connection;
    hub: Proxy;
  } | null>(null);

  const connect = () => {
    const connection = hubConnection(SIGNAL_R_URL);
    const hub = connection.createHubProxy("LogHub");

    hub.on("RecordsAdded", (records: string[]) => {
      dispatch(addLogs(records));
    });

    hub.on("HubConnected", (records: string[]) => {
      if (records == undefined) return;

      dispatch(setLogs(records));
      dispatch(showAppLoader(false));
    });

    hub.on("Ping", (): void => {
      hub.invoke("Pong");
    });

    connection.start().done(() => {
      setSignalrConnection({ connection: connection, hub: hub });
      hub.invoke("Connect");
      dispatch(showAppLoader(true));
    });

    return hub;
  };

  useEffect(() => {
    if (signalrConnection) return;
    const hub = connect();
    return () => {
      hub?.invoke("Disconnect");
    };
  }, []);

  return (
    <div className={classes.container}>
      <LogSettings />
      <ListLogs />
    </div>
  );
};
