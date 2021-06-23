import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { initApp } from "../../functions/app";
import { startWinService } from "../../functions/common";
import { AppState } from "../../store/store";
import { Loader } from "../controls/Loder";
import { Paperbase } from "./Paperbase";
import {enableMapSet} from "immer"

const AppLoader = () => {
  console.log(`[App][AppLoader] mount component`);
  const showAppLoader = useSelector((state: AppState) => state.window.showAppLoader);

  console.log(`[App][AppLoader] showAppLoader:`);
  console.log(showAppLoader);

  return (
      <Loader show={showAppLoader} />
  );
};

export const App = () => {
  console.log(`[App] mount component`);

  enableMapSet();

  const dispath = useDispatch();
  const isInit = useSelector((state: AppState) => state.window.isInit);

  startWinService();
  
  useEffect(()=> { 
    console.log(`[App][useEffect] dispath changed`);
    dispath(initApp()); 
  }, [dispath]);

  if(!isInit) return <Loader show />

  return (
    <>
      <Paperbase />
      <AppLoader />
    </>
  );
};
