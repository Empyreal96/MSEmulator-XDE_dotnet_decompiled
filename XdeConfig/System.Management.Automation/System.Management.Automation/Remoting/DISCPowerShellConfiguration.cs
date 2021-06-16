using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000371 RID: 881
	internal sealed class DISCPowerShellConfiguration : PSSessionConfiguration
	{
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06002B48 RID: 11080 RVA: 0x000EE9DC File Offset: 0x000ECBDC
		internal Hashtable ConfigHash
		{
			get
			{
				return this.configHash;
			}
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000EE9E8 File Offset: 0x000ECBE8
		internal DISCPowerShellConfiguration(string configFile, Func<string, bool> roleVerifier)
		{
			this.configFile = configFile;
			if (roleVerifier == null)
			{
				roleVerifier = ((string role) => false);
			}
			Runspace defaultRunspace = Runspace.DefaultRunspace;
			try
			{
				Runspace.DefaultRunspace = RunspaceFactory.CreateRunspace();
				Runspace.DefaultRunspace.Open();
				string text;
				ExternalScriptInfo scriptInfoForFile = DISCUtils.GetScriptInfoForFile(Runspace.DefaultRunspace.ExecutionContext, configFile, out text);
				this.configHash = DISCUtils.LoadConfigFile(Runspace.DefaultRunspace.ExecutionContext, scriptInfoForFile);
				this.MergeRoleRulesIntoConfigHash(roleVerifier);
				this.MergeRoleCapabilitiesIntoConfigHash();
				Runspace.DefaultRunspace.Close();
			}
			catch (PSSecurityException innerException)
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFilePath, configFile);
				PSInvalidOperationException ex = new PSInvalidOperationException(message, innerException);
				ex.SetErrorId("InvalidPSSessionConfigurationFilePath");
				throw ex;
			}
			finally
			{
				Runspace.DefaultRunspace = defaultRunspace;
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000EEACC File Offset: 0x000ECCCC
		private void MergeRoleRulesIntoConfigHash(Func<string, bool> roleVerifier)
		{
			if (this.configHash.ContainsKey(ConfigFileConstants.RoleDefinitions))
			{
				IDictionary dictionary = this.configHash[ConfigFileConstants.RoleDefinitions] as IDictionary;
				if (dictionary == null)
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.InvalidRoleEntry, this.configHash["Roles"].GetType().FullName);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					ex.SetErrorId("InvalidRoleEntryNotHashtable");
					throw ex;
				}
				foreach (object obj in dictionary.Keys)
				{
					if (roleVerifier(obj.ToString()))
					{
						IDictionary dictionary2 = dictionary[obj] as IDictionary;
						if (dictionary2 == null)
						{
							string message2 = StringUtil.Format(RemotingErrorIdStrings.InvalidRoleValue, obj.ToString());
							PSInvalidOperationException ex2 = new PSInvalidOperationException(message2);
							ex2.SetErrorId("InvalidRoleValueNotHashtable");
							throw ex2;
						}
						this.MergeConfigHashIntoConfigHash(dictionary2);
					}
				}
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000EEBDC File Offset: 0x000ECDDC
		private void MergeRoleCapabilitiesIntoConfigHash()
		{
			if (this.configHash.ContainsKey(ConfigFileConstants.RoleCapabilities))
			{
				string[] array = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.RoleCapabilities]);
				if (array != null)
				{
					foreach (string text in array)
					{
						string roleCapabilityPath = this.GetRoleCapabilityPath(text);
						if (string.IsNullOrEmpty(roleCapabilityPath))
						{
							string message = StringUtil.Format(RemotingErrorIdStrings.CouldNotFindRoleCapability, text, text + ".psrc");
							PSInvalidOperationException ex = new PSInvalidOperationException(message);
							ex.SetErrorId("CouldNotFindRoleCapability");
							throw ex;
						}
						DISCPowerShellConfiguration discpowerShellConfiguration = new DISCPowerShellConfiguration(roleCapabilityPath, null);
						IDictionary childConfigHash = discpowerShellConfiguration.ConfigHash;
						this.MergeConfigHashIntoConfigHash(childConfigHash);
					}
				}
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000EEC8C File Offset: 0x000ECE8C
		private void MergeConfigHashIntoConfigHash(IDictionary childConfigHash)
		{
			foreach (object obj in childConfigHash.Keys)
			{
				string text = obj.ToString();
				if (ConfigFileConstants.DisallowedRoleCapabilityKeys.Contains(text))
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.InvalidRoleCapabilityKey, text);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					ex.SetErrorId("InvalidRoleTookitKey");
					throw ex;
				}
				ArrayList arrayList = new ArrayList();
				if (this.configHash.ContainsKey(text))
				{
					IEnumerable enumerable = LanguagePrimitives.GetEnumerable(this.configHash[obj]);
					if (enumerable != null)
					{
						using (IEnumerator enumerator2 = enumerable.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object value = enumerator2.Current;
								arrayList.Add(value);
							}
							goto IL_CC;
						}
					}
					arrayList.Add(this.configHash[obj]);
				}
				IL_CC:
				IEnumerable enumerable2 = LanguagePrimitives.GetEnumerable(childConfigHash[obj]);
				if (enumerable2 != null)
				{
					using (IEnumerator enumerator3 = enumerable2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object value2 = enumerator3.Current;
							arrayList.Add(value2);
						}
						goto IL_12B;
					}
					goto IL_11C;
				}
				goto IL_11C;
				IL_12B:
				this.configHash[obj] = arrayList.ToArray();
				continue;
				IL_11C:
				arrayList.Add(childConfigHash[obj]);
				goto IL_12B;
			}
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000EEE48 File Offset: 0x000ED048
		private string GetRoleCapabilityPath(string roleCapability)
		{
			string searchPattern = "*";
			if (roleCapability.Contains('\\'))
			{
				string[] array = roleCapability.Split(new char[]
				{
					'\\'
				}, 2);
				searchPattern = array[0];
				roleCapability = array[1];
			}
			string[] array2 = ModuleIntrinsics.GetModulePath().Split(new char[]
			{
				';'
			});
			foreach (string path in array2)
			{
				try
				{
					foreach (string path2 in Directory.EnumerateDirectories(path, searchPattern))
					{
						string path3 = Path.Combine(path2, "RoleCapabilities");
						if (Directory.Exists(path3))
						{
							using (IEnumerator<string> enumerator2 = Directory.EnumerateFiles(path3, roleCapability + ".psrc").GetEnumerator())
							{
								if (enumerator2.MoveNext())
								{
									return enumerator2.Current;
								}
							}
						}
					}
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			return null;
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000EEF8C File Offset: 0x000ED18C
		public override InitialSessionState GetInitialSessionState(PSSenderInfo senderInfo)
		{
			InitialSessionState initialSessionState = null;
			string value = DISCPowerShellConfiguration.TryGetValue(this.configHash, ConfigFileConstants.SessionType);
			SessionType sessionType = SessionType.Default;
			bool flag = this.IsNonDefaultVisibiltySpecified(ConfigFileConstants.VisibleCmdlets);
			bool flag2 = this.IsNonDefaultVisibiltySpecified(ConfigFileConstants.VisibleFunctions);
			bool flag3 = this.IsNonDefaultVisibiltySpecified(ConfigFileConstants.VisibleAliases);
			bool flag4 = this.IsNonDefaultVisibiltySpecified(ConfigFileConstants.VisibleProviders);
			if (!string.IsNullOrEmpty(value))
			{
				sessionType = (SessionType)Enum.Parse(typeof(SessionType), value, true);
				if (sessionType == SessionType.Empty)
				{
					initialSessionState = InitialSessionState.Create();
				}
				else if (sessionType == SessionType.RestrictedRemoteServer)
				{
					initialSessionState = InitialSessionState.CreateRestricted(SessionCapabilities.RemoteServer);
				}
				else
				{
					initialSessionState = InitialSessionState.CreateDefault2();
				}
			}
			else
			{
				initialSessionState = InitialSessionState.CreateDefault2();
			}
			if (flag || flag2 || flag3 || flag4)
			{
				initialSessionState.DefaultCommandVisibility = SessionStateEntryVisibility.Private;
			}
			if (flag4)
			{
				string[] array = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.VisibleProviders]);
				if (array != null)
				{
					HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					foreach (string text in array)
					{
						if (!string.IsNullOrEmpty(text))
						{
							Collection<SessionStateProviderEntry> collection = initialSessionState.Providers.LookUpByName(text);
							foreach (SessionStateProviderEntry sessionStateProviderEntry in collection)
							{
								if (!hashSet.Contains(sessionStateProviderEntry.Name))
								{
									hashSet.Add(sessionStateProviderEntry.Name);
									sessionStateProviderEntry.Visibility = SessionStateEntryVisibility.Public;
								}
							}
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.AssembliesToLoad))
			{
				string[] array3 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.AssembliesToLoad]);
				if (array3 != null)
				{
					foreach (string name in array3)
					{
						initialSessionState.Assemblies.Add(new SessionStateAssemblyEntry(name));
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.ModulesToImport))
			{
				object[] array4 = DISCPowerShellConfiguration.TryGetObjectsOfType<object>(this.configHash[ConfigFileConstants.ModulesToImport], new Type[]
				{
					typeof(string),
					typeof(Hashtable)
				});
				if (this.configHash[ConfigFileConstants.ModulesToImport] != null && array4 == null)
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringOrHashtableArray, ConfigFileConstants.ModulesToImport);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					ex.SetErrorId("InvalidModulesToImportKeyEntries");
					throw ex;
				}
				if (array4 != null)
				{
					Collection<ModuleSpecification> collection2 = new Collection<ModuleSpecification>();
					foreach (object obj in array4)
					{
						ModuleSpecification moduleSpecification = null;
						string text2 = obj as string;
						if (!string.IsNullOrEmpty(text2))
						{
							moduleSpecification = new ModuleSpecification(text2);
						}
						else
						{
							Hashtable hashtable = obj as Hashtable;
							if (hashtable != null)
							{
								moduleSpecification = new ModuleSpecification(hashtable);
							}
						}
						if (moduleSpecification != null)
						{
							if (string.Equals(InitialSessionState.CoreModule, moduleSpecification.Name, StringComparison.OrdinalIgnoreCase))
							{
								if (sessionType == SessionType.Empty)
								{
									initialSessionState.ImportCorePSSnapIn();
								}
							}
							else
							{
								collection2.Add(moduleSpecification);
							}
						}
					}
					initialSessionState.ImportPSModule(collection2);
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.VisibleCmdlets))
			{
				object[] array6 = DISCPowerShellConfiguration.TryGetObjectsOfType<object>(this.configHash[ConfigFileConstants.VisibleCmdlets], new Type[]
				{
					typeof(string),
					typeof(Hashtable)
				});
				if (array6 == null)
				{
					string message2 = StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringOrHashtableArray, ConfigFileConstants.VisibleCmdlets);
					PSInvalidOperationException ex2 = new PSInvalidOperationException(message2);
					ex2.SetErrorId("InvalidVisibleCmdletsKeyEntries");
					throw ex2;
				}
				DISCPowerShellConfiguration.ProcessVisibleCommands(initialSessionState, array6);
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.AliasDefinitions))
			{
				Hashtable[] array7 = DISCPowerShellConfiguration.TryGetHashtableArray(this.configHash[ConfigFileConstants.AliasDefinitions]);
				if (array7 != null)
				{
					foreach (Hashtable alias in array7)
					{
						SessionStateAliasEntry sessionStateAliasEntry = this.CreateSessionStateAliasEntry(alias, flag3);
						if (sessionStateAliasEntry != null)
						{
							if (initialSessionState.Commands[sessionStateAliasEntry.Name] != null)
							{
								initialSessionState.Commands.Remove(sessionStateAliasEntry.Name, typeof(SessionStateAliasEntry));
							}
							initialSessionState.Commands.Add(sessionStateAliasEntry);
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.VisibleAliases))
			{
				string[] array9 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.VisibleAliases]);
				if (array9 != null)
				{
					foreach (string text3 in array9)
					{
						if (!string.IsNullOrEmpty(text3))
						{
							bool flag5 = false;
							Collection<SessionStateCommandEntry> collection3 = initialSessionState.Commands.LookUpByName(text3);
							foreach (SessionStateCommandEntry sessionStateCommandEntry in collection3)
							{
								if (sessionStateCommandEntry.CommandType == CommandTypes.Alias)
								{
									sessionStateCommandEntry.Visibility = SessionStateEntryVisibility.Public;
									flag5 = true;
								}
							}
							if (!flag5 || WildcardPattern.ContainsWildcardCharacters(text3))
							{
								initialSessionState.UnresolvedCommandsToExpose.Add(text3);
							}
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.FunctionDefinitions))
			{
				Hashtable[] array10 = DISCPowerShellConfiguration.TryGetHashtableArray(this.configHash[ConfigFileConstants.FunctionDefinitions]);
				if (array10 != null)
				{
					foreach (Hashtable function in array10)
					{
						SessionStateFunctionEntry sessionStateFunctionEntry = this.CreateSessionStateFunctionEntry(function, flag2);
						if (sessionStateFunctionEntry != null)
						{
							initialSessionState.Commands.Add(sessionStateFunctionEntry);
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.VisibleFunctions))
			{
				object[] array11 = DISCPowerShellConfiguration.TryGetObjectsOfType<object>(this.configHash[ConfigFileConstants.VisibleFunctions], new Type[]
				{
					typeof(string),
					typeof(Hashtable)
				});
				if (array11 == null)
				{
					string message3 = StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeStringOrHashtableArray, ConfigFileConstants.VisibleFunctions);
					PSInvalidOperationException ex3 = new PSInvalidOperationException(message3);
					ex3.SetErrorId("InvalidVisibleFunctionsKeyEntries");
					throw ex3;
				}
				DISCPowerShellConfiguration.ProcessVisibleCommands(initialSessionState, array11);
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.VariableDefinitions))
			{
				Hashtable[] array12 = DISCPowerShellConfiguration.TryGetHashtableArray(this.configHash[ConfigFileConstants.VariableDefinitions]);
				if (array12 != null)
				{
					foreach (Hashtable hashtable2 in array12)
					{
						if (hashtable2.ContainsKey(ConfigFileConstants.VariableValueToken) && hashtable2[ConfigFileConstants.VariableValueToken] is ScriptBlock)
						{
							initialSessionState.DynamicVariablesToDefine.Add(hashtable2);
						}
						else
						{
							SessionStateVariableEntry sessionStateVariableEntry = this.CreateSessionStateVariableEntry(hashtable2, initialSessionState.LanguageMode);
							if (sessionStateVariableEntry != null)
							{
								initialSessionState.Variables.Add(sessionStateVariableEntry);
							}
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.EnvironmentVariables))
			{
				Hashtable[] array13 = DISCPowerShellConfiguration.TryGetHashtableArray(this.configHash[ConfigFileConstants.EnvironmentVariables]);
				if (array13 != null)
				{
					foreach (Hashtable hashtable3 in array13)
					{
						foreach (object obj2 in hashtable3)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
							SessionStateVariableEntry item = new SessionStateVariableEntry(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString(), null);
							initialSessionState.EnvironmentVariables.Add(item);
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.TypesToProcess))
			{
				string[] array14 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.TypesToProcess]);
				if (array14 != null)
				{
					foreach (string text4 in array14)
					{
						if (!string.IsNullOrEmpty(text4))
						{
							initialSessionState.Types.Add(new SessionStateTypeEntry(text4));
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.FormatsToProcess))
			{
				string[] array15 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.FormatsToProcess]);
				if (array15 != null)
				{
					foreach (string text5 in array15)
					{
						if (!string.IsNullOrEmpty(text5))
						{
							initialSessionState.Formats.Add(new SessionStateFormatEntry(text5));
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.VisibleExternalCommands))
			{
				string[] array16 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.VisibleExternalCommands]);
				if (array16 != null)
				{
					foreach (string text6 in array16)
					{
						if (text6.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
						{
							SessionStateScriptEntry item2 = new SessionStateScriptEntry(text6, SessionStateEntryVisibility.Public);
							initialSessionState.Commands.Add(item2);
						}
						else
						{
							SessionStateApplicationEntry item3 = new SessionStateApplicationEntry(text6, SessionStateEntryVisibility.Public);
							initialSessionState.Commands.Add(item3);
						}
					}
				}
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.ScriptsToProcess))
			{
				string[] array17 = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[ConfigFileConstants.ScriptsToProcess]);
				if (array17 != null)
				{
					foreach (string text7 in array17)
					{
						if (!string.IsNullOrEmpty(text7))
						{
							initialSessionState.StartupScripts.Add(text7);
						}
					}
				}
			}
			if (flag || flag2 || flag3 || flag4)
			{
				if (sessionType == SessionType.Default)
				{
					initialSessionState.ImportPSCoreModule(InitialSessionState.EngineModules.ToArray<string>());
				}
				if (flag)
				{
					Collection<SessionStateCommandEntry> collection4 = initialSessionState.Commands["Import-Module"];
					if (collection4.Count == 1)
					{
						collection4[0].Visibility = SessionStateEntryVisibility.Private;
					}
				}
				if (flag3)
				{
					Collection<SessionStateCommandEntry> collection5 = initialSessionState.Commands["ipmo"];
					if (collection5.Count == 1)
					{
						collection5[0].Visibility = SessionStateEntryVisibility.Private;
					}
				}
				initialSessionState.DefaultCommandVisibility = SessionStateEntryVisibility.Private;
				initialSessionState.Variables.Add(new SessionStateVariableEntry("PSModuleAutoLoadingPreference", PSModuleAutoLoadingPreference.None, string.Empty, ScopedItemOptions.None));
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.ExecutionPolicy))
			{
				ExecutionPolicy executionPolicy = (ExecutionPolicy)Enum.Parse(typeof(ExecutionPolicy), this.configHash[ConfigFileConstants.ExecutionPolicy].ToString(), true);
				initialSessionState.ExecutionPolicy = executionPolicy;
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.LanguageMode))
			{
				PSLanguageMode languageMode = (PSLanguageMode)Enum.Parse(typeof(PSLanguageMode), this.configHash[ConfigFileConstants.LanguageMode].ToString(), true);
				initialSessionState.LanguageMode = languageMode;
			}
			if (this.configHash.ContainsKey(ConfigFileConstants.TranscriptDirectory))
			{
				initialSessionState.TranscriptDirectory = this.configHash[ConfigFileConstants.TranscriptDirectory].ToString();
			}
			return initialSessionState;
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000EF9EC File Offset: 0x000EDBEC
		private static void ProcessVisibleCommands(InitialSessionState iss, object[] commands)
		{
			Dictionary<string, Hashtable> dictionary = new Dictionary<string, Hashtable>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (ModuleSpecification moduleSpecification in iss.ModuleSpecificationsToImport)
			{
				hashSet.Add(moduleSpecification.Name);
			}
			foreach (object obj in commands)
			{
				if (obj != null)
				{
					string text = obj as string;
					if (!string.IsNullOrEmpty(text))
					{
						DISCPowerShellConfiguration.ProcessVisibleCommand(iss, text, hashSet);
					}
					else
					{
						IDictionary dictionary2 = obj as IDictionary;
						if (dictionary2 != null)
						{
							DISCPowerShellConfiguration.ProcessCommandModification(dictionary, dictionary2);
						}
					}
				}
			}
			foreach (string key in dictionary.Keys)
			{
				iss.CommandModifications.Add(key, dictionary[key]);
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000EFAFC File Offset: 0x000EDCFC
		private static void ProcessCommandModification(Dictionary<string, Hashtable> commandModifications, IDictionary commandModification)
		{
			string text = commandModification["Name"] as string;
			Hashtable[] array = null;
			if (commandModification.Contains("Parameters"))
			{
				array = DISCPowerShellConfiguration.TryGetHashtableArray(commandModification["Parameters"]);
				if (array != null)
				{
					foreach (Hashtable hashtable in array)
					{
						if (!hashtable.ContainsKey("Name"))
						{
							array = null;
							break;
						}
					}
				}
			}
			if (text == null || array == null)
			{
				string text2 = text;
				if (string.IsNullOrEmpty(text2))
				{
					IEnumerator enumerator = commandModification.Keys.GetEnumerator();
					enumerator.MoveNext();
					text2 = enumerator.Current.ToString();
				}
				string message = StringUtil.Format(RemotingErrorIdStrings.DISCCommandModificationSyntax, text2);
				PSInvalidOperationException ex = new PSInvalidOperationException(message);
				ex.SetErrorId("InvalidVisibleCommandKeyEntries");
				throw ex;
			}
			if (!commandModifications.ContainsKey(text))
			{
				commandModifications[text] = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			Hashtable hashtable2 = commandModifications[text];
			foreach (Hashtable dictionary in array)
			{
				string key = dictionary["Name"].ToString();
				if (!hashtable2.ContainsKey(key))
				{
					hashtable2[key] = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				Hashtable hashtable3 = (Hashtable)hashtable2[key];
				foreach (object obj in dictionary.Keys)
				{
					string text3 = (string)obj;
					if (!string.Equals("Name", text3, StringComparison.OrdinalIgnoreCase))
					{
						if (!hashtable3.Contains(text3))
						{
							hashtable3[text3] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						}
						HashSet<string> hashSet = (HashSet<string>)hashtable3[text3];
						foreach (string text4 in DISCPowerShellConfiguration.TryGetStringArray(dictionary[text3]))
						{
							if (!string.IsNullOrEmpty(text4))
							{
								hashSet.Add(text4);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000EFD18 File Offset: 0x000EDF18
		private static void ProcessVisibleCommand(InitialSessionState iss, string command, HashSet<string> moduleNames)
		{
			bool flag = false;
			if (command.IndexOf('\\') < 0)
			{
				Collection<SessionStateCommandEntry> collection = iss.Commands.LookUpByName(command);
				using (IEnumerator<SessionStateCommandEntry> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SessionStateCommandEntry sessionStateCommandEntry = enumerator.Current;
						if (sessionStateCommandEntry.CommandType == CommandTypes.Cmdlet || sessionStateCommandEntry.CommandType == CommandTypes.Function)
						{
							sessionStateCommandEntry.Visibility = SessionStateEntryVisibility.Public;
							flag = true;
						}
					}
					goto IL_95;
				}
			}
			string text;
			Utils.ParseCommandName(command, out text);
			if (!string.IsNullOrEmpty(text) && !moduleNames.Contains(text))
			{
				moduleNames.Add(text);
				iss.ImportPSModule(new string[]
				{
					text
				});
			}
			IL_95:
			if (!flag || WildcardPattern.ContainsWildcardCharacters(command))
			{
				iss.UnresolvedCommandsToExpose.Add(command);
			}
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000EFDE4 File Offset: 0x000EDFE4
		private SessionStateAliasEntry CreateSessionStateAliasEntry(Hashtable alias, bool isAliasVisibilityDefined)
		{
			string text = DISCPowerShellConfiguration.TryGetValue(alias, ConfigFileConstants.AliasNameToken);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			string text2 = DISCPowerShellConfiguration.TryGetValue(alias, ConfigFileConstants.AliasValueToken);
			if (string.IsNullOrEmpty(text2))
			{
				return null;
			}
			string description = DISCPowerShellConfiguration.TryGetValue(alias, ConfigFileConstants.AliasDescriptionToken);
			ScopedItemOptions options = ScopedItemOptions.None;
			string value = DISCPowerShellConfiguration.TryGetValue(alias, ConfigFileConstants.AliasOptionsToken);
			if (!string.IsNullOrEmpty(value))
			{
				options = (ScopedItemOptions)Enum.Parse(typeof(ScopedItemOptions), value, true);
			}
			SessionStateEntryVisibility visibility = SessionStateEntryVisibility.Private;
			if (!isAliasVisibilityDefined)
			{
				visibility = SessionStateEntryVisibility.Public;
			}
			return new SessionStateAliasEntry(text, text2, description, options, visibility);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000EFE70 File Offset: 0x000EE070
		private SessionStateFunctionEntry CreateSessionStateFunctionEntry(Hashtable function, bool isFunctionVisibilityDefined)
		{
			string text = DISCPowerShellConfiguration.TryGetValue(function, ConfigFileConstants.FunctionNameToken);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			string text2 = DISCPowerShellConfiguration.TryGetValue(function, ConfigFileConstants.FunctionValueToken);
			if (string.IsNullOrEmpty(text2))
			{
				return null;
			}
			ScopedItemOptions options = ScopedItemOptions.None;
			string value = DISCPowerShellConfiguration.TryGetValue(function, ConfigFileConstants.FunctionOptionsToken);
			if (!string.IsNullOrEmpty(value))
			{
				options = (ScopedItemOptions)Enum.Parse(typeof(ScopedItemOptions), value, true);
			}
			ScriptBlock scriptBlock = ScriptBlock.Create(text2);
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			SessionStateEntryVisibility visibility = SessionStateEntryVisibility.Private;
			if (!isFunctionVisibilityDefined)
			{
				visibility = SessionStateEntryVisibility.Public;
			}
			return new SessionStateFunctionEntry(text, text2, options, visibility, scriptBlock, null);
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000EFF04 File Offset: 0x000EE104
		private SessionStateVariableEntry CreateSessionStateVariableEntry(Hashtable variable, PSLanguageMode languageMode)
		{
			string text = DISCPowerShellConfiguration.TryGetValue(variable, ConfigFileConstants.VariableNameToken);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			string value = DISCPowerShellConfiguration.TryGetValue(variable, ConfigFileConstants.VariableValueToken);
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			string description = DISCPowerShellConfiguration.TryGetValue(variable, ConfigFileConstants.AliasDescriptionToken);
			ScopedItemOptions options = ScopedItemOptions.None;
			string value2 = DISCPowerShellConfiguration.TryGetValue(variable, ConfigFileConstants.AliasOptionsToken);
			if (!string.IsNullOrEmpty(value2))
			{
				options = (ScopedItemOptions)Enum.Parse(typeof(ScopedItemOptions), value2, true);
			}
			SessionStateEntryVisibility visibility = SessionStateEntryVisibility.Private;
			if (languageMode == PSLanguageMode.FullLanguage)
			{
				visibility = SessionStateEntryVisibility.Public;
			}
			return new SessionStateVariableEntry(text, value, description, options, new Collection<Attribute>(), visibility);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000EFF94 File Offset: 0x000EE194
		private bool IsNonDefaultVisibiltySpecified(string configFileKey)
		{
			if (this.configHash.ContainsKey(configFileKey))
			{
				string[] array = DISCPowerShellConfiguration.TryGetStringArray(this.configHash[configFileKey]);
				return array != null && array.Length != 0;
			}
			return false;
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000EFFCE File Offset: 0x000EE1CE
		internal static string TryGetValue(Hashtable table, string key)
		{
			if (table.ContainsKey(key))
			{
				return table[key].ToString();
			}
			return string.Empty;
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000EFFEC File Offset: 0x000EE1EC
		internal static Hashtable[] TryGetHashtableArray(object hashObj)
		{
			Hashtable hashtable = hashObj as Hashtable;
			if (hashtable != null)
			{
				return new Hashtable[]
				{
					hashtable
				};
			}
			Hashtable[] array = hashObj as Hashtable[];
			if (array == null)
			{
				object[] array2 = hashObj as object[];
				if (array2 != null)
				{
					array = new Hashtable[array2.Length];
					for (int i = 0; i < array.Length; i++)
					{
						Hashtable hashtable2 = array2[i] as Hashtable;
						if (hashtable2 == null)
						{
							return null;
						}
						array[i] = hashtable2;
					}
				}
			}
			return array;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000F0054 File Offset: 0x000EE254
		internal static string[] TryGetStringArray(object hashObj)
		{
			object[] array = hashObj as object[];
			if (array != null)
			{
				string[] array2 = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array[i].ToString();
				}
				return array2;
			}
			if (hashObj != null)
			{
				return new string[]
				{
					hashObj.ToString()
				};
			}
			return null;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000F00DC File Offset: 0x000EE2DC
		internal static T[] TryGetObjectsOfType<T>(object hashObj, IEnumerable<Type> types) where T : class
		{
			object[] objs = hashObj as object[];
			if (objs == null)
			{
				if (hashObj != null)
				{
					foreach (Type o in types)
					{
						if (hashObj.GetType().Equals(o))
						{
							return new T[]
							{
								hashObj as T
							};
						}
					}
				}
				return null;
			}
			T[] array = new T[objs.Length];
			for (int i = 0; i < objs.Length; i++)
			{
				int i1 = i;
				if (!types.Any((Type type) => objs[i1].GetType().Equals(type)))
				{
					return null;
				}
				array[i] = (objs[i] as T);
			}
			return array;
		}

		// Token: 0x040015C3 RID: 5571
		private string configFile;

		// Token: 0x040015C4 RID: 5572
		private Hashtable configHash;
	}
}
