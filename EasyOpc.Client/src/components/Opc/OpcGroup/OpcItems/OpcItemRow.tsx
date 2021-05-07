import React, { useEffect } from "react";
import { useSelector } from "react-redux";
import TableCell from "@material-ui/core/TableCell";
import TableRow from "@material-ui/core/TableRow";
import Box from "@material-ui/core/Box";
import { AppState } from "../../../../store/store";

type OpcItemProps = {
  opcServerId: string;
  opcGroupId: string;
  opcItemId: string;
};

export default function OpcItemRow(props: OpcItemProps) {
  const opcItem = useSelector((state: AppState) =>
    state.opc.opcServers.find((x) => x.id === props.opcServerId)?.opcGroups?.find(x => x.id === props.opcGroupId)?.opcItems?.get(props.opcItemId)
  );

  const boxStyle = { maxWidth: "200px" };
  return (
    <TableRow key={opcItem.id}>
      <TableCell>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
          style={boxStyle}
        >
          {opcItem.name}
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
          {opcItem.accessPath}
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
          {opcItem.value}
        </Box>
      </TableCell>
      <TableCell>{opcItem.quality}</TableCell>
      <TableCell>{opcItem.timestampUtc}</TableCell>
      <TableCell>{opcItem.timestampUtc}</TableCell>
    </TableRow>
  );
}
