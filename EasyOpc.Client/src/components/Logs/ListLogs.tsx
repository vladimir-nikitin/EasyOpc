import React from "react";
import { createStyles, makeStyles, Theme } from "@material-ui/core/styles";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";
import { useSelector } from "react-redux";
import { AppState } from "../../store/store";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      paddingLeft: '6px',
      width: "100%",
      height: "100%",
      overflow: "scroll",
      display: "flex"
    },
  })
);

export const ListLogs = () => {
  const classes = useStyles();
  const logs = useSelector((state: AppState) => state.log.logs);
  const listItemStyle = { padding: 0, margin: 0 };
  return (
    <div className={classes.root}>
      <List>
        {
          logs && logs.map((p, i) => 
            (<ListItem key={i} style={listItemStyle}>
              <ListItemText primary={p} style={listItemStyle} />
            </ListItem>))
        }
      </List>
    </div>
  );
};
