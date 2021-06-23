import React from "react";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";
import Box from "@material-ui/core/Box";
import { useSelector } from "react-redux";
import { AppState } from "../../../../store/store";

export const OpcUaItem = React.memo((props: {opcUaItemId: string}) => {

  const opcUaItem = useSelector((state: AppState) => state.opcUa.opcUaItems.has(props.opcUaItemId) ? state.opcUa.opcUaItems.get(props.opcUaItemId) : null);
  if(opcUaItem == null){
    return null;
  }
  
  const boxStyle = { maxWidth: "200px" };
  return (
    <TableRow>
      <TableCell>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
          style={boxStyle}
        >
          {opcUaItem.name}
        </Box>
      </TableCell>
      <TableCell>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
          style={boxStyle}
        >
          {opcUaItem.nodeId}
        </Box>
      </TableCell>
      <TableCell>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
          style={boxStyle}
        >
          {opcUaItem.value}
        </Box>
      </TableCell>
      <TableCell>{opcUaItem.timestampUtc}</TableCell>
      <TableCell>{opcUaItem.timestampUtc}</TableCell>
    </TableRow>
  );
});
