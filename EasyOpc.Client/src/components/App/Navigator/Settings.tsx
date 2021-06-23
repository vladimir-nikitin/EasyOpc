import React, { useEffect } from "react";
import clsx from "clsx";
import { WithStyles } from "@material-ui/core/styles";
import { DrawerProps } from "@material-ui/core/Drawer";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import SettingsIcon from "@material-ui/icons/Settings";
import { Omit } from "@material-ui/types";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../../../store/store";
import { setSelectedItem } from "../../../store/windowSlice";
import { SETTINGS_TYPE } from "../../../constans/settings";

type SettingsProps = Omit<DrawerProps, "classes"> & WithStyles<any>;

export const Settings = (props: SettingsProps) => {
  const { classes, ...other } = props;
  const { selectedItem } = useSelector((state: AppState) => state.window);
  useEffect(() => {}, [selectedItem]);
  const dispath = useDispatch();
  const itemPrimary = { primary: classes.itemPrimary, }

  return (
    <ListItem
      key={SETTINGS_TYPE}
      button
      className={clsx(
        classes.item,
        selectedItem &&
          selectedItem.type === SETTINGS_TYPE &&
          classes.itemActiveItem
      )}
      onClick={() => dispath(setSelectedItem({ type: SETTINGS_TYPE }))}
    >
      <ListItemIcon className={classes.itemIcon}>
        <SettingsIcon />
      </ListItemIcon>
      <ListItemText classes={itemPrimary}>
        Settings
      </ListItemText>
    </ListItem>
  );
}
