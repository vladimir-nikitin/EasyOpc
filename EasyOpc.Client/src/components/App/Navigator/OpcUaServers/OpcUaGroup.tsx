import React from "react";
import clsx from "clsx";
import { WithStyles } from "@material-ui/core/styles";
import { DrawerProps } from "@material-ui/core/Drawer";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import { useDispatch, useSelector } from "react-redux";
import Box from "@material-ui/core/Box";
import { AppState } from "../../../../store/store";
import { opcUaGroupSelector } from "../../../../store/opcUaSlice.selectors";
import { OPC_UA_GROUP_TYPE } from "../../../../constans/opc.ua";
import { setSelectedItem } from "../../../../store/windowSlice";

type OpcUaGroupProps = Omit<DrawerProps, "classes"> &
  WithStyles<any> & { opcUaServerId: string; opcUaGroupId: string };

export const OpcUaGroup = (props: OpcUaGroupProps) => {
  const dispath = useDispatch();
  const { selectedItem } = useSelector((state: AppState) => state.window);
  const opcUaGroup = opcUaGroupSelector((state: AppState) =>
    state.opcUa.opcUaServers
      .find((p) => p.id === props.opcUaServerId)
      ?.opcUaGroups?.find((p) => p.id === props.opcUaGroupId)
  );

  const itemPrimary = { primary: props.classes.itemPrimary };
  const textStyle = { marginLeft: 38 };

  return (
    <ListItem
      button
      className={clsx(
        props.classes.item,
        selectedItem &&
          selectedItem.type === OPC_UA_GROUP_TYPE &&
          selectedItem.item.id === props.opcUaGroupId &&
          props.classes.itemActiveItem
      )}
      onClick={() =>
        dispath(setSelectedItem({ type: OPC_UA_GROUP_TYPE, item: opcUaGroup }))
      }
    >
      <ListItemText classes={itemPrimary} style={textStyle}>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
        >
          {opcUaGroup.name}
        </Box>
      </ListItemText>
    </ListItem>
  );
};
