import React from "react";

type TabPanelProps = {
  children?: React.ReactNode;
  index: any;
  value: any;
}

export const TabPanel = (props: TabPanelProps) => {
  const { children, value, index, ...other } = props;
  const tabPanelStyle = {
    flexGrow: 1,
    display: "flex",
    margin: "20px 20px 0 20px",
    overflow: "hidden"
  };

  return value === index ? (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
      style={tabPanelStyle}>
      {children}
    </div>
  ) : null;
}