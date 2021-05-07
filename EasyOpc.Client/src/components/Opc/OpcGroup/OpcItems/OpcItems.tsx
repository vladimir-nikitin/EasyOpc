import React, { useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import OpcItemRow from "./OpcItemRow";
import { AppState } from "../../../../store/store";
import { makeStyles } from "@material-ui/core/styles";
import { getOpcItems } from "../../../../functions/opc";
import { OpcGroup, setOpcItems, updateOpcItems } from "../../../../store/opcSlice";
import { Connection, hubConnection, Proxy } from "signalr-no-jquery";
import { showAppLoader } from "../../../../store/windowSlice";
import { SIGNAL_R_URL } from "../../../../constans/common";
import TablePagination from "@material-ui/core/TablePagination";
import { OpcItemData } from "../../../../types/opc";
import { opcGroupSelector } from "../../../../store/opcSliceSelectors";

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

type OpcItemsProps = {
  opcServerId: string;
  opcGroupId: string;
};

export const OpcItems = React.memo((props: OpcItemsProps) => {
  console.log(`[OpcItems] mount component`);

  const dispatch = useDispatch();
  const classes = useStyles();

  const opcGroup = opcGroupSelector((state: AppState) => state.opc.opcServers.find((x) => x.id === props.opcServerId)?.opcGroups.find((x) => x.id === props.opcGroupId));
  const serviceMode  = useSelector((state: AppState) => state.window.serviceMode);

  const [signalrConnection, setSignalrConnection] = useState<{connection: Connection;hub: Proxy;} | null>(null);
  const [opcItems, setOpcItemsState] = useState<OpcItemData[] | null>(null);

  const connect = () => {
    const connection = hubConnection(SIGNAL_R_URL);
    const hub = connection.createHubProxy("OpcGroupHub");

    hub.on(
      "OnOpcItemsChanged",
      (opcGroupId: string, newOpcItems: OpcItemData[]): void => {
        dispatch(
          updateOpcItems({
            opcServerId: props.opcServerId,
            opcGroupId: opcGroupId,
            opcItems: newOpcItems,
          })
        );
      }
    );

    hub.on("OnConnected", (opcGroupId: string, error: string | null): void => {
      dispatch(showAppLoader(false));
      error && alert(error);
    });

    hub.on(
      "OnDisconnected",
      (opcGroupId: string, error: string | null): void => {
        console.log('OnDisconnected');
        //setSignalrConnection(null);
      }
    );

    hub.on("Ping", (): void => {
      hub.invoke("Pong", opcGroup.id);
    });

    connection.start().done(() => {
      //setSignalrConnection({ connection: connection, hub: hub });
      hub.invoke("Connect", opcGroup.id);
      dispatch(showAppLoader(true));
    });

    return { connection: connection, hub: hub };
  };

  useEffect(() => {
    if (!opcGroup.opcItems) {
      dispatch(showAppLoader(true));
      getOpcItems(dispatch, props.opcGroupId)
        .then((items) => {
          dispatch(
            setOpcItems({
              opcServerId: props.opcServerId,
              opcGroupId: props.opcGroupId,
              opcItems: items,
            })
          );
          setOpcItemsState(items);
        })
        .finally(() => {
          dispatch(showAppLoader(false));
        });
    } else {
      setOpcItemsState(Array.from(opcGroup.opcItems.values()));
    }
  }, [opcGroup]);

  useEffect(() => {

    if(!opcItems || opcItems.length == 0 || opcItems[0].opcGroupId != props.opcGroupId){
      return;
    }

    if(!serviceMode && opcItems && !signalrConnection){
      console.log(`[OpcItems] CONNECT ****************************************`)
      setSignalrConnection(connect());
    }
    else if(!serviceMode && opcItems && signalrConnection){
      return () => {
        if(!signalrConnection) return;

        console.log(`[OpcItems] DISCONNECT ****************************************`)

        try{
          signalrConnection?.connection?.stop();
          setSignalrConnection(null);
        }
        catch(ex){
        }
      }
    }

    return null;
  }, [serviceMode, opcItems, signalrConnection, opcGroup]);

  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(80);

  if (!opcGroup.opcItems) return null;

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
            {opcItems &&
              opcItems.slice(page * rowsPerPage, 
                (page * rowsPerPage) + rowsPerPage < opcItems.length ? 
                  (page * rowsPerPage) + rowsPerPage : opcItems.length).map((row, i) => (
                <OpcItemRow
                  key={row.id}
                  opcServerId={props.opcServerId}
                  opcGroupId={row.opcGroupId}
                  opcItemId={row.id}
                />
              ))}
          </TableBody>
        </Table>
      </div>
      <TablePagination
        className={classes.tablePagination}
        rowsPerPageOptions={[80, 100, 200]}
        component="div"
        count={opcGroup.opcItems.size}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={(event, newPage) => setPage(newPage)}
        onChangeRowsPerPage={(event) => setRowsPerPage(Number(event.target.value))}
      />
    </div>
  );
});
