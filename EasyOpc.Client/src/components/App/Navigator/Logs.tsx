import React, { useEffect } from "react";
import clsx from "clsx";
import { createStyles, Theme, WithStyles } from "@material-ui/core/styles";
import { DrawerProps } from "@material-ui/core/Drawer";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import { Omit } from "@material-ui/types";
import SubjectIcon from "@material-ui/icons/Subject";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { setSelectedItem } from "../../../store/windowSlice";
import { LOGS_TYPE } from "../../../constans/common";

export interface LogsProps
  extends Omit<DrawerProps, "classes">,
    WithStyles<any> {}

export default function Logs(props: LogsProps) {
  const { classes, ...other } = props;
  const { selectedItem } = useSelector((state: AppState) => state.window);

  //useEffect(() => {}, [selectedItem]);
  const dispath = useDispatch();
  const primary = { primary: classes.itemPrimary };

  return (
    <ListItem
      key={LOGS_TYPE}
      button
      className={clsx(
        classes.item,
        selectedItem &&
          selectedItem.type === LOGS_TYPE &&
          classes.itemActiveItem
      )}
      onClick={() => dispath(setSelectedItem({ type: LOGS_TYPE }))}
    >
      <ListItemIcon className={classes.itemIcon}>
        <SubjectIcon />
      </ListItemIcon>
      <ListItemText
        classes={primary}
      >
        Logs
      </ListItemText>
    </ListItem>
  );
}
