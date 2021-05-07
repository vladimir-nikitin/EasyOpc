import React from "react";
import clsx from "clsx";
import { WithStyles } from "@material-ui/core/styles";
import { DrawerProps } from "@material-ui/core/Drawer";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { setSelectedItem } from "../../../store/windowSlice";
import { OPC_GROUP_TYPE } from "../../../constans/opc";
import Box from "@material-ui/core/Box";
import { OpcGroup } from "../../../store/opcSlice";

export interface OpcGroupsProps
  extends Omit<DrawerProps, "classes">,
    WithStyles<any> {
  opcGroups: OpcGroup[];
}

export default function OpcGroups(props: OpcGroupsProps) {
  const { classes, opcGroups } = props;
  const dispath = useDispatch();
  const { selectedItem } = useSelector((state: AppState) => state.window);

  const itemPrimary = { primary: classes.itemPrimary, }
  const textStyle = { marginLeft: 38 }

  return (
    <List component="div" disablePadding>
      {opcGroups.map((group) => (
        <ListItem
          key={group.id}
          button
          className={clsx(
            classes.item,
            selectedItem &&
              selectedItem.type === OPC_GROUP_TYPE &&
              selectedItem.item.id === group.id &&
              classes.itemActiveItem
          )}
          onClick={() =>
            dispath(setSelectedItem({ type: OPC_GROUP_TYPE, item: group }))
          }
        >
          <ListItemText
            classes={itemPrimary}
            style={textStyle}
          >
            <Box
              component="div"
              textOverflow="ellipsis"
              overflow="hidden"
              fontStyle="inherit"
            >
              {group.name}
            </Box>
          </ListItemText>
        </ListItem>
      ))}
    </List>
  );
}
