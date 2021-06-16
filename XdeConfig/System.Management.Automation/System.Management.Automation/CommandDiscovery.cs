using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Sqm;
using System.Security;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x02000055 RID: 85
	internal class CommandDiscovery
	{
		// Token: 0x0600049A RID: 1178 RVA: 0x0001459C File Offset: 0x0001279C
		internal CommandDiscovery(ExecutionContext context)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			this._context = context;
			CommandDiscovery.discoveryTracer.ShowHeaders = false;
			this.cachedScriptInfo = new Dictionary<string, ScriptInfo>(StringComparer.OrdinalIgnoreCase);
			this.LoadScriptInfo();
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00014628 File Offset: 0x00012828
		private void AddCmdletToCache(CmdletConfigurationEntry entry)
		{
			if (!this.IsSpecialCmdlet(entry.ImplementingType))
			{
				CmdletInfo cmdletInfo = this.NewCmdletInfo(entry, SessionStateEntryVisibility.Public);
				this.AddCmdletInfoToCache(cmdletInfo.Name, cmdletInfo, true);
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001465C File Offset: 0x0001285C
		private bool IsSpecialCmdlet(Type implementingType)
		{
			return string.Equals(implementingType.FullName, "Microsoft.PowerShell.Commands.OutLineOutputCommand", StringComparison.OrdinalIgnoreCase) || string.Equals(implementingType.FullName, "Microsoft.PowerShell.Commands.FormatDefaultCommand", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00014694 File Offset: 0x00012894
		private CmdletInfo NewCmdletInfo(CmdletConfigurationEntry entry, SessionStateEntryVisibility visibility)
		{
			return new CmdletInfo(entry.Name, entry.ImplementingType, entry.HelpFileName, entry.PSSnapIn, this._context)
			{
				Visibility = visibility
			};
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000146CD File Offset: 0x000128CD
		private CmdletInfo NewCmdletInfo(SessionStateCmdletEntry entry)
		{
			return CommandDiscovery.NewCmdletInfo(entry, this._context);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000146DC File Offset: 0x000128DC
		internal static CmdletInfo NewCmdletInfo(SessionStateCmdletEntry entry, ExecutionContext context)
		{
			CmdletInfo cmdletInfo = new CmdletInfo(entry.Name, entry.ImplementingType, entry.HelpFileName, entry.PSSnapIn, context);
			cmdletInfo.Visibility = entry.Visibility;
			cmdletInfo.SetModule(entry.Module);
			return cmdletInfo;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00014724 File Offset: 0x00012924
		internal static AliasInfo NewAliasInfo(SessionStateAliasEntry entry, ExecutionContext context)
		{
			AliasInfo aliasInfo = new AliasInfo(entry.Name, entry.Definition, context, entry.Options);
			aliasInfo.Visibility = entry.Visibility;
			aliasInfo.SetModule(entry.Module);
			return aliasInfo;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00014764 File Offset: 0x00012964
		internal CmdletInfo AddCmdletInfoToCache(string name, CmdletInfo newCmdletInfo, bool isGlobal)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (newCmdletInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			if (isGlobal)
			{
				return this._context.EngineSessionState.ModuleScope.AddCmdletToCache(newCmdletInfo.Name, newCmdletInfo, CommandOrigin.Internal, this._context);
			}
			return this._context.EngineSessionState.CurrentScope.AddCmdletToCache(newCmdletInfo.Name, newCmdletInfo, CommandOrigin.Internal, this._context);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000147DC File Offset: 0x000129DC
		internal void AddSessionStateCmdletEntryToCache(SessionStateCmdletEntry entry)
		{
			this.AddSessionStateCmdletEntryToCache(entry, false);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000147E8 File Offset: 0x000129E8
		internal void AddSessionStateCmdletEntryToCache(SessionStateCmdletEntry entry, bool local)
		{
			if (!this.IsSpecialCmdlet(entry.ImplementingType))
			{
				CmdletInfo cmdletInfo = this.NewCmdletInfo(entry);
				this.AddCmdletInfoToCache(cmdletInfo.Name, cmdletInfo, !local);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00014820 File Offset: 0x00012A20
		private void LoadScriptInfo()
		{
			if (this._context.RunspaceConfiguration != null)
			{
				foreach (ScriptConfigurationEntry scriptConfigurationEntry in ((IEnumerable<ScriptConfigurationEntry>)this._context.RunspaceConfiguration.Scripts))
				{
					try
					{
						this.cachedScriptInfo.Add(scriptConfigurationEntry.Name, new ScriptInfo(scriptConfigurationEntry.Name, ScriptBlock.Create(this._context, scriptConfigurationEntry.Definition), this._context));
					}
					catch (ArgumentException)
					{
						PSNotSupportedException ex = PSTraceSource.NewNotSupportedException(DiscoveryExceptions.DuplicateScriptName, new object[]
						{
							scriptConfigurationEntry.Name
						});
						throw ex;
					}
				}
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000148E0 File Offset: 0x00012AE0
		internal CommandProcessorBase LookupCommandProcessor(string commandName, CommandOrigin commandOrigin, bool? useLocalScope)
		{
			CommandInfo commandInfo = this.LookupCommandInfo(commandName, commandOrigin);
			CommandProcessorBase commandProcessorBase = this.LookupCommandProcessor(commandInfo, commandOrigin, useLocalScope, null);
			commandProcessorBase.Command.MyInvocation.InvocationName = commandName;
			return commandProcessorBase;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00014918 File Offset: 0x00012B18
		internal CommandProcessorBase CreateScriptProcessorForMiniShell(ExternalScriptInfo scriptInfo, bool useLocalScope, SessionStateInternal sessionState)
		{
			CommandDiscovery.VerifyScriptRequirements(scriptInfo, this.Context);
			if (string.IsNullOrEmpty(scriptInfo.RequiresApplicationID))
			{
				if (scriptInfo.RequiresPSSnapIns != null && scriptInfo.RequiresPSSnapIns.Any<PSSnapInSpecification>())
				{
					Collection<string> pssnapinNames = CommandDiscovery.GetPSSnapinNames(scriptInfo.RequiresPSSnapIns);
					ScriptRequiresException ex = new ScriptRequiresException(scriptInfo.Name, pssnapinNames, "ScriptRequiresMissingPSSnapIns", true);
					throw ex;
				}
				return CommandDiscovery.CreateCommandProcessorForScript(scriptInfo, this._context, useLocalScope, sessionState);
			}
			else
			{
				if (string.Equals(this._context.ShellID, scriptInfo.RequiresApplicationID, StringComparison.OrdinalIgnoreCase))
				{
					return CommandDiscovery.CreateCommandProcessorForScript(scriptInfo, this._context, useLocalScope, sessionState);
				}
				string shellPathFromRegistry = CommandDiscovery.GetShellPathFromRegistry(scriptInfo.RequiresApplicationID);
				ScriptRequiresException ex2 = new ScriptRequiresException(scriptInfo.Name, scriptInfo.RequiresApplicationID, shellPathFromRegistry, "ScriptRequiresUnmatchedShellId");
				throw ex2;
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000149D0 File Offset: 0x00012BD0
		internal static void VerifyRequiredModules(ExternalScriptInfo scriptInfo, ExecutionContext context)
		{
			if (scriptInfo.RequiresModules != null)
			{
				foreach (ModuleSpecification moduleSpecification in scriptInfo.RequiresModules)
				{
					ErrorRecord errorRecord = null;
					ModuleCmdletBase.LoadRequiredModule(context, null, moduleSpecification, null, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.LoadElements, out errorRecord);
					if (errorRecord != null)
					{
						ScriptRequiresException ex = new ScriptRequiresException(scriptInfo.Name, new Collection<string>
						{
							moduleSpecification.Name
						}, "ScriptRequiresMissingModules", false, errorRecord);
						throw ex;
					}
				}
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00014A60 File Offset: 0x00012C60
		private static Collection<string> GetPSSnapinNames(IEnumerable<PSSnapInSpecification> PSSnapins)
		{
			Collection<string> collection = new Collection<string>();
			foreach (PSSnapInSpecification pssnapin in PSSnapins)
			{
				collection.Add(CommandDiscovery.BuildPSSnapInDisplayName(pssnapin));
			}
			return collection;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00014AB4 File Offset: 0x00012CB4
		private CommandProcessorBase CreateScriptProcessorForSingleShell(ExternalScriptInfo scriptInfo, ExecutionContext context, bool useLocalScope, SessionStateInternal sessionState)
		{
			CommandDiscovery.VerifyScriptRequirements(scriptInfo, this.Context);
			IEnumerable<PSSnapInSpecification> requiresPSSnapIns = scriptInfo.RequiresPSSnapIns;
			if (requiresPSSnapIns != null && requiresPSSnapIns.Any<PSSnapInSpecification>())
			{
				Collection<string> collection = null;
				CommandDiscovery.VerifyRequiredSnapins(requiresPSSnapIns, context, out collection);
				if (collection != null)
				{
					ScriptRequiresException ex = new ScriptRequiresException(scriptInfo.Name, collection, "ScriptRequiresMissingPSSnapIns", true);
					throw ex;
				}
			}
			else if (!string.IsNullOrEmpty(scriptInfo.RequiresApplicationID))
			{
				ScriptRequiresException ex2 = new ScriptRequiresException(scriptInfo.Name, string.Empty, string.Empty, "RequiresShellIDInvalidForSingleShell");
				throw ex2;
			}
			return CommandDiscovery.CreateCommandProcessorForScript(scriptInfo, this._context, useLocalScope, sessionState);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00014B3C File Offset: 0x00012D3C
		private static void VerifyRequiredSnapins(IEnumerable<PSSnapInSpecification> requiresPSSnapIns, ExecutionContext context, out Collection<string> requiresMissingPSSnapIns)
		{
			requiresMissingPSSnapIns = null;
			bool flag = false;
			RunspaceConfigForSingleShell runspaceConfigForSingleShell = null;
			if (context.InitialSessionState != null)
			{
				flag = true;
			}
			else if (context.RunspaceConfiguration != null)
			{
				runspaceConfigForSingleShell = (context.RunspaceConfiguration as RunspaceConfigForSingleShell);
			}
			foreach (PSSnapInSpecification pssnapInSpecification in requiresPSSnapIns)
			{
				IEnumerable<PSSnapInInfo> pssnapIn;
				if (flag)
				{
					pssnapIn = context.InitialSessionState.GetPSSnapIn(pssnapInSpecification.Name);
				}
				else
				{
					pssnapIn = runspaceConfigForSingleShell.ConsoleInfo.GetPSSnapIn(pssnapInSpecification.Name, false);
				}
				if (pssnapIn == null || pssnapIn.Count<PSSnapInInfo>() == 0)
				{
					if (requiresMissingPSSnapIns == null)
					{
						requiresMissingPSSnapIns = new Collection<string>();
					}
					requiresMissingPSSnapIns.Add(CommandDiscovery.BuildPSSnapInDisplayName(pssnapInSpecification));
				}
				else
				{
					PSSnapInInfo pssnapInInfo = pssnapIn.First<PSSnapInInfo>();
					if (pssnapInSpecification.Version != null && !CommandDiscovery.AreInstalledRequiresVersionsCompatible(pssnapInSpecification.Version, pssnapInInfo.Version))
					{
						if (requiresMissingPSSnapIns == null)
						{
							requiresMissingPSSnapIns = new Collection<string>();
						}
						requiresMissingPSSnapIns.Add(CommandDiscovery.BuildPSSnapInDisplayName(pssnapInSpecification));
					}
				}
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00014C44 File Offset: 0x00012E44
		internal static void VerifyScriptRequirements(ExternalScriptInfo scriptInfo, ExecutionContext context)
		{
			CommandDiscovery.VerifyElevatedPriveleges(scriptInfo);
			CommandDiscovery.VerifyPSVersion(scriptInfo);
			CommandDiscovery.VerifyRequiredModules(scriptInfo, context);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00014C5C File Offset: 0x00012E5C
		internal static void VerifyPSVersion(ExternalScriptInfo scriptInfo)
		{
			Version requiresPSVersion = scriptInfo.RequiresPSVersion;
			if (requiresPSVersion != null && !Utils.IsPSVersionSupported(requiresPSVersion))
			{
				ScriptRequiresException ex = new ScriptRequiresException(scriptInfo.Name, requiresPSVersion, PSVersionInfo.PSVersion.ToString(), "ScriptRequiresUnmatchedPSVersion");
				throw ex;
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00014CA0 File Offset: 0x00012EA0
		internal static void VerifyElevatedPriveleges(ExternalScriptInfo scriptInfo)
		{
			bool requiresElevation = scriptInfo.RequiresElevation;
			bool flag = Utils.IsAdministrator();
			if (requiresElevation && !flag)
			{
				ScriptRequiresException ex = new ScriptRequiresException(scriptInfo.Name, "ScriptRequiresElevation");
				throw ex;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00014CD3 File Offset: 0x00012ED3
		private static bool AreInstalledRequiresVersionsCompatible(Version requires, Version installed)
		{
			return requires.Major == installed.Major && requires.Minor <= installed.Minor;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00014CF6 File Offset: 0x00012EF6
		private static string BuildPSSnapInDisplayName(PSSnapInSpecification PSSnapin)
		{
			if (!(PSSnapin.Version == null))
			{
				return StringUtil.Format(DiscoveryExceptions.PSSnapInNameVersion, PSSnapin.Name, PSSnapin.Version);
			}
			return PSSnapin.Name;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00014D24 File Offset: 0x00012F24
		internal CommandProcessorBase LookupCommandProcessor(CommandInfo commandInfo, CommandOrigin commandOrigin, bool? useLocalScope, SessionStateInternal sessionState)
		{
			CommandProcessorBase commandProcessorBase = null;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			while (commandInfo.CommandType == CommandTypes.Alias && !hashSet.Contains(commandInfo.Name) && (commandOrigin == CommandOrigin.Internal || commandInfo.Visibility == SessionStateEntryVisibility.Public))
			{
				hashSet.Add(commandInfo.Name);
				AliasInfo aliasInfo = (AliasInfo)commandInfo;
				commandInfo = aliasInfo.ResolvedCommand;
				if (commandInfo == null)
				{
					commandInfo = CommandDiscovery.LookupCommandInfo(aliasInfo.Definition, commandOrigin, this._context);
				}
				if (commandInfo == null)
				{
					CommandNotFoundException ex = new CommandNotFoundException(aliasInfo.Name, null, "AliasNotResolvedException", DiscoveryExceptions.AliasNotResolvedException, new object[]
					{
						aliasInfo.UnresolvedCommandName
					});
					throw ex;
				}
				PSSQMAPI.IncrementData(CommandTypes.Alias);
			}
			CommandDiscovery.ShouldRun(this._context, this._context.EngineHostInterface, commandInfo, commandOrigin);
			CommandTypes commandType = commandInfo.CommandType;
			if (commandType <= CommandTypes.ExternalScript)
			{
				switch (commandType)
				{
				case CommandTypes.Alias:
				case CommandTypes.Alias | CommandTypes.Function:
					goto IL_256;
				case CommandTypes.Function:
				case CommandTypes.Filter:
					break;
				default:
				{
					if (commandType == CommandTypes.Cmdlet)
					{
						commandProcessorBase = new CommandProcessor((CmdletInfo)commandInfo, this._context);
						goto IL_277;
					}
					if (commandType != CommandTypes.ExternalScript)
					{
						goto IL_256;
					}
					ExternalScriptInfo externalScriptInfo = (ExternalScriptInfo)commandInfo;
					externalScriptInfo.SignatureChecked = true;
					try
					{
						if (!this._context.IsSingleShell)
						{
							commandProcessorBase = this.CreateScriptProcessorForMiniShell(externalScriptInfo, useLocalScope ?? true, sessionState);
						}
						else
						{
							commandProcessorBase = this.CreateScriptProcessorForSingleShell(externalScriptInfo, this._context, useLocalScope ?? true, sessionState);
						}
						goto IL_277;
					}
					catch (ScriptRequiresSyntaxException ex2)
					{
						CommandNotFoundException ex3 = new CommandNotFoundException(ex2.Message, ex2);
						throw ex3;
					}
					catch (PSArgumentException innerException)
					{
						CommandNotFoundException ex4 = new CommandNotFoundException(commandInfo.Name, innerException, "ScriptRequiresInvalidFormat", DiscoveryExceptions.ScriptRequiresInvalidFormat, new object[0]);
						throw ex4;
					}
					break;
				}
				}
			}
			else if (commandType <= CommandTypes.Script)
			{
				if (commandType == CommandTypes.Application)
				{
					commandProcessorBase = new NativeCommandProcessor((ApplicationInfo)commandInfo, this._context);
					goto IL_277;
				}
				if (commandType != CommandTypes.Script)
				{
					goto IL_256;
				}
				commandProcessorBase = CommandDiscovery.CreateCommandProcessorForScript((ScriptInfo)commandInfo, this._context, useLocalScope ?? true, sessionState);
				goto IL_277;
			}
			else if (commandType != CommandTypes.Workflow && commandType != CommandTypes.Configuration)
			{
				goto IL_256;
			}
			FunctionInfo functionInfo = (FunctionInfo)commandInfo;
			commandProcessorBase = CommandDiscovery.CreateCommandProcessorForScript(functionInfo, this._context, useLocalScope ?? true, sessionState);
			goto IL_277;
			IL_256:
			CommandNotFoundException ex5 = new CommandNotFoundException(commandInfo.Name, null, "CommandNotFoundException", DiscoveryExceptions.CommandNotFoundException, new object[0]);
			throw ex5;
			IL_277:
			PSSQMAPI.IncrementData(commandInfo.CommandType);
			commandProcessorBase.Command.CommandOriginInternal = commandOrigin;
			commandProcessorBase.Command.MyInvocation.InvocationName = commandInfo.Name;
			return commandProcessorBase;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00014FF4 File Offset: 0x000131F4
		internal static void ShouldRun(ExecutionContext context, PSHost host, CommandInfo commandInfo, CommandOrigin commandOrigin)
		{
			try
			{
				if (commandOrigin == CommandOrigin.Runspace && commandInfo.Visibility != SessionStateEntryVisibility.Public)
				{
					CommandNotFoundException ex = new CommandNotFoundException(commandInfo.Name, null, "CommandNotFoundException", DiscoveryExceptions.CommandNotFoundException, new object[0]);
					throw ex;
				}
				context.AuthorizationManager.ShouldRunInternal(commandInfo, commandOrigin, host);
			}
			catch (PSSecurityException exception)
			{
				MshLog.LogCommandHealthEvent(context, exception, Severity.Warning);
				MshLog.LogCommandLifecycleEvent(context, CommandState.Terminated, commandInfo.Name);
				throw;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00015064 File Offset: 0x00013264
		private static CommandProcessorBase CreateCommandProcessorForScript(ScriptInfo scriptInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState)
		{
			SessionStateInternal sessionStateInternal;
			if ((sessionStateInternal = sessionState) == null)
			{
				sessionStateInternal = (scriptInfo.ScriptBlock.SessionStateInternal ?? context.EngineSessionState);
			}
			sessionState = sessionStateInternal;
			CommandProcessorBase scriptAsCmdletProcessor = CommandDiscovery.GetScriptAsCmdletProcessor(scriptInfo, context, useNewScope, true, sessionState);
			if (scriptAsCmdletProcessor != null)
			{
				return scriptAsCmdletProcessor;
			}
			return new DlrScriptCommandProcessor(scriptInfo, context, useNewScope, sessionState);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000150A8 File Offset: 0x000132A8
		private static CommandProcessorBase CreateCommandProcessorForScript(ExternalScriptInfo scriptInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState)
		{
			SessionStateInternal sessionStateInternal;
			if ((sessionStateInternal = sessionState) == null)
			{
				sessionStateInternal = (scriptInfo.ScriptBlock.SessionStateInternal ?? context.EngineSessionState);
			}
			sessionState = sessionStateInternal;
			CommandProcessorBase scriptAsCmdletProcessor = CommandDiscovery.GetScriptAsCmdletProcessor(scriptInfo, context, useNewScope, true, sessionState);
			if (scriptAsCmdletProcessor != null)
			{
				return scriptAsCmdletProcessor;
			}
			return new DlrScriptCommandProcessor(scriptInfo, context, useNewScope, sessionState);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000150EC File Offset: 0x000132EC
		internal static CommandProcessorBase CreateCommandProcessorForScript(FunctionInfo functionInfo, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState)
		{
			SessionStateInternal sessionStateInternal;
			if ((sessionStateInternal = sessionState) == null)
			{
				sessionStateInternal = (functionInfo.ScriptBlock.SessionStateInternal ?? context.EngineSessionState);
			}
			sessionState = sessionStateInternal;
			CommandProcessorBase scriptAsCmdletProcessor = CommandDiscovery.GetScriptAsCmdletProcessor(functionInfo, context, useNewScope, false, sessionState);
			if (scriptAsCmdletProcessor != null)
			{
				return scriptAsCmdletProcessor;
			}
			return new DlrScriptCommandProcessor(functionInfo, context, useNewScope, sessionState);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00015130 File Offset: 0x00013330
		internal static CommandProcessorBase CreateCommandProcessorForScript(ScriptBlock scriptblock, ExecutionContext context, bool useNewScope, SessionStateInternal sessionState)
		{
			SessionStateInternal sessionStateInternal;
			if ((sessionStateInternal = sessionState) == null)
			{
				sessionStateInternal = (scriptblock.SessionStateInternal ?? context.EngineSessionState);
			}
			sessionState = sessionStateInternal;
			if (scriptblock.UsesCmdletBinding)
			{
				FunctionInfo scriptCommandInfo = new FunctionInfo("", scriptblock, context);
				return CommandDiscovery.GetScriptAsCmdletProcessor(scriptCommandInfo, context, useNewScope, false, sessionState);
			}
			return new DlrScriptCommandProcessor(scriptblock, context, useNewScope, CommandOrigin.Internal, sessionState);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00015180 File Offset: 0x00013380
		private static CommandProcessorBase GetScriptAsCmdletProcessor(IScriptCommandInfo scriptCommandInfo, ExecutionContext context, bool useNewScope, bool fromScriptFile, SessionStateInternal sessionState)
		{
			if (scriptCommandInfo.ScriptBlock == null || !scriptCommandInfo.ScriptBlock.UsesCmdletBinding)
			{
				return null;
			}
			SessionStateInternal sessionStateInternal;
			if ((sessionStateInternal = sessionState) == null)
			{
				sessionStateInternal = (scriptCommandInfo.ScriptBlock.SessionStateInternal ?? context.EngineSessionState);
			}
			sessionState = sessionStateInternal;
			return new CommandProcessor(scriptCommandInfo, context, useNewScope, fromScriptFile, sessionState);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000151CC File Offset: 0x000133CC
		internal CommandInfo LookupCommandInfo(string commandName)
		{
			return this.LookupCommandInfo(commandName, CommandOrigin.Internal);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000151D6 File Offset: 0x000133D6
		internal CommandInfo LookupCommandInfo(string commandName, CommandOrigin commandOrigin)
		{
			return CommandDiscovery.LookupCommandInfo(commandName, commandOrigin, this._context);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000151E5 File Offset: 0x000133E5
		internal static CommandInfo LookupCommandInfo(string commandName, CommandOrigin commandOrigin, ExecutionContext context)
		{
			return CommandDiscovery.LookupCommandInfo(commandName, CommandTypes.All, SearchResolutionOptions.None, commandOrigin, context);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000151F8 File Offset: 0x000133F8
		internal static CommandInfo LookupCommandInfo(string commandName, CommandTypes commandTypes, SearchResolutionOptions searchResolutionOptions, CommandOrigin commandOrigin, ExecutionContext context)
		{
			if (string.IsNullOrEmpty(commandName))
			{
				return null;
			}
			CommandInfo commandInfo = null;
			Exception innerException = null;
			CommandLookupEventArgs commandLookupEventArgs = null;
			EventHandler<CommandLookupEventArgs> preCommandLookupAction = context.EngineIntrinsics.InvokeCommand.PreCommandLookupAction;
			if (preCommandLookupAction != null)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Executing PreCommandLookupAction: {0}", new object[]
				{
					commandName
				});
				try
				{
					context.CommandDiscovery.RegisterLookupCommandInfoAction("ActivePreLookup", commandName);
					commandLookupEventArgs = new CommandLookupEventArgs(commandName, commandOrigin, context);
					preCommandLookupAction(commandName, commandLookupEventArgs);
					CommandDiscovery.discoveryTracer.WriteLine("PreCommandLookupAction returned: {0}", new object[]
					{
						commandLookupEventArgs.Command
					});
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
				finally
				{
					context.CommandDiscovery.UnregisterLookupCommandInfoAction("ActivePreLookup", commandName);
				}
			}
			PSModuleAutoLoadingPreference commandDiscoveryPreference = CommandDiscovery.GetCommandDiscoveryPreference(context, SpecialVariables.PSModuleAutoLoadingPreferenceVarPath, "PSModuleAutoLoadingPreference");
			if (commandLookupEventArgs == null || !commandLookupEventArgs.StopSearch)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Looking up command: {0}", new object[]
				{
					commandName
				});
				commandInfo = CommandDiscovery.TryNormalSearch(commandName, context, commandOrigin, searchResolutionOptions, commandTypes, ref innerException);
				if (commandInfo == null)
				{
					if (commandDiscoveryPreference != PSModuleAutoLoadingPreference.None)
					{
						commandInfo = CommandDiscovery.TryModuleAutoLoading(commandName, context, commandName, commandOrigin, commandInfo, ref innerException);
					}
					if (commandInfo == null)
					{
						if (commandDiscoveryPreference == PSModuleAutoLoadingPreference.All)
						{
							commandInfo = CommandDiscovery.TryModuleAutoDiscovery(commandName, context, commandName, commandOrigin, searchResolutionOptions, commandTypes, ref innerException);
						}
						if (commandInfo == null)
						{
							commandInfo = CommandDiscovery.InvokeCommandNotFoundHandler(commandName, context, commandName, commandOrigin, commandInfo);
						}
					}
				}
			}
			else if (commandLookupEventArgs.Command != null)
			{
				commandInfo = commandLookupEventArgs.Command;
			}
			if (commandInfo != null)
			{
				EventHandler<CommandLookupEventArgs> postCommandLookupAction = context.EngineIntrinsics.InvokeCommand.PostCommandLookupAction;
				if (postCommandLookupAction != null)
				{
					CommandDiscovery.discoveryTracer.WriteLine("Executing PostCommandLookupAction: {0}", new object[]
					{
						commandName
					});
					try
					{
						context.CommandDiscovery.RegisterLookupCommandInfoAction("ActivePostCommand", commandName);
						commandLookupEventArgs = new CommandLookupEventArgs(commandName, commandOrigin, context);
						commandLookupEventArgs.Command = commandInfo;
						postCommandLookupAction(commandName, commandLookupEventArgs);
						if (commandLookupEventArgs != null)
						{
							commandInfo = commandLookupEventArgs.Command;
							CommandDiscovery.discoveryTracer.WriteLine("PreCommandLookupAction returned: {0}", new object[]
							{
								commandLookupEventArgs.Command
							});
						}
					}
					catch (Exception e2)
					{
						CommandProcessorBase.CheckForSevereException(e2);
					}
					finally
					{
						context.CommandDiscovery.UnregisterLookupCommandInfoAction("ActivePostCommand", commandName);
					}
				}
			}
			if (commandInfo == null)
			{
				CommandDiscovery.discoveryTracer.TraceError("'{0}' is not recognized as a cmdlet, function, operable program or script file.", new object[]
				{
					commandName
				});
				CommandNotFoundException ex = new CommandNotFoundException(commandName, innerException, "CommandNotFoundException", DiscoveryExceptions.CommandNotFoundException, new object[0]);
				throw ex;
			}
			return commandInfo;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001547C File Offset: 0x0001367C
		internal static void AutoloadModulesWithJobSourceAdapters(ExecutionContext context, CommandOrigin commandOrigin)
		{
			if (!context.IsModuleWithJobSourceAdapterLoaded)
			{
				PSModuleAutoLoadingPreference commandDiscoveryPreference = CommandDiscovery.GetCommandDiscoveryPreference(context, SpecialVariables.PSModuleAutoLoadingPreferenceVarPath, "PSModuleAutoLoadingPreference");
				if (commandDiscoveryPreference != PSModuleAutoLoadingPreference.None)
				{
					CmdletInfo cmdlet = context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Import-Module");
					if (commandOrigin == CommandOrigin.Internal || (cmdlet != null && cmdlet.Visibility == SessionStateEntryVisibility.Public))
					{
						foreach (string text in ExecutionContext.ModulesWithJobSourceAdapters)
						{
							List<PSModuleInfo> modules = context.Modules.GetModules(new string[]
							{
								text
							}, false);
							if (modules == null || modules.Count == 0)
							{
								Exception ex = null;
								CommandDiscovery.AutoloadSpecifiedModule(text, context, cmdlet.Visibility, out ex);
							}
						}
						context.IsModuleWithJobSourceAdapterLoaded = true;
					}
				}
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00015550 File Offset: 0x00013750
		internal static Collection<PSModuleInfo> AutoloadSpecifiedModule(string moduleName, ExecutionContext context, SessionStateEntryVisibility visibility, out Exception exception)
		{
			exception = null;
			Collection<PSModuleInfo> result = null;
			Command command = new Command(new CmdletInfo("Import-Module", typeof(ImportModuleCommand), null, null, context)
			{
				Visibility = visibility
			});
			CommandDiscovery.discoveryTracer.WriteLine("Attempting to load module: {0}", new object[]
			{
				moduleName
			});
			try
			{
				PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand(command).AddParameter("Name", moduleName).AddParameter("Scope", "GLOBAL").AddParameter("PassThru").AddParameter("ErrorAction", ActionPreference.Ignore).AddParameter("WarningAction", ActionPreference.Ignore).AddParameter("InformationAction", ActionPreference.Ignore).AddParameter("Verbose", false).AddParameter("Debug", false);
				result = powerShell.Invoke<PSModuleInfo>();
			}
			catch (Exception ex)
			{
				exception = ex;
				CommandDiscovery.discoveryTracer.WriteLine("Encountered error importing module: {0}", new object[]
				{
					ex.Message
				});
				CommandProcessorBase.CheckForSevereException(ex);
			}
			return result;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00015678 File Offset: 0x00013878
		private static CommandInfo InvokeCommandNotFoundHandler(string commandName, ExecutionContext context, string originalCommandName, CommandOrigin commandOrigin, CommandInfo result)
		{
			EventHandler<CommandLookupEventArgs> commandNotFoundAction = context.EngineIntrinsics.InvokeCommand.CommandNotFoundAction;
			if (commandNotFoundAction != null)
			{
				CommandDiscovery.discoveryTracer.WriteLine("Executing CommandNotFoundAction: {0}", new object[]
				{
					commandName
				});
				try
				{
					context.CommandDiscovery.RegisterLookupCommandInfoAction("ActiveCommandNotFound", originalCommandName);
					CommandLookupEventArgs commandLookupEventArgs = new CommandLookupEventArgs(originalCommandName, commandOrigin, context);
					commandNotFoundAction(originalCommandName, commandLookupEventArgs);
					result = commandLookupEventArgs.Command;
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
				finally
				{
					context.CommandDiscovery.UnregisterLookupCommandInfoAction("ActiveCommandNotFound", originalCommandName);
				}
			}
			return result;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001571C File Offset: 0x0001391C
		private static CommandInfo TryNormalSearch(string commandName, ExecutionContext context, CommandOrigin commandOrigin, SearchResolutionOptions searchResolutionOptions, CommandTypes commandTypes, ref Exception lastError)
		{
			CommandInfo result = null;
			CommandSearcher commandSearcher = new CommandSearcher(commandName, searchResolutionOptions, commandTypes, context);
			commandSearcher.CommandOrigin = commandOrigin;
			try
			{
				if (!commandSearcher.MoveNext())
				{
					if (commandName.Contains("-") || commandName.Contains("\\"))
					{
						goto IL_80;
					}
					CommandDiscovery.discoveryTracer.WriteLine("The command [{0}] was not found, trying again with get- prepended", new object[]
					{
						commandName
					});
					commandName = "get" + '-' + commandName;
					try
					{
						result = CommandDiscovery.LookupCommandInfo(commandName, commandTypes, searchResolutionOptions, commandOrigin, context);
						goto IL_80;
					}
					catch (CommandNotFoundException)
					{
						goto IL_80;
					}
				}
				result = ((IEnumerator<CommandInfo>)commandSearcher).Current;
				IL_80:;
			}
			catch (ArgumentException ex)
			{
				lastError = ex;
			}
			catch (PathTooLongException ex2)
			{
				lastError = ex2;
			}
			catch (FileLoadException ex3)
			{
				lastError = ex3;
			}
			catch (FormatException ex4)
			{
				lastError = ex4;
			}
			catch (MetadataException ex5)
			{
				lastError = ex5;
			}
			return result;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00015824 File Offset: 0x00013A24
		private static CommandInfo TryModuleAutoDiscovery(string commandName, ExecutionContext context, string originalCommandName, CommandOrigin commandOrigin, SearchResolutionOptions searchResolutionOptions, CommandTypes commandTypes, ref Exception lastError)
		{
			CommandInfo commandInfo = null;
			bool flag = false;
			try
			{
				int num = commandName.IndexOfAny(new char[]
				{
					':',
					'\\'
				});
				if (num != -1)
				{
					return null;
				}
				CmdletInfo cmdlet = context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Get-Module");
				if (commandOrigin == CommandOrigin.Internal || (cmdlet != null && cmdlet.Visibility == SessionStateEntryVisibility.Public))
				{
					cmdlet = context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Import-Module");
					if (commandOrigin == CommandOrigin.Internal || (cmdlet != null && cmdlet.Visibility == SessionStateEntryVisibility.Public))
					{
						CommandDiscovery.discoveryTracer.WriteLine("Executing non module-qualified search: {0}", new object[]
						{
							commandName
						});
						context.CommandDiscovery.RegisterLookupCommandInfoAction("ActiveModuleSearch", commandName);
						flag = context.TakeResponsibilityForModuleAnalysisAppDomain();
						foreach (string path in ModuleUtils.GetDefaultAvailableModuleFiles(true, true, context))
						{
							string fullPath = Path.GetFullPath(path);
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
							Dictionary<string, List<CommandTypes>> exportedCommands = AnalysisCache.GetExportedCommands(fullPath, false, context);
							if (exportedCommands != null)
							{
								List<CommandTypes> list = null;
								if (exportedCommands.TryGetValue(commandName, out list))
								{
									CommandDiscovery.discoveryTracer.WriteLine("Found in module: {0}", new object[]
									{
										fullPath
									});
									Exception ex;
									Collection<PSModuleInfo> collection = CommandDiscovery.AutoloadSpecifiedModule(fullPath, context, (cmdlet != null) ? cmdlet.Visibility : SessionStateEntryVisibility.Private, out ex);
									lastError = ex;
									if (collection == null || collection.Count == 0)
									{
										string resourceStr = StringUtil.Format(DiscoveryExceptions.CouldNotAutoImportMatchingModule, commandName, fileNameWithoutExtension);
										CommandNotFoundException ex2 = new CommandNotFoundException(originalCommandName, lastError, "CouldNotAutoloadMatchingModule", resourceStr, new object[0]);
										throw ex2;
									}
									commandInfo = CommandDiscovery.LookupCommandInfo(commandName, commandTypes, searchResolutionOptions, commandOrigin, context);
								}
								if (commandInfo != null)
								{
									break;
								}
							}
						}
						if (context.CurrentCommandProcessor != null)
						{
							ProgressRecord progressRecord = new ProgressRecord(0, Modules.ScriptAnalysisPreparing, " ");
							progressRecord.RecordType = ProgressRecordType.Completed;
							context.CurrentCommandProcessor.CommandRuntime.WriteProgress(progressRecord);
						}
					}
				}
			}
			catch (CommandNotFoundException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				context.CommandDiscovery.UnregisterLookupCommandInfoAction("ActiveModuleSearch", commandName);
				if (flag)
				{
					context.ReleaseResponsibilityForModuleAnalysisAppDomain();
				}
			}
			return commandInfo;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00015AA8 File Offset: 0x00013CA8
		private static CommandInfo TryModuleAutoLoading(string commandName, ExecutionContext context, string originalCommandName, CommandOrigin commandOrigin, CommandInfo result, ref Exception lastError)
		{
			int num = commandName.IndexOfAny(new char[]
			{
				':',
				'\\'
			});
			if (num == -1 || commandName[num] == ':')
			{
				return null;
			}
			string text = commandName.Substring(0, num);
			string text2 = commandName.Substring(num + 1, commandName.Length - num - 1);
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || text.EndsWith(".", StringComparison.Ordinal))
			{
				return null;
			}
			try
			{
				CommandDiscovery.discoveryTracer.WriteLine("Executing module-qualified search: {0}", new object[]
				{
					commandName
				});
				context.CommandDiscovery.RegisterLookupCommandInfoAction("ActiveModuleSearch", commandName);
				CmdletInfo cmdlet = context.SessionState.InvokeCommand.GetCmdlet("Microsoft.PowerShell.Core\\Import-Module");
				if (commandOrigin == CommandOrigin.Internal || (cmdlet != null && cmdlet.Visibility == SessionStateEntryVisibility.Public))
				{
					List<PSModuleInfo> modules = context.Modules.GetModules(new string[]
					{
						text
					}, false);
					PSModuleInfo psmoduleInfo;
					if (modules == null || modules.Count == 0)
					{
						CommandDiscovery.discoveryTracer.WriteLine("Attempting to load module: {0}", new object[]
						{
							text
						});
						Exception ex;
						Collection<PSModuleInfo> collection = CommandDiscovery.AutoloadSpecifiedModule(text, context, cmdlet.Visibility, out ex);
						lastError = ex;
						if (collection == null || collection.Count == 0)
						{
							string resourceStr = StringUtil.Format(DiscoveryExceptions.CouldNotAutoImportModule, text);
							CommandNotFoundException ex2 = new CommandNotFoundException(originalCommandName, lastError, "CouldNotAutoLoadModule", resourceStr, new object[0]);
							throw ex2;
						}
						psmoduleInfo = collection[0];
					}
					else
					{
						psmoduleInfo = modules[0];
					}
					if (psmoduleInfo.ExportedCommands.ContainsKey(text2))
					{
						result = psmoduleInfo.ExportedCommands[text2];
					}
				}
			}
			catch (CommandNotFoundException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			finally
			{
				context.CommandDiscovery.UnregisterLookupCommandInfoAction("ActiveModuleSearch", commandName);
			}
			return result;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00015CB8 File Offset: 0x00013EB8
		internal void RegisterLookupCommandInfoAction(string currentAction, string command)
		{
			HashSet<string> hashSet = null;
			if (currentAction != null)
			{
				if (!(currentAction == "ActivePreLookup"))
				{
					if (!(currentAction == "ActiveModuleSearch"))
					{
						if (!(currentAction == "ActiveCommandNotFound"))
						{
							if (currentAction == "ActivePostCommand")
							{
								hashSet = this.activePostCommand;
							}
						}
						else
						{
							hashSet = this.activeCommandNotFound;
						}
					}
					else
					{
						hashSet = this.activeModuleSearch;
					}
				}
				else
				{
					hashSet = this.activePreLookup;
				}
			}
			if (hashSet.Contains(command))
			{
				throw new InvalidOperationException();
			}
			hashSet.Add(command);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00015D3C File Offset: 0x00013F3C
		internal void UnregisterLookupCommandInfoAction(string currentAction, string command)
		{
			HashSet<string> hashSet = null;
			if (currentAction != null)
			{
				if (!(currentAction == "ActivePreLookup"))
				{
					if (!(currentAction == "ActiveModuleSearch"))
					{
						if (!(currentAction == "ActiveCommandNotFound"))
						{
							if (currentAction == "ActivePostCommand")
							{
								hashSet = this.activePostCommand;
							}
						}
						else
						{
							hashSet = this.activeCommandNotFound;
						}
					}
					else
					{
						hashSet = this.activeModuleSearch;
					}
				}
				else
				{
					hashSet = this.activePreLookup;
				}
			}
			if (hashSet.Contains(command))
			{
				hashSet.Remove(command);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00015DBC File Offset: 0x00013FBC
		internal IEnumerable<string> GetCommandPathSearcher(IEnumerable<string> patterns)
		{
			IEnumerable<string> lookupDirectoryPaths = this.GetLookupDirectoryPaths();
			return new CommandPathSearch(patterns, lookupDirectoryPaths, this._context);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00015DE0 File Offset: 0x00013FE0
		internal IEnumerable<string> GetLookupDirectoryPaths()
		{
			LookupPathCollection lookupPathCollection = new LookupPathCollection();
			string environmentVariable = Environment.GetEnvironmentVariable("PATH");
			CommandDiscovery.discoveryTracer.WriteLine("PATH: {0}", new object[]
			{
				environmentVariable
			});
			if (environmentVariable == null || !string.Equals(this.pathCacheKey, environmentVariable, StringComparison.OrdinalIgnoreCase) || this.cachedPath == null)
			{
				this.cachedLookupPaths = null;
				this.pathCacheKey = environmentVariable;
				if (this.pathCacheKey != null)
				{
					string[] array = this.pathCacheKey.Split(new char[]
					{
						';'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array != null)
					{
						this.cachedPath = new Collection<string>();
						foreach (string text in array)
						{
							string item = text.TrimStart(new char[0]);
							this.cachedPath.Add(item);
							lookupPathCollection.Add(item);
						}
					}
				}
			}
			else
			{
				lookupPathCollection.AddRange(this.cachedPath);
			}
			if (this.cachedLookupPaths == null)
			{
				this.cachedLookupPaths = lookupPathCollection;
			}
			return this.cachedLookupPaths;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00015EEC File Offset: 0x000140EC
		internal HashSet<string> GetAllowedExtensionsFromPathExt()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			string environmentVariable = Environment.GetEnvironmentVariable("PATHEXT");
			CommandDiscovery.discoveryTracer.WriteLine("PATHEXT: {0}", new object[]
			{
				environmentVariable
			});
			if (environmentVariable == null || !string.Equals(this.pathExtCacheKey, environmentVariable, StringComparison.OrdinalIgnoreCase) || this.cachedPathExt == null)
			{
				this.pathExtCacheKey = environmentVariable;
				if (this.pathExtCacheKey != null)
				{
					string[] array = environmentVariable.Split(new char[]
					{
						';'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array != null)
					{
						this.cachedPathExt = new Collection<string>();
						foreach (string text in array)
						{
							string item = text.TrimStart(new char[0]);
							this.cachedPathExt.Add(item);
							hashSet.Add(item);
						}
					}
				}
			}
			else
			{
				foreach (string item2 in this.cachedPathExt)
				{
					hashSet.Add(item2);
				}
			}
			return hashSet;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001601C File Offset: 0x0001421C
		internal static IEnumerable<string> PathExtensions
		{
			get
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PATHEXT");
				Collection<string> collection;
				if (CommandDiscovery.pathExtensionsCacheKey != null && environmentVariable != null && CommandDiscovery.cachedPathExtensions != null && environmentVariable.Equals(CommandDiscovery.pathExtensionsCacheKey, StringComparison.OrdinalIgnoreCase))
				{
					collection = CommandDiscovery.cachedPathExtensions;
				}
				else
				{
					collection = new Collection<string>();
					if (environmentVariable != null)
					{
						string[] array = environmentVariable.Split(new char[]
						{
							';'
						}, StringSplitOptions.RemoveEmptyEntries);
						if (array != null)
						{
							foreach (string item in array)
							{
								collection.Add(item);
							}
						}
						CommandDiscovery.pathExtensionsCacheKey = environmentVariable;
						CommandDiscovery.cachedPathExtensions = collection;
					}
				}
				return collection;
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000163E0 File Offset: 0x000145E0
		internal IEnumerator<CmdletInfo> GetCmdletInfo(string cmdletName, bool searchAllScopes)
		{
			PSSnapinQualifiedName commandName = PSSnapinQualifiedName.GetInstance(cmdletName);
			if (commandName != null)
			{
				SessionStateScopeEnumerator scopeEnumerator = new SessionStateScopeEnumerator(this._context.EngineSessionState.CurrentScope);
				foreach (SessionStateScope scope in ((IEnumerable<SessionStateScope>)scopeEnumerator))
				{
					List<CmdletInfo> cmdlets;
					if (scope.CmdletTable.TryGetValue(commandName.ShortName, out cmdlets))
					{
						foreach (CmdletInfo cmdletInfo in cmdlets)
						{
							if (!string.IsNullOrEmpty(commandName.PSSnapInName))
							{
								if (string.Equals(cmdletInfo.ModuleName, commandName.PSSnapInName, StringComparison.OrdinalIgnoreCase))
								{
									yield return cmdletInfo;
									if (!searchAllScopes)
									{
										yield break;
									}
								}
								else if (InitialSessionState.IsEngineModule(cmdletInfo.ModuleName) && string.Equals(cmdletInfo.ModuleName, InitialSessionState.GetNestedModuleDllName(commandName.PSSnapInName), StringComparison.OrdinalIgnoreCase))
								{
									yield return cmdletInfo;
									if (!searchAllScopes)
									{
										yield break;
									}
								}
							}
							else
							{
								yield return cmdletInfo;
								if (!searchAllScopes)
								{
									yield break;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001640C File Offset: 0x0001460C
		internal void UpdateCmdletCache()
		{
			if (!this._cmdletCacheInitialized)
			{
				foreach (CmdletConfigurationEntry entry in ((IEnumerable<CmdletConfigurationEntry>)this._context.RunspaceConfiguration.Cmdlets))
				{
					this.AddCmdletToCache(entry);
				}
				this._cmdletCacheInitialized = true;
				return;
			}
			foreach (CmdletConfigurationEntry cmdletConfigurationEntry in this._context.RunspaceConfiguration.Cmdlets.UpdateList)
			{
				if (cmdletConfigurationEntry != null)
				{
					switch (cmdletConfigurationEntry.Action)
					{
					case UpdateAction.Add:
						this.AddCmdletToCache(cmdletConfigurationEntry);
						break;
					case UpdateAction.Remove:
						this.RemoveCmdletFromCache(cmdletConfigurationEntry);
						break;
					}
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000164E4 File Offset: 0x000146E4
		private void RemoveCmdletFromCache(CmdletConfigurationEntry entry)
		{
			IDictionary<string, List<CmdletInfo>> cmdletTable = this._context.EngineSessionState.GetCmdletTable();
			if (cmdletTable.ContainsKey(entry.Name))
			{
				List<CmdletInfo> list = cmdletTable[entry.Name];
				int cmdletRemovalIndex = this.GetCmdletRemovalIndex(list, (entry.PSSnapIn == null) ? string.Empty : entry.PSSnapIn.Name);
				if (cmdletRemovalIndex >= 0)
				{
					string name = list[cmdletRemovalIndex].Name;
					list.RemoveAt(cmdletRemovalIndex);
					this._context.EngineSessionState.RemoveCmdlet(name, cmdletRemovalIndex, true);
				}
				if (list.Count == 0)
				{
					this._context.EngineSessionState.RemoveCmdletEntry(entry.Name, true);
				}
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001658C File Offset: 0x0001478C
		private int GetCmdletRemovalIndex(List<CmdletInfo> cacheEntry, string PSSnapin)
		{
			int result = -1;
			for (int i = 0; i < cacheEntry.Count; i++)
			{
				if (string.Equals(cacheEntry[i].ModuleName, PSSnapin, StringComparison.OrdinalIgnoreCase))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000165C8 File Offset: 0x000147C8
		internal ScriptInfo GetScriptInfo(string name)
		{
			ScriptInfo result = null;
			if (this.cachedScriptInfo.ContainsKey(name))
			{
				result = this.cachedScriptInfo[name];
			}
			return result;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x000165F3 File Offset: 0x000147F3
		internal Dictionary<string, ScriptInfo> ScriptCache
		{
			get
			{
				return this.cachedScriptInfo;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x000165FB File Offset: 0x000147FB
		internal ExecutionContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00016604 File Offset: 0x00014804
		internal static string GetShellPathFromRegistry(string shellID)
		{
			string result = null;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Utils.GetRegistryConfigurationPath(shellID));
				if (registryKey != null)
				{
					RegistryValueKind valueKind = registryKey.GetValueKind("path");
					if (valueKind == RegistryValueKind.ExpandString || valueKind == RegistryValueKind.String)
					{
						result = (registryKey.GetValue("path") as string);
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (IOException)
			{
			}
			catch (ArgumentException)
			{
			}
			return result;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00016680 File Offset: 0x00014880
		internal static PSModuleAutoLoadingPreference GetCommandDiscoveryPreference(ExecutionContext context, VariablePath variablePath, string environmentVariable)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			object variableValue = context.GetVariableValue(variablePath);
			try
			{
				if (variableValue != null)
				{
					return LanguagePrimitives.ConvertTo<PSModuleAutoLoadingPreference>(variableValue);
				}
				string environmentVariable2 = Environment.GetEnvironmentVariable(environmentVariable);
				if (!string.IsNullOrEmpty(environmentVariable2))
				{
					return LanguagePrimitives.ConvertTo<PSModuleAutoLoadingPreference>(environmentVariable2);
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				return PSModuleAutoLoadingPreference.All;
			}
			return PSModuleAutoLoadingPreference.All;
		}

		// Token: 0x040001B6 RID: 438
		[TraceSource("CommandDiscovery", "Traces the discovery of cmdlets, scripts, functions, applications, etc.")]
		internal static PSTraceSource discoveryTracer = PSTraceSource.GetTracer("CommandDiscovery", "Traces the discovery of cmdlets, scripts, functions, applications, etc.", false);

		// Token: 0x040001B7 RID: 439
		private HashSet<string> activePreLookup = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040001B8 RID: 440
		private HashSet<string> activeModuleSearch = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040001B9 RID: 441
		private HashSet<string> activeCommandNotFound = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040001BA RID: 442
		private HashSet<string> activePostCommand = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040001BB RID: 443
		private LookupPathCollection cachedLookupPaths;

		// Token: 0x040001BC RID: 444
		private string pathCacheKey;

		// Token: 0x040001BD RID: 445
		private Collection<string> cachedPath;

		// Token: 0x040001BE RID: 446
		private string pathExtCacheKey;

		// Token: 0x040001BF RID: 447
		private Collection<string> cachedPathExt;

		// Token: 0x040001C0 RID: 448
		private static string pathExtensionsCacheKey;

		// Token: 0x040001C1 RID: 449
		private static Collection<string> cachedPathExtensions;

		// Token: 0x040001C2 RID: 450
		private bool _cmdletCacheInitialized;

		// Token: 0x040001C3 RID: 451
		private Dictionary<string, ScriptInfo> cachedScriptInfo;

		// Token: 0x040001C4 RID: 452
		private ExecutionContext _context;
	}
}
