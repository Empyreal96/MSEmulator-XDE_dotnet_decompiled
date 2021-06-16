using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Security;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020000AC RID: 172
	public class ModuleIntrinsics
	{
		// Token: 0x060008CE RID: 2254 RVA: 0x0003522A File Offset: 0x0003342A
		internal ModuleIntrinsics(ExecutionContext context)
		{
			this._context = context;
			ModuleIntrinsics.SetModulePath();
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0003524E File Offset: 0x0003344E
		internal Dictionary<string, PSModuleInfo> ModuleTable
		{
			get
			{
				return this._moduleTable;
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00035258 File Offset: 0x00033458
		internal void IncrementModuleNestingDepth(PSCmdlet cmdlet, string path)
		{
			if (++this._moduleNestingDepth > 10)
			{
				string message = StringUtil.Format(Modules.ModuleTooDeeplyNested, path, 10);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_ModuleTooDeeplyNested", ErrorCategory.InvalidOperation, path);
				cmdlet.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000352A9 File Offset: 0x000334A9
		internal void DecrementModuleNestingCount()
		{
			this._moduleNestingDepth--;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x000352B9 File Offset: 0x000334B9
		internal int ModuleNestingDepth
		{
			get
			{
				return this._moduleNestingDepth;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000352C4 File Offset: 0x000334C4
		internal PSModuleInfo CreateModule(string name, string path, ScriptBlock scriptBlock, SessionState ss, out List<object> results, params object[] arguments)
		{
			return this.CreateModuleImplementation(name, path, scriptBlock, null, ss, null, out results, arguments);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000352E4 File Offset: 0x000334E4
		internal PSModuleInfo CreateModule(string path, ExternalScriptInfo scriptInfo, IScriptExtent scriptPosition, SessionState ss, object privateData, params object[] arguments)
		{
			List<object> list;
			return this.CreateModuleImplementation(ModuleIntrinsics.GetModuleName(path), path, scriptInfo, scriptPosition, ss, privateData, out list, arguments);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00035308 File Offset: 0x00033508
		private PSModuleInfo CreateModuleImplementation(string name, string path, object moduleCode, IScriptExtent scriptPosition, SessionState ss, object privateData, out List<object> result, params object[] arguments)
		{
			if (ss == null)
			{
				ss = new SessionState(this._context, true, true);
			}
			SessionStateInternal engineSessionState = this._context.EngineSessionState;
			PSModuleInfo psmoduleInfo = new PSModuleInfo(name, path, this._context, ss);
			ss.Internal.Module = psmoduleInfo;
			psmoduleInfo.PrivateData = privateData;
			bool flag = false;
			int num = 0;
			ScriptBlock scriptBlock;
			try
			{
				this._context.EngineSessionState = ss.Internal;
				ExternalScriptInfo externalScriptInfo = moduleCode as ExternalScriptInfo;
				if (externalScriptInfo != null)
				{
					scriptBlock = externalScriptInfo.ScriptBlock;
					this._context.Debugger.RegisterScriptFile(externalScriptInfo);
				}
				else
				{
					scriptBlock = (moduleCode as ScriptBlock);
					if (scriptBlock != null)
					{
						PSLanguageMode? languageMode = scriptBlock.LanguageMode;
						scriptBlock = scriptBlock.Clone(true);
						scriptBlock.LanguageMode = languageMode;
						scriptBlock.SessionState = ss;
					}
					else if (moduleCode is string)
					{
						scriptBlock = ScriptBlock.Create(this._context, (string)moduleCode);
					}
				}
				if (scriptBlock == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				scriptBlock.SessionStateInternal = ss.Internal;
				InvocationInfo invocationInfo = new InvocationInfo(externalScriptInfo, scriptPosition);
				psmoduleInfo._definitionExtent = scriptBlock.Ast.Extent;
				Ast ast = scriptBlock.Ast;
				while (ast.Parent != null)
				{
					ast = ast.Parent;
				}
				List<object> list = new List<object>();
				try
				{
					Pipe outputPipe = new Pipe(list);
					scriptBlock.InvokeWithPipe(false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, AutomationNull.Value, AutomationNull.Value, outputPipe, invocationInfo, false, null, null, arguments ?? new object[0]);
				}
				catch (ExitException ex)
				{
					num = (int)ex.Argument;
					flag = true;
				}
				result = list;
			}
			finally
			{
				this._context.EngineSessionState = engineSessionState;
			}
			if (flag)
			{
				this._context.SetVariable(SpecialVariables.LastExitCodeVarPath, num);
			}
			psmoduleInfo.ImplementingAssembly = scriptBlock.AssemblyDefiningPSTypes;
			psmoduleInfo.CreateExportedTypeDefinitions(scriptBlock.Ast as ScriptBlockAst);
			return psmoduleInfo;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000354FC File Offset: 0x000336FC
		internal ScriptBlock CreateBoundScriptBlock(ExecutionContext context, ScriptBlock sb, bool linkToGlobal)
		{
			PSModuleInfo psmoduleInfo = new PSModuleInfo(context, linkToGlobal);
			return psmoduleInfo.NewBoundScriptBlock(sb, context);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00035519 File Offset: 0x00033719
		internal List<PSModuleInfo> GetModules(string[] patterns, bool all)
		{
			return this.GetModuleCore(patterns, all, false);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00035524 File Offset: 0x00033724
		internal List<PSModuleInfo> GetExactMatchModules(string moduleName, bool all, bool exactMatch)
		{
			if (moduleName == null)
			{
				moduleName = string.Empty;
			}
			return this.GetModuleCore(new string[]
			{
				moduleName
			}, all, exactMatch);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00035558 File Offset: 0x00033758
		private List<PSModuleInfo> GetModuleCore(string[] patterns, bool all, bool exactMatch)
		{
			string value = null;
			List<WildcardPattern> list = new List<WildcardPattern>();
			if (exactMatch)
			{
				value = patterns[0];
			}
			else
			{
				if (patterns == null)
				{
					patterns = new string[]
					{
						"*"
					};
				}
				foreach (string pattern in patterns)
				{
					list.Add(new WildcardPattern(pattern, WildcardOptions.IgnoreCase));
				}
			}
			List<PSModuleInfo> list2 = new List<PSModuleInfo>();
			if (all)
			{
				using (Dictionary<string, PSModuleInfo>.KeyCollection.Enumerator enumerator = this.ModuleTable.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string key = enumerator.Current;
						PSModuleInfo psmoduleInfo = this.ModuleTable[key];
						if ((exactMatch && psmoduleInfo.Name.Equals(value, StringComparison.OrdinalIgnoreCase)) || (!exactMatch && SessionStateUtilities.MatchesAnyWildcardPattern(psmoduleInfo.Name, list, false)))
						{
							list2.Add(psmoduleInfo);
						}
					}
					goto IL_211;
				}
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			foreach (string key2 in this._context.EngineSessionState.ModuleTable.Keys)
			{
				PSModuleInfo psmoduleInfo2 = this._context.EngineSessionState.ModuleTable[key2];
				if ((exactMatch && psmoduleInfo2.Name.Equals(value, StringComparison.OrdinalIgnoreCase)) || (!exactMatch && SessionStateUtilities.MatchesAnyWildcardPattern(psmoduleInfo2.Name, list, false)))
				{
					list2.Add(psmoduleInfo2);
					dictionary[key2] = true;
				}
			}
			if (this._context.EngineSessionState != this._context.TopLevelSessionState)
			{
				foreach (string key3 in this._context.TopLevelSessionState.ModuleTable.Keys)
				{
					if (!dictionary.ContainsKey(key3))
					{
						PSModuleInfo psmoduleInfo3 = this.ModuleTable[key3];
						if ((exactMatch && psmoduleInfo3.Name.Equals(value, StringComparison.OrdinalIgnoreCase)) || (!exactMatch && SessionStateUtilities.MatchesAnyWildcardPattern(psmoduleInfo3.Name, list, false)))
						{
							list2.Add(psmoduleInfo3);
						}
					}
				}
			}
			IL_211:
			return (from m in list2
			orderby m.Name
			select m).ToList<PSModuleInfo>();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000357D0 File Offset: 0x000339D0
		internal List<PSModuleInfo> GetModules(ModuleSpecification[] fullyQualifiedName, bool all)
		{
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			if (all)
			{
				foreach (ModuleSpecification moduleSpec in fullyQualifiedName)
				{
					foreach (string key in this.ModuleTable.Keys)
					{
						PSModuleInfo psmoduleInfo = this.ModuleTable[key];
						if (ModuleIntrinsics.IsModuleMatchingModuleSpec(psmoduleInfo, moduleSpec))
						{
							list.Add(psmoduleInfo);
						}
					}
				}
			}
			else
			{
				foreach (ModuleSpecification moduleSpec2 in fullyQualifiedName)
				{
					Dictionary<string, bool> dictionary = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
					foreach (string key2 in this._context.EngineSessionState.ModuleTable.Keys)
					{
						PSModuleInfo psmoduleInfo2 = this._context.EngineSessionState.ModuleTable[key2];
						if (ModuleIntrinsics.IsModuleMatchingModuleSpec(psmoduleInfo2, moduleSpec2))
						{
							list.Add(psmoduleInfo2);
							dictionary[key2] = true;
						}
					}
					if (this._context.EngineSessionState != this._context.TopLevelSessionState)
					{
						foreach (string key3 in this._context.TopLevelSessionState.ModuleTable.Keys)
						{
							if (!dictionary.ContainsKey(key3))
							{
								PSModuleInfo psmoduleInfo3 = this.ModuleTable[key3];
								if (ModuleIntrinsics.IsModuleMatchingModuleSpec(psmoduleInfo3, moduleSpec2))
								{
									list.Add(psmoduleInfo3);
								}
							}
						}
					}
				}
			}
			return (from m in list
			orderby m.Name
			select m).ToList<PSModuleInfo>();
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000359D4 File Offset: 0x00033BD4
		internal static bool IsModuleMatchingModuleSpec(PSModuleInfo moduleInfo, ModuleSpecification moduleSpec)
		{
			return moduleInfo != null && moduleSpec != null && moduleInfo.Name.Equals(moduleSpec.Name, StringComparison.OrdinalIgnoreCase) && (moduleSpec.Guid == null || moduleSpec.Guid.Equals(moduleInfo.Guid)) && ((moduleSpec.Version == null && moduleSpec.RequiredVersion == null && moduleSpec.MaximumVersion == null) || (moduleSpec.RequiredVersion != null && moduleSpec.RequiredVersion.Equals(moduleInfo.Version)) || (moduleSpec.MaximumVersion == null && moduleSpec.Version != null && moduleSpec.RequiredVersion == null && moduleSpec.Version <= moduleInfo.Version) || (moduleSpec.MaximumVersion != null && moduleSpec.Version == null && moduleSpec.RequiredVersion == null && ModuleCmdletBase.GetMaximumVersion(moduleSpec.MaximumVersion) >= moduleInfo.Version) || (moduleSpec.MaximumVersion != null && moduleSpec.Version != null && moduleSpec.RequiredVersion == null && ModuleCmdletBase.GetMaximumVersion(moduleSpec.MaximumVersion) >= moduleInfo.Version && moduleSpec.Version <= moduleInfo.Version));
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00035B4C File Offset: 0x00033D4C
		internal static Version GetManifestModuleVersion(string manifestPath)
		{
			if (manifestPath != null && manifestPath.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase) && Runspace.DefaultRunspace != null)
			{
				try
				{
					Hashtable moduleManifestProperties = PsUtils.GetModuleManifestProperties(manifestPath, PsUtils.ManifestModuleVersionPropertyName);
					object obj = moduleManifestProperties["ModuleVersion"];
					Version result;
					if (obj != null && LanguagePrimitives.TryConvertTo<Version>(obj, out result))
					{
						return result;
					}
				}
				catch (PSInvalidOperationException)
				{
				}
			}
			return new Version(0, 0);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00035BB8 File Offset: 0x00033DB8
		internal static bool IsPowerShellModuleExtension(string extension)
		{
			foreach (string value in ModuleIntrinsics.PSModuleProcessableExtensions)
			{
				if (extension.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00035BF0 File Offset: 0x00033DF0
		internal static string GetModuleName(string path)
		{
			string text = (path == null) ? string.Empty : Path.GetFileName(path);
			string extension = Path.GetExtension(text);
			if (!string.IsNullOrEmpty(extension) && ModuleIntrinsics.IsPowerShellModuleExtension(extension))
			{
				return text.Substring(0, text.Length - extension.Length);
			}
			return text;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00035C3C File Offset: 0x00033E3C
		internal static string GetPersonalModulePath()
		{
			return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Utils.ProductNameForDirectory), Utils.ModuleDirectory);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00035C68 File Offset: 0x00033E68
		internal static string GetSystemwideModulePath()
		{
			string result = null;
			string defaultPowerShellShellID = Utils.DefaultPowerShellShellID;
			string text = null;
			try
			{
				text = Utils.GetApplicationBase(defaultPowerShellShellID);
			}
			catch (SecurityException)
			{
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLowerInvariant().Replace("\\syswow64\\", "\\system32\\");
				result = Path.Combine(text, Utils.ModuleDirectory);
			}
			return result;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00035CC8 File Offset: 0x00033EC8
		internal static string GetDscModulePath()
		{
			string result = null;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			if (!string.IsNullOrEmpty(folderPath))
			{
				result = Path.Combine(folderPath, Utils.DscModuleDirectory);
			}
			return result;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00035CF4 File Offset: 0x00033EF4
		private static string CombineSystemModulePaths()
		{
			string systemwideModulePath = ModuleIntrinsics.GetSystemwideModulePath();
			string dscModulePath = ModuleIntrinsics.GetDscModulePath();
			if (systemwideModulePath != null && dscModulePath != null)
			{
				return dscModulePath + ";" + systemwideModulePath;
			}
			if (systemwideModulePath != null || dscModulePath != null)
			{
				return systemwideModulePath ?? dscModulePath;
			}
			return null;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00035D30 File Offset: 0x00033F30
		private static string GetExpandedEnvironmentVariable(string name, EnvironmentVariableTarget target)
		{
			string text = Environment.GetEnvironmentVariable(name, target);
			if (!string.IsNullOrEmpty(text))
			{
				text = Environment.ExpandEnvironmentVariables(text);
			}
			return text;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00035D58 File Offset: 0x00033F58
		public static string GetModulePath(string currentModulePath, string systemWideModulePath, string personalModulePath)
		{
			if (currentModulePath == null)
			{
				if (personalModulePath == null)
				{
					currentModulePath = ModuleIntrinsics.GetPersonalModulePath();
				}
				else
				{
					currentModulePath = personalModulePath;
				}
				currentModulePath += ';';
				if (systemWideModulePath == null)
				{
					currentModulePath += ModuleIntrinsics.CombineSystemModulePaths();
				}
				else
				{
					currentModulePath += systemWideModulePath;
				}
			}
			else if (systemWideModulePath != null)
			{
				if (personalModulePath == null)
				{
					if (!systemWideModulePath.Equals(currentModulePath, StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
					currentModulePath = ModuleIntrinsics.GetPersonalModulePath() + ';' + systemWideModulePath;
				}
				else
				{
					string text = personalModulePath + ';' + systemWideModulePath;
					if (!text.Equals(currentModulePath, StringComparison.OrdinalIgnoreCase) && !systemWideModulePath.Equals(currentModulePath, StringComparison.OrdinalIgnoreCase) && !personalModulePath.Equals(currentModulePath, StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
					currentModulePath = text;
				}
			}
			else
			{
				if (personalModulePath == null)
				{
					return null;
				}
				if (!personalModulePath.Equals(currentModulePath, StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				currentModulePath = personalModulePath + ';' + ModuleIntrinsics.CombineSystemModulePaths();
			}
			string dscModulePath = ModuleIntrinsics.GetDscModulePath();
			string[] array = currentModulePath.Split(new char[]
			{
				';'
			});
			bool flag = false;
			foreach (string a in array)
			{
				if (string.Equals(a, dscModulePath, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num = currentModulePath.IndexOf(ModuleIntrinsics.GetSystemwideModulePath(), StringComparison.OrdinalIgnoreCase);
				if (num != -1)
				{
					string text2 = currentModulePath.Insert(num, dscModulePath + ";");
					currentModulePath = text2;
				}
				else
				{
					currentModulePath += (currentModulePath.EndsWith(";", StringComparison.OrdinalIgnoreCase) ? dscModulePath : (";" + dscModulePath));
				}
			}
			return currentModulePath;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00035ED0 File Offset: 0x000340D0
		internal static string GetModulePath()
		{
			return ModuleIntrinsics.GetExpandedEnvironmentVariable("PSMODULEPATH", EnvironmentVariableTarget.Process);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00035EEC File Offset: 0x000340EC
		internal static void SetModulePath()
		{
			string expandedEnvironmentVariable = ModuleIntrinsics.GetExpandedEnvironmentVariable("PSMODULEPATH", EnvironmentVariableTarget.Process);
			string expandedEnvironmentVariable2 = ModuleIntrinsics.GetExpandedEnvironmentVariable("PSMODULEPATH", EnvironmentVariableTarget.Machine);
			string expandedEnvironmentVariable3 = ModuleIntrinsics.GetExpandedEnvironmentVariable("PSMODULEPATH", EnvironmentVariableTarget.User);
			string modulePath = ModuleIntrinsics.GetModulePath(expandedEnvironmentVariable, expandedEnvironmentVariable2, expandedEnvironmentVariable3);
			if (!string.IsNullOrEmpty(modulePath))
			{
				Environment.SetEnvironmentVariable("PSMODULEPATH", modulePath);
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00035F3C File Offset: 0x0003413C
		internal static IEnumerable<string> GetModulePath(bool preferSystemModulePath, ExecutionContext context)
		{
			List<string> list = new List<string>();
			string environmentVariable = Environment.GetEnvironmentVariable("PSMODULEPATH");
			if (environmentVariable == null)
			{
				ModuleIntrinsics.SetModulePath();
				environmentVariable = Environment.GetEnvironmentVariable("PSMODULEPATH");
			}
			if (preferSystemModulePath)
			{
				list.Add(ModuleIntrinsics.GetSystemwideModulePath());
			}
			if (string.IsNullOrWhiteSpace(environmentVariable))
			{
				return list;
			}
			foreach (string text in environmentVariable.Split(new char[]
			{
				';'
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				string text2 = text.Trim();
				string text3 = "";
				if (text2.StartsWith("filesystem::", StringComparison.OrdinalIgnoreCase))
				{
					text3 = text2.Remove(0, "filesystem::".Length);
				}
				try
				{
					ProviderInfo providerInfo = null;
					if (context.EngineSessionState.IsProviderLoaded(context.ProviderNames.FileSystem))
					{
						if (new Uri(text2).IsUnc || (!string.IsNullOrEmpty(text3) && new Uri(text3).IsUnc) || Directory.Exists(text2))
						{
							list.Add(text2);
							goto IL_14D;
						}
						IEnumerable<string> resolvedProviderPathFromPSPath = context.SessionState.Path.GetResolvedProviderPathFromPSPath(WildcardPattern.Escape(text2), out providerInfo);
						if (!providerInfo.NameEquals(context.ProviderNames.FileSystem))
						{
							goto IL_14D;
						}
						using (IEnumerator<string> enumerator = resolvedProviderPathFromPSPath.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string item = enumerator.Current;
								list.Add(item);
							}
							goto IL_14D;
						}
					}
					list.Add(text2);
					IL_14D:;
				}
				catch (ItemNotFoundException)
				{
				}
				catch (DriveNotFoundException)
				{
				}
				catch (UriFormatException)
				{
				}
				catch (NotSupportedException)
				{
				}
			}
			return list;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00036130 File Offset: 0x00034330
		private static void SortAndRemoveDuplicates<T>(List<T> input, Func<T, string> keyGetter)
		{
			input.Sort(delegate(T x, T y)
			{
				string strA = keyGetter(x);
				string strB = keyGetter(y);
				return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase);
			});
			bool flag = true;
			string value = null;
			List<T> list = new List<T>(input.Count);
			foreach (T t in input)
			{
				string text = keyGetter(t);
				if (flag || !text.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(t);
				}
				value = text;
				flag = false;
			}
			input.Clear();
			input.AddRange(list);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00036240 File Offset: 0x00034440
		internal static void ExportModuleMembers(PSCmdlet cmdlet, SessionStateInternal sessionState, List<WildcardPattern> functionPatterns, List<WildcardPattern> cmdletPatterns, List<WildcardPattern> aliasPatterns, List<WildcardPattern> variablePatterns, List<string> doNotExportCmdlets)
		{
			sessionState.UseExportList = true;
			if (functionPatterns != null)
			{
				IDictionary<string, FunctionInfo> functionTable = sessionState.ModuleScope.FunctionTable;
				foreach (KeyValuePair<string, FunctionInfo> keyValuePair in functionTable)
				{
					if ((keyValuePair.Value.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.None && SessionStateUtilities.MatchesAnyWildcardPattern(keyValuePair.Key, functionPatterns, false))
					{
						string text;
						if (keyValuePair.Value.CommandType == CommandTypes.Workflow)
						{
							text = StringUtil.Format(Modules.ExportingWorkflow, keyValuePair.Key);
							sessionState.ExportedWorkflows.Add((WorkflowInfo)keyValuePair.Value);
						}
						else
						{
							text = StringUtil.Format(Modules.ExportingFunction, keyValuePair.Key);
							sessionState.ExportedFunctions.Add(keyValuePair.Value);
						}
						cmdlet.WriteVerbose(text);
					}
				}
				ModuleIntrinsics.SortAndRemoveDuplicates<FunctionInfo>(sessionState.ExportedFunctions, (FunctionInfo ci) => ci.Name);
				ModuleIntrinsics.SortAndRemoveDuplicates<WorkflowInfo>(sessionState.ExportedWorkflows, (WorkflowInfo ci) => ci.Name);
			}
			if (cmdletPatterns != null)
			{
				IDictionary<string, List<CmdletInfo>> cmdletTable = sessionState.ModuleScope.CmdletTable;
				if (sessionState.Module.CompiledExports.Count > 0)
				{
					CmdletInfo[] array = sessionState.Module.CompiledExports.ToArray();
					sessionState.Module.CompiledExports.Clear();
					CmdletInfo[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						CmdletInfo element = array2[i];
						if (doNotExportCmdlets == null)
						{
							goto IL_1B2;
						}
						if (!doNotExportCmdlets.Exists((string cmdletName) => string.Equals(element.FullName, cmdletName, StringComparison.OrdinalIgnoreCase)))
						{
							goto IL_1B2;
						}
						IL_23E:
						i++;
						continue;
						IL_1B2:
						if (SessionStateUtilities.MatchesAnyWildcardPattern(element.Name, cmdletPatterns, false))
						{
							string text2 = StringUtil.Format(Modules.ExportingCmdlet, element.Name);
							cmdlet.WriteVerbose(text2);
							CmdletInfo cmdletInfo = new CmdletInfo(element.Name, element.ImplementingType, element.HelpFile, null, element.Context);
							cmdletInfo.SetModule(sessionState.Module);
							sessionState.Module.CompiledExports.Add(cmdletInfo);
							goto IL_23E;
						}
						goto IL_23E;
					}
				}
				foreach (KeyValuePair<string, List<CmdletInfo>> keyValuePair2 in cmdletTable)
				{
					CmdletInfo cmdletToImport = keyValuePair2.Value[0];
					if ((doNotExportCmdlets == null || !doNotExportCmdlets.Exists((string cmdletName) => string.Equals(cmdletToImport.FullName, cmdletName, StringComparison.OrdinalIgnoreCase))) && SessionStateUtilities.MatchesAnyWildcardPattern(keyValuePair2.Key, cmdletPatterns, false))
					{
						string text3 = StringUtil.Format(Modules.ExportingCmdlet, keyValuePair2.Key);
						cmdlet.WriteVerbose(text3);
						CmdletInfo cmdletInfo2 = new CmdletInfo(cmdletToImport.Name, cmdletToImport.ImplementingType, cmdletToImport.HelpFile, null, cmdletToImport.Context);
						cmdletInfo2.SetModule(sessionState.Module);
						sessionState.Module.CompiledExports.Add(cmdletInfo2);
					}
				}
				ModuleIntrinsics.SortAndRemoveDuplicates<CmdletInfo>(sessionState.Module.CompiledExports, (CmdletInfo ci) => ci.Name);
			}
			if (variablePatterns != null)
			{
				IDictionary<string, PSVariable> variables = sessionState.ModuleScope.Variables;
				foreach (KeyValuePair<string, PSVariable> keyValuePair3 in variables)
				{
					if (!keyValuePair3.Value.IsAllScope && Array.IndexOf<string>(PSModuleInfo._builtinVariables, keyValuePair3.Key) == -1 && SessionStateUtilities.MatchesAnyWildcardPattern(keyValuePair3.Key, variablePatterns, false))
					{
						string text4 = StringUtil.Format(Modules.ExportingVariable, keyValuePair3.Key);
						cmdlet.WriteVerbose(text4);
						sessionState.ExportedVariables.Add(keyValuePair3.Value);
					}
				}
				ModuleIntrinsics.SortAndRemoveDuplicates<PSVariable>(sessionState.ExportedVariables, (PSVariable v) => v.Name);
			}
			if (aliasPatterns != null)
			{
				IEnumerable<AliasInfo> aliasTable = sessionState.ModuleScope.AliasTable;
				if (sessionState.Module.CompiledAliasExports.Count > 0)
				{
					AliasInfo[] array3 = sessionState.Module.CompiledAliasExports.ToArray();
					foreach (AliasInfo aliasInfo in array3)
					{
						if (SessionStateUtilities.MatchesAnyWildcardPattern(aliasInfo.Name, aliasPatterns, false))
						{
							string text5 = StringUtil.Format(Modules.ExportingAlias, aliasInfo.Name);
							cmdlet.WriteVerbose(text5);
							sessionState.ExportedAliases.Add(ModuleIntrinsics.NewAliasInfo(aliasInfo, sessionState));
						}
					}
				}
				foreach (AliasInfo aliasInfo2 in aliasTable)
				{
					if ((aliasInfo2.Options & ScopedItemOptions.AllScope) == ScopedItemOptions.None && SessionStateUtilities.MatchesAnyWildcardPattern(aliasInfo2.Name, aliasPatterns, false))
					{
						string text6 = StringUtil.Format(Modules.ExportingAlias, aliasInfo2.Name);
						cmdlet.WriteVerbose(text6);
						sessionState.ExportedAliases.Add(ModuleIntrinsics.NewAliasInfo(aliasInfo2, sessionState));
					}
				}
				ModuleIntrinsics.SortAndRemoveDuplicates<AliasInfo>(sessionState.ExportedAliases, (AliasInfo ci) => ci.Name);
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000367E4 File Offset: 0x000349E4
		private static AliasInfo NewAliasInfo(AliasInfo alias, SessionStateInternal sessionState)
		{
			AliasInfo aliasInfo = new AliasInfo(alias.Name, alias.Definition, alias.Context, alias.Options);
			aliasInfo.SetModule(sessionState.Module);
			return aliasInfo;
		}

		// Token: 0x040003FE RID: 1022
		private const int MaxModuleNestingDepth = 10;

		// Token: 0x040003FF RID: 1023
		[TraceSource("Modules", "Module loading and analysis")]
		internal static PSTraceSource Tracer = PSTraceSource.GetTracer("Modules", "Module loading and analysis");

		// Token: 0x04000400 RID: 1024
		private readonly ExecutionContext _context;

		// Token: 0x04000401 RID: 1025
		private readonly Dictionary<string, PSModuleInfo> _moduleTable = new Dictionary<string, PSModuleInfo>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000402 RID: 1026
		private int _moduleNestingDepth;

		// Token: 0x04000403 RID: 1027
		internal static string[] PSModuleProcessableExtensions = new string[]
		{
			".psd1",
			".ps1",
			".psm1",
			".cdxml",
			".xaml",
			".dll"
		};

		// Token: 0x04000404 RID: 1028
		internal static string[] PSModuleExtensions = new string[]
		{
			".psd1",
			".psm1",
			".cdxml",
			".xaml",
			".dll"
		};
	}
}
