export const openUrl = (url: string) => require("electron").shell.openExternal(url);

export const openFiles: () => string[] = () => {
    const { remote } = require('electron'),
        dialog = remote.dialog,
        WIN = remote.getCurrentWindow();
    return dialog.showOpenDialogSync(WIN);
}

export const readFile: (path: string) => string = (path) =>
    require("fs").readFileSync(path, 'utf-8');

export const selectFolder: () => string[] = () => {
    const { remote } = require('electron'),
        dialog = remote.dialog,
        WIN = remote.getCurrentWindow();
    return dialog.showOpenDialogSync(WIN, { properties: ["openDirectory"]});
}

export const startWinService: () => void = () => {
    try{
        const path = require ('path');
        const exeDirectory = path.dirname (process.execPath);

        console.log(`exeDirectory:`);
        console.log(exeDirectory);

        const configFilePath = `${exeDirectory}\\config.json`;
        console.log(`configFilePath:`);
        console.log(configFilePath);

        const content = readFile(configFilePath);
        console.log(`content:`);
        console.log(content);

        var mode = JSON.parse(content).mode;
        console.log(`App mode: ${mode}`)

        if(mode === "desktop"){
            let spawn = require("child_process").spawn;
            let bat = spawn(`${exeDirectory}\\Service\\EasyOpcWinService.exe`, ["-d"]);
        }
        else if(mode === "winservice"){
    
        }
    }
    catch(ex){ 
        console.error(ex); 
    }
}


