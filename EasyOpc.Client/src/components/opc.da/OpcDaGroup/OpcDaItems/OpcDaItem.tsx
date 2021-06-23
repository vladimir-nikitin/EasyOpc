import React from "react";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";
import Box from "@material-ui/core/Box";
import { useSelector } from "react-redux";
import { AppState } from "../../../../store/store";

export const OpcDaItem = React.memo((props: {opcDaItemId: string}) => {

  const opcDaItem = useSelector((state: AppState) => state.opcDa.opcDaItems.has(props.opcDaItemId) ? state.opcDa.opcDaItems.get(props.opcDaItemId) : null);
  if(opcDaItem == null){
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
          {opcDaItem.name}
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
          {opcDaItem.accessPath}
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
          {opcDaItem.value}
        </Box>
      </TableCell>
      <TableCell>{opcDaItem.quality}</TableCell>
      <TableCell>{opcDaItem.timestampUtc}</TableCell>
      <TableCell>{opcDaItem.timestampUtc}</TableCell>
    </TableRow>
  );
});
