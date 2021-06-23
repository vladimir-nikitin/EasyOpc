import React, { FC, PropsWithChildren } from "react";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles,
} from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import MuiDialogTitle from "@material-ui/core/DialogTitle";
import MuiDialogContent from "@material-ui/core/DialogContent";
import MuiDialogActions from "@material-ui/core/DialogActions";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import Typography from "@material-ui/core/Typography";

const styles = (theme: Theme) =>
  createStyles({
    root: {
      margin: 0,
      padding: theme.spacing(2),
      "-webkit-app-region": "no-drag",
    },
    closeButton: {
      position: "absolute",
      right: theme.spacing(1),
      top: theme.spacing(1),
      color: theme.palette.grey[500],
    },
  });

export interface DialogTitleProps extends WithStyles<typeof styles> {
  id: string;
  children: React.ReactNode;
  onClose: () => void;
}

const DialogTitle = withStyles(styles)((props: DialogTitleProps) => {
  const { children, classes, onClose, ...other } = props;
  return (
    <MuiDialogTitle disableTypography className={classes.root} {...other}>
      <Typography variant="h6">{children}</Typography>
      {onClose ? (
        <IconButton
          aria-label="close"
          className={classes.closeButton}
          onClick={onClose}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </MuiDialogTitle>
  );
});

const DialogContent = withStyles((theme: Theme) => ({
  root: {
    padding: theme.spacing(2),
  },
}))(MuiDialogContent);

const DialogActions = withStyles((theme: Theme) => ({
  root: {
    margin: 0,
    padding: theme.spacing(1),
  },
}))(MuiDialogActions);

export type SimpleDialogProps = PropsWithChildren<{
  isOpen: boolean;
  header: string;
  isOkButtonDisabled?: boolean | null;
  okHandle: () => void;
  cancelHandle: () => void;
}>;

export const SimpleDialog = (props:SimpleDialogProps) => {
  const dialogStyle = { padding: 0 };
  return (
    <Dialog
      onClose={props.cancelHandle}
      aria-labelledby="customized-dialog-title"
      open={props.isOpen}
    >
      <DialogTitle id="customized-dialog-title" onClose={props.cancelHandle}>
        {props.header}
      </DialogTitle>
      <DialogContent dividers style={dialogStyle}>
        {props.children}
      </DialogContent>
      <DialogActions>
        <Button autoFocus onClick={props.cancelHandle} color="primary">
          Cancel
        </Button>
        <Button onClick={props.okHandle} color="primary" disabled={props.isOkButtonDisabled ?? false}>
          Ok
        </Button>
      </DialogActions>
    </Dialog>
  );
}