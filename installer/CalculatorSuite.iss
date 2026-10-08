#define MyAppName "Calculator Suite"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Naqeebs Multi Services"
#define MyAppExeName "CalculatorSuite.exe"

[Setup]
AppId={{B2B3DB8A-4B61-4E7A-9FE4-9BB8A5A6C2C1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\CalculatorSuite
DefaultGroupName={#MyAppName}
OutputDir=Output
OutputBaseFilename=CalculatorSuite-Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
WizardStyle=modern
SetupIconFile=..\CalculatorSuite\Assets\icon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent
