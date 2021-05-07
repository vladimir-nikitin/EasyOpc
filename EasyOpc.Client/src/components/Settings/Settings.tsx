import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { makeStyles } from "@material-ui/core/styles";
import Paper from "@material-ui/core/Paper";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Grid from "@material-ui/core/Grid";
import StorageIcon from "@material-ui/icons/Storage";
import { AppState } from "../../store/store";

const useStyles = makeStyles({
  paper: {
    height: "300px",
    width: "500px",
    marginLeft: 'auto',
    marginRight: 'auto',
    marginTop: '40px'
  },
  searchBar: {
    borderBottom: "1px solid rgba(0, 0, 0, 0.12)",
  },
  block: {
    display: "block",
  },
  input: {
    margin: '5px 0'
  },
});

export type SettingsProps = {
};

function Settings(props: SettingsProps) {
  const dispatch = useDispatch();
  const classes = useStyles();
  const serviceMode = useSelector((state: AppState) => state.window.serviceMode);

  return <></>
}

export default Settings;
