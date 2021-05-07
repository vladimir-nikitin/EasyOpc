import React, { useEffect, useState } from "react";
import AppBar from "@material-ui/core/AppBar";
import Button from "@material-ui/core/Button";
import Grid from "@material-ui/core/Grid";
import HelpIcon from "@material-ui/icons/Help";
import Hidden from "@material-ui/core/Hidden";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import Toolbar from "@material-ui/core/Toolbar";
import Tooltip from "@material-ui/core/Tooltip";
import Typography from "@material-ui/core/Typography";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles,
} from "@material-ui/core/styles";
import { useDispatch, useSelector } from "react-redux";
import HomeIcon from "@material-ui/icons/Home";
import { AppState } from "../../store/store";
import MinimizeIcon from "@material-ui/icons/Minimize";
import CloseIcon from "@material-ui/icons/Close";
import FullscreenIcon from "@material-ui/icons/Fullscreen";
import FullscreenExitIcon from "@material-ui/icons/FullscreenExit";
import { openUrl } from "../../functions/common";
import {
  HELP_URL,
  LOGS_TYPE,
  SETTINGS_TYPE,
  SITE_URL,
} from "../../constans/common";
import { OPC_GROUP_TYPE, OPC_SERVER_TYPE } from "../../constans/opc";
import { PlayArrow, Stop } from "@material-ui/icons";
import { changeServiceMode } from "../../functions/app";

const lightColor = "rgba(255, 255, 255, 0.7)";

const styles = (theme: Theme) =>
  createStyles({
    secondaryBar: {
      zIndex: 0,
      "-webkit-app-region": "drag",
    },
    menuButton: {
      marginLeft: -theme.spacing(1),
    },
    iconButtonAvatar: {
      padding: 4,
    },
    link: {
      textDecoration: "none",
      color: lightColor,
      "&:hover": {
        color: theme.palette.common.white,
      },
    },
    wbContainer: {
      "-webkit-app-region": "no-drag",
    },
    button: {
      borderColor: lightColor,
    },
  });

interface HeaderProps extends WithStyles<typeof styles> {
}

function Header(props: HeaderProps) {
  const { classes } = props;
  const dispatch = useDispatch();

  const [headerText, setHeaderText] = useState("");
  const { selectedItem, serviceMode } = useSelector((state: AppState) => state.window);

  useEffect(() => {
    switch (selectedItem?.type) {
      case OPC_SERVER_TYPE:
        setHeaderText(selectedItem.item.name);
        break;
      case OPC_GROUP_TYPE:
        setHeaderText(selectedItem.item.name);
        break;
      case SETTINGS_TYPE:
        setHeaderText(SETTINGS_TYPE);
        break;
      case LOGS_TYPE:
        setHeaderText(LOGS_TYPE);
        break;
      default:
        setHeaderText("");
        break;
    }
  }, [selectedItem]);

  const { remote } = require("electron");
  const [isWindowMax, setIsWindowMax] = useState(false);

  const closeWindow = () => remote.getCurrentWindow().close();
  const hideWindow = () => remote.getCurrentWindow().hide();

  const maxWindow = () => {
    isWindowMax
      ? remote.getCurrentWindow().unmaximize()
      : remote.getCurrentWindow().maximize();
    setIsWindowMax(!isWindowMax);
  };

  return (
    <React.Fragment>
      <AppBar
        color="primary"
        position="sticky"
        elevation={0}
        className={classes.secondaryBar}
      >
        <Toolbar>
          <Grid container spacing={1} alignItems="center">
            <Hidden smUp>
              <Grid item>
                <IconButton
                  color="inherit"
                  aria-label="open drawer"
                  className={classes.menuButton}
                >
                  <MenuIcon />
                </IconButton>
              </Grid>
            </Hidden>
            <Grid item xs />
            <Grid item className={classes.wbContainer}>
              <IconButton color="inherit" onClick={hideWindow}>
                <MinimizeIcon />
              </IconButton>
            </Grid>
            <Grid item className={classes.wbContainer}>
              <IconButton color="inherit" onClick={maxWindow}>
                {isWindowMax ? <FullscreenExitIcon /> : <FullscreenIcon />}
              </IconButton>
            </Grid>
            <Grid item className={classes.wbContainer}>
              <IconButton color="inherit" onClick={closeWindow}>
                <CloseIcon />
              </IconButton>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <AppBar
        component="div"
        className={classes.secondaryBar}
        color="primary"
        position="static"
        elevation={0}
      >
        <Toolbar>
          <Grid container alignItems="center" spacing={1}>
            <Grid item xs>
              <Typography color="inherit" variant="h5" component="h1">
                {headerText}
              </Typography>
            </Grid>
            <Grid item className={classes.wbContainer}>
              <Button
                className={classes.button}
                variant="outlined"
                color="inherit"
                size="small"
                startIcon={serviceMode ? <PlayArrow /> : <Stop />}
                onClick={() => dispatch(changeServiceMode(!serviceMode))}
              >
                {serviceMode ? 'Start' : 'Stop'}
              </Button>
            </Grid>
            <Grid item className={classes.wbContainer}>
              <Tooltip title="Web site">
                <IconButton color="inherit" onClick={() => openUrl(SITE_URL)}>
                  <HomeIcon />
                </IconButton>
              </Tooltip>
            </Grid>
            <Grid item className={classes.wbContainer}>
              <Tooltip title="Help">
                <IconButton color="inherit" onClick={() => openUrl(HELP_URL)}>
                  <HelpIcon />
                </IconButton>
              </Tooltip>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
    </React.Fragment>
  );
}

export default withStyles(styles)(Header);
