import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import { makeStyles } from "@material-ui/core/styles";
import { Connection, hubConnection, Proxy } from "signalr-no-jquery";
import { AppState } from "../../../../store/store";
import { SIGNAL_R_URL } from "../../../../constans/common";
import { OpcUaItemData } from "../../../../types/opc.ua";
import { setOpcUaItems, updateOpcUaItems } from "../../../../store/opcUaSlice";
import { showAppLoader } from "../../../../store/windowSlice";
import { getPageOpcUaItems } from "../../../../functions/opc.ua";
import TablePagination from "@material-ui/core/TablePagination";
import { OpcUaItem } from "./OpcUaItem";

const useStyles = makeStyles({
  container: {
    display: "flex",
    flexDirection: "column",
    overflow: "hidden",
    height: "100%",
  },
  tableContainer: {
    display: "flex",
    flexDirection: "column",
    overflowY: "scroll",
    height: "100%",
  },
  tableBody: {
    overflowY: 'scroll'
  },
  tablePagination: {
    height: "56px",
  }
});

type OpcUaItemsProps = {
  opcUaServerId: string;
  opcUaGroupId: string;
};

export const OpcUaItems = React.memo((props: OpcUaItemsProps) => {
  console.log(`[OpcUaItems] mount component`);

  const dispatch = useDispatch();
  const classes = useStyles();

  const serviceMode  = useSelector((state: AppState) => state.window.serviceMode);
  const [signalrConnection, setSignalrConnection] = useState<{connection: Connection;hub: Proxy;} | null>(null);
  const [opcUaItemIds, setOpcUaItemIds] = useState<string[] | null>(null);

  const connect = () => {
    const connection = hubConnection(SIGNAL_R_URL);
    const hub = connection.createHubProxy("OpcUaGroupHub");

    hub.on( "OnOpcUaItemsChanged", (opcUaGroupId: string, newOpcUaItems: OpcUaItemData[]) => dispatch(updateOpcUaItems({opcUaItems: newOpcUaItems})));

    hub.on("OnConnected", (opcGroupId: string, error: string | null): void => {
      dispatch(showAppLoader(false));
      if(error){
        error && alert(error);
      }
      else {
        console.log(`##OnConnected`)
        console.log(`##setSignalrConnection({ connection: connection, hub: hub });`)
        setSignalrConnection({ connection: connection, hub: hub });
      }
    });

    hub.on(
      "OnDisconnected", (opcUaGroupId: string, error: string | null): void => {
        console.log('OnDisconnected');
        //setSignalrConnection(null);
      }
    );

    hub.on("Ping", (): void => {
      hub.invoke("Pong", props.opcUaGroupId);
    });

    connection.start().done(() => {
      hub.invoke("Connect", props.opcUaGroupId);
      dispatch(showAppLoader(true));
    });

    return { connection: connection, hub: hub };
  };

  useEffect(() => {
    if(!serviceMode && !signalrConnection){
      //setSignalrConnection(connect());
      connect();
    }
    else if(!serviceMode && signalrConnection){
      return () => {
        if(signalrConnection == null) return;

        try{
          signalrConnection?.connection?.stop();
          console.log(`##setSignalrConnection(null);`)
          setSignalrConnection(null);
        }
        catch(ex){
        }
      }
    }

    return null;
  }, [serviceMode, signalrConnection, props.opcUaGroupId]);

  const [page, setPage] = useState(0);
  const [totalCount, setTotalCount] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(100);

  useEffect(() => setPage(0), [props.opcUaGroupId])

  useEffect(() => {
    getPageOpcUaItems(dispatch, props.opcUaGroupId, page, rowsPerPage)
        .then(page => {
          setTotalCount(page.totalCount);
          dispatch(setOpcUaItems({opcUaItems: page.items}));
          setOpcUaItemIds(page.items.map(p => p.id));
        })
    
  }, [page, rowsPerPage, props.opcUaGroupId]);

  useEffect(() => {
    if(serviceMode || signalrConnection == null || opcUaItemIds == null || opcUaItemIds.length == 0)
      return;

    console.log(`##Subscribe`)
    signalrConnection.hub.invoke("Subscribe", props.opcUaGroupId, Array.from(opcUaItemIds.values()));

  }, [opcUaItemIds, signalrConnection, serviceMode]);

  return (
    <div className={classes.container}>
      <div className={classes.tableContainer}>
        <Table
          size="small"
          aria-label="a dense table"
          stickyHeader
        >
          <TableHead>
            <TableRow>
              <TableCell>Item ID</TableCell>
              <TableCell>NodeId</TableCell>
              <TableCell>Value</TableCell>
              <TableCell>Timestamp(UTC)</TableCell>
              <TableCell>Timestamp(Local)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody component={"tbody"} className={classes.tableBody}>
            {opcUaItemIds && opcUaItemIds.map(id => (<OpcUaItem key={id} opcUaItemId={id} />))}
          </TableBody>
        </Table>
      </div>
      <TablePagination
        className={classes.tablePagination}
        rowsPerPageOptions={[100, 200, 300]}
        component="div"
        count={totalCount}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={(event, newPage) => setPage(newPage)}
        onChangeRowsPerPage={(event) => setRowsPerPage(Number(event.target.value))}
      />
    </div>
  );
});
