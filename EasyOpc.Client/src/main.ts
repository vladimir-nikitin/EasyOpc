const url = require("url");
const path = require("path");

import { app, BrowserWindow, Menu, Tray } from "electron";

let window: BrowserWindow | null;
let tray = null;

const createWindow = () => {
  window = new BrowserWindow({
    minWidth: 1100, minHeight: 700, width: 1100, height: 700, frame: false, webPreferences: {
      nodeIntegration: true,
      enableRemoteModule: true
    }
  });

  window.loadURL(
    url.format({
      pathname: path.join(__dirname, "index.html"),
      protocol: "file:",
      slashes: true
    })
  );

  window.on("closed", () => {
    window = null;
  });
};

app.on("ready", createWindow);

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});

app.on("activate", () => {
  if (window === null) {
    createWindow();
  }
});

app.on('ready', () => {
  tray = new Tray(path.join(__dirname, 'resources/icons/tray-icon.png'));

  if (process.platform === 'win32') {
    tray.on('balloon-click', tray.popUpContextMenu);
    tray.on('double-click', () => window.show());
  }

  const menu = Menu.buildFromTemplate([
    {
      label: 'Exit',
      click() { app.quit(); }
    }
  ]);

  tray.setToolTip('EasyOPC Demo');
  tray.setContextMenu(menu);
});