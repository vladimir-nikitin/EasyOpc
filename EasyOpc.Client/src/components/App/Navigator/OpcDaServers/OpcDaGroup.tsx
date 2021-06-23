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
import { opcDaGroupSelector } from "../../../../store/opcDaSlice.selectors";
import { OPC_DA_GROUP_TYPE } from "../../../../constans/opc.da";
import { setSelectedItem } from "../../../../store/windowSlice";

type OpcDaGroupProps = Omit<DrawerProps, "classes"> &
  WithStyles<any> & { opcDaServerId: string; opcDaGroupId: string };

export const OpcDaGroup = (props: OpcDaGroupProps) => {
  const dispath = useDispatch();
  const { selectedItem } = useSelector((state: AppState) => state.window);
  const opcDaGroup = opcDaGroupSelector((state: AppState) =>
    state.opcDa.opcDaServers
      .find((p) => p.id === props.opcDaServerId)
      ?.opcDaGroups?.find((p) => p.id === props.opcDaGroupId)
  );

  const itemPrimary = { primary: props.classes.itemPrimary };
  const textStyle = { marginLeft: 38 };

  return (
    <ListItem
      button
      className={clsx(
        props.classes.item,
        selectedItem &&
          selectedItem.type === OPC_DA_GROUP_TYPE &&
          selectedItem.item.id === props.opcDaGroupId &&
          props.classes.itemActiveItem
      )}
      onClick={() =>
        dispath(setSelectedItem({ type: OPC_DA_GROUP_TYPE, item: opcDaGroup }))
      }
    >
      <ListItemText classes={itemPrimary} style={textStyle}>
        <Box
          component="div"
          textOverflow="ellipsis"
          overflow="hidden"
          fontStyle="inherit"
        >
          {opcDaGroup.name}
        </Box>
      </ListItemText>
    </ListItem>
  );
};
