import React from "react";

interface TabPanelProps {
  children?: React.ReactNode;
  index: any;
  value: any;
}

export const TabPanel = (props: TabPanelProps) => {
  const { children, value, index, ...other } = props;
  const tabPanelStyle = {
    flexGrow: 1,
    display: "flex",
    height: "500px",
    margin: "20px 20px 0 20px",
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