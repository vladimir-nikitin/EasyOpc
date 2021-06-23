import React, { FC } from "react";
import { SimpleDialog } from "./SimpleDialog";
import DialogContentText from "@material-ui/core/DialogContentText";

type SimpleConfirmDialogProps = {
  isOpen: boolean;
  header: string;
  text: string;
  okHandle: () => void;
  cancelHandle: () => void;
};

export const SimpleConfirmDialog = (props: SimpleConfirmDialogProps) => {
  const textStyle = { margin: 60 };
  return (
    <SimpleDialog
      cancelHandle={props.cancelHandle}
      okHandle={props.okHandle}
      isOpen={props.isOpen}
      header={props.header}
    >
      <DialogContentText id="alert-dialog-description" style={textStyle}>
        {props.text}
      </DialogContentText>
    </SimpleDialog>
  );
};
