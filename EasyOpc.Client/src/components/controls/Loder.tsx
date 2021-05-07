import CircularProgress from "@material-ui/core/CircularProgress";
import { makeStyles } from "@material-ui/core/styles";
import React, { FC, useEffect } from "react";

const useStyles = makeStyles({
  container: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    position: 'absolute',
    top: 0,
    bottom: 0,
    left: 0,
    right: 0,
    zIndex: 9999999
  }
});

export const Loader: FC<{ show: boolean }> = ({ show }) => {
  const classes = useStyles();
  useEffect(() => {}, [show]);

  if (!show) return null;

  return (
    <div className={classes.container}>
      <CircularProgress  />
      <p>Loading...</p>
    </div>
  );
};
