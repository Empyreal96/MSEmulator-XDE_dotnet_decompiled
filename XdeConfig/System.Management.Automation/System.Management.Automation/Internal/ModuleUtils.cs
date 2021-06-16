using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x020000B1 RID: 177
	internal static class ModuleUtils
	{
		// Token: 0x06000908 RID: 2312 RVA: 0x00037000 File Offset: 0x00035200
		private static void RecurseDirectories(string directory, Action<string> directoryAction, bool doNotRecurseForNestedModules)
		{
			string[] array = new string[0];
			string[] array2 = new string[0];
			bool flag = false;
			try
			{
				array = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
				array2 = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
			}
			catch (IOException)
			{
				return;
			}
			catch (UnauthorizedAccessException)
			{
				return;
			}
			if (doNotRecurseForNestedModules)
			{
				foreach (string path in array2)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(directory);
					Version version;
					if (Path.GetExtension(path).Equals(".psd1", StringComparison.OrdinalIgnoreCase) && Version.TryParse(fileNameWithoutExtension, out version))
					{
						fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(directory));
					}
					if (Path.GetFileNameWithoutExtension(path).Equals(fileNameWithoutExtension, StringComparison.OrdinalIgnoreCase))
					{
						foreach (string value in ModuleIntrinsics.PSModuleExtensions)
						{
							if (Path.GetExtension(path).Equals(value, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (string obj in array)
				{
					try
					{
						directoryAction(obj);
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
				}
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00037174 File Offset: 0x00035374
		internal static List<string> GetAllAvailableModuleFiles(string directory)
		{
			List<string> availableModuleFiles = new List<string>();
			ModuleUtils.RecurseDirectories(directory, delegate(string subDirectory)
			{
				List<string> allAvailableModuleFiles = ModuleUtils.GetAllAvailableModuleFiles(subDirectory);
				availableModuleFiles.AddRange(allAvailableModuleFiles);
			}, false);
			string[] files = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
			foreach (string text in files)
			{
				foreach (string value in ModuleIntrinsics.PSModuleExtensions)
				{
					if (Path.GetExtension(text).Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						availableModuleFiles.Add(text);
						break;
					}
				}
			}
			return availableModuleFiles;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00037214 File Offset: 0x00035414
		internal static List<string> GetDefaultAvailableModuleFiles(bool force, bool isForAutoDiscovery, ExecutionContext context)
		{
			Pipeline currentlyRunningPipeline = context.CurrentRunspace.GetCurrentlyRunningPipeline();
			if (!force && currentlyRunningPipeline != null)
			{
				lock (ModuleUtils.cachedAvailableModuleFiles)
				{
					if (currentlyRunningPipeline.InstanceId == ModuleUtils.pipelineInstanceIdForModuleFileCache && ModuleUtils.cachedAvailableModuleFiles.Count > 0)
					{
						return ModuleUtils.cachedAvailableModuleFiles;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			List<string> modulePaths = new List<string>();
			bool flag2 = false;
			foreach (string text in ModuleIntrinsics.GetModulePath(isForAutoDiscovery, context))
			{
				flag2 = false;
				ProgressRecord progressRecord = new ProgressRecord(0, Modules.DeterminingAvailableModules, string.Format(CultureInfo.InvariantCulture, Modules.SearchingUncShare, new object[]
				{
					text
				}));
				progressRecord.RecordType = ProgressRecordType.Processing;
				try
				{
					Uri uri = new Uri(text);
					flag2 = uri.IsUnc;
					if (flag2 && context.CurrentCommandProcessor != null)
					{
						context.CurrentCommandProcessor.CommandRuntime.WriteProgress(progressRecord);
					}
				}
				catch (UriFormatException)
				{
					flag2 = false;
				}
				catch (InvalidOperationException)
				{
					flag2 = false;
				}
				foreach (string item in ModuleUtils.GetDefaultAvailableModuleFiles(text, modulePaths))
				{
					if (hashSet.Add(item))
					{
						list.Add(item);
					}
				}
				if (flag2 && context.CurrentCommandProcessor != null)
				{
					progressRecord.RecordType = ProgressRecordType.Completed;
					context.CurrentCommandProcessor.CommandRuntime.WriteProgress(progressRecord);
				}
			}
			if (currentlyRunningPipeline != null)
			{
				lock (ModuleUtils.cachedAvailableModuleFiles)
				{
					ModuleUtils.pipelineInstanceIdForModuleFileCache = currentlyRunningPipeline.InstanceId;
					ModuleUtils.cachedAvailableModuleFiles = list;
				}
			}
			return list;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00037430 File Offset: 0x00035630
		internal static List<string> GetModuleVersionsFromAbsolutePath(string directory)
		{
			List<string> list = new List<string>();
			string fileName = Path.GetFileName(directory);
			foreach (Version version in ModuleUtils.GetModuleVersionSubfolders(directory))
			{
				string str = Path.Combine(directory, Path.Combine(version.ToString(), fileName));
				string text = str + ".psd1";
				if (File.Exists(text))
				{
					bool flag = version.Equals(ModuleIntrinsics.GetManifestModuleVersion(text));
					if (flag)
					{
						list.Add(text);
					}
				}
			}
			foreach (string str2 in ModuleIntrinsics.PSModuleExtensions)
			{
				string text2 = Path.Combine(directory, fileName) + str2;
				if (Utils.NativeFileExists(text2))
				{
					list.Add(text2);
					break;
				}
			}
			return list;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00037584 File Offset: 0x00035784
		internal static List<string> GetDefaultAvailableModuleFiles(string directory, List<string> modulePaths)
		{
			List<string> availableModuleFiles = new List<string>();
			HashSet<string> uniqueModuleFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			bool flag = modulePaths.Contains(directory);
			ModuleUtils.RecurseDirectories(directory, delegate(string subDirectory)
			{
				List<string> defaultAvailableModuleFiles = ModuleUtils.GetDefaultAvailableModuleFiles(subDirectory, modulePaths);
				foreach (string moduleFile in defaultAvailableModuleFiles)
				{
					ModuleUtils.AddModuleFileUniqueWithSystemPreference(availableModuleFiles, uniqueModuleFiles, moduleFile);
				}
			}, !flag);
			string fileName = Path.GetFileName(directory);
			foreach (Version version in ModuleUtils.GetModuleVersionSubfolders(directory))
			{
				string str = Path.Combine(directory, Path.Combine(version.ToString(), fileName));
				string text = str + ".psd1";
				if (File.Exists(text))
				{
					bool flag2 = version.Equals(ModuleIntrinsics.GetManifestModuleVersion(text));
					if (flag2)
					{
						ModuleUtils.AddModuleFileUniqueWithSystemPreference(availableModuleFiles, uniqueModuleFiles, text);
					}
				}
			}
			foreach (string str2 in ModuleIntrinsics.PSModuleExtensions)
			{
				string text2 = Path.Combine(directory, fileName) + str2;
				if (Utils.NativeFileExists(text2))
				{
					ModuleUtils.AddModuleFileUniqueWithSystemPreference(availableModuleFiles, uniqueModuleFiles, text2);
					break;
				}
			}
			return availableModuleFiles;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000376D8 File Offset: 0x000358D8
		private static void AddModuleFileUniqueWithSystemPreference(List<string> availableModuleFiles, HashSet<string> uniqueModuleFiles, string moduleFile)
		{
			if (uniqueModuleFiles.Add(moduleFile))
			{
				if (moduleFile.IndexOf("Microsoft.PowerShell", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					availableModuleFiles.Insert(0, moduleFile);
					return;
				}
				availableModuleFiles.Add(moduleFile);
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0003770C File Offset: 0x0003590C
		internal static List<Version> GetModuleVersionSubfolders(string moduleBase)
		{
			List<Version> list = new List<Version>();
			if (!string.IsNullOrWhiteSpace(moduleBase) && Directory.Exists(moduleBase))
			{
				string[] directories = Directory.GetDirectories(moduleBase);
				foreach (string path in directories)
				{
					string fileName = Path.GetFileName(path);
					Version item;
					if (Version.TryParse(fileName, out item))
					{
						list.Add(item);
					}
				}
				list.Sort((Version x, Version y) => x.CompareTo(y));
				list.Reverse();
			}
			return list;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x000380BC File Offset: 0x000362BC
		internal static IEnumerable<CommandInfo> GetMatchingCommands(string pattern, ExecutionContext context, CommandOrigin commandOrigin, bool rediscoverImportedModules = false, bool moduleVersionRequired = false)
		{
			WildcardPattern commandPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			CmdletInfo cmdletInfo = context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Get-Module");
			PSModuleAutoLoadingPreference moduleAutoLoadingPreference = CommandDiscovery.GetCommandDiscoveryPreference(context, SpecialVariables.PSModuleAutoLoadingPreferenceVarPath, "PSModuleAutoLoadingPreference");
			if (moduleAutoLoadingPreference != PSModuleAutoLoadingPreference.None && (commandOrigin == CommandOrigin.Internal || (cmdletInfo != null && cmdletInfo.Visibility == SessionStateEntryVisibility.Public)))
			{
				foreach (string modulePath in ModuleUtils.GetDefaultAvailableModuleFiles(true, false, context))
				{
					string moduleName = Path.GetFileNameWithoutExtension(modulePath);
					List<PSModuleInfo> modules = context.Modules.GetExactMatchModules(moduleName, false, true);
					PSModuleInfo tempModuleInfo = null;
					if (modules.Count != 0)
					{
						if (!rediscoverImportedModules)
						{
							continue;
						}
						if (modules.Exists((PSModuleInfo module) => module.ModuleHasPrivateMembers))
						{
							continue;
						}
						if (modules.Count == 1)
						{
							PSModuleInfo psModule = modules[0];
							tempModuleInfo = new PSModuleInfo(psModule.Name, psModule.Path, null, null);
							tempModuleInfo.SetModuleBase(psModule.ModuleBase);
							foreach (KeyValuePair<string, CommandInfo> entry in psModule.ExportedCommands)
							{
								WildcardPattern wildcardPattern = commandPattern;
								KeyValuePair<string, CommandInfo> keyValuePair = entry;
								if (wildcardPattern.IsMatch(keyValuePair.Value.Name))
								{
									CommandInfo current = null;
									KeyValuePair<string, CommandInfo> keyValuePair2 = entry;
									CommandTypes commandType2 = keyValuePair2.Value.CommandType;
									if (commandType2 <= CommandTypes.Cmdlet)
									{
										switch (commandType2)
										{
										case CommandTypes.Alias:
										{
											KeyValuePair<string, CommandInfo> keyValuePair3 = entry;
											current = new AliasInfo(keyValuePair3.Value.Name, null, context);
											break;
										}
										case CommandTypes.Function:
										{
											KeyValuePair<string, CommandInfo> keyValuePair4 = entry;
											current = new FunctionInfo(keyValuePair4.Value.Name, ScriptBlock.Create(""), context);
											break;
										}
										case CommandTypes.Alias | CommandTypes.Function:
											break;
										case CommandTypes.Filter:
										{
											KeyValuePair<string, CommandInfo> keyValuePair5 = entry;
											current = new FilterInfo(keyValuePair5.Value.Name, ScriptBlock.Create(""), context);
											break;
										}
										default:
											if (commandType2 == CommandTypes.Cmdlet)
											{
												KeyValuePair<string, CommandInfo> keyValuePair6 = entry;
												current = new CmdletInfo(keyValuePair6.Value.Name, null, null, null, context);
											}
											break;
										}
									}
									else if (commandType2 != CommandTypes.Workflow)
									{
										if (commandType2 == CommandTypes.Configuration)
										{
											KeyValuePair<string, CommandInfo> keyValuePair7 = entry;
											current = new ConfigurationInfo(keyValuePair7.Value.Name, ScriptBlock.Create(""), context);
										}
									}
									else
									{
										KeyValuePair<string, CommandInfo> keyValuePair8 = entry;
										current = new WorkflowInfo(keyValuePair8.Value.Name, ScriptBlock.Create(""), context);
									}
									current.SetModule(tempModuleInfo);
									yield return current;
								}
							}
							continue;
						}
					}
					string moduleShortName = Path.GetFileNameWithoutExtension(modulePath);
					Dictionary<string, List<CommandTypes>> exportedCommands = AnalysisCache.GetExportedCommands(modulePath, false, context);
					if (exportedCommands != null)
					{
						tempModuleInfo = new PSModuleInfo(moduleShortName, modulePath, null, null);
						if (InitialSessionState.IsEngineModule(moduleShortName))
						{
							tempModuleInfo.SetModuleBase(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID));
						}
						if (moduleVersionRequired && modulePath.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
						{
							tempModuleInfo.SetVersion(ModuleIntrinsics.GetManifestModuleVersion(modulePath));
						}
						foreach (KeyValuePair<string, List<CommandTypes>> keyValuePair9 in exportedCommands)
						{
							string exportedCommandKey = keyValuePair9.Key;
							Dictionary<string, List<CommandTypes>>.Enumerator exportedCommandEnumerator;
							KeyValuePair<string, List<CommandTypes>> keyValuePair10 = exportedCommandEnumerator.Current;
							List<CommandTypes> exportedCommandValue = keyValuePair10.Value;
							if (commandPattern.IsMatch(exportedCommandKey) && exportedCommandValue != null)
							{
								foreach (CommandTypes commandType in exportedCommandValue)
								{
									bool shouldExportCommand = true;
									if (context.InitialSessionState != null && commandOrigin == CommandOrigin.Runspace)
									{
										foreach (SessionStateCommandEntry sessionStateCommandEntry in context.InitialSessionState.Commands[exportedCommandKey])
										{
											string b = null;
											if (sessionStateCommandEntry.Module != null)
											{
												b = sessionStateCommandEntry.Module.Name;
											}
											else if (sessionStateCommandEntry.PSSnapIn != null)
											{
												b = sessionStateCommandEntry.PSSnapIn.Name;
											}
											if (string.Equals(moduleShortName, b, StringComparison.OrdinalIgnoreCase) && sessionStateCommandEntry.Visibility == SessionStateEntryVisibility.Private)
											{
												shouldExportCommand = false;
											}
										}
									}
									if (shouldExportCommand)
									{
										CommandInfo current2 = null;
										CommandTypes commandTypes = commandType;
										if (commandTypes <= CommandTypes.Cmdlet)
										{
											switch (commandTypes)
											{
											case CommandTypes.Alias:
												current2 = new AliasInfo(exportedCommandKey, null, context);
												break;
											case CommandTypes.Function:
												current2 = new FunctionInfo(exportedCommandKey, ScriptBlock.Create(""), context);
												break;
											default:
												if (commandTypes == CommandTypes.Cmdlet)
												{
													current2 = new CmdletInfo(exportedCommandKey, null, null, null, context);
												}
												break;
											}
										}
										else if (commandTypes != CommandTypes.Workflow)
										{
											if (commandTypes == CommandTypes.Configuration)
											{
												current2 = new ConfigurationInfo(exportedCommandKey, ScriptBlock.Create(""), context);
											}
										}
										else
										{
											current2 = new WorkflowInfo(exportedCommandKey, ScriptBlock.Create(""), context);
										}
										if (current2 != null)
										{
											current2.SetModule(tempModuleInfo);
										}
										yield return current2;
									}
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x04000411 RID: 1041
		private static List<string> cachedAvailableModuleFiles = new List<string>();

		// Token: 0x04000412 RID: 1042
		private static long pipelineInstanceIdForModuleFileCache = -1L;
	}
}
