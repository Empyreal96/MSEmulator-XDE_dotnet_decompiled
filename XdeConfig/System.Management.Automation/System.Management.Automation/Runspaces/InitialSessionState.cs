using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000834 RID: 2100
	public class InitialSessionState
	{
		// Token: 0x0600505E RID: 20574 RVA: 0x001A8ABC File Offset: 0x001A6CBC
		private static void RemoveDisallowedEntries<T>(InitialSessionStateEntryCollection<T> list, List<string> allowedNames, Func<T, string> nameGetter) where T : InitialSessionStateEntry
		{
			List<string> list2 = new List<string>();
			foreach (T arg in ((IEnumerable<T>)list))
			{
				string entryName = nameGetter(arg);
				if (!allowedNames.Exists((string allowedName) => allowedName.Equals(entryName, StringComparison.OrdinalIgnoreCase)))
				{
					list2.Add(arg.Name);
				}
			}
			foreach (string name in list2)
			{
				list.Remove(name, null);
			}
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001A8BB0 File Offset: 0x001A6DB0
		private static void MakeDisallowedEntriesPrivate<T>(InitialSessionStateEntryCollection<T> list, List<string> allowedNames, Func<T, string> nameGetter) where T : ConstrainedSessionStateEntry
		{
			foreach (T t in ((IEnumerable<T>)list))
			{
				string entryName = nameGetter(t);
				SessionStateAliasEntry aliasEntry = t as SessionStateAliasEntry;
				if (aliasEntry != null)
				{
					if (allowedNames.Exists((string allowedName) => allowedName.Equals(aliasEntry.Definition, StringComparison.OrdinalIgnoreCase)))
					{
						aliasEntry.Visibility = SessionStateEntryVisibility.Public;
						continue;
					}
				}
				if (!allowedNames.Exists((string allowedName) => allowedName.Equals(entryName, StringComparison.OrdinalIgnoreCase)))
				{
					t.Visibility = SessionStateEntryVisibility.Private;
				}
			}
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001A8C6C File Offset: 0x001A6E6C
		public static InitialSessionState CreateFromSessionConfigurationFile(string path)
		{
			return InitialSessionState.CreateFromSessionConfigurationFile(path, null);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001A8C78 File Offset: 0x001A6E78
		public static InitialSessionState CreateFromSessionConfigurationFile(string path, Func<string, bool> roleVerifier)
		{
			DISCPowerShellConfiguration discpowerShellConfiguration = new DISCPowerShellConfiguration(path, roleVerifier);
			return discpowerShellConfiguration.GetInitialSessionState(null);
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x001A8C94 File Offset: 0x001A6E94
		public static InitialSessionState CreateRestricted(SessionCapabilities sessionCapabilities)
		{
			if (SessionCapabilities.RemoteServer == sessionCapabilities)
			{
				return InitialSessionState.CreateRestrictedForRemoteServer();
			}
			if (SessionCapabilities.WorkflowServer == sessionCapabilities)
			{
				return InitialSessionState.CreateRestrictedForWorkflowServerMinimum();
			}
			if (sessionCapabilities == (SessionCapabilities.RemoteServer | SessionCapabilities.WorkflowServer))
			{
				return InitialSessionState.CreateRestrictedForWorkflowServer();
			}
			if (sessionCapabilities == (SessionCapabilities.RemoteServer | SessionCapabilities.WorkflowServer | SessionCapabilities.Language))
			{
				return InitialSessionState.CreateRestrictedForWorkflowServerWithFullLanguage();
			}
			return InitialSessionState.Create();
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001A8CD8 File Offset: 0x001A6ED8
		private static InitialSessionState CreateRestrictedForRemoteServer()
		{
			InitialSessionState initialSessionState = InitialSessionState.Create();
			initialSessionState.LanguageMode = PSLanguageMode.NoLanguage;
			initialSessionState.ThrowOnRunspaceOpenError = true;
			initialSessionState.UseFullLanguageModeInDebugger = false;
			initialSessionState.Commands.Add(InitialSessionState.BuiltInFunctions);
			initialSessionState.Commands.Add(InitialSessionState.BuiltInAliases);
			Collection<PSSnapInInfo> collection = PSSnapInReader.ReadEnginePSSnapIns();
			foreach (PSSnapInInfo psSnapInInfo in collection)
			{
				PSSnapInException ex;
				initialSessionState.ImportPSSnapIn(psSnapInInfo, out ex);
			}
			List<string> list = new List<string>();
			list.Add("Get-Command");
			list.Add("Get-FormatData");
			list.Add("Clear-Host");
			list.Add("Select-Object");
			list.Add("Get-Help");
			list.Add("Measure-Object");
			list.Add("Out-Default");
			list.Add("Exit-PSSession");
			InitialSessionState.MakeDisallowedEntriesPrivate<SessionStateCommandEntry>(initialSessionState.Commands, list, (SessionStateCommandEntry commandEntry) => commandEntry.Name);
			InitialSessionState.IncludePowerShellCoreFormats(initialSessionState);
			List<string> list2 = new List<string>();
			list2.Add("types.ps1xml");
			list2.Add("typesV3.ps1xml");
			InitialSessionState.RemoveDisallowedEntries<SessionStateTypeEntry>(initialSessionState.Types, list2, (SessionStateTypeEntry typeEntry) => Path.GetFileName(typeEntry.FileName));
			foreach (SessionStateProviderEntry sessionStateProviderEntry in ((IEnumerable<SessionStateProviderEntry>)initialSessionState.Providers))
			{
				sessionStateProviderEntry.Visibility = SessionStateEntryVisibility.Private;
			}
			initialSessionState.Variables.Add(InitialSessionState.BuiltInVariables);
			foreach (KeyValuePair<string, CommandMetadata> keyValuePair in CommandMetadata.GetRestrictedCommands(SessionCapabilities.RemoteServer))
			{
				string key = keyValuePair.Key;
				Collection<SessionStateCommandEntry> collection2 = initialSessionState.Commands[key];
				collection2[0].Visibility = SessionStateEntryVisibility.Private;
				string definition = ProxyCommand.Create(keyValuePair.Value, "", false);
				initialSessionState.Commands.Add(new SessionStateFunctionEntry(key, definition));
			}
			return initialSessionState;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001A8F28 File Offset: 0x001A7128
		private static void IncludePowerShellCoreFormats(InitialSessionState iss)
		{
			string applicationBase = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID);
			if (string.IsNullOrEmpty(applicationBase))
			{
				return;
			}
			iss.Formats.Clear();
			foreach (string path in InitialSessionState.PSCoreFormatFileNames)
			{
				iss.Formats.Add(new SessionStateFormatEntry(Path.Combine(applicationBase, path)));
			}
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001A8FA8 File Offset: 0x001A71A8
		private static InitialSessionState CreateRestrictedForWorkflowServer()
		{
			InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
			initialSessionState.LanguageMode = PSLanguageMode.NoLanguage;
			initialSessionState.ThrowOnRunspaceOpenError = true;
			initialSessionState.UseFullLanguageModeInDebugger = false;
			foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry.GetType() == typeof(SessionStateApplicationEntry))
				{
					initialSessionState.Commands.Remove(sessionStateCommandEntry.Name, sessionStateCommandEntry);
					break;
				}
			}
			List<string> list = new List<string>();
			list.AddRange(InitialSessionState.JobCmdlets);
			list.AddRange(InitialSessionState.ImplicitRemotingCmdlets);
			list.AddRange(InitialSessionState.MiscCmdlets);
			list.AddRange(InitialSessionState.AutoDiscoveryCmdlets);
			InitialSessionState.MakeDisallowedEntriesPrivate<SessionStateCommandEntry>(initialSessionState.Commands, list, (SessionStateCommandEntry commandEntry) => commandEntry.Name);
			foreach (SessionStateCommandEntry sessionStateCommandEntry2 in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry2.GetType() == typeof(SessionStateAliasEntry))
				{
					if (InitialSessionState.allowedAliases.Contains(sessionStateCommandEntry2.Name, StringComparer.OrdinalIgnoreCase))
					{
						sessionStateCommandEntry2.Visibility = SessionStateEntryVisibility.Public;
					}
					else
					{
						sessionStateCommandEntry2.Visibility = SessionStateEntryVisibility.Private;
					}
				}
			}
			List<string> allowedNames = new List<string>
			{
				"Certificate.Format.ps1xml",
				"Event.format.ps1xml",
				"Diagnostics.format.ps1xml",
				"DotNetTypes.Format.ps1xml",
				"FileSystem.Format.ps1xml",
				"Help.Format.ps1xml",
				"HelpV3.format.ps1xml",
				"PowerShellCore.format.ps1xml",
				"PowerShellTrace.format.ps1xml",
				"Registry.format.ps1xml",
				"WSMan.format.ps1xml"
			};
			InitialSessionState.RemoveDisallowedEntries<SessionStateFormatEntry>(initialSessionState.Formats, allowedNames, (SessionStateFormatEntry formatEntry) => Path.GetFileName(formatEntry.FileName));
			List<string> list2 = new List<string>();
			list2.AddRange(InitialSessionState.DefaultTypeFiles);
			InitialSessionState.RemoveDisallowedEntries<SessionStateTypeEntry>(initialSessionState.Types, list2, (SessionStateTypeEntry typeEntry) => Path.GetFileName(typeEntry.FileName));
			initialSessionState.Variables.Clear();
			return initialSessionState;
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001A9234 File Offset: 0x001A7434
		private static InitialSessionState CreateRestrictedForWorkflowServerWithFullLanguage()
		{
			InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
			initialSessionState.LanguageMode = PSLanguageMode.FullLanguage;
			initialSessionState.ThrowOnRunspaceOpenError = true;
			initialSessionState.UseFullLanguageModeInDebugger = false;
			foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry.GetType() == typeof(SessionStateApplicationEntry))
				{
					initialSessionState.Commands.Remove(sessionStateCommandEntry.Name, sessionStateCommandEntry);
					break;
				}
			}
			List<string> list = new List<string>();
			list.AddRange(InitialSessionState.JobCmdlets);
			list.AddRange(InitialSessionState.ImplicitRemotingCmdlets);
			list.AddRange(InitialSessionState.MiscCmdlets);
			list.AddRange(InitialSessionState.MiscCommands);
			list.AddRange(InitialSessionState.AutoDiscoveryCmdlets);
			list.AddRange(InitialSessionState.LanguageHelperCmdlets);
			list.AddRange(InitialSessionState.DebugCmdlets);
			InitialSessionState.MakeDisallowedEntriesPrivate<SessionStateCommandEntry>(initialSessionState.Commands, list, (SessionStateCommandEntry commandEntry) => commandEntry.Name);
			foreach (SessionStateCommandEntry sessionStateCommandEntry2 in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry2.GetType() == typeof(SessionStateAliasEntry))
				{
					if (InitialSessionState.allowedAliases.Contains(sessionStateCommandEntry2.Name, StringComparer.OrdinalIgnoreCase))
					{
						sessionStateCommandEntry2.Visibility = SessionStateEntryVisibility.Public;
					}
					else
					{
						sessionStateCommandEntry2.Visibility = SessionStateEntryVisibility.Private;
					}
				}
			}
			List<string> allowedNames = new List<string>
			{
				"Certificate.Format.ps1xml",
				"Event.format.ps1xml",
				"Diagnostics.format.ps1xml",
				"DotNetTypes.Format.ps1xml",
				"FileSystem.Format.ps1xml",
				"Help.Format.ps1xml",
				"HelpV3.format.ps1xml",
				"PowerShellCore.format.ps1xml",
				"PowerShellTrace.format.ps1xml",
				"Registry.format.ps1xml",
				"WSMan.format.ps1xml"
			};
			InitialSessionState.RemoveDisallowedEntries<SessionStateFormatEntry>(initialSessionState.Formats, allowedNames, (SessionStateFormatEntry formatEntry) => Path.GetFileName(formatEntry.FileName));
			List<string> list2 = new List<string>();
			list2.AddRange(InitialSessionState.DefaultTypeFiles);
			InitialSessionState.RemoveDisallowedEntries<SessionStateTypeEntry>(initialSessionState.Types, list2, (SessionStateTypeEntry typeEntry) => Path.GetFileName(typeEntry.FileName));
			initialSessionState.Variables.Clear();
			Hashtable value = new Hashtable
			{
				{
					"Get-Command:ListImported",
					true
				}
			};
			initialSessionState.Variables.Add(new SessionStateVariableEntry("PSDefaultParameterValues", value, "Default Get-Command Action"));
			return initialSessionState;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001A950C File Offset: 0x001A770C
		private static InitialSessionState CreateRestrictedForWorkflowServerMinimum()
		{
			InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
			initialSessionState.LanguageMode = PSLanguageMode.NoLanguage;
			initialSessionState.ThrowOnRunspaceOpenError = true;
			initialSessionState.UseFullLanguageModeInDebugger = false;
			foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)initialSessionState.Commands))
			{
				if (sessionStateCommandEntry.GetType() == typeof(SessionStateApplicationEntry))
				{
					initialSessionState.Commands.Remove(sessionStateCommandEntry.Name, sessionStateCommandEntry);
					break;
				}
			}
			List<string> list = new List<string>
			{
				"Get-Command"
			};
			list.AddRange(InitialSessionState.JobCmdlets);
			list.AddRange(InitialSessionState.MiscCmdlets);
			InitialSessionState.MakeDisallowedEntriesPrivate<SessionStateCommandEntry>(initialSessionState.Commands, list, (SessionStateCommandEntry commandEntry) => commandEntry.Name);
			initialSessionState.Formats.Clear();
			List<string> list2 = new List<string>();
			list2.AddRange(InitialSessionState.DefaultTypeFiles);
			InitialSessionState.RemoveDisallowedEntries<SessionStateTypeEntry>(initialSessionState.Types, list2, (SessionStateTypeEntry typeEntry) => Path.GetFileName(typeEntry.FileName));
			initialSessionState.Variables.Clear();
			SessionStateVariableEntry item = new SessionStateVariableEntry("PSDisableModuleAutoDiscovery", true, "True if we disable module autodiscovery", ScopedItemOptions.Constant);
			initialSessionState.Variables.Add(item);
			return initialSessionState;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001A9668 File Offset: 0x001A7868
		protected InitialSessionState()
		{
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001A96E8 File Offset: 0x001A78E8
		public static InitialSessionState Create()
		{
			return new InitialSessionState();
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001A96FC File Offset: 0x001A78FC
		public static InitialSessionState CreateDefault()
		{
			InitialSessionState initialSessionState = new InitialSessionState();
			initialSessionState.Variables.Add(InitialSessionState.BuiltInVariables);
			initialSessionState.Commands.Add(new SessionStateApplicationEntry("*"));
			initialSessionState.Commands.Add(new SessionStateScriptEntry("*"));
			initialSessionState.Commands.Add(InitialSessionState.BuiltInFunctions);
			initialSessionState.Commands.Add(InitialSessionState.BuiltInAliases);
			Collection<PSSnapInInfo> collection = PSSnapInReader.ReadEnginePSSnapIns();
			foreach (PSSnapInInfo psSnapInInfo in collection)
			{
				try
				{
					PSSnapInException ex;
					initialSessionState.ImportPSSnapIn(psSnapInInfo, out ex);
				}
				catch (PSSnapInException ex2)
				{
					throw ex2;
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = initialSessionState.Assemblies.Count - 1; i >= 0; i--)
			{
				string fileName = initialSessionState.Assemblies[i].FileName;
				if (!string.IsNullOrEmpty(fileName))
				{
					if (hashSet.Contains(fileName))
					{
						initialSessionState.Assemblies.RemoveItem(i);
					}
					else
					{
						hashSet.Add(fileName);
					}
				}
			}
			initialSessionState.LanguageMode = PSLanguageMode.FullLanguage;
			initialSessionState.AuthorizationManager = new PSAuthorizationManager(Utils.DefaultPowerShellShellID);
			return initialSessionState.Clone();
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x001A9848 File Offset: 0x001A7A48
		public static InitialSessionState CreateDefault2()
		{
			InitialSessionState initialSessionState = new InitialSessionState();
			initialSessionState.Variables.Add(InitialSessionState.BuiltInVariables);
			initialSessionState.Commands.Add(new SessionStateApplicationEntry("*"));
			initialSessionState.Commands.Add(new SessionStateScriptEntry("*"));
			initialSessionState.Commands.Add(InitialSessionState.BuiltInFunctions);
			initialSessionState.Commands.Add(InitialSessionState.BuiltInAliases);
			initialSessionState.ImportCorePSSnapIn();
			initialSessionState.LanguageMode = PSLanguageMode.FullLanguage;
			initialSessionState.AuthorizationManager = new PSAuthorizationManager(Utils.DefaultPowerShellShellID);
			return initialSessionState.Clone();
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x001A98D9 File Offset: 0x001A7AD9
		internal static bool IsEngineModule(string moduleName)
		{
			return InitialSessionState.EngineModules.Contains(moduleName) || InitialSessionState.NestedEngineModules.Contains(moduleName);
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x001A98F5 File Offset: 0x001A7AF5
		internal static bool IsNestedEngineModule(string moduleName)
		{
			return InitialSessionState.NestedEngineModules.Contains(moduleName);
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x001A9902 File Offset: 0x001A7B02
		internal static bool IsConstantEngineModule(string moduleName)
		{
			return InitialSessionState.ConstantEngineModules.Contains(moduleName) || InitialSessionState.ConstantEngineNestedModules.Contains(moduleName);
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x001A9920 File Offset: 0x001A7B20
		public InitialSessionState Clone()
		{
			InitialSessionState initialSessionState = new InitialSessionState();
			initialSessionState.Variables.Add(this.Variables.Clone());
			initialSessionState.EnvironmentVariables.Add(this.EnvironmentVariables.Clone());
			initialSessionState.Commands.Add(this.Commands.Clone());
			initialSessionState.Assemblies.Add(this.Assemblies.Clone());
			initialSessionState.Providers.Add(this.Providers.Clone());
			initialSessionState.Types.Add(this.Types.Clone());
			initialSessionState.Formats.Add(this.Formats.Clone());
			foreach (string item in this.StartupScripts)
			{
				initialSessionState.StartupScripts.Add(item);
			}
			foreach (string item2 in this.UnresolvedCommandsToExpose)
			{
				initialSessionState.UnresolvedCommandsToExpose.Add(item2);
			}
			foreach (Hashtable item3 in this.DynamicVariablesToDefine)
			{
				initialSessionState.DynamicVariablesToDefine.Add(item3);
			}
			foreach (string key in this.CommandModifications.Keys)
			{
				initialSessionState.CommandModifications.Add(key, this.CommandModifications[key]);
			}
			initialSessionState.DefaultCommandVisibility = this.DefaultCommandVisibility;
			initialSessionState.AuthorizationManager = this.AuthorizationManager;
			initialSessionState.LanguageMode = this.LanguageMode;
			initialSessionState.TranscriptDirectory = this.TranscriptDirectory;
			if (this._wasExecutionPolicySet)
			{
				initialSessionState.ExecutionPolicy = this.ExecutionPolicy;
			}
			initialSessionState.UseFullLanguageModeInDebugger = this.UseFullLanguageModeInDebugger;
			initialSessionState.ThreadOptions = this.ThreadOptions;
			initialSessionState.ThrowOnRunspaceOpenError = this.ThrowOnRunspaceOpenError;
			initialSessionState.ApartmentState = this.ApartmentState;
			foreach (ModuleSpecification item4 in this.ModuleSpecificationsToImport)
			{
				initialSessionState.ModuleSpecificationsToImport.Add(item4);
			}
			foreach (string item5 in this.CoreModulesToImport)
			{
				initialSessionState.CoreModulesToImport.Add(item5);
			}
			initialSessionState.DisableFormatUpdates = this.DisableFormatUpdates;
			foreach (PSSnapInInfo item6 in this.defaultSnapins)
			{
				initialSessionState.defaultSnapins.Add(item6);
			}
			foreach (KeyValuePair<string, PSSnapInInfo> keyValuePair in this._importedSnapins)
			{
				initialSessionState.ImportedSnapins.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return initialSessionState;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x001A9CBC File Offset: 0x001A7EBC
		public static InitialSessionState Create(string snapInName)
		{
			return new InitialSessionState();
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x001A9CC3 File Offset: 0x001A7EC3
		public static InitialSessionState Create(string[] snapInNameCollection, out PSConsoleLoadException warning)
		{
			warning = null;
			return new InitialSessionState();
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001A9CCD File Offset: 0x001A7ECD
		public static InitialSessionState CreateFrom(string snapInPath, out PSConsoleLoadException warnings)
		{
			warnings = null;
			return new InitialSessionState();
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001A9CD7 File Offset: 0x001A7ED7
		public static InitialSessionState CreateFrom(string[] snapInPathCollection, out PSConsoleLoadException warnings)
		{
			warnings = null;
			return new InitialSessionState();
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x001A9CE1 File Offset: 0x001A7EE1
		// (set) Token: 0x06005075 RID: 20597 RVA: 0x001A9CE9 File Offset: 0x001A7EE9
		public PSLanguageMode LanguageMode
		{
			get
			{
				return this._languageMode;
			}
			set
			{
				this._languageMode = value;
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x001A9CF2 File Offset: 0x001A7EF2
		// (set) Token: 0x06005077 RID: 20599 RVA: 0x001A9CFA File Offset: 0x001A7EFA
		public string TranscriptDirectory
		{
			get
			{
				return this._transcriptDirectory;
			}
			set
			{
				this._transcriptDirectory = value;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x001A9D03 File Offset: 0x001A7F03
		// (set) Token: 0x06005079 RID: 20601 RVA: 0x001A9D0B File Offset: 0x001A7F0B
		public ExecutionPolicy ExecutionPolicy
		{
			get
			{
				return this._executionPolicy;
			}
			set
			{
				this._executionPolicy = value;
				this._wasExecutionPolicySet = true;
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x001A9D1B File Offset: 0x001A7F1B
		// (set) Token: 0x0600507B RID: 20603 RVA: 0x001A9D23 File Offset: 0x001A7F23
		public bool UseFullLanguageModeInDebugger
		{
			get
			{
				return this._useFullLanguageModeInDebugger;
			}
			set
			{
				this._useFullLanguageModeInDebugger = value;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x0600507C RID: 20604 RVA: 0x001A9D2C File Offset: 0x001A7F2C
		// (set) Token: 0x0600507D RID: 20605 RVA: 0x001A9D34 File Offset: 0x001A7F34
		public ApartmentState ApartmentState
		{
			get
			{
				return this.apartmentState;
			}
			set
			{
				this.apartmentState = value;
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x001A9D3D File Offset: 0x001A7F3D
		// (set) Token: 0x0600507F RID: 20607 RVA: 0x001A9D45 File Offset: 0x001A7F45
		public PSThreadOptions ThreadOptions
		{
			get
			{
				return this.createThreadOptions;
			}
			set
			{
				this.createThreadOptions = value;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005080 RID: 20608 RVA: 0x001A9D4E File Offset: 0x001A7F4E
		// (set) Token: 0x06005081 RID: 20609 RVA: 0x001A9D56 File Offset: 0x001A7F56
		public bool ThrowOnRunspaceOpenError
		{
			get
			{
				return this.throwOnRunspaceOpenError;
			}
			set
			{
				this.throwOnRunspaceOpenError = value;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06005082 RID: 20610 RVA: 0x001A9D5F File Offset: 0x001A7F5F
		// (set) Token: 0x06005083 RID: 20611 RVA: 0x001A9D67 File Offset: 0x001A7F67
		public virtual AuthorizationManager AuthorizationManager
		{
			get
			{
				return this._authorizationManager;
			}
			set
			{
				this._authorizationManager = value;
			}
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001A9D70 File Offset: 0x001A7F70
		public void ImportPSModule(string[] name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			foreach (string moduleName in name)
			{
				this._moduleSpecificationsToImport.Add(new ModuleSpecification(moduleName));
			}
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001A9DB0 File Offset: 0x001A7FB0
		internal void ClearPSModules()
		{
			this._moduleSpecificationsToImport.Clear();
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001A9DC0 File Offset: 0x001A7FC0
		public void ImportPSModule(IEnumerable<ModuleSpecification> modules)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("modules");
			}
			foreach (ModuleSpecification item in modules)
			{
				this._moduleSpecificationsToImport.Add(item);
			}
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001A9E1C File Offset: 0x001A801C
		public void ImportPSModulesFromPath(string path)
		{
			string text = Environment.ExpandEnvironmentVariables(path);
			List<string> defaultAvailableModuleFiles = ModuleUtils.GetDefaultAvailableModuleFiles(text, new List<string>
			{
				text
			});
			foreach (string text2 in defaultAvailableModuleFiles)
			{
				this.ImportPSModule(new string[]
				{
					text2
				});
			}
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001A9E98 File Offset: 0x001A8098
		internal void ImportPSCoreModule(string[] name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			foreach (string item in name)
			{
				this._coreModulesToImport.Add(item);
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x001A9ED4 File Offset: 0x001A80D4
		public ReadOnlyCollection<ModuleSpecification> Modules
		{
			get
			{
				return new ReadOnlyCollection<ModuleSpecification>(this._moduleSpecificationsToImport);
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x0600508A RID: 20618 RVA: 0x001A9EE1 File Offset: 0x001A80E1
		internal Collection<ModuleSpecification> ModuleSpecificationsToImport
		{
			get
			{
				return this._moduleSpecificationsToImport;
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x001A9EE9 File Offset: 0x001A80E9
		internal Dictionary<string, PSSnapInInfo> ImportedSnapins
		{
			get
			{
				return this._importedSnapins;
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x001A9EF1 File Offset: 0x001A80F1
		internal HashSet<string> CoreModulesToImport
		{
			get
			{
				return this._coreModulesToImport;
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x0600508D RID: 20621 RVA: 0x001A9EFC File Offset: 0x001A80FC
		public virtual InitialSessionStateEntryCollection<SessionStateAssemblyEntry> Assemblies
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._assemblies == null)
					{
						this._assemblies = new InitialSessionStateEntryCollection<SessionStateAssemblyEntry>();
					}
				}
				return this._assemblies;
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x001A9F50 File Offset: 0x001A8150
		public virtual InitialSessionStateEntryCollection<SessionStateTypeEntry> Types
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._types == null)
					{
						this._types = new InitialSessionStateEntryCollection<SessionStateTypeEntry>();
					}
				}
				return this._types;
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x001A9FA4 File Offset: 0x001A81A4
		public virtual InitialSessionStateEntryCollection<SessionStateFormatEntry> Formats
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._formats == null)
					{
						this._formats = new InitialSessionStateEntryCollection<SessionStateFormatEntry>();
					}
				}
				return this._formats;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x001A9FF8 File Offset: 0x001A81F8
		// (set) Token: 0x06005091 RID: 20625 RVA: 0x001AA000 File Offset: 0x001A8200
		public bool DisableFormatUpdates { get; set; }

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x001AA00C File Offset: 0x001A820C
		public virtual InitialSessionStateEntryCollection<SessionStateProviderEntry> Providers
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._providers == null)
					{
						this._providers = new InitialSessionStateEntryCollection<SessionStateProviderEntry>();
					}
				}
				return this._providers;
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06005093 RID: 20627 RVA: 0x001AA060 File Offset: 0x001A8260
		public virtual InitialSessionStateEntryCollection<SessionStateCommandEntry> Commands
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._commands == null)
					{
						this._commands = new InitialSessionStateEntryCollection<SessionStateCommandEntry>();
					}
				}
				return this._commands;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06005094 RID: 20628 RVA: 0x001AA0B4 File Offset: 0x001A82B4
		// (set) Token: 0x06005095 RID: 20629 RVA: 0x001AA0BC File Offset: 0x001A82BC
		internal SessionStateEntryVisibility DefaultCommandVisibility { get; set; }

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06005096 RID: 20630 RVA: 0x001AA0C8 File Offset: 0x001A82C8
		internal HashSet<string> UnresolvedCommandsToExpose
		{
			get
			{
				if (this._unresolvedCommandsToExpose == null)
				{
					lock (this._syncObject)
					{
						if (this._unresolvedCommandsToExpose == null)
						{
							this._unresolvedCommandsToExpose = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						}
					}
				}
				return this._unresolvedCommandsToExpose;
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x001AA128 File Offset: 0x001A8328
		internal Dictionary<string, Hashtable> CommandModifications
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._commandModifications == null)
					{
						this._commandModifications = new Dictionary<string, Hashtable>(StringComparer.OrdinalIgnoreCase);
					}
				}
				return this._commandModifications;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06005098 RID: 20632 RVA: 0x001AA180 File Offset: 0x001A8380
		internal List<Hashtable> DynamicVariablesToDefine
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._dynamicVariablesToDefine == null)
					{
						this._dynamicVariablesToDefine = new List<Hashtable>();
					}
				}
				return this._dynamicVariablesToDefine;
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x001AA1D4 File Offset: 0x001A83D4
		public virtual InitialSessionStateEntryCollection<SessionStateVariableEntry> Variables
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._variables == null)
					{
						this._variables = new InitialSessionStateEntryCollection<SessionStateVariableEntry>();
					}
				}
				return this._variables;
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x0600509A RID: 20634 RVA: 0x001AA228 File Offset: 0x001A8428
		public virtual InitialSessionStateEntryCollection<SessionStateVariableEntry> EnvironmentVariables
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._environmentVariables == null)
					{
						this._environmentVariables = new InitialSessionStateEntryCollection<SessionStateVariableEntry>();
					}
				}
				return this._environmentVariables;
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x0600509B RID: 20635 RVA: 0x001AA27C File Offset: 0x001A847C
		public virtual HashSet<string> StartupScripts
		{
			get
			{
				lock (this._syncObject)
				{
					if (this._startupScripts == null)
					{
						this._startupScripts = new HashSet<string>();
					}
				}
				return this._startupScripts;
			}
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001AA2D0 File Offset: 0x001A84D0
		internal void Bind(ExecutionContext context, bool updateOnly)
		{
			this.Bind(context, updateOnly, null, false, false);
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001AA2EC File Offset: 0x001A84EC
		internal void Bind(ExecutionContext context, bool updateOnly, PSModuleInfo module, bool noClobber, bool local)
		{
			this.Host = context.EngineHostInterface;
			lock (this._syncObject)
			{
				SessionStateInternal engineSessionState = context.EngineSessionState;
				if (!updateOnly)
				{
					engineSessionState.Applications.Clear();
					engineSessionState.Scripts.Clear();
				}
				foreach (SessionStateAssemblyEntry sessionStateAssemblyEntry in ((IEnumerable<SessionStateAssemblyEntry>)this.Assemblies))
				{
					Exception ex = null;
					Assembly left = context.AddAssembly(sessionStateAssemblyEntry.Name, sessionStateAssemblyEntry.FileName, out ex);
					if (left == null || ex != null)
					{
						if (ex == null)
						{
							string message = StringUtil.Format(global::Modules.ModuleAssemblyFound, sessionStateAssemblyEntry.Name);
							ex = new DllNotFoundException(message);
						}
						if ((!string.IsNullOrEmpty(context.ModuleBeingProcessed) && Path.GetExtension(context.ModuleBeingProcessed).Equals(".psd1", StringComparison.OrdinalIgnoreCase)) || this.throwOnRunspaceOpenError)
						{
							throw ex;
						}
						context.ReportEngineStartupError(ex.Message);
					}
				}
				InitialSessionState initialSessionState = null;
				if (this.DefaultCommandVisibility == SessionStateEntryVisibility.Private)
				{
					engineSessionState.DefaultCommandVisibility = SessionStateEntryVisibility.Private;
				}
				foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)this.Commands))
				{
					SessionStateCmdletEntry sessionStateCmdletEntry = sessionStateCommandEntry as SessionStateCmdletEntry;
					if (sessionStateCmdletEntry != null)
					{
						if (noClobber && ModuleCmdletBase.CommandFound(sessionStateCmdletEntry.Name, engineSessionState))
						{
							sessionStateCmdletEntry._isImported = false;
						}
						else
						{
							engineSessionState.AddSessionStateEntry(sessionStateCmdletEntry, local);
							sessionStateCommandEntry.SetModule(module);
						}
					}
					else
					{
						sessionStateCommandEntry.SetModule(module);
						SessionStateFunctionEntry sessionStateFunctionEntry = sessionStateCommandEntry as SessionStateFunctionEntry;
						if (sessionStateFunctionEntry != null)
						{
							engineSessionState.AddSessionStateEntry(sessionStateFunctionEntry);
						}
						else
						{
							SessionStateAliasEntry sessionStateAliasEntry = sessionStateCommandEntry as SessionStateAliasEntry;
							if (sessionStateAliasEntry != null)
							{
								engineSessionState.AddSessionStateEntry(sessionStateAliasEntry, "LOCAL");
							}
							else
							{
								SessionStateApplicationEntry sessionStateApplicationEntry = sessionStateCommandEntry as SessionStateApplicationEntry;
								if (sessionStateApplicationEntry != null)
								{
									engineSessionState.AddSessionStateEntry(sessionStateApplicationEntry);
								}
								else
								{
									SessionStateScriptEntry sessionStateScriptEntry = sessionStateCommandEntry as SessionStateScriptEntry;
									if (sessionStateScriptEntry != null)
									{
										engineSessionState.AddSessionStateEntry(sessionStateScriptEntry);
									}
									else
									{
										SessionStateWorkflowEntry sessionStateWorkflowEntry = sessionStateCommandEntry as SessionStateWorkflowEntry;
										if (sessionStateWorkflowEntry != null)
										{
											if (initialSessionState == null)
											{
												initialSessionState = this.Clone();
												List<SessionStateCommandEntry> list = (from e in initialSessionState.Commands
												where !(e is SessionStateWorkflowEntry)
												select e).ToList<SessionStateCommandEntry>();
												initialSessionState.Commands.Clear();
												foreach (SessionStateCommandEntry item in list)
												{
													initialSessionState.Commands.Add(item);
												}
											}
											engineSessionState.AddSessionStateEntry(initialSessionState, sessionStateWorkflowEntry);
										}
									}
								}
							}
						}
					}
				}
				foreach (SessionStateProviderEntry providerEntry in ((IEnumerable<SessionStateProviderEntry>)this.Providers))
				{
					engineSessionState.AddSessionStateEntry(providerEntry);
				}
				foreach (SessionStateVariableEntry entry in ((IEnumerable<SessionStateVariableEntry>)this.Variables))
				{
					engineSessionState.AddSessionStateEntry(entry);
				}
				foreach (SessionStateVariableEntry sessionStateVariableEntry in ((IEnumerable<SessionStateVariableEntry>)this.EnvironmentVariables))
				{
					Environment.SetEnvironmentVariable(sessionStateVariableEntry.Name, sessionStateVariableEntry.Value.ToString());
				}
				try
				{
					this.UpdateTypes(context, updateOnly);
				}
				catch (RuntimeException ex2)
				{
					MshLog.LogEngineHealthEvent(context, 103, ex2, Severity.Warning);
					if (this.ThrowOnRunspaceOpenError)
					{
						throw;
					}
					context.ReportEngineStartupError(ex2.Message);
				}
				try
				{
					this.UpdateFormats(context, updateOnly);
				}
				catch (RuntimeException ex3)
				{
					MshLog.LogEngineHealthEvent(context, 103, ex3, Severity.Warning);
					if (this.ThrowOnRunspaceOpenError)
					{
						throw;
					}
					context.ReportEngineStartupError(ex3.Message);
				}
				if (!updateOnly)
				{
					engineSessionState.LanguageMode = this.LanguageMode;
				}
				if (this._wasExecutionPolicySet)
				{
					string shellID = context.ShellID;
					SecuritySupport.SetExecutionPolicy(ExecutionPolicyScope.Process, this.ExecutionPolicy, shellID);
				}
			}
			InitialSessionState.SetSessionStateDrive(context, false);
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001AA7B8 File Offset: 0x001A89B8
		internal Exception BindRunspace(Runspace initializedRunspace, PSTraceSource runspaceInitTracer)
		{
			this.ProcessImportModule(initializedRunspace, this.CoreModulesToImport, ModuleIntrinsics.GetSystemwideModulePath());
			initializedRunspace.ExecutionContext.EngineSessionState.Module = null;
			InitialSessionState.SetSessionStateDrive(initializedRunspace.ExecutionContext, true);
			Exception ex = this.ProcessImportModule(initializedRunspace, this.ModuleSpecificationsToImport, "");
			if (ex != null)
			{
				runspaceInitTracer.WriteLine("Runspace open failed while loading module: First error {1}", new object[]
				{
					ex
				});
				return ex;
			}
			string[] modulesForUnResolvedCommands = this.GetModulesForUnResolvedCommands(this.UnresolvedCommandsToExpose, initializedRunspace.ExecutionContext);
			if (modulesForUnResolvedCommands.Length > 0)
			{
				this.ProcessImportModule(initializedRunspace, modulesForUnResolvedCommands, "");
			}
			this.ProcessDynamicVariables(initializedRunspace);
			this.ProcessCommandModifications(initializedRunspace);
			Exception ex2 = this.ProcessStartupScripts(initializedRunspace);
			if (ex2 != null)
			{
				runspaceInitTracer.WriteLine("Runspace open failed while running startup script: First error {1}", new object[]
				{
					ex2
				});
				return PSTraceSource.NewInvalidOperationException(ex2, RemotingErrorIdStrings.StartupScriptThrewTerminatingError, new object[]
				{
					ex2.Message
				});
			}
			if (!string.IsNullOrEmpty(this.TranscriptDirectory))
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.AddCommand(new Command("Start-Transcript")).AddParameter("OutputDirectory", this.TranscriptDirectory);
					Exception ex3 = this.ProcessPowerShellCommand(powerShell, initializedRunspace);
					if (ex3 != null)
					{
						return ex3;
					}
				}
			}
			return null;
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001AA914 File Offset: 0x001A8B14
		private string[] GetModulesForUnResolvedCommands(IEnumerable<string> unresolvedCommands, ExecutionContext context)
		{
			Collection<string> collection = new Collection<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string commandName in unresolvedCommands)
			{
				string value;
				string text = Utils.ParseCommandName(commandName, out value);
				if (string.IsNullOrEmpty(value) && !WildcardPattern.ContainsWildcardCharacters(text))
				{
					hashSet.Add(text);
				}
			}
			if (hashSet.Count > 0)
			{
				Runspace defaultRunspace = Runspace.DefaultRunspace;
				try
				{
					using (Runspace runspace = RunspaceFactory.CreateRunspace())
					{
						runspace.Open();
						Runspace.DefaultRunspace = runspace;
						foreach (string key in hashSet)
						{
							foreach (string path in ModuleUtils.GetDefaultAvailableModuleFiles(true, true, context))
							{
								string fullPath = Path.GetFullPath(path);
								Dictionary<string, List<CommandTypes>> exportedCommands = AnalysisCache.GetExportedCommands(fullPath, false, context);
								if (exportedCommands != null && exportedCommands.ContainsKey(key))
								{
									collection.Add(Path.GetFileNameWithoutExtension(fullPath));
									break;
								}
							}
						}
					}
				}
				finally
				{
					Runspace.DefaultRunspace = defaultRunspace;
				}
			}
			return collection.ToArray<string>();
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001AAA98 File Offset: 0x001A8C98
		private void ProcessCommandModifications(Runspace initializedRunspace)
		{
			foreach (string text in this.CommandModifications.Keys)
			{
				Hashtable hashtable = this.CommandModifications[text];
				CommandInfo command = initializedRunspace.SessionStateProxy.InvokeCommand.GetCommand(text, CommandTypes.Function | CommandTypes.Cmdlet);
				if (command != null)
				{
					FunctionInfo functionInfo = command as FunctionInfo;
					if (functionInfo != null)
					{
						string text2 = functionInfo.Name + "_" + Guid.NewGuid().ToString("N");
						functionInfo.Rename(text2);
						initializedRunspace.ExecutionContext.EngineSessionState.GlobalScope.FunctionTable.Add(text2, functionInfo);
						initializedRunspace.ExecutionContext.EngineSessionState.GlobalScope.FunctionTable.Remove(text);
						command = initializedRunspace.SessionStateProxy.InvokeCommand.GetCommand(text2, CommandTypes.Function);
					}
					CommandMetadata commandMetadata = new CommandMetadata(command);
					List<string> list = new List<string>();
					foreach (object obj in hashtable.Keys)
					{
						string item = (string)obj;
						list.Add(item);
					}
					foreach (string text3 in commandMetadata.Parameters.Keys.ToArray<string>())
					{
						if (!hashtable.ContainsKey(text3))
						{
							commandMetadata.Parameters.Remove(text3);
						}
						else
						{
							list.Remove(text3);
							InitialSessionState.ProcessCommandModification(hashtable, commandMetadata, text3);
						}
					}
					foreach (string parameterName in list)
					{
						InitialSessionState.ProcessCommandModification(hashtable, commandMetadata, parameterName);
					}
					string script = ProxyCommand.Create(commandMetadata, "", false);
					ScriptBlock scriptBlock = ScriptBlock.Create(script);
					scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
					initializedRunspace.ExecutionContext.EngineSessionState.GlobalScope.FunctionTable.Add(text, new FunctionInfo(text, scriptBlock, initializedRunspace.ExecutionContext));
				}
			}
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001AAD10 File Offset: 0x001A8F10
		private static void ProcessCommandModification(Hashtable commandModification, CommandMetadata metadata, string parameterName)
		{
			if (!metadata.Parameters.ContainsKey(parameterName))
			{
				metadata.Parameters[parameterName] = new ParameterMetadata(parameterName);
			}
			Hashtable hashtable = (Hashtable)commandModification[parameterName];
			foreach (object obj in hashtable.Keys)
			{
				string[] array = ((HashSet<string>)hashtable[obj]).ToList<string>().ToArray();
				string a;
				if ((a = obj.ToString()) != null)
				{
					if (!(a == "ValidateSet"))
					{
						if (a == "ValidatePattern")
						{
							string regexPattern = "^(" + string.Join("|", array) + ")$";
							ValidatePatternAttribute item = new ValidatePatternAttribute(regexPattern);
							metadata.Parameters[parameterName].Attributes.Add(item);
						}
					}
					else
					{
						ValidateSetAttribute item2 = new ValidateSetAttribute(array);
						metadata.Parameters[parameterName].Attributes.Add(item2);
					}
				}
			}
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x001AAE34 File Offset: 0x001A9034
		private Exception ProcessDynamicVariables(Runspace initializedRunspace)
		{
			foreach (Hashtable hashtable in this.DynamicVariablesToDefine)
			{
				if (hashtable.ContainsKey("Name"))
				{
					string value = hashtable["Name"].ToString();
					ScriptBlock scriptBlock = hashtable["Value"] as ScriptBlock;
					if (!string.IsNullOrEmpty(value) && scriptBlock != null)
					{
						scriptBlock.SessionStateInternal = initializedRunspace.ExecutionContext.EngineSessionState;
						using (PowerShell powerShell = PowerShell.Create())
						{
							powerShell.AddCommand(new Command("Invoke-Command")).AddParameter("ScriptBlock", scriptBlock).AddParameter("NoNewScope");
							powerShell.AddCommand(new Command("Set-Variable")).AddParameter("Name", value);
							Exception ex = this.ProcessPowerShellCommand(powerShell, initializedRunspace);
							if (ex != null)
							{
								return ex;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x001AAF50 File Offset: 0x001A9150
		private Exception ProcessStartupScripts(Runspace initializedRunspace)
		{
			foreach (string command in this.StartupScripts)
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.AddCommand(new Command(command, false, false));
					Exception ex = this.ProcessPowerShellCommand(powerShell, initializedRunspace);
					if (ex != null)
					{
						return ex;
					}
				}
			}
			return null;
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x001AAFE0 File Offset: 0x001A91E0
		private Exception ProcessPowerShellCommand(PowerShell psToInvoke, Runspace initializedRunspace)
		{
			PSLanguageMode languageMode = initializedRunspace.SessionStateProxy.LanguageMode;
			try
			{
				initializedRunspace.SessionStateProxy.LanguageMode = PSLanguageMode.FullLanguage;
				psToInvoke.Runspace = initializedRunspace;
				foreach (Command command in psToInvoke.Commands.Commands)
				{
					command.CommandOrigin = CommandOrigin.Internal;
				}
				try
				{
					psToInvoke.Invoke();
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					if (this.ThrowOnRunspaceOpenError)
					{
						return ex;
					}
				}
			}
			finally
			{
				if (initializedRunspace.SessionStateProxy.LanguageMode == PSLanguageMode.FullLanguage)
				{
					initializedRunspace.SessionStateProxy.LanguageMode = languageMode;
				}
			}
			if (this.ThrowOnRunspaceOpenError)
			{
				ArrayList arrayList = (ArrayList)initializedRunspace.GetExecutionContext.DollarErrorVariable;
				if (arrayList.Count > 0)
				{
					ErrorRecord errorRecord = arrayList[0] as ErrorRecord;
					if (errorRecord != null)
					{
						return new Exception(errorRecord.ToString());
					}
					Exception ex2 = arrayList[0] as Exception;
					if (ex2 != null)
					{
						return ex2;
					}
					return new Exception(arrayList[0].ToString());
				}
			}
			return null;
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x001AB118 File Offset: 0x001A9318
		private RunspaceOpenModuleLoadException ProcessImportModule(Runspace initializedRunspace, IEnumerable moduleList, string path)
		{
			RunspaceOpenModuleLoadException ex = null;
			foreach (object obj in moduleList)
			{
				string text = obj as string;
				if (text != null)
				{
					ex = this.ProcessImportModule(initializedRunspace, text, null, path);
				}
				else
				{
					ModuleSpecification moduleSpecification = obj as ModuleSpecification;
					if (moduleSpecification != null)
					{
						if (moduleSpecification.RequiredVersion == null && moduleSpecification.Version == null && moduleSpecification.MaximumVersion == null && moduleSpecification.Guid == null)
						{
							ex = this.ProcessImportModule(initializedRunspace, moduleSpecification.Name, null, path);
						}
						else
						{
							Collection<PSModuleInfo> moduleIfAvailable = ModuleCmdletBase.GetModuleIfAvailable(moduleSpecification, initializedRunspace);
							if (moduleIfAvailable != null && moduleIfAvailable.Count > 0)
							{
								ex = this.ProcessImportModule(initializedRunspace, moduleSpecification.Name, moduleIfAvailable[0], path);
							}
							else
							{
								string text2 = "0.0.0.0";
								if (moduleSpecification.RequiredVersion != null)
								{
									text2 = moduleSpecification.RequiredVersion.ToString();
								}
								else if (moduleSpecification.Version != null)
								{
									text2 = moduleSpecification.Version.ToString();
									if (moduleSpecification.MaximumVersion != null)
									{
										text2 = text2 + " - " + moduleSpecification.MaximumVersion;
									}
								}
								else if (moduleSpecification.MaximumVersion != null)
								{
									text2 = moduleSpecification.MaximumVersion;
								}
								string message = StringUtil.Format(global::Modules.RequiredModuleNotFoundWrongGuidVersion, new object[]
								{
									moduleSpecification.Name,
									moduleSpecification.Guid,
									text2
								});
								RunspaceOpenModuleLoadException exception = new RunspaceOpenModuleLoadException(message);
								ex = this.ValidateAndReturnRunspaceOpenModuleLoadException(null, moduleSpecification.Name, exception);
							}
						}
					}
				}
			}
			if (ex == null)
			{
				foreach (string text3 in this.UnresolvedCommandsToExpose.ToArray<string>())
				{
					string moduleName;
					string text4 = Utils.ParseCommandName(text3, out moduleName);
					bool flag = false;
					foreach (CommandInfo commandInfo in this.LookupCommands(text4, moduleName, initializedRunspace.ExecutionContext))
					{
						if (!flag)
						{
							flag = true;
						}
						try
						{
							commandInfo.Visibility = SessionStateEntryVisibility.Public;
						}
						catch (PSNotImplementedException)
						{
						}
					}
					if (flag && !WildcardPattern.ContainsWildcardCharacters(text4))
					{
						this.UnresolvedCommandsToExpose.Remove(text3);
					}
				}
			}
			return ex;
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x001AB634 File Offset: 0x001A9834
		private IEnumerable<CommandInfo> LookupCommands(string commandPattern, string moduleName, ExecutionContext context)
		{
			bool isWildCardPattern = WildcardPattern.ContainsWildcardCharacters(commandPattern);
			SearchResolutionOptions searchOptions = isWildCardPattern ? (SearchResolutionOptions.ResolveFunctionPatterns | SearchResolutionOptions.CommandNameIsPattern | SearchResolutionOptions.SearchAllScopes) : (SearchResolutionOptions.ResolveFunctionPatterns | SearchResolutionOptions.SearchAllScopes);
			bool found = false;
			bool haveModuleName = !string.IsNullOrEmpty(moduleName);
			CommandOrigin cmdOrigin = CommandOrigin.Runspace;
			for (;;)
			{
				foreach (CommandInfo commandInfo in context.SessionState.InvokeCommand.GetCommands(commandPattern, CommandTypes.All, searchOptions, new CommandOrigin?(cmdOrigin)))
				{
					if (!haveModuleName || moduleName.Equals(commandInfo.ModuleName, StringComparison.OrdinalIgnoreCase))
					{
						if (!found)
						{
							found = true;
						}
						yield return commandInfo;
						if (!isWildCardPattern)
						{
							break;
						}
					}
				}
				if (found || cmdOrigin == CommandOrigin.Internal)
				{
					break;
				}
				cmdOrigin = CommandOrigin.Internal;
			}
			yield break;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x001AB668 File Offset: 0x001A9868
		private RunspaceOpenModuleLoadException ProcessImportModule(Runspace initializedRunspace, string name, PSModuleInfo moduleInfoToLoad, string path)
		{
			RunspaceOpenModuleLoadException result;
			using (PowerShell powerShell = PowerShell.Create())
			{
				CommandInfo commandInfo = new CmdletInfo("Import-Module", typeof(ImportModuleCommand), null, null, initializedRunspace.ExecutionContext);
				Command command = new Command(commandInfo);
				if (moduleInfoToLoad != null)
				{
					command.Parameters.Add("ModuleInfo", moduleInfoToLoad);
					name = moduleInfoToLoad.Name;
				}
				else
				{
					if (!string.IsNullOrEmpty(path))
					{
						name = Path.Combine(path, name);
					}
					command.Parameters.Add("Name", name);
				}
				if (!this.ThrowOnRunspaceOpenError)
				{
					command.MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
				}
				powerShell.AddCommand(command);
				if (!this.ThrowOnRunspaceOpenError)
				{
					commandInfo = new CmdletInfo("Out-Default", typeof(OutDefaultCommand), null, null, initializedRunspace.ExecutionContext);
					powerShell.AddCommand(new Command(commandInfo));
				}
				powerShell.Runspace = initializedRunspace;
				powerShell.Invoke();
				if (this.DefaultCommandVisibility != SessionStateEntryVisibility.Public)
				{
					foreach (CommandInfo commandInfo2 in initializedRunspace.ExecutionContext.SessionState.InvokeCommand.GetCommands(name + "\\*", CommandTypes.All, false))
					{
						try
						{
							commandInfo2.Visibility = this.DefaultCommandVisibility;
						}
						catch (PSNotImplementedException)
						{
						}
					}
				}
				result = this.ValidateAndReturnRunspaceOpenModuleLoadException(powerShell, name, null);
			}
			return result;
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x001AB808 File Offset: 0x001A9A08
		private RunspaceOpenModuleLoadException ValidateAndReturnRunspaceOpenModuleLoadException(PowerShell pse, string moduleName, RunspaceOpenModuleLoadException exception)
		{
			if (this.ThrowOnRunspaceOpenError)
			{
				RunspaceOpenModuleLoadException ex = null;
				if (exception != null)
				{
					ex = exception;
				}
				else if (pse.Streams.Error.Count > 0)
				{
					PSDataCollection<ErrorRecord> psdataCollection = new PSDataCollection<ErrorRecord>();
					ErrorRecord errorRecord = pse.Streams.Error[0];
					Exception exception2 = errorRecord.Exception;
					foreach (ErrorRecord item in pse.Streams.Error)
					{
						psdataCollection.Add(item);
					}
					ex = new RunspaceOpenModuleLoadException(moduleName, psdataCollection);
				}
				if (ex != null)
				{
					return ex;
				}
			}
			return null;
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x001AB8B4 File Offset: 0x001A9AB4
		internal void ResetRunspaceState(ExecutionContext context)
		{
			lock (this._syncObject)
			{
				SessionStateInternal engineSessionState = context.EngineSessionState;
				engineSessionState.InitializeSessionStateInternalSpecialVariables(true);
				foreach (SessionStateVariableEntry sessionStateVariableEntry in InitialSessionState.BuiltInVariables)
				{
					PSVariable value = new PSVariable(sessionStateVariableEntry.Name, sessionStateVariableEntry.Value, sessionStateVariableEntry.Options, sessionStateVariableEntry.Attributes, sessionStateVariableEntry.Description)
					{
						Visibility = sessionStateVariableEntry.Visibility
					};
					engineSessionState.GlobalScope.SetVariable(sessionStateVariableEntry.Name, value, false, true, engineSessionState, CommandOrigin.Internal, true);
				}
				engineSessionState.InitializeFixedVariables();
				foreach (SessionStateVariableEntry sessionStateVariableEntry2 in ((IEnumerable<SessionStateVariableEntry>)this.Variables))
				{
					PSVariable value2 = new PSVariable(sessionStateVariableEntry2.Name, sessionStateVariableEntry2.Value, sessionStateVariableEntry2.Options, sessionStateVariableEntry2.Attributes, sessionStateVariableEntry2.Description)
					{
						Visibility = sessionStateVariableEntry2.Visibility
					};
					engineSessionState.GlobalScope.SetVariable(sessionStateVariableEntry2.Name, value2, false, true, engineSessionState, CommandOrigin.Internal, true);
				}
				InitialSessionState.CreateQuestionVariable(context);
				InitialSessionState.SetSessionStateDrive(context, true);
				context.ResetManagers();
				context.PSDebugTraceLevel = 0;
				context.PSDebugTraceStep = false;
			}
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001ABA3C File Offset: 0x001A9C3C
		internal static void SetSessionStateDrive(ExecutionContext context, bool setLocation)
		{
			try
			{
				bool flag = true;
				if (context.EngineSessionState.ProviderCount > 0)
				{
					if (context.EngineSessionState.CurrentDrive == null)
					{
						bool flag2 = false;
						try
						{
							ProviderInfo singleProvider = context.EngineSessionState.GetSingleProvider(context.ProviderNames.FileSystem);
							Collection<PSDriveInfo> drives = singleProvider.Drives;
							if (drives != null && drives.Count > 0)
							{
								context.EngineSessionState.CurrentDrive = drives[0];
								flag2 = true;
							}
						}
						catch (ProviderNotFoundException)
						{
						}
						if (!flag2)
						{
							Collection<PSDriveInfo> collection = context.EngineSessionState.Drives(null);
							if (collection != null && collection.Count > 0)
							{
								context.EngineSessionState.CurrentDrive = collection[0];
							}
							else
							{
								ItemNotFoundException e = new ItemNotFoundException(Directory.GetCurrentDirectory(), "PathNotFound", SessionStateStrings.PathNotFound);
								context.ReportEngineStartupError(e);
								flag = false;
							}
						}
					}
					if (flag && setLocation)
					{
						CmdletProviderContext context2 = new CmdletProviderContext(context);
						try
						{
							context.EngineSessionState.SetLocation(Directory.GetCurrentDirectory(), context2);
						}
						catch (ItemNotFoundException)
						{
							Process currentProcess = Process.GetCurrentProcess();
							string directoryName = Path.GetDirectoryName(PsUtils.GetMainModule(currentProcess).FileName);
							context.EngineSessionState.SetLocation(directoryName, context2);
						}
					}
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001ABBB4 File Offset: 0x001A9DB4
		internal static void CreateQuestionVariable(ExecutionContext context)
		{
			QuestionMarkVariable variable = new QuestionMarkVariable(context);
			context.EngineSessionState.SetVariableAtScope(variable, "global", true, CommandOrigin.Internal);
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001ABBEC File Offset: 0x001A9DEC
		internal void Unbind(ExecutionContext context)
		{
			lock (this._syncObject)
			{
				SessionStateInternal engineSessionState = context.EngineSessionState;
				foreach (SessionStateAssemblyEntry sessionStateAssemblyEntry in ((IEnumerable<SessionStateAssemblyEntry>)this.Assemblies))
				{
					context.RemoveAssembly(sessionStateAssemblyEntry.Name);
				}
				foreach (SessionStateCommandEntry sessionStateCommandEntry in ((IEnumerable<SessionStateCommandEntry>)this.Commands))
				{
					SessionStateCmdletEntry sessionStateCmdletEntry = sessionStateCommandEntry as SessionStateCmdletEntry;
					if (sessionStateCmdletEntry != null && context.TopLevelSessionState.GetCmdletTable().ContainsKey(sessionStateCmdletEntry.Name))
					{
						List<CmdletInfo> list = context.TopLevelSessionState.GetCmdletTable()[sessionStateCmdletEntry.Name];
						for (int i = list.Count - 1; i >= 0; i--)
						{
							if (list[i].ModuleName.Equals(sessionStateCommandEntry.PSSnapIn.Name))
							{
								string name = list[i].Name;
								list.RemoveAt(i);
								context.TopLevelSessionState.RemoveCmdlet(name, i, true);
							}
						}
						if (list.Count == 0)
						{
							context.TopLevelSessionState.RemoveCmdletEntry(sessionStateCmdletEntry.Name, true);
						}
					}
				}
				if (this._providers != null && this._providers.Count > 0)
				{
					Dictionary<string, List<ProviderInfo>> providers = context.TopLevelSessionState.Providers;
					foreach (SessionStateProviderEntry sessionStateProviderEntry in ((IEnumerable<SessionStateProviderEntry>)this._providers))
					{
						if (providers.ContainsKey(sessionStateProviderEntry.Name))
						{
							List<ProviderInfo> list2 = providers[sessionStateProviderEntry.Name];
							for (int j = list2.Count - 1; j >= 0; j--)
							{
								ProviderInfo providerInfo = list2[j];
								if (providerInfo.ImplementingType == sessionStateProviderEntry.ImplementingType)
								{
									InitialSessionState.RemoveAllDrivesForProvider(providerInfo, context.TopLevelSessionState);
									list2.RemoveAt(j);
								}
							}
							if (list2.Count == 0)
							{
								providers.Remove(sessionStateProviderEntry.Name);
							}
						}
					}
				}
				List<string> list3 = new List<string>();
				if (this.Formats != null)
				{
					list3.AddRange(from f in this.Formats
					select f.FileName);
				}
				List<string> list4 = new List<string>();
				if (this.Types != null)
				{
					list4.AddRange(from t in this.Types
					select t.FileName);
				}
				InitialSessionState.RemoveTypesAndFormats(context, list3, list4);
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001ABF14 File Offset: 0x001AA114
		internal static void RemoveTypesAndFormats(ExecutionContext context, IList<string> formatFilesToRemove, IList<string> typeFilesToRemove)
		{
			if (formatFilesToRemove != null && formatFilesToRemove.Count > 0)
			{
				InitialSessionStateEntryCollection<SessionStateFormatEntry> initialSessionStateEntryCollection = new InitialSessionStateEntryCollection<SessionStateFormatEntry>();
				HashSet<string> hashSet = new HashSet<string>(formatFilesToRemove, StringComparer.OrdinalIgnoreCase);
				foreach (SessionStateFormatEntry sessionStateFormatEntry in ((IEnumerable<SessionStateFormatEntry>)context.InitialSessionState.Formats))
				{
					if (!hashSet.Contains(sessionStateFormatEntry.FileName))
					{
						initialSessionStateEntryCollection.Add(sessionStateFormatEntry);
					}
				}
				context.InitialSessionState.Formats.Clear();
				context.InitialSessionState.Formats.Add(initialSessionStateEntryCollection);
				context.InitialSessionState.UpdateFormats(context, false);
			}
			if (typeFilesToRemove != null && typeFilesToRemove.Count > 0)
			{
				InitialSessionStateEntryCollection<SessionStateTypeEntry> initialSessionStateEntryCollection2 = new InitialSessionStateEntryCollection<SessionStateTypeEntry>();
				List<string> list = new List<string>();
				foreach (string text in typeFilesToRemove)
				{
					list.Add(ModuleCmdletBase.ResolveRootedFilePath(text, context) ?? text);
				}
				foreach (SessionStateTypeEntry sessionStateTypeEntry in ((IEnumerable<SessionStateTypeEntry>)context.InitialSessionState.Types))
				{
					if (sessionStateTypeEntry.FileName == null)
					{
						initialSessionStateEntryCollection2.Add(sessionStateTypeEntry);
					}
					else
					{
						string item = ModuleCmdletBase.ResolveRootedFilePath(sessionStateTypeEntry.FileName, context) ?? sessionStateTypeEntry.FileName;
						if (!list.Contains(item))
						{
							initialSessionStateEntryCollection2.Add(sessionStateTypeEntry);
						}
					}
				}
				if (initialSessionStateEntryCollection2.Count > 0)
				{
					context.InitialSessionState.Types.Clear();
					context.InitialSessionState.Types.Add(initialSessionStateEntryCollection2);
					context.InitialSessionState.UpdateTypes(context, false);
					return;
				}
				context.TypeTable.Clear(true);
			}
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x001AC0FC File Offset: 0x001AA2FC
		internal void UpdateTypes(ExecutionContext context, bool updateOnly)
		{
			bool flag = !updateOnly;
			bool flag2 = false;
			TypeTable typeTable = null;
			StringBuilder stringBuilder = new StringBuilder("\n");
			foreach (SessionStateTypeEntry sessionStateTypeEntry in ((IEnumerable<SessionStateTypeEntry>)this.Types))
			{
				string moduleName = "";
				if (sessionStateTypeEntry.PSSnapIn != null && !string.IsNullOrEmpty(sessionStateTypeEntry.PSSnapIn.Name))
				{
					moduleName = sessionStateTypeEntry.PSSnapIn.Name;
				}
				Collection<string> collection = new Collection<string>();
				if (sessionStateTypeEntry.TypeTable != null)
				{
					if (flag && this.Types.Count == 1)
					{
						context.TypeTable = sessionStateTypeEntry.TypeTable;
						typeTable = sessionStateTypeEntry.TypeTable;
						break;
					}
					throw PSTraceSource.NewInvalidOperationException(TypesXmlStrings.TypeTableCannotCoExist, new object[0]);
				}
				else
				{
					if (sessionStateTypeEntry.FileName != null)
					{
						bool flag3;
						context.TypeTable.Update(moduleName, sessionStateTypeEntry.FileName, collection, flag, context.AuthorizationManager, context.EngineHostInterface, out flag3);
					}
					else
					{
						context.TypeTable.Update(sessionStateTypeEntry.TypeData, collection, sessionStateTypeEntry.IsRemove, flag);
					}
					if (updateOnly && context.InitialSessionState != null)
					{
						context.InitialSessionState.Types.Add(sessionStateTypeEntry);
					}
					flag = false;
					foreach (string text in collection)
					{
						if (!string.IsNullOrEmpty(text))
						{
							flag2 = true;
							if (this.ThrowOnRunspaceOpenError || this.RefreshTypeAndFormatSetting)
							{
								stringBuilder.Append(text);
								stringBuilder.Append('\n');
							}
							else
							{
								context.ReportEngineStartupError(ExtendedTypeSystem.TypesXmlError, new object[]
								{
									text
								});
							}
						}
					}
					if (this.ThrowOnRunspaceOpenError && collection.Count > 0)
					{
						string typesXmlError = ExtendedTypeSystem.TypesXmlError;
						InitialSessionState.ThrowTypeOrFormatErrors(typesXmlError, stringBuilder.ToString(), "ErrorsUpdatingTypes");
					}
				}
			}
			if (typeTable != null)
			{
				this.Types.Clear();
				this.Types.Add(typeTable.typesInfo);
			}
			if (this.RefreshTypeAndFormatSetting && flag2)
			{
				string typesXmlError2 = ExtendedTypeSystem.TypesXmlError;
				InitialSessionState.ThrowTypeOrFormatErrors(typesXmlError2, stringBuilder.ToString(), "ErrorsUpdatingTypes");
			}
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x001AC354 File Offset: 0x001AA554
		internal void UpdateFormats(ExecutionContext context, bool update)
		{
			if (this.DisableFormatUpdates || this.Formats.Count == 0)
			{
				return;
			}
			Collection<PSSnapInTypeAndFormatErrors> collection = new Collection<PSSnapInTypeAndFormatErrors>();
			InitialSessionStateEntryCollection<SessionStateFormatEntry> formats;
			if (update && context.InitialSessionState != null)
			{
				formats = context.InitialSessionState.Formats;
				formats.Add(this.Formats);
			}
			else
			{
				formats = this.Formats;
			}
			foreach (SessionStateFormatEntry sessionStateFormatEntry in ((IEnumerable<SessionStateFormatEntry>)formats))
			{
				string psSnapinName = sessionStateFormatEntry.FileName;
				PSSnapInInfo pssnapIn = sessionStateFormatEntry.PSSnapIn;
				if (pssnapIn != null && !string.IsNullOrEmpty(pssnapIn.Name))
				{
					psSnapinName = pssnapIn.Name;
				}
				if (sessionStateFormatEntry.Formattable != null)
				{
					if (formats.Count != 1)
					{
						throw PSTraceSource.NewInvalidOperationException(FormatAndOutXmlLoadingStrings.FormatTableCannotCoExist, new object[0]);
					}
					context.FormatDBManager = sessionStateFormatEntry.Formattable.FormatDBManager;
				}
				else if (sessionStateFormatEntry.FormatData != null)
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(psSnapinName, sessionStateFormatEntry.FormatData));
				}
				else
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(psSnapinName, sessionStateFormatEntry.FileName));
				}
			}
			if (collection.Count > 0)
			{
				context.FormatDBManager.UpdateDataBase(collection, context.AuthorizationManager, context.EngineHostInterface, true);
				StringBuilder stringBuilder = new StringBuilder("\n");
				bool flag = false;
				foreach (PSSnapInTypeAndFormatErrors pssnapInTypeAndFormatErrors in collection)
				{
					if (pssnapInTypeAndFormatErrors.Errors != null && pssnapInTypeAndFormatErrors.Errors.Count > 0)
					{
						foreach (string text in pssnapInTypeAndFormatErrors.Errors)
						{
							if (!string.IsNullOrEmpty(text))
							{
								flag = true;
								if (this.ThrowOnRunspaceOpenError || this.RefreshTypeAndFormatSetting)
								{
									stringBuilder.Append(text);
									stringBuilder.Append('\n');
								}
								else
								{
									context.ReportEngineStartupError(FormatAndOutXmlLoadingStrings.FormatLoadingErrors, new object[]
									{
										text
									});
								}
							}
						}
					}
				}
				if ((this.ThrowOnRunspaceOpenError || this.RefreshTypeAndFormatSetting) && flag)
				{
					string formatLoadingErrors = FormatAndOutXmlLoadingStrings.FormatLoadingErrors;
					InitialSessionState.ThrowTypeOrFormatErrors(formatLoadingErrors, stringBuilder.ToString(), "ErrorsUpdatingFormats");
				}
			}
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x001AC5B8 File Offset: 0x001AA7B8
		private static void ThrowTypeOrFormatErrors(string resourceString, string errorMsg, string errorId)
		{
			string message = StringUtil.Format(resourceString, errorMsg);
			RuntimeException ex = new RuntimeException(message);
			ex.SetErrorId(errorId);
			throw ex;
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x001AC5DC File Offset: 0x001AA7DC
		public PSSnapInInfo ImportPSSnapIn(string name, out PSSnapInException warning)
		{
			if (string.IsNullOrEmpty(name))
			{
				PSTraceSource.NewArgumentNullException("name");
			}
			PSSnapInInfo pssnapInInfo = PSSnapInReader.Read("2", name);
			if (!Utils.IsPSVersionSupported(pssnapInInfo.PSVersion.ToString()))
			{
				InitialSessionState._PSSnapInTracer.TraceError("MshSnapin {0} and current monad engine's versions don't match.", new object[]
				{
					name
				});
				throw PSTraceSource.NewArgumentException("mshSnapInID", ConsoleInfoErrorStrings.AddPSSnapInBadMonadVersion, new object[]
				{
					pssnapInInfo.PSVersion.ToString(),
					"2.0"
				});
			}
			PSSnapInInfo pssnapInInfo2 = this.ImportPSSnapIn(pssnapInInfo, out warning);
			if (pssnapInInfo2 != null)
			{
				this._importedSnapins.Add(pssnapInInfo2.Name, pssnapInInfo2);
			}
			return pssnapInInfo2;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001AC684 File Offset: 0x001AA884
		internal PSSnapInInfo ImportCorePSSnapIn()
		{
			PSSnapInInfo pssnapInInfo = PSSnapInReader.ReadCoreEngineSnapIn();
			this.defaultSnapins.Add(pssnapInInfo);
			try
			{
				PSSnapInException ex;
				this.ImportPSSnapIn(pssnapInInfo, out ex);
			}
			catch (PSSnapInException ex2)
			{
				throw ex2;
			}
			return pssnapInInfo;
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001AC6C4 File Offset: 0x001AA8C4
		internal PSSnapInInfo ImportPSSnapIn(PSSnapInInfo psSnapInInfo, out PSSnapInException warning)
		{
			bool flag = true;
			foreach (SessionStateAssemblyEntry sessionStateAssemblyEntry in ((IEnumerable<SessionStateAssemblyEntry>)this.Assemblies))
			{
				PSSnapInInfo pssnapIn = sessionStateAssemblyEntry.PSSnapIn;
				if (pssnapIn != null)
				{
					string assemblyName = sessionStateAssemblyEntry.PSSnapIn.AssemblyName;
					if (!string.IsNullOrEmpty(assemblyName) && string.Equals(assemblyName, psSnapInInfo.AssemblyName, StringComparison.OrdinalIgnoreCase))
					{
						warning = null;
						flag = false;
						break;
					}
				}
			}
			Dictionary<string, SessionStateCmdletEntry> dictionary = null;
			Dictionary<string, List<SessionStateAliasEntry>> dictionary2 = null;
			Dictionary<string, SessionStateProviderEntry> dictionary3 = null;
			if (psSnapInInfo == null)
			{
				ArgumentNullException ex = new ArgumentNullException("psSnapInInfo");
				throw ex;
			}
			if (!string.IsNullOrEmpty(psSnapInInfo.CustomPSSnapInType))
			{
				this.LoadCustomPSSnapIn(psSnapInInfo);
				warning = null;
				return psSnapInInfo;
			}
			string helpFile = null;
			if (flag)
			{
				InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for psSnapIn {0}", new object[]
				{
					psSnapInInfo.Name
				});
				Assembly assembly = PSSnapInHelpers.LoadPSSnapInAssembly(psSnapInInfo, out dictionary, out dictionary3);
				if (assembly == null)
				{
					InitialSessionState._PSSnapInTracer.TraceError("Loading assembly for psSnapIn {0} failed", new object[]
					{
						psSnapInInfo.Name
					});
					warning = null;
					return null;
				}
				InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for psSnapIn {0} succeeded", new object[]
				{
					psSnapInInfo.Name
				});
				PSSnapInHelpers.AnalyzePSSnapInAssembly(assembly, psSnapInInfo.Name, psSnapInInfo, null, true, out dictionary, out dictionary2, out dictionary3, out helpFile);
			}
			foreach (string text in psSnapInInfo.Types)
			{
				string text2 = Path.Combine(psSnapInInfo.ApplicationBase, text);
				if (!File.Exists(text2))
				{
					text2 = text;
				}
				SessionStateTypeEntry sessionStateTypeEntry = new SessionStateTypeEntry(text2);
				sessionStateTypeEntry.SetPSSnapIn(psSnapInInfo);
				this.Types.Add(sessionStateTypeEntry);
			}
			foreach (string text3 in psSnapInInfo.Formats)
			{
				string text4 = Path.Combine(psSnapInInfo.ApplicationBase, text3);
				if (!File.Exists(text4))
				{
					text4 = text3;
				}
				SessionStateFormatEntry sessionStateFormatEntry = new SessionStateFormatEntry(text4);
				sessionStateFormatEntry.SetPSSnapIn(psSnapInInfo);
				this.Formats.Add(sessionStateFormatEntry);
			}
			SessionStateAssemblyEntry sessionStateAssemblyEntry2 = new SessionStateAssemblyEntry(psSnapInInfo.AssemblyName, psSnapInInfo.AbsoluteModulePath);
			sessionStateAssemblyEntry2.SetPSSnapIn(psSnapInInfo);
			this.Assemblies.Add(sessionStateAssemblyEntry2);
			if (psSnapInInfo.Name.Equals(InitialSessionState.CoreSnapin, StringComparison.OrdinalIgnoreCase))
			{
				sessionStateAssemblyEntry2 = new SessionStateAssemblyEntry("Microsoft.PowerShell.Security", null);
				this.Assemblies.Add(sessionStateAssemblyEntry2);
			}
			if (dictionary != null)
			{
				foreach (SessionStateCmdletEntry sessionStateCmdletEntry in dictionary.Values)
				{
					SessionStateCmdletEntry sessionStateCmdletEntry2 = (SessionStateCmdletEntry)sessionStateCmdletEntry.Clone();
					sessionStateCmdletEntry2.Visibility = this.DefaultCommandVisibility;
					this.Commands.Add(sessionStateCmdletEntry2);
				}
			}
			if (dictionary2 != null)
			{
				foreach (List<SessionStateAliasEntry> list in dictionary2.Values)
				{
					foreach (SessionStateAliasEntry sessionStateAliasEntry in list)
					{
						sessionStateAliasEntry.Visibility = this.DefaultCommandVisibility;
						this.Commands.Add(sessionStateAliasEntry);
					}
				}
			}
			if (dictionary3 != null)
			{
				foreach (SessionStateProviderEntry item in dictionary3.Values)
				{
					this.Providers.Add(item);
				}
			}
			warning = null;
			if (psSnapInInfo.Name.Equals(InitialSessionState.CoreSnapin, StringComparison.OrdinalIgnoreCase))
			{
				foreach (SessionStateFunctionEntry sessionStateFunctionEntry in InitialSessionState.BuiltInFunctions)
				{
					Collection<SessionStateCommandEntry> collection = this.Commands[sessionStateFunctionEntry.Name];
					foreach (SessionStateCommandEntry sessionStateCommandEntry in collection)
					{
						if (sessionStateCommandEntry is SessionStateFunctionEntry)
						{
							((SessionStateFunctionEntry)sessionStateCommandEntry).SetHelpFile(helpFile);
						}
					}
				}
			}
			return psSnapInInfo;
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x001ACB48 File Offset: 0x001AAD48
		internal List<PSSnapInInfo> GetPSSnapIn(string psSnapinName)
		{
			List<PSSnapInInfo> list = null;
			foreach (PSSnapInInfo pssnapInInfo in this.defaultSnapins)
			{
				if (pssnapInInfo.Name.Equals(psSnapinName, StringComparison.OrdinalIgnoreCase))
				{
					if (list == null)
					{
						list = new List<PSSnapInInfo>();
					}
					list.Add(pssnapInInfo);
				}
			}
			PSSnapInInfo item = null;
			if (this._importedSnapins.TryGetValue(psSnapinName, out item))
			{
				if (list == null)
				{
					list = new List<PSSnapInInfo>();
				}
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x001ACBD4 File Offset: 0x001AADD4
		private void LoadCustomPSSnapIn(PSSnapInInfo psSnapInInfo)
		{
			if (psSnapInInfo == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(psSnapInInfo.CustomPSSnapInType))
			{
				return;
			}
			Dictionary<string, SessionStateCmdletEntry> dictionary = null;
			Dictionary<string, SessionStateProviderEntry> dictionary2 = null;
			InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for mshsnapin {0}", new object[]
			{
				psSnapInInfo.Name
			});
			Assembly assembly = PSSnapInHelpers.LoadPSSnapInAssembly(psSnapInInfo, out dictionary, out dictionary2);
			if (assembly == null)
			{
				InitialSessionState._PSSnapInTracer.TraceError("Loading assembly for mshsnapin {0} failed", new object[]
				{
					psSnapInInfo.Name
				});
				return;
			}
			CustomPSSnapIn customPSSnapIn = null;
			try
			{
				Type type = assembly.GetType(psSnapInInfo.CustomPSSnapInType, true);
				if (type != null)
				{
					customPSSnapIn = (CustomPSSnapIn)assembly.CreateInstance(psSnapInInfo.CustomPSSnapInType);
				}
				InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for mshsnapin {0} succeeded", new object[]
				{
					psSnapInInfo.Name
				});
			}
			catch (TypeLoadException ex)
			{
				throw new PSSnapInException(psSnapInInfo.Name, ex.Message);
			}
			catch (ArgumentException ex2)
			{
				throw new PSSnapInException(psSnapInInfo.Name, ex2.Message);
			}
			catch (MissingMethodException ex3)
			{
				throw new PSSnapInException(psSnapInInfo.Name, ex3.Message);
			}
			catch (InvalidCastException ex4)
			{
				throw new PSSnapInException(psSnapInInfo.Name, ex4.Message);
			}
			catch (TargetInvocationException ex5)
			{
				if (ex5.InnerException != null)
				{
					throw new PSSnapInException(psSnapInInfo.Name, ex5.InnerException.Message);
				}
				throw new PSSnapInException(psSnapInInfo.Name, ex5.Message);
			}
			this.MergeCustomPSSnapIn(psSnapInInfo, customPSSnapIn);
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x001ACD7C File Offset: 0x001AAF7C
		private void MergeCustomPSSnapIn(PSSnapInInfo psSnapInInfo, CustomPSSnapIn customPSSnapIn)
		{
			if (psSnapInInfo == null || customPSSnapIn == null)
			{
				return;
			}
			InitialSessionState._PSSnapInTracer.WriteLine("Merging configuration from custom mshsnapin {0}", new object[]
			{
				psSnapInInfo.Name
			});
			if (customPSSnapIn.Cmdlets != null)
			{
				foreach (CmdletConfigurationEntry cmdletConfigurationEntry in customPSSnapIn.Cmdlets)
				{
					SessionStateCmdletEntry sessionStateCmdletEntry = new SessionStateCmdletEntry(cmdletConfigurationEntry.Name, cmdletConfigurationEntry.ImplementingType, cmdletConfigurationEntry.HelpFileName);
					sessionStateCmdletEntry.SetPSSnapIn(psSnapInInfo);
					this.Commands.Add(sessionStateCmdletEntry);
				}
			}
			if (customPSSnapIn.Providers != null)
			{
				foreach (ProviderConfigurationEntry providerConfigurationEntry in customPSSnapIn.Providers)
				{
					SessionStateProviderEntry sessionStateProviderEntry = new SessionStateProviderEntry(providerConfigurationEntry.Name, providerConfigurationEntry.ImplementingType, providerConfigurationEntry.HelpFileName);
					sessionStateProviderEntry.SetPSSnapIn(psSnapInInfo);
					this.Providers.Add(sessionStateProviderEntry);
				}
			}
			if (customPSSnapIn.Types != null)
			{
				foreach (TypeConfigurationEntry typeConfigurationEntry in customPSSnapIn.Types)
				{
					string fileName = Path.Combine(psSnapInInfo.ApplicationBase, typeConfigurationEntry.FileName);
					SessionStateTypeEntry sessionStateTypeEntry = new SessionStateTypeEntry(fileName);
					sessionStateTypeEntry.SetPSSnapIn(psSnapInInfo);
					this.Types.Add(sessionStateTypeEntry);
				}
			}
			if (customPSSnapIn.Formats != null)
			{
				foreach (FormatConfigurationEntry formatConfigurationEntry in customPSSnapIn.Formats)
				{
					string fileName2 = Path.Combine(psSnapInInfo.ApplicationBase, formatConfigurationEntry.FileName);
					SessionStateFormatEntry sessionStateFormatEntry = new SessionStateFormatEntry(fileName2);
					sessionStateFormatEntry.SetPSSnapIn(psSnapInInfo);
					this.Formats.Add(sessionStateFormatEntry);
				}
			}
			SessionStateAssemblyEntry item = new SessionStateAssemblyEntry(psSnapInInfo.AssemblyName, psSnapInInfo.AbsoluteModulePath);
			this.Assemblies.Add(item);
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x001ACF9C File Offset: 0x001AB19C
		internal static Assembly LoadAssemblyFromFile(string fileName)
		{
			InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for psSnapIn {0}", new object[]
			{
				fileName
			});
			Assembly assembly = ClrFacade.LoadFrom(fileName);
			if (assembly == null)
			{
				InitialSessionState._PSSnapInTracer.TraceError("Loading assembly for psSnapIn {0} failed", new object[]
				{
					fileName
				});
				return null;
			}
			InitialSessionState._PSSnapInTracer.WriteLine("Loading assembly for psSnapIn {0} succeeded", new object[]
			{
				fileName
			});
			return assembly;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x001AD010 File Offset: 0x001AB210
		internal void ImportCmdletsFromAssembly(Assembly assembly, PSModuleInfo module)
		{
			if (assembly == null)
			{
				ArgumentNullException ex = new ArgumentNullException("assembly");
				throw ex;
			}
			Dictionary<string, SessionStateCmdletEntry> dictionary = null;
			Dictionary<string, List<SessionStateAliasEntry>> dictionary2 = null;
			Dictionary<string, SessionStateProviderEntry> dictionary3 = null;
			string assemblyLocation = ClrFacade.GetAssemblyLocation(assembly);
			string text = null;
			PSSnapInHelpers.AnalyzePSSnapInAssembly(assembly, assemblyLocation, null, module, true, out dictionary, out dictionary2, out dictionary3, out text);
			SessionStateAssemblyEntry item = new SessionStateAssemblyEntry(assembly.FullName, assemblyLocation);
			this.Assemblies.Add(item);
			if (dictionary != null)
			{
				foreach (SessionStateCmdletEntry item2 in dictionary.Values)
				{
					this.Commands.Add(item2);
				}
			}
			if (dictionary2 != null)
			{
				foreach (List<SessionStateAliasEntry> list in dictionary2.Values)
				{
					foreach (SessionStateAliasEntry item3 in list)
					{
						this.Commands.Add(item3);
					}
				}
			}
			if (dictionary3 != null)
			{
				foreach (SessionStateProviderEntry item4 in dictionary3.Values)
				{
					this.Providers.Add(item4);
				}
			}
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x001AD198 File Offset: 0x001AB398
		internal static string GetHelpPagingFunctionText()
		{
			CommandMetadata commandMetadata = new CommandMetadata(typeof(GetHelpCommand));
			return string.Format(CultureInfo.InvariantCulture, "\r\n<#\r\n.FORWARDHELPTARGETNAME Get-Help\r\n.FORWARDHELPCATEGORY Cmdlet \r\n#>\r\n{0}\r\nparam({1})\r\n\r\n      #Set the outputencoding to Console::OutputEncoding. More.com doesn't work well with Unicode.\r\n      $outputEncoding=[System.Console]::OutputEncoding\r\n\r\n      Get-Help @PSBoundParameters | more\r\n", new object[]
			{
				commandMetadata.GetDecl(),
				commandMetadata.GetParamBlock()
			});
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001AD1DE File Offset: 0x001AB3DE
		internal static string GetMkdirFunctionText()
		{
			return "\r\n<# \r\n.FORWARDHELPTARGETNAME New-Item\r\n.FORWARDHELPCATEGORY Cmdlet \r\n#>\r\n\r\n[CmdletBinding(DefaultParameterSetName='pathSet',\r\n    SupportsShouldProcess=$true,\r\n    SupportsTransactions=$true,\r\n    ConfirmImpact='Medium')]\r\n    [OutputType([System.IO.DirectoryInfo])]\r\nparam(\r\n    [Parameter(ParameterSetName='nameSet', Position=0, ValueFromPipelineByPropertyName=$true)]\r\n    [Parameter(ParameterSetName='pathSet', Mandatory=$true, Position=0, ValueFromPipelineByPropertyName=$true)]\r\n    [System.String[]]\r\n    ${Path},\r\n\r\n    [Parameter(ParameterSetName='nameSet', Mandatory=$true, ValueFromPipelineByPropertyName=$true)]\r\n    [AllowNull()]\r\n    [AllowEmptyString()]\r\n    [System.String]\r\n    ${Name},\r\n\r\n    [Parameter(ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]\r\n    [System.Object]\r\n    ${Value},\r\n\r\n    [Switch]\r\n    ${Force},\r\n\r\n    [Parameter(ValueFromPipelineByPropertyName=$true)]\r\n    [System.Management.Automation.PSCredential]\r\n    ${Credential}\r\n)\r\n\r\nbegin {\r\n\r\n    try {\r\n        $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('New-Item', [System.Management.Automation.CommandTypes]::Cmdlet)\r\n        $scriptCmd = {& $wrappedCmd -Type Directory @PSBoundParameters }\r\n        $steppablePipeline = $scriptCmd.GetSteppablePipeline()\r\n        $steppablePipeline.Begin($PSCmdlet)\r\n    } catch {\r\n        throw\r\n    }\r\n\r\n}\r\n\r\nprocess {\r\n\r\n    try {\r\n        $steppablePipeline.Process($_)\r\n    } catch {\r\n        throw\r\n    }\r\n\r\n}\r\n\r\nend {\r\n\r\n    try {\r\n        $steppablePipeline.End()\r\n    } catch {\r\n        throw\r\n    }\r\n\r\n}\r\n\r\n";
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x001AD1E5 File Offset: 0x001AB3E5
		internal static string GetGetVerbText()
		{
			return "\r\nparam(\r\n    [Parameter(ValueFromPipeline=$true)]\r\n    [string[]]\r\n    $verb = '*'\r\n)\r\nbegin {\r\n    $allVerbs = [System.Reflection.IntrospectionExtensions]::GetTypeInfo([PSObject]).Assembly.ExportedTypes |\r\n        Microsoft.PowerShell.Core\\Where-Object {$_.Name -match '^Verbs.'} |\r\n        Microsoft.PowerShell.Utility\\Get-Member -type Properties -static |\r\n        Microsoft.PowerShell.Utility\\Select-Object @{\r\n            Name='Verb'\r\n            Expression = {$_.Name}\r\n        }, @{\r\n            Name='Group'\r\n            Expression = {\r\n                $str = \"$($_.TypeName)\"\r\n                $str.Substring($str.LastIndexOf('Verbs') + 5)\r\n            }\r\n        }\r\n}\r\nprocess {\r\n    foreach ($v in $verb) {\r\n        $allVerbs | Microsoft.PowerShell.Core\\Where-Object { $_.Verb -like $v }\r\n    }\r\n}\r\n# .Link\r\n# http://go.microsoft.com/fwlink/?LinkID=160712\r\n# .ExternalHelp System.Management.Automation.dll-help.xml\r\n";
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x001AD1EC File Offset: 0x001AB3EC
		internal static string GetOSTFunctionText()
		{
			return "\r\n[CmdletBinding()]\r\nparam(\r\n    [ValidateRange(2, 2147483647)]\r\n    [int]\r\n    ${Width},\r\n\r\n    [Parameter(ValueFromPipeline=$true)]\r\n    [psobject]\r\n    ${InputObject})\r\n\r\nbegin\r\n{\r\n    try {\r\n        $PSBoundParameters['Stream'] = $true\r\n        $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Out-String',[System.Management.Automation.CommandTypes]::Cmdlet)\r\n        $scriptCmd = {& $wrappedCmd @PSBoundParameters }\r\n        $steppablePipeline = $scriptCmd.GetSteppablePipeline($myInvocation.CommandOrigin)\r\n        $steppablePipeline.Begin($PSCmdlet)\r\n    } catch {\r\n        throw\r\n    }\r\n}\r\n\r\nprocess\r\n{\r\n    try {\r\n        $steppablePipeline.Process($_)\r\n    } catch {\r\n        throw\r\n    }\r\n}\r\n\r\nend\r\n{\r\n    try {\r\n        $steppablePipeline.End()\r\n    } catch {\r\n        throw\r\n    }\r\n}\r\n<#\r\n.ForwardHelpTargetName Out-String\r\n.ForwardHelpCategory Cmdlet \r\n#>\r\n";
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x001AD1F4 File Offset: 0x001AB3F4
		internal static SessionStateAliasEntry[] BuiltInAliases
		{
			get
			{
				return new SessionStateAliasEntry[]
				{
					new SessionStateAliasEntry("foreach", "ForEach-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("%", "ForEach-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("where", "Where-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("?", "Where-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ac", "Add-Content", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("clc", "Clear-Content", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cli", "Clear-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("clp", "Clear-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("clv", "Clear-Variable", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("compare", "Compare-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cpi", "Copy-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cpp", "Copy-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cvpa", "Convert-Path", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("dbp", "Disable-PSBreakpoint", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("diff", "Compare-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ebp", "Enable-PSBreakpoint", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("epal", "Export-Alias", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("epcsv", "Export-Csv", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("fc", "Format-Custom", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("fl", "Format-List", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ft", "Format-Table", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("fw", "Format-Wide", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gal", "Get-Alias", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gbp", "Get-PSBreakpoint", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gc", "Get-Content", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gci", "Get-ChildItem", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gcm", "Get-Command", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gdr", "Get-PSDrive", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gcs", "Get-PSCallStack", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ghy", "Get-History", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gi", "Get-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gl", "Get-Location", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gm", "Get-Member", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gmo", "Get-Module", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gp", "Get-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gpv", "Get-ItemPropertyValue", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gps", "Get-Process", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("group", "Group-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gsv", "Get-Service", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gu", "Get-Unique", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gv", "Get-Variable", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("iex", "Invoke-Expression", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ihy", "Invoke-History", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ii", "Invoke-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ipmo", "Import-Module", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ipal", "Import-Alias", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ipcsv", "Import-Csv", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("measure", "Measure-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("mi", "Move-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("mp", "Move-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("nal", "New-Alias", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ndr", "New-PSDrive", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ni", "New-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("nv", "New-Variable", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("nmo", "New-Module", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("oh", "Out-Host", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rbp", "Remove-PSBreakpoint", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rdr", "Remove-PSDrive", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ri", "Remove-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rni", "Rename-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rnp", "Rename-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rp", "Remove-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rmo", "Remove-Module", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rv", "Remove-Variable", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rvpa", "Resolve-Path", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sal", "Set-Alias", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sasv", "Start-Service", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sbp", "Set-PSBreakpoint", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sc", "Set-Content", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("select", "Select-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("si", "Set-Item", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sl", "Set-Location", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sleep", "Start-Sleep", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sort", "Sort-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sp", "Set-ItemProperty", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("saps", "Start-Process", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("start", "Start-Process", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("spps", "Stop-Process", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("spsv", "Stop-Service", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sv", "Set-Variable", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("tee", "Tee-Object", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("write", "Write-Output", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("asnp", "Add-PSSnapIn", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gsnp", "Get-PSSnapIn", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gwmi", "Get-WmiObject", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("irm", "Invoke-RestMethod", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("iwr", "Invoke-WebRequest", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("iwmi", "Invoke-WMIMethod", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ogv", "Out-GridView", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ise", "powershell_ise.exe", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rsnp", "Remove-PSSnapin", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rwmi", "Remove-WMIObject", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("swmi", "Set-WMIInstance", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("shcm", "Show-Command", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("trcm", "Trace-Command", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cat", "Get-Content", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cd", "Set-Location", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("clear", "Clear-Host", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cp", "Copy-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("h", "Get-History", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("history", "Get-History", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("kill", "Stop-Process", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("lp", "Out-Printer", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ls", "Get-ChildItem", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("man", "help", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("mount", "New-PSDrive", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("md", "mkdir", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("mv", "Move-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("popd", "Pop-Location", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ps", "Get-Process", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("pushd", "Push-Location", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("pwd", "Get-Location", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("r", "Invoke-History", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rm", "Remove-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rmdir", "Remove-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("echo", "Write-Output", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cls", "Clear-Host", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("chdir", "Set-Location", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("copy", "Copy-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("del", "Remove-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("dir", "Get-ChildItem", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("erase", "Remove-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("move", "Move-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rd", "Remove-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ren", "Rename-Item", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("set", "Set-Variable", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("type", "Get-Content", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("icm", "Invoke-Command", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("clhy", "Clear-History", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gjb", "Get-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rcjb", "Receive-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rjb", "Remove-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sajb", "Start-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("spjb", "Stop-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("wjb", "Wait-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sujb", "Suspend-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rujb", "Resume-Job", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("cnsn", "Connect-PSSession", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("dnsn", "Disconnect-PSSession", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("nsn", "New-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("npssc", "New-PSSessionConfigurationFile", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("gsn", "Get-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rsn", "Remove-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("ipsn", "Import-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("epsn", "Export-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("etsn", "Enter-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("rcsn", "Receive-PSSession", "", ScopedItemOptions.ReadOnly | ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("exsn", "Exit-PSSession", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("sls", "Select-String", "", ScopedItemOptions.None),
					new SessionStateAliasEntry("wget", "Invoke-WebRequest", "", ScopedItemOptions.AllScope),
					new SessionStateAliasEntry("curl", "Invoke-WebRequest", "", ScopedItemOptions.AllScope)
				};
			}
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x001AE16C File Offset: 0x001AC36C
		internal static void RemoveAllDrivesForProvider(ProviderInfo pi, SessionStateInternal ssi)
		{
			foreach (PSDriveInfo drive in ssi.GetDrivesForProvider(pi.FullName))
			{
				try
				{
					ssi.RemoveDrive(drive, true, null);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x001AE1D8 File Offset: 0x001AC3D8
		internal static string GetNestedModuleDllName(string moduleName)
		{
			string result = null;
			if (!InitialSessionState.EngineModuleNestedModuleMapping.TryGetValue(moduleName, out result))
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x001AE200 File Offset: 0x001AC400
		internal void SaveAsConsoleFile(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!path.EndsWith(".psc1", StringComparison.OrdinalIgnoreCase))
			{
				throw PSTraceSource.NewArgumentException("path", ConsoleInfoErrorStrings.BadConsoleExtension, new object[0]);
			}
			PSConsoleFileElement.WriteToFile(path, PSVersionInfo.PSVersion, this.ImportedSnapins.Values);
		}

		// Token: 0x0400290C RID: 10508
		internal const string FormatEnumerationLimit = "FormatEnumerationLimit";

		// Token: 0x0400290D RID: 10509
		internal const int DefaultFormatEnumerationLimit = 4;

		// Token: 0x0400290E RID: 10510
		internal const ActionPreference defaultDebugPreference = ActionPreference.SilentlyContinue;

		// Token: 0x0400290F RID: 10511
		internal const ActionPreference defaultErrorActionPreference = ActionPreference.Continue;

		// Token: 0x04002910 RID: 10512
		internal const ActionPreference defaultProgressPreference = ActionPreference.Continue;

		// Token: 0x04002911 RID: 10513
		internal const ActionPreference defaultVerbosePreference = ActionPreference.SilentlyContinue;

		// Token: 0x04002912 RID: 10514
		internal const ActionPreference defaultWarningPreference = ActionPreference.Continue;

		// Token: 0x04002913 RID: 10515
		internal const ActionPreference defaultInformationPreference = ActionPreference.SilentlyContinue;

		// Token: 0x04002914 RID: 10516
		internal const bool defaultWhatIfPreference = false;

		// Token: 0x04002915 RID: 10517
		internal const ConfirmImpact defaultConfirmPreference = ConfirmImpact.High;

		// Token: 0x04002916 RID: 10518
		internal const string DefaultPromptContent = "PS $($executionContext.SessionState.Path.CurrentLocation)$('>' * ($nestedPromptLevel + 1)) ";

		// Token: 0x04002917 RID: 10519
		internal const string DefaultPromptComments = "# .Link\n# http://go.microsoft.com/fwlink/?LinkID=225750\n# .ExternalHelp System.Management.Automation.dll-help.xml\n";

		// Token: 0x04002918 RID: 10520
		private static string[] PSCoreFormatFileNames = new string[]
		{
			"Certificate.format.ps1xml",
			"Diagnostics.Format.ps1xml",
			"DotNetTypes.format.ps1xml",
			"Event.Format.ps1xml",
			"FileSystem.format.ps1xml",
			"Help.format.ps1xml",
			"HelpV3.format.ps1xml",
			"PowerShellCore.format.ps1xml",
			"PowerShellTrace.format.ps1xml",
			"Registry.format.ps1xml",
			"WSMan.Format.ps1xml"
		};

		// Token: 0x04002919 RID: 10521
		private static List<string> allowedAliases = new List<string>
		{
			"compare",
			"diff",
			"%",
			"foreach",
			"exsn",
			"fc",
			"fl",
			"ft",
			"fw",
			"gcm",
			"gjb",
			"gmo",
			"gv",
			"group",
			"ipmo",
			"measure",
			"rv",
			"rcjb",
			"rjb",
			"rmo",
			"rujb",
			"select",
			"set",
			"sv",
			"sort",
			"spjb",
			"sujb",
			"wjb",
			"?",
			"where"
		};

		// Token: 0x0400291A RID: 10522
		private static readonly string[] JobCmdlets = new string[]
		{
			"Get-Job",
			"Stop-Job",
			"Wait-Job",
			"Suspend-Job",
			"Resume-Job",
			"Remove-Job",
			"Receive-Job"
		};

		// Token: 0x0400291B RID: 10523
		private static readonly string[] ImplicitRemotingCmdlets = new string[]
		{
			"Get-Command",
			"Select-Object",
			"Measure-Object",
			"Get-Help",
			"Get-FormatData",
			"Exit-PSSession",
			"Out-Default"
		};

		// Token: 0x0400291C RID: 10524
		private static readonly string[] AutoDiscoveryCmdlets = new string[]
		{
			"Get-Module"
		};

		// Token: 0x0400291D RID: 10525
		private static readonly string[] LanguageHelperCmdlets = new string[]
		{
			"Compare-Object",
			"ForEach-Object",
			"Group-Object",
			"Sort-Object",
			"Where-Object",
			"Out-File",
			"Out-Null",
			"Out-String",
			"Format-Custom",
			"Format-List",
			"Format-Table",
			"Format-Wide",
			"Remove-Module",
			"Get-Variable",
			"Set-Variable",
			"Remove-Variable",
			"Get-Credential",
			"Set-StrictMode"
		};

		// Token: 0x0400291E RID: 10526
		private static readonly string[] DebugCmdlets = new string[]
		{
			"Disable-PSBreakpoint",
			"Enable-PSBreakpoint",
			"Get-PSBreakpoint",
			"Remove-PSBreakpoint",
			"Set-PSBreakpoint"
		};

		// Token: 0x0400291F RID: 10527
		private static readonly string[] MiscCmdlets = new string[]
		{
			"Join-Path",
			"Import-Module"
		};

		// Token: 0x04002920 RID: 10528
		private static readonly string[] MiscCommands = new string[]
		{
			"TabExpansion2"
		};

		// Token: 0x04002921 RID: 10529
		private static readonly string[] DefaultTypeFiles = new string[]
		{
			"types.ps1xml",
			"typesv3.ps1xml"
		};

		// Token: 0x04002922 RID: 10530
		private PSLanguageMode _languageMode = PSLanguageMode.NoLanguage;

		// Token: 0x04002923 RID: 10531
		private string _transcriptDirectory;

		// Token: 0x04002924 RID: 10532
		private ExecutionPolicy _executionPolicy = ExecutionPolicy.Restricted;

		// Token: 0x04002925 RID: 10533
		private bool _wasExecutionPolicySet;

		// Token: 0x04002926 RID: 10534
		private bool _useFullLanguageModeInDebugger;

		// Token: 0x04002927 RID: 10535
		private ApartmentState apartmentState = ApartmentState.Unknown;

		// Token: 0x04002928 RID: 10536
		private PSThreadOptions createThreadOptions;

		// Token: 0x04002929 RID: 10537
		private bool throwOnRunspaceOpenError;

		// Token: 0x0400292A RID: 10538
		internal bool RefreshTypeAndFormatSetting;

		// Token: 0x0400292B RID: 10539
		private AuthorizationManager _authorizationManager = new PSAuthorizationManager(Utils.DefaultPowerShellShellID);

		// Token: 0x0400292C RID: 10540
		internal PSHost Host;

		// Token: 0x0400292D RID: 10541
		private Collection<ModuleSpecification> _moduleSpecificationsToImport = new Collection<ModuleSpecification>();

		// Token: 0x0400292E RID: 10542
		private HashSet<string> _coreModulesToImport = new HashSet<string>();

		// Token: 0x0400292F RID: 10543
		private Dictionary<string, PSSnapInInfo> _importedSnapins = new Dictionary<string, PSSnapInInfo>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002930 RID: 10544
		private InitialSessionStateEntryCollection<SessionStateAssemblyEntry> _assemblies;

		// Token: 0x04002931 RID: 10545
		private InitialSessionStateEntryCollection<SessionStateTypeEntry> _types;

		// Token: 0x04002932 RID: 10546
		private InitialSessionStateEntryCollection<SessionStateFormatEntry> _formats;

		// Token: 0x04002933 RID: 10547
		private InitialSessionStateEntryCollection<SessionStateProviderEntry> _providers;

		// Token: 0x04002934 RID: 10548
		private InitialSessionStateEntryCollection<SessionStateCommandEntry> _commands;

		// Token: 0x04002935 RID: 10549
		private HashSet<string> _unresolvedCommandsToExpose;

		// Token: 0x04002936 RID: 10550
		private Dictionary<string, Hashtable> _commandModifications;

		// Token: 0x04002937 RID: 10551
		private List<Hashtable> _dynamicVariablesToDefine;

		// Token: 0x04002938 RID: 10552
		private InitialSessionStateEntryCollection<SessionStateVariableEntry> _variables;

		// Token: 0x04002939 RID: 10553
		private InitialSessionStateEntryCollection<SessionStateVariableEntry> _environmentVariables;

		// Token: 0x0400293A RID: 10554
		private HashSet<string> _startupScripts = new HashSet<string>();

		// Token: 0x0400293B RID: 10555
		private object _syncObject = new object();

		// Token: 0x0400293C RID: 10556
		private static string TabExpansionFunctionText = "\r\n<# Options include:\r\n     RelativeFilePaths - [bool]\r\n         Always resolve file paths using Resolve-Path -Relative.\r\n         The default is to use some heuristics to guess if relative or absolute is better.\r\n\r\n   To customize your own custom options, pass a hashtable to CompleteInput, e.g.\r\n         return [System.Management.Automation.CommandCompletion]::CompleteInput($inputScript, $cursorColumn,\r\n             @{ RelativeFilePaths=$false } \r\n#>\r\n\r\n[CmdletBinding(DefaultParameterSetName = 'ScriptInputSet')]\r\nParam(\r\n    [Parameter(ParameterSetName = 'ScriptInputSet', Mandatory = $true, Position = 0)]\r\n    [string] $inputScript,\r\n    \r\n    [Parameter(ParameterSetName = 'ScriptInputSet', Mandatory = $true, Position = 1)]\r\n    [int] $cursorColumn,\r\n\r\n    [Parameter(ParameterSetName = 'AstInputSet', Mandatory = $true, Position = 0)]\r\n    [System.Management.Automation.Language.Ast] $ast,\r\n\r\n    [Parameter(ParameterSetName = 'AstInputSet', Mandatory = $true, Position = 1)]\r\n    [System.Management.Automation.Language.Token[]] $tokens,\r\n\r\n    [Parameter(ParameterSetName = 'AstInputSet', Mandatory = $true, Position = 2)]\r\n    [System.Management.Automation.Language.IScriptPosition] $positionOfCursor,\r\n    \r\n    [Parameter(ParameterSetName = 'ScriptInputSet', Position = 2)]\r\n    [Parameter(ParameterSetName = 'AstInputSet', Position = 3)]\r\n    [Hashtable] $options = $null\r\n)\r\n\r\nEnd\r\n{\r\n    if ($psCmdlet.ParameterSetName -eq 'ScriptInputSet')\r\n    {\r\n        return [System.Management.Automation.CommandCompletion]::CompleteInput(\r\n            <#inputScript#>  $inputScript,\r\n            <#cursorColumn#> $cursorColumn,\r\n            <#options#>      $options)\r\n    }\r\n    else\r\n    {\r\n        return [System.Management.Automation.CommandCompletion]::CompleteInput(\r\n            <#ast#>              $ast,\r\n            <#tokens#>           $tokens,\r\n            <#positionOfCursor#> $positionOfCursor,\r\n            <#options#>          $options)\r\n    }\r\n}\r\n        ";

		// Token: 0x0400293D RID: 10557
		private static string ImportSystemModulesText = "";

		// Token: 0x0400293E RID: 10558
		internal static SessionStateVariableEntry[] BuiltInVariables = new SessionStateVariableEntry[]
		{
			new SessionStateVariableEntry("$", null, string.Empty),
			new SessionStateVariableEntry("^", null, string.Empty),
			new SessionStateVariableEntry("StackTrace", null, string.Empty),
			new SessionStateVariableEntry("OutputEncoding", Encoding.ASCII, RunspaceInit.OutputEncodingDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(Encoding)
			})),
			new SessionStateVariableEntry("ConfirmPreference", ConfirmImpact.High, RunspaceInit.ConfirmPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ConfirmImpact)
			})),
			new SessionStateVariableEntry("DebugPreference", ActionPreference.SilentlyContinue, RunspaceInit.DebugPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("ErrorActionPreference", ActionPreference.Continue, RunspaceInit.ErrorActionPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("ProgressPreference", ActionPreference.Continue, RunspaceInit.ProgressPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("VerbosePreference", ActionPreference.SilentlyContinue, RunspaceInit.VerbosePreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("WarningPreference", ActionPreference.Continue, RunspaceInit.WarningPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("InformationPreference", ActionPreference.SilentlyContinue, RunspaceInit.InformationPreferenceDescription, ScopedItemOptions.None, new ArgumentTypeConverterAttribute(new Type[]
			{
				typeof(ActionPreference)
			})),
			new SessionStateVariableEntry("ErrorView", "NormalView", RunspaceInit.ErrorViewDescription),
			new SessionStateVariableEntry("NestedPromptLevel", 0, RunspaceInit.NestedPromptLevelDescription),
			new SessionStateVariableEntry("WhatIfPreference", false, RunspaceInit.WhatIfPreferenceDescription),
			new SessionStateVariableEntry("FormatEnumerationLimit", 4, RunspaceInit.FormatEnunmerationLimitDescription),
			new SessionStateVariableEntry("PSEmailServer", string.Empty, RunspaceInit.PSEmailServerDescription),
			new SessionStateVariableEntry("PSSessionOption", new PSSessionOption(), RemotingErrorIdStrings.PSDefaultSessionOptionDescription, ScopedItemOptions.None),
			new SessionStateVariableEntry("PSSessionConfigurationName", "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", RemotingErrorIdStrings.PSSessionConfigurationName, ScopedItemOptions.None),
			new SessionStateVariableEntry("PSSessionApplicationName", "wsman", RemotingErrorIdStrings.PSSessionAppName, ScopedItemOptions.None)
		};

		// Token: 0x0400293F RID: 10559
		internal static string DefaultPromptString = "\"PS $($executionContext.SessionState.Path.CurrentLocation)$('>' * ($nestedPromptLevel + 1)) \"\n# .Link\n# http://go.microsoft.com/fwlink/?LinkID=225750\n# .ExternalHelp System.Management.Automation.dll-help.xml\n";

		// Token: 0x04002940 RID: 10560
		internal static SessionStateFunctionEntry[] BuiltInFunctions = new SessionStateFunctionEntry[]
		{
			new SessionStateFunctionEntry("prompt", InitialSessionState.DefaultPromptString),
			new SessionStateFunctionEntry("TabExpansion2", InitialSessionState.TabExpansionFunctionText),
			new SessionStateFunctionEntry("Clear-Host", "$space = New-Object System.Management.Automation.Host.BufferCell\n$space.Character = ' '\n$space.ForegroundColor = $host.ui.rawui.ForegroundColor\n$space.BackgroundColor = $host.ui.rawui.BackgroundColor\n$rect = New-Object System.Management.Automation.Host.Rectangle\n$rect.Top = $rect.Bottom = $rect.Right = $rect.Left = -1\n$origin = New-Object System.Management.Automation.Host.Coordinates\n$Host.UI.RawUI.CursorPosition = $origin\n$Host.UI.RawUI.SetBufferContents($rect, $space)\n# .Link\n# http://go.microsoft.com/fwlink/?LinkID=225747\n# .ExternalHelp System.Management.Automation.dll-help.xml\n"),
			new SessionStateFunctionEntry("more", "param([string[]]$paths)\n\n$OutputEncoding = [System.Console]::OutputEncoding\n\nif($paths)\n{\n    foreach ($file in $paths)\n    {\n        Get-Content $file | more.com\n    }\n}\nelse\n{\n    $input | more.com\n}\n"),
			new SessionStateFunctionEntry("help", InitialSessionState.GetHelpPagingFunctionText()),
			new SessionStateFunctionEntry("mkdir", InitialSessionState.GetMkdirFunctionText()),
			new SessionStateFunctionEntry("Get-Verb", InitialSessionState.GetGetVerbText()),
			new SessionStateFunctionEntry("oss", InitialSessionState.GetOSTFunctionText()),
			new SessionStateFunctionEntry("A:", "Set-Location A:"),
			new SessionStateFunctionEntry("B:", "Set-Location B:"),
			new SessionStateFunctionEntry("C:", "Set-Location C:"),
			new SessionStateFunctionEntry("D:", "Set-Location D:"),
			new SessionStateFunctionEntry("E:", "Set-Location E:"),
			new SessionStateFunctionEntry("F:", "Set-Location F:"),
			new SessionStateFunctionEntry("G:", "Set-Location G:"),
			new SessionStateFunctionEntry("H:", "Set-Location H:"),
			new SessionStateFunctionEntry("I:", "Set-Location I:"),
			new SessionStateFunctionEntry("J:", "Set-Location J:"),
			new SessionStateFunctionEntry("K:", "Set-Location K:"),
			new SessionStateFunctionEntry("L:", "Set-Location L:"),
			new SessionStateFunctionEntry("M:", "Set-Location M:"),
			new SessionStateFunctionEntry("N:", "Set-Location N:"),
			new SessionStateFunctionEntry("O:", "Set-Location O:"),
			new SessionStateFunctionEntry("P:", "Set-Location P:"),
			new SessionStateFunctionEntry("Q:", "Set-Location Q:"),
			new SessionStateFunctionEntry("R:", "Set-Location R:"),
			new SessionStateFunctionEntry("S:", "Set-Location S:"),
			new SessionStateFunctionEntry("T:", "Set-Location T:"),
			new SessionStateFunctionEntry("U:", "Set-Location U:"),
			new SessionStateFunctionEntry("V:", "Set-Location V:"),
			new SessionStateFunctionEntry("W:", "Set-Location W:"),
			new SessionStateFunctionEntry("X:", "Set-Location X:"),
			new SessionStateFunctionEntry("Y:", "Set-Location Y:"),
			new SessionStateFunctionEntry("Z:", "Set-Location Z:"),
			new SessionStateFunctionEntry("cd..", "Set-Location .."),
			new SessionStateFunctionEntry("cd\\", "Set-Location \\"),
			new SessionStateFunctionEntry("ImportSystemModules", InitialSessionState.ImportSystemModulesText),
			new SessionStateFunctionEntry("Pause", string.Format(CultureInfo.InvariantCulture, "Read-Host '{0}' | Out-Null", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(RunspaceInit.PauseDefinitionString)
			}))
		};

		// Token: 0x04002941 RID: 10561
		private static PSTraceSource _PSSnapInTracer = PSTraceSource.GetTracer("PSSnapInLoadUnload", "Loading and unloading mshsnapins", false);

		// Token: 0x04002942 RID: 10562
		internal static string CoreSnapin = "Microsoft.PowerShell.Core";

		// Token: 0x04002943 RID: 10563
		internal static string CoreModule = "Microsoft.PowerShell.Core";

		// Token: 0x04002944 RID: 10564
		internal Collection<PSSnapInInfo> defaultSnapins = new Collection<PSSnapInInfo>();

		// Token: 0x04002945 RID: 10565
		internal static HashSet<string> EngineModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Microsoft.PowerShell.Utility",
			"Microsoft.PowerShell.Management",
			"Microsoft.PowerShell.Diagnostics",
			"Microsoft.PowerShell.Host",
			"Microsoft.PowerShell.Security",
			"Microsoft.WSMan.Management"
		};

		// Token: 0x04002946 RID: 10566
		internal static HashSet<string> NestedEngineModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Microsoft.PowerShell.Commands.Utility",
			"Microsoft.PowerShell.Commands.Management",
			"Microsoft.PowerShell.Commands.Diagnostics",
			"Microsoft.PowerShell.ConsoleHost"
		};

		// Token: 0x04002947 RID: 10567
		internal static Dictionary<string, string> EngineModuleNestedModuleMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"Microsoft.PowerShell.Utility",
				"Microsoft.PowerShell.Commands.Utility"
			},
			{
				"Microsoft.PowerShell.Management",
				"Microsoft.PowerShell.Commands.Management"
			},
			{
				"Microsoft.PowerShell.Diagnostics",
				"Microsoft.PowerShell.Commands.Diagnostics"
			},
			{
				"Microsoft.PowerShell.Host",
				"Microsoft.PowerShell.ConsoleHost"
			}
		};

		// Token: 0x04002948 RID: 10568
		internal static Dictionary<string, string> NestedModuleEngineModuleMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"Microsoft.PowerShell.Commands.Utility",
				"Microsoft.PowerShell.Utility"
			},
			{
				"Microsoft.PowerShell.Commands.Management",
				"Microsoft.PowerShell.Management"
			},
			{
				"Microsoft.PowerShell.Commands.Diagnostics",
				"Microsoft.PowerShell.Diagnostics"
			},
			{
				"Microsoft.PowerShell.ConsoleHost",
				"Microsoft.PowerShell.Host"
			},
			{
				"Microsoft.PowerShell.Security",
				"Microsoft.PowerShell.Security"
			},
			{
				"Microsoft.WSMan.Management",
				"Microsoft.WSMan.Management"
			}
		};

		// Token: 0x04002949 RID: 10569
		internal static HashSet<string> ConstantEngineModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			InitialSessionState.CoreModule
		};

		// Token: 0x0400294A RID: 10570
		internal static HashSet<string> ConstantEngineNestedModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"System.Management.Automation"
		};
	}
}
