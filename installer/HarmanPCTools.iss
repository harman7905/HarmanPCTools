#ifndef MyAppVersion
#define MyAppVersion "2.0.2"
#endif

#define MyAppName "Harman PC Tools"
#define MyAppPublisher "Harman PC Tools"
#define MyAppExeName "HarmanPCTools.exe"

[Setup]
AppId={{9AB0EA47-53E5-4A22-8B9B-7B54A5E8B5F3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\Harman PC Tools
DefaultGroupName={#MyAppName}
OutputDir=..\dist\installer
OutputBaseFilename=HarmanPCTools-Setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
SetupIconFile=..\HarmanPCTools\Assets\HarmanPCTools.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

[Files]
Source: "..\dist\publish\HarmanPCTools.exe"; DestDir: "{app}"; Flags: ignoreversion

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"

[Icons]
Name: "{group}\Harman PC Tools"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\Harman PC Tools"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch Harman PC Tools"; Flags: nowait postinstall skipifsilent
