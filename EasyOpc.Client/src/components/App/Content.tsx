import React, { useEffect } from "react";
import { useSelector } from "react-redux";
import { AppState } from "../../store/store";
import { WithStyles } from "@material-ui/core/styles";
import Settings from "../Settings/Settings";
import { Logs } from "../Logs/Logs";
import { SETTINGS_TYPE } from "../../constans/settings";
import { LOGS_TYPE } from "../../constans/logs";
import { OPC_DA_GROUP_TYPE, OPC_DA_SERVER_TYPE } from "../../constans/opc.da";
import { OpcDaServer } from "../opc.da/OpcDaServer";
import { OPC_UA_GROUP_TYPE, OPC_UA_SERVER_TYPE } from "../../constans/opc.ua";
import { OpcUaServer } from "../opc.ua/OpcUaServer";
import { OpcDaGroup } from "../opc.da/OpcDaGroup/OpcDaGroup";
import { OpcUaGroup } from "../opc.ua/OpcUaGroup/OpcUaGroup";

export const Content = (props: WithStyles) => {
  console.log(`[App][Content] mount component`);
  const selectedItem = useSelector((state: AppState) => state.window.selectedItem);

  console.log(`[App][Content] selectedItem:`);
  console.log(selectedItem);

  if (!selectedItem) return <></>;

  if (selectedItem.type === SETTINGS_TYPE) return <Settings />;
  else if (selectedItem.type === LOGS_TYPE) return <Logs />;
  else if (selectedItem.type === OPC_DA_SERVER_TYPE)
    return <OpcDaServer opcDaServerId={selectedItem.item.id} {...props} />;
  else if (selectedItem.type === OPC_DA_GROUP_TYPE)
    return <OpcDaGroup opcDaServerId={selectedItem.item.opcDaServerId} opcDaGroupId={selectedItem.item.id} {...props} />;
  else if (selectedItem.type === OPC_UA_SERVER_TYPE)
    return <OpcUaServer opcUaServerId={selectedItem.item.id} {...props} />;
  else if (selectedItem.type === OPC_UA_GROUP_TYPE)
    return <OpcUaGroup opcUaServerId={selectedItem.item.opcUaServerId} opcUaGroupId={selectedItem.item.id} {...props} />;

  return <></>;
}
