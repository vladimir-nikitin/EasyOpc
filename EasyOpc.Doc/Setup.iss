;------------------------------------------------------------------------------
;
;       Пример установочного скрипта для Inno Setup 5.5.5
;       (c) maisvendoo, 15.04.2015
;
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;   Определяем некоторые константы
;------------------------------------------------------------------------------

; Имя приложения
#define   Name       "EasyOPC"
; Версия приложения
#define   Version    "12.2.3"
; Фирма-разработчик
#define   Publisher  "EasyOPC"
; Сафт фирмы разработчика
#define   URL        "www.easyopc.com"
; Имя исполняемого модуля
#define   ExeName    "EasyOPC.exe"

;------------------------------------------------------------------------------
;   Параметры установки
;------------------------------------------------------------------------------
[Setup]

; Уникальный идентификатор приложения, 
;сгенерированный через Tools -> Generate GUID
AppId={{56B95FA3-10CF-4F82-9F3B-92094CCA39FD}

PrivilegesRequired=admin

; Прочая информация, отображаемая при установке
AppName={#Name}
AppVersion={#Version}
AppPublisher={#Publisher}
AppPublisherURL={#URL}
AppSupportURL={#URL}
AppUpdatesURL={#URL}

; Путь установки по-умолчанию
DefaultDirName={pf}\{#Name}
; Имя группы в меню "Пуск"
DefaultGroupName={#Name}

; Каталог, куда будет записан собранный setup и имя исполняемого файла
OutputDir=D:\Public_new\output
OutputBaseFileName=setup

; Файл иконки
SetupIconFile=D:\Public_new\input\resources\icons\favicon.ico

; Параметры сжатия
Compression=lzma
SolidCompression=yes

[Types]
Name: "full"; Description: "Full installation"
Name: "compact"; Description: "Desktop only installation"
Name: "custom"; Description: "Custom installation"; Flags: iscustom

[Components]
Name: "desktop"; Description: "Desktop application"; Types: full compact custom; Flags: fixed
Name: "winservice"; Description: "Win-service"; Types: full

;------------------------------------------------------------------------------
;   Опционально - некоторые задачи, которые надо выполнить при установке
;------------------------------------------------------------------------------
[Tasks]
; Создание иконки на рабочем столе
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


;------------------------------------------------------------------------------
;   Файл иконки в Control Panel
;------------------------------------------------------------------------------
[Setup]
UninstallDisplayIcon="{app}\resources\icons\favicon.ico"

;------------------------------------------------------------------------------
;   Файлы, которые надо включить в пакет установщика
;------------------------------------------------------------------------------
[Files]

; Исполняемый файл
Source: "D:\Public_new\input\EasyOPC.exe"; DestDir: "{app}"; Flags: ignoreversion

; Прилагающиеся ресурсы
Source: "D:\Public_new\input\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

;------------------------------------------------------------------------------
;   Указываем установщику, где он должен взять иконки
;------------------------------------------------------------------------------ 
[Icons]
Name: "{group}\{#Name}"; Filename: "{app}\{#ExeName}"
Name: "{commondesktop}\{#Name}"; Filename: "{app}\{#ExeName}"; Tasks: desktopicon
;Name: "{commonstartup}\{#Name}"; Filename: "{app}\{#ExeName}"; Parameters: "/minimized"

[Run]
Filename: cmd.exe;Parameters: "/c netsh advfirewall firewall add rule name=""EasyOPC"" dir=in action=allow program=""{app}\Service\EasyOpcWinService.exe"" enable=yes"
Filename: cmd.exe;Components: "winservice";Parameters: "/c C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /user= /password= ""{app}\Service\EasyOpcWinService.exe"""
Filename: cmd.exe;Components: "winservice";Parameters: "/c sc start EasyOpcWinService"
Filename: cmd.exe;Components: "desktop";Parameters: "/c Echo {{""mode"": ""desktop""} > ""{app}\config.json"""
Filename: cmd.exe;Components: "winservice";Parameters: "/c Echo {{""mode"": ""winservice""} > ""{app}\config.json"""

[UninstallRun]
Filename: cmd.exe;Parameters: "/c sc stop EasyOpcWinService"
Filename: cmd.exe;Parameters: "/c sc delete EasyOpcWinService"
Filename: cmd.exe;Parameters: "/c Taskkill /F /IM {#ExeName}"