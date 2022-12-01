# Bannerlord Dedicated Server Patches
This is a mod for server side patching of known crashes for Bannerlord Dedicated Servers. It works by overwriting existing .NET methods with harmony patches and either working around or directly addressing known issues. The majority of .NET errors are null references, so null checking is the majority of the patches. I will do my best to keep this mod up to date with the latest server files, especially if TW implements some of these patches themselves.

## Installation

- Put the **[latest release dlls](https://github.com/HornsGuy/BannerlordServerPatches/releases/download/v1.0.2.0/ServerPatches_v1.0.2.0.zip)** in "Mount & Blade II Dedicated Server\bin\Win64_Shipping_Server"
- Add the following xml node to your `[Dedicated Server Files]/Modules/Multiplayer/SubModule.xml` file, between the  `<SubModules> </SubModules>` tags. 
```xml
<SubModule>
	<Name value="ServerPatches" />
	<DLLName value="ServerPatches.dll" />
	<SubModuleClassType value="ServerPatches.ServerPatches" />
	<Tags>
		<Tag key="DedicatedServerType" value="custom" />
	</Tags>
</SubModule>
```
- Your SubModule.xml file should look like this
```xml
<?xml version="1.0" encoding="utf-8"?>
<Module>
	<Name value="Multiplayer" />
	<Id value="Multiplayer" />
	<Version value="v1.0.1" />
	<DefaultModule value="true" />
	<ModuleCategory value="Multiplayer" />
	<Official value="true" />
	<DependedModules>
		<DependedModule Id="Native" DependentVersion="v1.0.1" Optional="false" />
	</DependedModules>
	<SubModules>
        <!-- Added here -->
		<SubModule>
			<Name value="ServerPatches" />
			<DLLName value="ServerPatches.dll" />
			<SubModuleClassType value="ServerPatches.ServerPatches" />
			<Tags>
				<Tag key="DedicatedServerType" value="custom" />
			</Tags>
		</SubModule>
        <!-- Your other submodules -->
	</SubModules>
</Module>
```
## Settings
You can modify how the ServerPatches dll behaves by changing the values in "ServerPatches/settings.json". The "ServerPatches" folder should be located in the same directory as the server exe

### Options
- LoggingAddGameLogsMessages - Enables/Disables the logging of "AddGameLogsMessage" REST messages. If turned on, the contents will appear in "ServerPatches/Rest/*.txt"
- NumberOfLogsToKeep - The number of logs to preserve before deleting the oldest log file
- NumberOfRestLogsToKeep - The number of rest logs to preserve before deleting the oldest log file
- SettingsVersion - Used for versioning of the settings file. Do not modify.

## ChangeLog

### v1.0.2.0
- Two known crashes have been fixed by TW, so their corresponding patches have been removed. Thanks TW Development Team!

<details>
  <summary>v1.0.1.2</summary>
	
- Added Settings
- Fixed an issue related to Rest Message "AddGameLogsMessage" causing communications with the master server to break down, though there is still more work to do
</details>

<details>
  <summary>v1.0.1.1</summary>
  
- Fixed 2 Issues the Calradic Campaign server was having with Siege
  
</details>

## Contributing

If your server has .NET crashes that aren't addressed by this server side module, please open an issue and add the stack trace and dmp to the issue.

### Stack Trace
1. Open "Event Viewer"
2. Navigate to the "Windows Logs->Application" log
3. Look for an Error entry with ".NET Runtime" as the source
4. Copy the contents of that and paste it into your issue

### Crash Dump
1. Open File Explorer
2. Enter "%localappdata%\CrashDumps" into the search bar
3. Grab the latest dmp for "DedicatedCustomerServer.Starter.exe"

Note: Enabling full crash dumps is very beneficial for figuring out the best solution for each crash! To enable full crash dumps, follow these instructions: https://helgeklein.com/blog/creating-an-application-crash-dump/

## Limitations
Some errors occur within the game engine which is written in C++. Unfortunately, these errors cannot be addressed by modding and must be addressed by TaleWorlds directly. If you do see these errors, posting the information on the TW support forums would be your best solution.

Linux/Wine specific issues fall outside the goal of this project.
