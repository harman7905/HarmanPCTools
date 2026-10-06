#ifndef MyAppVersion
#define MyAppVersion "2.1.0"
#endif

#define MyAppName "Harman PC Toolkit"
#define MyAppPublisher "Harman"
#define MyAppExeName "HarmanPCTools.exe"

[Setup]
AppId={{9AB0EA47-53E5-4A22-8B9B-7B54A5E8B5F3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\Harman PC Toolkit
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
Name: "{group}\Harman PC Toolkit"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\Harman PC Toolkit"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch Harman PC Toolkit"; Flags: nowait postinstall skipifsilent
