import React, { useEffect } from "react";
import { useSelector } from "react-redux";
import { AppState } from "../../store/store";
import OpcGroup from "../Opc/OpcGroup/OpcGroup";
import OpcServer from "../Opc/OpcServer/OpcServer";
import { WithStyles } from "@material-ui/core/styles";
import { OPC_GROUP_TYPE, OPC_SERVER_TYPE } from "../../constans/opc";
import { LOGS_TYPE, SETTINGS_TYPE } from "../../constans/common";
import Settings from "../Settings/Settings";
import { Logs } from "../Logs/Logs";

export interface ContentProps extends WithStyles {}

function Content(props: ContentProps) {
  console.log(`[App][Content] mount component`);
  const selectedItem = useSelector((state: AppState) => state.window.selectedItem);

  console.log(`[App][Content] selectedItem:`);
  console.log(selectedItem);

  if (!selectedItem) return <></>;

  if (selectedItem.type === OPC_SERVER_TYPE)
    return <OpcServer opcServer={selectedItem.item} {...props} />;
  else if (selectedItem.type === OPC_GROUP_TYPE)
    return <OpcGroup opcServerId={selectedItem.item.opcServerId} opcGroupId={selectedItem.item.id} />;
  else if (selectedItem.type === SETTINGS_TYPE) return <Settings />;
  else if (selectedItem.type === LOGS_TYPE) return <Logs />;

  return <></>;
}

export default Content;
