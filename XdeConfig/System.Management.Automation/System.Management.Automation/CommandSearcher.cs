using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Provider;

namespace System.Management.Automation
{
	// Token: 0x0200005B RID: 91
	internal class CommandSearcher : IEnumerable<CommandInfo>, IEnumerable, IEnumerator<CommandInfo>, IDisposable, IEnumerator
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x00017192 File Offset: 0x00015392
		internal CommandSearcher(string commandName, SearchResolutionOptions options, CommandTypes commandTypes, ExecutionContext context)
		{
			this.commandName = commandName;
			this._context = context;
			this.commandResolutionOptions = options;
			this.commandTypes = commandTypes;
			this.Reset();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000171CF File Offset: 0x000153CF
		IEnumerator<CommandInfo> IEnumerable<CommandInfo>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000171D2 File Offset: 0x000153D2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000171D8 File Offset: 0x000153D8
		public bool MoveNext()
		{
			this._currentMatch = null;
			if (this.currentState == CommandSearcher.SearchState.SearchingAliases)
			{
				this._currentMatch = this.SearchForAliases();
				if (this._currentMatch != null && SessionState.IsVisible(this._commandOrigin, this._currentMatch))
				{
					return true;
				}
				this._currentMatch = null;
				this.currentState = CommandSearcher.SearchState.SearchingFunctions;
			}
			if (this.currentState == CommandSearcher.SearchState.SearchingFunctions)
			{
				this._currentMatch = this.SearchForFunctions();
				if (this._currentMatch != null)
				{
					return true;
				}
				this.currentState = CommandSearcher.SearchState.SearchingCmdlets;
			}
			if (this.currentState == CommandSearcher.SearchState.SearchingCmdlets)
			{
				this._currentMatch = this.SearchForCmdlets();
				if (this._currentMatch != null)
				{
					return true;
				}
				this.currentState = CommandSearcher.SearchState.SearchingBuiltinScripts;
			}
			if (this.currentState == CommandSearcher.SearchState.SearchingBuiltinScripts)
			{
				this._currentMatch = this.SearchForBuiltinScripts();
				if (this._currentMatch != null)
				{
					return true;
				}
				this.currentState = CommandSearcher.SearchState.StartSearchingForExternalCommands;
			}
			if (this.currentState == CommandSearcher.SearchState.StartSearchingForExternalCommands)
			{
				if ((this.commandTypes & (CommandTypes.ExternalScript | CommandTypes.Application)) == (CommandTypes)0)
				{
					return false;
				}
				if (this._commandOrigin == CommandOrigin.Runspace && this.commandName.IndexOfAny(CommandSearcher._pathSeparators) >= 0)
				{
					bool flag = false;
					if ((this._context.EngineSessionState.Applications.Count == 1 && this._context.EngineSessionState.Applications[0].Equals("*", StringComparison.OrdinalIgnoreCase)) || (this._context.EngineSessionState.Scripts.Count == 1 && this._context.EngineSessionState.Scripts[0].Equals("*", StringComparison.OrdinalIgnoreCase)))
					{
						flag = true;
					}
					else
					{
						foreach (string path in this._context.EngineSessionState.Applications)
						{
							if (CommandSearcher.checkPath(path, this.commandName))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							foreach (string path2 in this._context.EngineSessionState.Scripts)
							{
								if (CommandSearcher.checkPath(path2, this.commandName))
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				this.currentState = CommandSearcher.SearchState.PowerShellPathResolution;
				this._currentMatch = this.ProcessBuiltinScriptState();
				if (this._currentMatch != null)
				{
					this.currentState = CommandSearcher.SearchState.QualifiedFileSystemPath;
					return true;
				}
			}
			if (this.currentState == CommandSearcher.SearchState.PowerShellPathResolution)
			{
				this.currentState = CommandSearcher.SearchState.QualifiedFileSystemPath;
				this._currentMatch = this.ProcessPathResolutionState();
				if (this._currentMatch != null)
				{
					return true;
				}
			}
			if (this.currentState == CommandSearcher.SearchState.QualifiedFileSystemPath || this.currentState == CommandSearcher.SearchState.PathSearch)
			{
				this._currentMatch = this.ProcessQualifiedFileSystemState();
				if (this._currentMatch != null)
				{
					return true;
				}
			}
			if (this.currentState == CommandSearcher.SearchState.PathSearch)
			{
				this.currentState = CommandSearcher.SearchState.PowerShellRelativePath;
				this._currentMatch = this.ProcessPathSearchState();
				if (this._currentMatch != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000174AC File Offset: 0x000156AC
		private CommandInfo SearchForAliases()
		{
			CommandInfo result = null;
			if (this._context.EngineSessionState != null && (this.commandTypes & CommandTypes.Alias) != (CommandTypes)0)
			{
				result = this.GetNextAlias();
			}
			return result;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000174DC File Offset: 0x000156DC
		private CommandInfo SearchForFunctions()
		{
			CommandInfo result = null;
			if (this._context.EngineSessionState != null && (this.commandTypes & (CommandTypes.Function | CommandTypes.Filter | CommandTypes.Workflow | CommandTypes.Configuration)) != (CommandTypes)0)
			{
				result = this.GetNextFunction();
			}
			return result;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00017510 File Offset: 0x00015710
		private CommandInfo SearchForCmdlets()
		{
			CommandInfo result = null;
			if ((this.commandTypes & CommandTypes.Cmdlet) != (CommandTypes)0)
			{
				result = this.GetNextCmdlet();
			}
			return result;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00017534 File Offset: 0x00015734
		private CommandInfo SearchForBuiltinScripts()
		{
			CommandInfo result = null;
			if ((this.commandTypes & CommandTypes.Script) != (CommandTypes)0)
			{
				result = this.GetNextBuiltinScript();
			}
			return result;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00017558 File Offset: 0x00015758
		private CommandInfo ProcessBuiltinScriptState()
		{
			CommandInfo result = null;
			if (this._context.EngineSessionState != null && this._context.EngineSessionState.ProviderCount > 0 && CommandSearcher.IsQualifiedPSPath(this.commandName))
			{
				result = this.GetNextFromPath();
			}
			return result;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001759C File Offset: 0x0001579C
		private CommandInfo ProcessPathResolutionState()
		{
			CommandInfo result = null;
			try
			{
				if (Path.IsPathRooted(this.commandName) && File.Exists(this.commandName))
				{
					try
					{
						result = this.GetInfoFromPath(this.commandName);
					}
					catch (FileLoadException)
					{
					}
					catch (FormatException)
					{
					}
					catch (MetadataException)
					{
					}
				}
			}
			catch (ArgumentException)
			{
			}
			return result;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00017618 File Offset: 0x00015818
		private CommandInfo ProcessQualifiedFileSystemState()
		{
			try
			{
				this.setupPathSearcher();
			}
			catch (ArgumentException)
			{
				this.currentState = CommandSearcher.SearchState.NoMoreMatches;
				throw;
			}
			catch (PathTooLongException)
			{
				this.currentState = CommandSearcher.SearchState.NoMoreMatches;
				throw;
			}
			CommandInfo commandInfo = null;
			this.currentState = CommandSearcher.SearchState.PathSearch;
			if (this.canDoPathLookup)
			{
				try
				{
					while (commandInfo == null && this.pathSearcher.MoveNext())
					{
						commandInfo = this.GetInfoFromPath(((IEnumerator<string>)this.pathSearcher).Current);
					}
				}
				catch (InvalidOperationException)
				{
				}
			}
			return commandInfo;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000176A8 File Offset: 0x000158A8
		private CommandInfo ProcessPathSearchState()
		{
			CommandInfo result = null;
			string text = this.DoPowerShellRelativePathLookup();
			if (!string.IsNullOrEmpty(text))
			{
				result = this.GetInfoFromPath(text);
			}
			return result;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x000176CF File Offset: 0x000158CF
		CommandInfo IEnumerator<CommandInfo>.Current
		{
			get
			{
				if ((this.currentState == CommandSearcher.SearchState.SearchingAliases && this._currentMatch == null) || this.currentState == CommandSearcher.SearchState.NoMoreMatches || this._currentMatch == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				return this._currentMatch;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x000176FF File Offset: 0x000158FF
		object IEnumerator.Current
		{
			get
			{
				return ((IEnumerator<CommandInfo>)this).Current;
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00017707 File Offset: 0x00015907
		public void Dispose()
		{
			if (this.pathSearcher != null)
			{
				this.pathSearcher.Dispose();
				this.pathSearcher = null;
			}
			this.Reset();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00017730 File Offset: 0x00015930
		private CommandInfo GetNextFromPath()
		{
			CommandInfo result = null;
			CommandDiscovery.discoveryTracer.WriteLine("The name appears to be a qualified path: {0}", new object[]
			{
				this.commandName
			});
			CommandDiscovery.discoveryTracer.WriteLine("Trying to resolve the path as an PSPath", new object[0]);
			Collection<string> collection = new Collection<string>();
			try
			{
				ProviderInfo providerInfo;
				CmdletProvider cmdletProvider;
				collection = this._context.LocationGlobber.GetGlobbedProviderPathsFromMonadPath(this.commandName, false, out providerInfo, out cmdletProvider);
			}
			catch (ItemNotFoundException)
			{
				CommandDiscovery.discoveryTracer.TraceError("The path could not be found: {0}", new object[]
				{
					this.commandName
				});
			}
			catch (DriveNotFoundException)
			{
				CommandDiscovery.discoveryTracer.TraceError("A drive could not be found for the path: {0}", new object[]
				{
					this.commandName
				});
			}
			catch (ProviderNotFoundException)
			{
				CommandDiscovery.discoveryTracer.TraceError("A provider could not be found for the path: {0}", new object[]
				{
					this.commandName
				});
			}
			catch (InvalidOperationException)
			{
				CommandDiscovery.discoveryTracer.TraceError("The path specified a home directory, but the provider home directory was not set. {0}", new object[]
				{
					this.commandName
				});
			}
			catch (ProviderInvocationException ex)
			{
				CommandDiscovery.discoveryTracer.TraceError("The provider associated with the path '{0}' encountered an error: {1}", new object[]
				{
					this.commandName,
					ex.Message
				});
			}
			catch (PSNotSupportedException)
			{
				CommandDiscovery.discoveryTracer.TraceError("The provider associated with the path '{0}' does not implement ContainerCmdletProvider", new object[]
				{
					this.commandName
				});
			}
			if (collection.Count > 1)
			{
				CommandDiscovery.discoveryTracer.TraceError("The path resolved to more than one result so this path cannot be used.", new object[0]);
			}
			else if (collection.Count == 1 && File.Exists(collection[0]))
			{
				string text = collection[0];
				CommandDiscovery.discoveryTracer.WriteLine("Path resolved to: {0}", new object[]
				{
					text
				});
				result = this.GetInfoFromPath(text);
			}
			return result;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001794C File Offset: 0x00015B4C
		private static bool checkPath(string path, string commandName)
		{
			return path.StartsWith(commandName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00017958 File Offset: 0x00015B58
		private CommandInfo GetInfoFromPath(string path)
		{
			CommandInfo result = null;
			if (!File.Exists(path))
			{
				CommandDiscovery.discoveryTracer.TraceError("The path does not exist: {0}", new object[]
				{
					path
				});
			}
			else
			{
				string text = null;
				try
				{
					text = Path.GetExtension(path);
				}
				catch (ArgumentException)
				{
				}
				if (text == null)
				{
					result = null;
				}
				else if (string.Equals(text, ".ps1", StringComparison.OrdinalIgnoreCase))
				{
					if ((this.commandTypes & CommandTypes.ExternalScript) != (CommandTypes)0)
					{
						string fileName = Path.GetFileName(path);
						CommandDiscovery.discoveryTracer.WriteLine("Command Found: path ({0}) is a script with name: {1}", new object[]
						{
							path,
							fileName
						});
						result = new ExternalScriptInfo(fileName, path, this._context);
					}
				}
				else if ((this.commandTypes & CommandTypes.Application) != (CommandTypes)0)
				{
					string fileName2 = Path.GetFileName(path);
					CommandDiscovery.discoveryTracer.WriteLine("Command Found: path ({0}) is an application with name: {1}", new object[]
					{
						path,
						fileName2
					});
					result = new ApplicationInfo(fileName2, path, this._context);
				}
			}
			if (this.ShouldSkipCommandResolutionForConstrainedLanguage(result, this._context))
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00017A64 File Offset: 0x00015C64
		private CommandInfo GetNextAlias()
		{
			CommandInfo commandInfo = null;
			if ((this.commandResolutionOptions & SearchResolutionOptions.ResolveAliasPatterns) != SearchResolutionOptions.None)
			{
				if (this.matchingAlias == null)
				{
					Collection<AliasInfo> collection = new Collection<AliasInfo>();
					WildcardPattern wildcardPattern = new WildcardPattern(this.commandName, WildcardOptions.IgnoreCase);
					foreach (KeyValuePair<string, AliasInfo> keyValuePair in this._context.EngineSessionState.GetAliasTable())
					{
						if (wildcardPattern.IsMatch(keyValuePair.Key))
						{
							collection.Add(keyValuePair.Value);
						}
					}
					AliasInfo aliasFromModules = this.GetAliasFromModules(this.commandName);
					if (aliasFromModules != null)
					{
						collection.Add(aliasFromModules);
					}
					this.matchingAlias = collection.GetEnumerator();
				}
				if (!this.matchingAlias.MoveNext())
				{
					this.currentState = CommandSearcher.SearchState.SearchingFunctions;
					this.matchingAlias = null;
				}
				else
				{
					commandInfo = this.matchingAlias.Current;
				}
			}
			else
			{
				this.currentState = CommandSearcher.SearchState.SearchingFunctions;
				commandInfo = (this._context.EngineSessionState.GetAlias(this.commandName) ?? this.GetAliasFromModules(this.commandName));
			}
			if (this.ShouldSkipCommandResolutionForConstrainedLanguage(commandInfo, this._context))
			{
				commandInfo = null;
			}
			if (commandInfo != null)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Alias found: {0}  {1}", new object[]
				{
					commandInfo.Name,
					commandInfo.Definition
				});
			}
			return commandInfo;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00017BC4 File Offset: 0x00015DC4
		private CommandInfo GetNextFunction()
		{
			CommandInfo result = null;
			if ((this.commandResolutionOptions & SearchResolutionOptions.ResolveFunctionPatterns) != SearchResolutionOptions.None)
			{
				if (this.matchingFunctionEnumerator == null)
				{
					Collection<CommandInfo> collection = new Collection<CommandInfo>();
					WildcardPattern wildcardPattern = new WildcardPattern(this.commandName, WildcardOptions.IgnoreCase);
					foreach (object obj in this._context.EngineSessionState.GetFunctionTable())
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (wildcardPattern.IsMatch((string)dictionaryEntry.Key))
						{
							collection.Add((CommandInfo)dictionaryEntry.Value);
						}
					}
					CommandInfo functionFromModules = this.GetFunctionFromModules(this.commandName);
					if (functionFromModules != null)
					{
						collection.Add(functionFromModules);
					}
					this.matchingFunctionEnumerator = collection.GetEnumerator();
				}
				if (!this.matchingFunctionEnumerator.MoveNext())
				{
					this.currentState = CommandSearcher.SearchState.SearchingCmdlets;
					this.matchingFunctionEnumerator = null;
				}
				else
				{
					result = this.matchingFunctionEnumerator.Current;
				}
			}
			else
			{
				this.currentState = CommandSearcher.SearchState.SearchingCmdlets;
				result = this.GetFunction(this.commandName);
			}
			if (this.ShouldSkipCommandResolutionForConstrainedLanguage(result, this._context))
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00017CF4 File Offset: 0x00015EF4
		private bool ShouldSkipCommandResolutionForConstrainedLanguage(CommandInfo result, ExecutionContext executionContext)
		{
			return result != null && ((result.DefiningLanguageMode == PSLanguageMode.ConstrainedLanguage && executionContext.LanguageMode == PSLanguageMode.FullLanguage) || (result is FunctionInfo && executionContext.LanguageMode == PSLanguageMode.ConstrainedLanguage && result.DefiningLanguageMode == PSLanguageMode.FullLanguage && executionContext.Debugger != null && executionContext.Debugger.InBreakpoint && !executionContext.TopLevelSessionState.GetFunctionTableAtScope("GLOBAL").ContainsKey(result.Name)));
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00017D90 File Offset: 0x00015F90
		private AliasInfo GetAliasFromModules(string command)
		{
			AliasInfo result = null;
			if (command.IndexOf('\\') > 0)
			{
				PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(command);
				if (instance != null && !string.IsNullOrEmpty(instance.PSSnapInName))
				{
					PSModuleInfo importedModuleByName = this.GetImportedModuleByName(instance.PSSnapInName);
					if (importedModuleByName != null)
					{
						importedModuleByName.ExportedAliases.TryGetValue(instance.ShortName, out result);
					}
				}
			}
			return result;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00017DE8 File Offset: 0x00015FE8
		private CommandInfo GetFunctionFromModules(string command)
		{
			FunctionInfo result = null;
			if (command.IndexOf('\\') > 0)
			{
				PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(command);
				if (instance != null && !string.IsNullOrEmpty(instance.PSSnapInName))
				{
					PSModuleInfo importedModuleByName = this.GetImportedModuleByName(instance.PSSnapInName);
					if (importedModuleByName != null)
					{
						importedModuleByName.ExportedFunctions.TryGetValue(instance.ShortName, out result);
					}
				}
			}
			return result;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00017E40 File Offset: 0x00016040
		private PSModuleInfo GetImportedModuleByName(string moduleName)
		{
			PSModuleInfo psmoduleInfo = null;
			List<PSModuleInfo> modules = this._context.Modules.GetModules(new string[]
			{
				moduleName
			}, false);
			if (modules != null && modules.Count > 0)
			{
				foreach (PSModuleInfo psmoduleInfo2 in modules)
				{
					if (this._context.previousModuleImported.ContainsKey(psmoduleInfo2.Name) && (string)this._context.previousModuleImported[psmoduleInfo2.Name] == psmoduleInfo2.Path)
					{
						psmoduleInfo = psmoduleInfo2;
						break;
					}
				}
				if (psmoduleInfo == null)
				{
					psmoduleInfo = modules[0];
				}
			}
			return psmoduleInfo;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00017F08 File Offset: 0x00016108
		private CommandInfo GetFunction(string function)
		{
			CommandInfo commandInfo = this._context.EngineSessionState.GetFunction(function);
			if (commandInfo != null)
			{
				if (commandInfo is FilterInfo)
				{
					CommandDiscovery.discoveryTracer.WriteLine("Filter found: {0}", new object[]
					{
						function
					});
				}
				else if (commandInfo is ConfigurationInfo)
				{
					CommandDiscovery.discoveryTracer.WriteLine("Configuration found: {0}", new object[]
					{
						function
					});
				}
				else
				{
					CommandDiscovery.discoveryTracer.WriteLine("Function found: {0}  {1}", new object[]
					{
						function
					});
				}
			}
			else
			{
				commandInfo = this.GetFunctionFromModules(function);
			}
			return commandInfo;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00017F9C File Offset: 0x0001619C
		private CmdletInfo GetNextCmdlet()
		{
			CmdletInfo result = null;
			if (this.matchingCmdlet == null)
			{
				if ((this.commandResolutionOptions & SearchResolutionOptions.CommandNameIsPattern) != SearchResolutionOptions.None)
				{
					Collection<CmdletInfo> collection = new Collection<CmdletInfo>();
					PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(this.commandName);
					if (instance == null)
					{
						return result;
					}
					WildcardPattern wildcardPattern = new WildcardPattern(instance.ShortName, WildcardOptions.IgnoreCase);
					SessionStateInternal engineSessionState = this._context.EngineSessionState;
					foreach (List<CmdletInfo> list in engineSessionState.GetCmdletTable().Values)
					{
						foreach (CmdletInfo cmdletInfo in list)
						{
							if (wildcardPattern.IsMatch(cmdletInfo.Name) && (string.IsNullOrEmpty(instance.PSSnapInName) || instance.PSSnapInName.Equals(cmdletInfo.ModuleName, StringComparison.OrdinalIgnoreCase)))
							{
								collection.Add(cmdletInfo);
							}
						}
					}
					this.matchingCmdlet = collection.GetEnumerator();
				}
				else
				{
					this.matchingCmdlet = this._context.CommandDiscovery.GetCmdletInfo(this.commandName, (this.commandResolutionOptions & SearchResolutionOptions.SearchAllScopes) != SearchResolutionOptions.None);
				}
			}
			if (!this.matchingCmdlet.MoveNext())
			{
				this.currentState = CommandSearcher.SearchState.SearchingBuiltinScripts;
				this.matchingCmdlet = null;
			}
			else
			{
				result = this.matchingCmdlet.Current;
			}
			return CommandSearcher.traceResult(result);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00018114 File Offset: 0x00016314
		private static CmdletInfo traceResult(CmdletInfo result)
		{
			if (result != null)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Cmdlet found: {0}  {1}", new object[]
				{
					result.Name,
					result.ImplementingType
				});
			}
			return result;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00018150 File Offset: 0x00016350
		private ScriptInfo GetNextBuiltinScript()
		{
			ScriptInfo scriptInfo = null;
			if ((this.commandResolutionOptions & SearchResolutionOptions.CommandNameIsPattern) != SearchResolutionOptions.None)
			{
				if (this.matchingScript == null)
				{
					Collection<string> collection = new Collection<string>();
					WildcardPattern wildcardPattern = new WildcardPattern(this.commandName, WildcardOptions.IgnoreCase);
					WildcardPattern wildcardPattern2 = new WildcardPattern(this.commandName + ".ps1", WildcardOptions.IgnoreCase);
					foreach (string text in this._context.CommandDiscovery.ScriptCache.Keys)
					{
						if (wildcardPattern.IsMatch(text) || wildcardPattern2.IsMatch(text))
						{
							collection.Add(text);
						}
					}
					this.matchingScript = collection.GetEnumerator();
				}
				if (!this.matchingScript.MoveNext())
				{
					this.currentState = CommandSearcher.SearchState.StartSearchingForExternalCommands;
					this.matchingScript = null;
				}
				else
				{
					scriptInfo = this._context.CommandDiscovery.GetScriptInfo(this.matchingScript.Current);
				}
			}
			else
			{
				this.currentState = CommandSearcher.SearchState.StartSearchingForExternalCommands;
				scriptInfo = (this._context.CommandDiscovery.GetScriptInfo(this.commandName) ?? this._context.CommandDiscovery.GetScriptInfo(this.commandName + ".ps1"));
			}
			if (scriptInfo != null)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Script found: {0}", new object[]
				{
					scriptInfo.Name
				});
			}
			return scriptInfo;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000182C0 File Offset: 0x000164C0
		private string DoPowerShellRelativePathLookup()
		{
			string result = null;
			if (this._context.EngineSessionState != null && this._context.EngineSessionState.ProviderCount > 0 && (this.commandName[0] == '.' || this.commandName[0] == '~' || this.commandName[0] == '\\'))
			{
				using (CommandDiscovery.discoveryTracer.TraceScope("{0} appears to be a relative path. Trying to resolve relative path", new object[]
				{
					this.commandName
				}))
				{
					result = this.ResolvePSPath(this.commandName);
				}
			}
			return result;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001836C File Offset: 0x0001656C
		private string ResolvePSPath(string path)
		{
			string text = null;
			try
			{
				ProviderInfo providerInfo = null;
				string text2 = null;
				if (WildcardPattern.ContainsWildcardCharacters(path))
				{
					CmdletProvider cmdletProvider;
					Collection<string> globbedProviderPathsFromMonadPath = this._context.LocationGlobber.GetGlobbedProviderPathsFromMonadPath(path, false, out providerInfo, out cmdletProvider);
					if (globbedProviderPathsFromMonadPath.Count == 0)
					{
						text2 = null;
						CommandDiscovery.discoveryTracer.TraceError("The relative path with wildcard did not resolve to valid path. {0}", new object[]
						{
							path
						});
					}
					else if (globbedProviderPathsFromMonadPath.Count > 1)
					{
						text2 = null;
						CommandDiscovery.discoveryTracer.TraceError("The relative path with wildcard resolved to mutiple paths. {0}", new object[]
						{
							path
						});
					}
					else
					{
						text2 = globbedProviderPathsFromMonadPath[0];
					}
				}
				if (text2 == null || providerInfo == null)
				{
					text2 = this._context.LocationGlobber.GetProviderPath(path, out providerInfo);
				}
				if (providerInfo.NameEquals(this._context.ProviderNames.FileSystem))
				{
					text = text2;
					CommandDiscovery.discoveryTracer.WriteLine("The relative path was resolved to: {0}", new object[]
					{
						text
					});
				}
				else
				{
					CommandDiscovery.discoveryTracer.TraceError("The relative path was not a file system path. {0}", new object[]
					{
						path
					});
				}
			}
			catch (InvalidOperationException)
			{
				CommandDiscovery.discoveryTracer.TraceError("The home path was not specified for the provider. {0}", new object[]
				{
					path
				});
			}
			catch (ProviderInvocationException ex)
			{
				CommandDiscovery.discoveryTracer.TraceError("While resolving the path, \"{0}\", an error was encountered by the provider: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
			catch (ItemNotFoundException)
			{
				CommandDiscovery.discoveryTracer.TraceError("The path does not exist: {0}", new object[]
				{
					path
				});
			}
			catch (DriveNotFoundException ex2)
			{
				CommandDiscovery.discoveryTracer.TraceError("The drive does not exist: {0}", new object[]
				{
					ex2.ItemName
				});
			}
			return text;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00018544 File Offset: 0x00016744
		internal IEnumerable<string> ConstructSearchPatternsFromName(string name)
		{
			Collection<string> collection = new Collection<string>();
			bool flag = false;
			if (!string.IsNullOrEmpty(Path.GetExtension(name)))
			{
				collection.Add(name);
				flag = true;
			}
			if ((this.commandTypes & CommandTypes.ExternalScript) != (CommandTypes)0)
			{
				collection.Add(name + ".ps1");
				collection.Add(name + ".psm1");
				collection.Add(name + ".psd1");
			}
			if ((this.commandTypes & CommandTypes.Application) != (CommandTypes)0)
			{
				foreach (string str in CommandDiscovery.PathExtensions)
				{
					collection.Add(name + str);
				}
			}
			if (!flag)
			{
				collection.Add(name);
			}
			return collection;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00018608 File Offset: 0x00016808
		private static bool IsQualifiedPSPath(string commandName)
		{
			return LocationGlobber.IsAbsolutePath(commandName) || LocationGlobber.IsProviderQualifiedPath(commandName) || LocationGlobber.IsHomePath(commandName) || LocationGlobber.IsProviderDirectPath(commandName);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00018638 File Offset: 0x00016838
		private static CommandSearcher.CanDoPathLookupResult CanDoPathLookup(string possiblePath)
		{
			CommandSearcher.CanDoPathLookupResult result = CommandSearcher.CanDoPathLookupResult.Yes;
			if (WildcardPattern.ContainsWildcardCharacters(possiblePath))
			{
				result = CommandSearcher.CanDoPathLookupResult.WildcardCharacters;
			}
			else
			{
				try
				{
					if (Path.IsPathRooted(possiblePath))
					{
						return CommandSearcher.CanDoPathLookupResult.PathIsRooted;
					}
				}
				catch (ArgumentException)
				{
					return CommandSearcher.CanDoPathLookupResult.IllegalCharacters;
				}
				if (possiblePath.IndexOfAny(Utils.DirectorySeparators) != -1)
				{
					result = CommandSearcher.CanDoPathLookupResult.DirectorySeparator;
				}
				else if (possiblePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
				{
					result = CommandSearcher.CanDoPathLookupResult.IllegalCharacters;
				}
			}
			return result;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001869C File Offset: 0x0001689C
		private void setupPathSearcher()
		{
			if (this.pathSearcher != null)
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet = this._context.CommandDiscovery.GetAllowedExtensionsFromPathExt();
			hashSet.Add(".ps1");
			if ((this.commandResolutionOptions & SearchResolutionOptions.CommandNameIsPattern) != SearchResolutionOptions.None)
			{
				this.canDoPathLookup = true;
				this.canDoPathLookupResult = CommandSearcher.CanDoPathLookupResult.Yes;
				Collection<string> patterns = new Collection<string>
				{
					this.commandName
				};
				this.pathSearcher = new CommandPathSearch(patterns, this._context.CommandDiscovery.GetLookupDirectoryPaths(), hashSet, false, this._context);
				return;
			}
			this.canDoPathLookupResult = CommandSearcher.CanDoPathLookup(this.commandName);
			if (this.canDoPathLookupResult == CommandSearcher.CanDoPathLookupResult.Yes)
			{
				this.canDoPathLookup = true;
				this.pathSearcher = new CommandPathSearch(this.ConstructSearchPatternsFromName(this.commandName), this._context.CommandDiscovery.GetLookupDirectoryPaths(), hashSet, false, this._context);
				return;
			}
			if (this.canDoPathLookupResult != CommandSearcher.CanDoPathLookupResult.PathIsRooted)
			{
				if (this.canDoPathLookupResult == CommandSearcher.CanDoPathLookupResult.DirectorySeparator)
				{
					this.canDoPathLookup = true;
					string text = Path.GetDirectoryName(this.commandName);
					text = this.ResolvePSPath(text);
					CommandDiscovery.discoveryTracer.WriteLine("The path is relative, so only doing the lookup in the specified directory: {0}", new object[]
					{
						text
					});
					if (text == null)
					{
						this.canDoPathLookup = false;
						return;
					}
					Collection<string> lookupPaths = new Collection<string>
					{
						text
					};
					string fileName = Path.GetFileName(this.commandName);
					if (!string.IsNullOrEmpty(fileName))
					{
						this.pathSearcher = new CommandPathSearch(this.ConstructSearchPatternsFromName(fileName), lookupPaths, hashSet, false, this._context);
						return;
					}
					this.canDoPathLookup = false;
				}
				return;
			}
			this.canDoPathLookup = true;
			string directoryName = Path.GetDirectoryName(this.commandName);
			Collection<string> lookupPaths2 = new Collection<string>
			{
				directoryName
			};
			CommandDiscovery.discoveryTracer.WriteLine("The path is rooted, so only doing the lookup in the specified directory: {0}", new object[]
			{
				directoryName
			});
			string fileName2 = Path.GetFileName(this.commandName);
			if (!string.IsNullOrEmpty(fileName2))
			{
				this.pathSearcher = new CommandPathSearch(this.ConstructSearchPatternsFromName(fileName2), lookupPaths2, hashSet, false, this._context);
				return;
			}
			this.canDoPathLookup = false;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000188A8 File Offset: 0x00016AA8
		public void Reset()
		{
			if (this._commandOrigin == CommandOrigin.Runspace)
			{
				if (this._context.EngineSessionState.Applications.Count == 0)
				{
					this.commandTypes &= ~CommandTypes.Application;
				}
				if (this._context.EngineSessionState.Scripts.Count == 0)
				{
					this.commandTypes &= ~CommandTypes.ExternalScript;
				}
			}
			if (this.pathSearcher != null)
			{
				this.pathSearcher.Reset();
			}
			this._currentMatch = null;
			this.currentState = CommandSearcher.SearchState.SearchingAliases;
			this.matchingAlias = null;
			this.matchingCmdlet = null;
			this.matchingScript = null;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001893F File Offset: 0x00016B3F
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00018947 File Offset: 0x00016B47
		internal CommandOrigin CommandOrigin
		{
			get
			{
				return this._commandOrigin;
			}
			set
			{
				this._commandOrigin = value;
			}
		}

		// Token: 0x040001DF RID: 479
		private static readonly char[] _pathSeparators = new char[]
		{
			'\\',
			'/',
			':'
		};

		// Token: 0x040001E0 RID: 480
		private IEnumerator<CmdletInfo> matchingCmdlet;

		// Token: 0x040001E1 RID: 481
		private IEnumerator<string> matchingScript;

		// Token: 0x040001E2 RID: 482
		private string commandName;

		// Token: 0x040001E3 RID: 483
		private SearchResolutionOptions commandResolutionOptions;

		// Token: 0x040001E4 RID: 484
		private CommandTypes commandTypes = CommandTypes.All;

		// Token: 0x040001E5 RID: 485
		private CommandPathSearch pathSearcher;

		// Token: 0x040001E6 RID: 486
		private ExecutionContext _context;

		// Token: 0x040001E7 RID: 487
		private CommandOrigin _commandOrigin = CommandOrigin.Internal;

		// Token: 0x040001E8 RID: 488
		private IEnumerator<AliasInfo> matchingAlias;

		// Token: 0x040001E9 RID: 489
		private IEnumerator<CommandInfo> matchingFunctionEnumerator;

		// Token: 0x040001EA RID: 490
		private CommandInfo _currentMatch;

		// Token: 0x040001EB RID: 491
		private bool canDoPathLookup;

		// Token: 0x040001EC RID: 492
		private CommandSearcher.CanDoPathLookupResult canDoPathLookupResult;

		// Token: 0x040001ED RID: 493
		private CommandSearcher.SearchState currentState;

		// Token: 0x0200005C RID: 92
		private enum CanDoPathLookupResult
		{
			// Token: 0x040001EF RID: 495
			Yes,
			// Token: 0x040001F0 RID: 496
			PathIsRooted,
			// Token: 0x040001F1 RID: 497
			WildcardCharacters,
			// Token: 0x040001F2 RID: 498
			DirectorySeparator,
			// Token: 0x040001F3 RID: 499
			IllegalCharacters
		}

		// Token: 0x0200005D RID: 93
		private enum SearchState
		{
			// Token: 0x040001F5 RID: 501
			SearchingAliases,
			// Token: 0x040001F6 RID: 502
			SearchingFunctions,
			// Token: 0x040001F7 RID: 503
			SearchingCmdlets,
			// Token: 0x040001F8 RID: 504
			SearchingBuiltinScripts,
			// Token: 0x040001F9 RID: 505
			StartSearchingForExternalCommands,
			// Token: 0x040001FA RID: 506
			PowerShellPathResolution,
			// Token: 0x040001FB RID: 507
			QualifiedFileSystemPath,
			// Token: 0x040001FC RID: 508
			PathSearch,
			// Token: 0x040001FD RID: 509
			GetPathSearch,
			// Token: 0x040001FE RID: 510
			PowerShellRelativePath,
			// Token: 0x040001FF RID: 511
			NoMoreMatches
		}
	}
}
