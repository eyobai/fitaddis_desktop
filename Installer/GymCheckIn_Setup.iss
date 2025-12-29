; GymCheckIn Installer Script for Inno Setup
; ==========================================

#define MyAppName "Fit Addis Gym Check-In"
#define MyAppVersion "1.0"
#define MyAppPublisher "Fit Addis"
#define MyAppExeName "GymCheckIn.exe"
#define MyAppPath "C:\Users\hp\Documents\fitaddis_desktop"
; ZKTeco SDK location
#define MySDKPath "C:\Users\hp\Downloads\9774a946c3f659ddf2ae90bc8dadc3eb(1)\ZKFingerSDK_Windows_Standard\ZKFinger Standard SDK 5.3.0.33"

[Setup]
AppId={{A1B2C3D4-E5F6-7890-ABCD-GYMCHECKIN001}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir={#MyAppPath}\Installer\Output
OutputBaseFilename=GymCheckIn_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startupicon"; Description: "Start automatically when Windows starts"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
; Main application files
Source: "{#MyAppPath}\bin\x86\Release\GymCheckIn.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\bin\x86\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\bin\x86\Release\x86\*"; DestDir: "{app}\x86"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyAppPath}\bin\x86\Release\x64\*"; DestDir: "{app}\x64"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyAppPath}\bin\x86\Release\Sounds\*"; DestDir: "{app}\Sounds"; Flags: ignoreversion createallsubdirs recursesubdirs

; ZKTeco SDK installer
Source: "{#MySDKPath}\*"; DestDir: "{tmp}\ZKFingerSDK"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Install ZKTeco SDK first
Filename: "{tmp}\ZKFingerSDK\setup.exe"; Description: "Installing ZKTeco Fingerprint SDK..."; StatusMsg: "Installing ZKTeco SDK..."; Flags: waituntilterminated

; Launch the application after install
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
; Add to Windows startup if user selected the option
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: """{app}\{#MyAppExeName}"""; Flags: uninsdeletevalue; Tasks: startupicon

[UninstallDelete]
; Clean up any leftover files
Type: filesandordirs; Name: "{app}"
