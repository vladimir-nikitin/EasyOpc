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
import { OpcDaItemData } from "../../../../types/opc.da";
import { setOpcDaItems, updateOpcDaItems } from "../../../../store/opcDaSlice";
import { showAppLoader } from "../../../../store/windowSlice";
import { getPageOpcDaItems } from "../../../../functions/opc.da";
import TablePagination from "@material-ui/core/TablePagination";
import { OpcDaItem } from "./OpcDaItem";


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

type OpcDaItemsProps = {
  opcDaServerId: string;
  opcDaGroupId: string;
};

export const OpcDaItems = React.memo((props: OpcDaItemsProps) => {
  console.log(`[OpcItems] mount component`);

  const dispatch = useDispatch();
  const classes = useStyles();

  const serviceMode  = useSelector((state: AppState) => state.window.serviceMode);
  const [signalrConnection, setSignalrConnection] = useState<{connection: Connection;hub: Proxy;} | null>(null);
  const [opcDaItemIds, setOpcDaItemIds] = useState<string[] | null>(null);

  const connect = () => {
    const connection = hubConnection(SIGNAL_R_URL);
    const hub = connection.createHubProxy("OpcDaGroupHub");

    hub.on( "OnOpcDaItemsChanged", (opcGroupId: string, newOpcDaItems: OpcDaItemData[]) => dispatch(updateOpcDaItems({opcDaItems: newOpcDaItems})));

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
      "OnDisconnected", (opcGroupId: string, error: string | null): void => {
        console.log('OnDisconnected');
        //setSignalrConnection(null);
      }
    );

    hub.on("Ping", (): void => {
      hub.invoke("Pong", props.opcDaGroupId);
    });

    connection.start().done(() => {
      hub.invoke("Connect", props.opcDaGroupId);
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
  }, [serviceMode, signalrConnection, props.opcDaGroupId]);

  const [page, setPage] = useState(0);
  const [totalCount, setTotalCount] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(100);

  useEffect(() => setPage(0), [props.opcDaGroupId])

  useEffect(() => {
    getPageOpcDaItems(dispatch, props.opcDaGroupId, page, rowsPerPage)
        .then(page => {
          setTotalCount(page.totalCount);
          dispatch(setOpcDaItems({opcDaItems: page.items}));
          setOpcDaItemIds(page.items.map(p => p.id));
        })
    
  }, [page, rowsPerPage, props.opcDaGroupId]);

  useEffect(() => {
    if(serviceMode || signalrConnection == null || opcDaItemIds == null || opcDaItemIds.length == 0)
      return;

    console.log(`##Subscribe`)
    signalrConnection.hub.invoke("Subscribe", props.opcDaGroupId, Array.from(opcDaItemIds.values()));

  }, [opcDaItemIds, signalrConnection, serviceMode]);

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
              <TableCell>Access Path</TableCell>
              <TableCell>Value</TableCell>
              <TableCell>Quality</TableCell>
              <TableCell>Timestamp(UTC)</TableCell>
              <TableCell>Timestamp(Local)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody component={"tbody"} className={classes.tableBody}>
            {opcDaItemIds && opcDaItemIds.map(id => (<OpcDaItem key={id} opcDaItemId={id} />))}
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
