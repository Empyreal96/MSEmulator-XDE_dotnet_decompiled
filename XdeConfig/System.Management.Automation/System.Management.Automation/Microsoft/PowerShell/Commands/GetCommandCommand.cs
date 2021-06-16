using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Reflection;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200006D RID: 109
	[OutputType(new Type[]
	{
		typeof(AliasInfo),
		typeof(ApplicationInfo),
		typeof(FunctionInfo),
		typeof(CmdletInfo),
		typeof(ExternalScriptInfo),
		typeof(FilterInfo),
		typeof(WorkflowInfo),
		typeof(string),
		typeof(PSObject)
	})]
	[Cmdlet("Get", "Command", DefaultParameterSetName = "CmdletSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113309")]
	public sealed class GetCommandCommand : PSCmdlet
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001A91D File Offset: 0x00018B1D
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x0001A928 File Offset: 0x00018B28
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "AllCommandSet")]
		[ValidateNotNullOrEmpty]
		public string[] Name
		{
			get
			{
				return this.names;
			}
			set
			{
				this.nameContainsWildcard = false;
				this.names = value;
				if (value != null)
				{
					for (int i = 0; i < value.Length; i++)
					{
						string pattern = value[i];
						if (WildcardPattern.ContainsWildcardCharacters(pattern))
						{
							this.nameContainsWildcard = true;
							return;
						}
					}
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001A96A File Offset: 0x00018B6A
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x0001A972 File Offset: 0x00018B72
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "CmdletSet")]
		public string[] Verb
		{
			get
			{
				return this.verbs;
			}
			set
			{
				if (value == null)
				{
					value = new string[0];
				}
				this.verbs = value;
				this.verbPatterns = null;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001A98D File Offset: 0x00018B8D
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0001A995 File Offset: 0x00018B95
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "CmdletSet")]
		[ArgumentCompleter(typeof(NounArgumentCompleter))]
		public string[] Noun
		{
			get
			{
				return this.nouns;
			}
			set
			{
				if (value == null)
				{
					value = new string[0];
				}
				this.nouns = value;
				this.nounPatterns = null;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001A9B0 File Offset: 0x00018BB0
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[Alias(new string[]
		{
			"PSSnapin"
		})]
		public string[] Module
		{
			get
			{
				return this._modules;
			}
			set
			{
				if (value == null)
				{
					value = new string[0];
				}
				this._modules = value;
				this._modulePatterns = null;
				this.isModuleSpecified = true;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001A9DA File Offset: 0x00018BDA
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x0001A9E2 File Offset: 0x00018BE2
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public ModuleSpecification[] FullyQualifiedModule
		{
			get
			{
				return this._moduleSpecifications;
			}
			set
			{
				if (value != null)
				{
					this._moduleSpecifications = value;
				}
				this.isFullyQualifiedModuleSpecified = true;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001A9F5 File Offset: 0x00018BF5
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x0001A9FD File Offset: 0x00018BFD
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "AllCommandSet")]
		[Alias(new string[]
		{
			"Type"
		})]
		public CommandTypes CommandType
		{
			get
			{
				return this.commandType;
			}
			set
			{
				this.commandType = value;
				this.isCommandTypeSpecified = true;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001AA0D File Offset: 0x00018C0D
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x0001AA15 File Offset: 0x00018C15
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int TotalCount
		{
			get
			{
				return this.totalCount;
			}
			set
			{
				this.totalCount = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001AA1E File Offset: 0x00018C1E
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001AA2B File Offset: 0x00018C2B
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public SwitchParameter Syntax
		{
			get
			{
				return this.usage;
			}
			set
			{
				this.usage = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001AA39 File Offset: 0x00018C39
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x0001AA41 File Offset: 0x00018C41
		[Parameter]
		public SwitchParameter ShowCommandInfo { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001AA4A File Offset: 0x00018C4A
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0001AA52 File Offset: 0x00018C52
		[Alias(new string[]
		{
			"Args"
		})]
		[AllowEmptyCollection]
		[Parameter(Position = 1, ValueFromRemainingArguments = true)]
		[AllowNull]
		public object[] ArgumentList
		{
			get
			{
				return this.commandArgs;
			}
			set
			{
				this.commandArgs = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001AA5B File Offset: 0x00018C5B
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0001AA68 File Offset: 0x00018C68
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public SwitchParameter All
		{
			get
			{
				return this.all;
			}
			set
			{
				this.all = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001AA76 File Offset: 0x00018C76
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0001AA83 File Offset: 0x00018C83
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public SwitchParameter ListImported
		{
			get
			{
				return this.listImported;
			}
			set
			{
				this.listImported = value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001AA91 File Offset: 0x00018C91
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0001AA99 File Offset: 0x00018C99
		[Parameter]
		[ValidateNotNullOrEmpty]
		public string[] ParameterName
		{
			get
			{
				return this._parameterNames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._parameterNames = value;
				this._parameterNameWildcards = SessionStateUtilities.CreateWildcardsFromStrings(this._parameterNames, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001AAC2 File Offset: 0x00018CC2
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001AAF8 File Offset: 0x00018CF8
		[Parameter]
		[ValidateNotNullOrEmpty]
		public PSTypeName[] ParameterType
		{
			get
			{
				return this._parameterTypes;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				List<PSTypeName> list = new List<PSTypeName>(value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					PSTypeName ptn = value[i];
					if (!value.Any((PSTypeName otherPtn) => otherPtn.Name.StartsWith(ptn.Name + "#", StringComparison.OrdinalIgnoreCase)) && (i == 0 || !(ptn.Type != null) || !ptn.Type.Equals(typeof(object))))
					{
						list.Add(ptn);
					}
				}
				this._parameterTypes = list.ToArray();
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001AB98 File Offset: 0x00018D98
		protected override void BeginProcessing()
		{
			base.BeginProcessing();
			if (this.ShowCommandInfo.IsPresent && this.Syntax.IsPresent)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSArgumentException(DiscoveryExceptions.GetCommandShowCommandInfoParamError), "GetCommandCannotSpecifySyntaxAndShowCommandInfoTogether", ErrorCategory.InvalidArgument, null));
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		protected override void ProcessRecord()
		{
			if (this.isModuleSpecified && this.isFullyQualifiedModuleSpecified)
			{
				string message = string.Format(CultureInfo.InvariantCulture, SessionStateStrings.GetContent_TailAndHeadCannotCoexist, new object[]
				{
					"Module",
					"FullyQualifiedModule"
				});
				ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(message), "ModuleAndFullyQualifiedModuleCannotBeSpecifiedTogether", ErrorCategory.InvalidOperation, null);
				base.ThrowTerminatingError(errorRecord);
			}
			if (this._modulePatterns == null)
			{
				this._modulePatterns = SessionStateUtilities.CreateWildcardsFromStrings(this.Module, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			}
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "CmdletSet")
				{
					this.AccumulateMatchingCmdlets();
					return;
				}
				if (!(parameterSetName == "AllCommandSet"))
				{
					return;
				}
				this.AccumulateMatchingCommands();
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001ACC4 File Offset: 0x00018EC4
		protected override void EndProcessing()
		{
			if (this.Name == null && !this.all && this.totalCount == -1)
			{
				CommandTypes commandTypesToIgnore = (CommandTypes)0;
				if ((this.CommandType & CommandTypes.Alias) != CommandTypes.Alias || !this.isCommandTypeSpecified)
				{
					commandTypesToIgnore |= CommandTypes.Alias;
				}
				if ((this.commandType & CommandTypes.Application) != CommandTypes.Application || !this.isCommandTypeSpecified)
				{
					commandTypesToIgnore |= CommandTypes.Application;
				}
				this.accumulatedResults = (from commandInfo in this.accumulatedResults
				where (commandInfo.CommandType & commandTypesToIgnore) == (CommandTypes)0 || commandInfo.Name.IndexOf('-') > 0
				select commandInfo).ToList<CommandInfo>();
			}
			if (this._matchedParameterNames != null && this.ParameterName != null)
			{
				foreach (string text in this.ParameterName)
				{
					if (!WildcardPattern.ContainsWildcardCharacters(text) && !this._matchedParameterNames.Contains(text))
					{
						string message = string.Format(CultureInfo.InvariantCulture, DiscoveryExceptions.CommandParameterNotFound, new object[]
						{
							text
						});
						ArgumentException exception = new ArgumentException(message, text);
						ErrorRecord errorRecord = new ErrorRecord(exception, "CommandParameterNotFound", ErrorCategory.ObjectNotFound, text);
						base.WriteError(errorRecord);
					}
				}
			}
			if (this.names == null || this.nameContainsWildcard)
			{
				this.accumulatedResults = this.accumulatedResults.OrderBy((CommandInfo a) => a, new GetCommandCommand.CommandInfoComparer()).ToList<CommandInfo>();
			}
			this.OutputResultsHelper(this.accumulatedResults);
			object variableValue = base.Context.GetVariableValue(new VariablePath("PSSenderInfo", VariablePathFlags.None));
			if (variableValue != null && variableValue is PSSenderInfo)
			{
				base.Context.HelpSystem.ResetHelpProviders();
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001AE70 File Offset: 0x00019070
		private void OutputResultsHelper(IEnumerable<CommandInfo> results)
		{
			CommandOrigin commandOrigin = base.MyInvocation.CommandOrigin;
			foreach (CommandInfo commandInfo in results)
			{
				if (SessionState.IsVisible(commandOrigin, commandInfo))
				{
					if (this.Syntax)
					{
						if (!string.IsNullOrEmpty(commandInfo.Syntax))
						{
							PSObject psobject = PSObject.AsPSObject(commandInfo.Syntax);
							psobject.IsHelpObject = true;
							base.WriteObject(psobject);
						}
					}
					else if (this.ShowCommandInfo.IsPresent)
					{
						base.WriteObject(GetCommandCommand.ConvertToShowCommandInfo(commandInfo));
					}
					else
					{
						base.WriteObject(commandInfo);
					}
				}
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001AF24 File Offset: 0x00019124
		private void AccumulateMatchingCmdlets()
		{
			this.commandType = (CommandTypes.Alias | CommandTypes.Function | CommandTypes.Filter | CommandTypes.Cmdlet | CommandTypes.Workflow | CommandTypes.Configuration);
			this.AccumulateMatchingCommands(new Collection<string>
			{
				"*"
			});
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001AF70 File Offset: 0x00019170
		private bool IsNounVerbMatch(CommandInfo command)
		{
			bool result = false;
			if (this.verbPatterns == null)
			{
				this.verbPatterns = SessionStateUtilities.CreateWildcardsFromStrings(this.Verb, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			}
			if (this.nounPatterns == null)
			{
				this.nounPatterns = SessionStateUtilities.CreateWildcardsFromStrings(this.Noun, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			}
			if (!string.IsNullOrEmpty(command.ModuleName))
			{
				if (this.isFullyQualifiedModuleSpecified)
				{
					if (!this._moduleSpecifications.Any((ModuleSpecification moduleSpecification) => ModuleIntrinsics.IsModuleMatchingModuleSpec(command.Module, moduleSpecification)))
					{
						return result;
					}
				}
				else if (!SessionStateUtilities.MatchesAnyWildcardPattern(command.ModuleName, this._modulePatterns, true))
				{
					return result;
				}
			}
			else if (this._modulePatterns.Count > 0 || this._moduleSpecifications.Any<ModuleSpecification>())
			{
				return result;
			}
			CmdletInfo cmdletInfo = command as CmdletInfo;
			string verb;
			string noun;
			if (cmdletInfo != null)
			{
				verb = cmdletInfo.Verb;
				noun = cmdletInfo.Noun;
			}
			else if (!CmdletInfo.SplitCmdletName(command.Name, out verb, out noun))
			{
				return result;
			}
			if (SessionStateUtilities.MatchesAnyWildcardPattern(verb, this.verbPatterns, true) && SessionStateUtilities.MatchesAnyWildcardPattern(noun, this.nounPatterns, true))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001B098 File Offset: 0x00019298
		private void AccumulateMatchingCommands()
		{
			Collection<string> collection = SessionStateUtilities.ConvertArrayToCollection<string>(this.Name);
			if (collection.Count == 0)
			{
				collection.Add("*");
			}
			this.AccumulateMatchingCommands(collection);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001B0CC File Offset: 0x000192CC
		private void AccumulateMatchingCommands(IEnumerable<string> commandNames)
		{
			SearchResolutionOptions searchResolutionOptions = SearchResolutionOptions.None;
			if (this.All)
			{
				searchResolutionOptions = SearchResolutionOptions.SearchAllScopes;
			}
			if ((this.CommandType & CommandTypes.Alias) != (CommandTypes)0)
			{
				searchResolutionOptions |= SearchResolutionOptions.ResolveAliasPatterns;
			}
			if ((this.CommandType & (CommandTypes.Function | CommandTypes.Filter | CommandTypes.Workflow | CommandTypes.Configuration)) != (CommandTypes)0)
			{
				searchResolutionOptions |= SearchResolutionOptions.ResolveFunctionPatterns;
			}
			foreach (string text in commandNames)
			{
				try
				{
					string text2;
					string pattern = Utils.ParseCommandName(text, out text2);
					bool flag = text2 != null;
					if (this.Module.Length == 1 && !WildcardPattern.ContainsWildcardCharacters(this.Module[0]))
					{
						text2 = this.Module[0];
					}
					bool flag2 = WildcardPattern.ContainsWildcardCharacters(pattern);
					if (flag2)
					{
						searchResolutionOptions |= SearchResolutionOptions.CommandNameIsPattern;
					}
					int num = 0;
					bool flag4;
					bool flag3 = this.FindCommandForName(searchResolutionOptions, text, flag2, true, ref num, out flag4);
					if (!flag3 || flag2)
					{
						if (!flag2 || !string.IsNullOrEmpty(text2))
						{
							string commandName = text;
							if (!flag && !string.IsNullOrEmpty(text2))
							{
								commandName = text2 + "\\" + text;
							}
							try
							{
								CommandDiscovery.LookupCommandInfo(commandName, base.MyInvocation.CommandOrigin, base.Context);
							}
							catch (CommandNotFoundException)
							{
							}
							flag3 = this.FindCommandForName(searchResolutionOptions, text, flag2, false, ref num, out flag4);
						}
						else if (!this.ListImported && (this.TotalCount < 0 || num < this.TotalCount))
						{
							foreach (CommandInfo commandInfo in ModuleUtils.GetMatchingCommands(pattern, base.Context, base.MyInvocation.CommandOrigin, true, this.isFullyQualifiedModuleSpecified))
							{
								CommandInfo commandInfo2 = commandInfo;
								if (this.IsCommandMatch(ref commandInfo2, out flag4) && !this.IsCommandInResult(commandInfo2) && this.IsParameterMatch(commandInfo2))
								{
									this.accumulatedResults.Add(commandInfo2);
									num++;
									if (this.TotalCount >= 0 && num >= this.TotalCount)
									{
										break;
									}
								}
							}
						}
					}
					if (!flag4 && !flag3 && !flag2)
					{
						CommandNotFoundException ex = new CommandNotFoundException(text, null, "CommandNotFoundException", DiscoveryExceptions.CommandNotFoundException, new object[0]);
						base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
					}
				}
				catch (CommandNotFoundException ex2)
				{
					base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
				}
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001B36C File Offset: 0x0001956C
		private bool FindCommandForName(SearchResolutionOptions options, string commandName, bool isPattern, bool emitErrors, ref int currentCount, out bool isDuplicate)
		{
			CommandSearcher commandSearcher = new CommandSearcher(commandName, options, this.CommandType, base.Context);
			bool result = false;
			isDuplicate = false;
			for (;;)
			{
				try
				{
					IL_1A:
					if (!commandSearcher.MoveNext())
					{
						break;
					}
				}
				catch (ArgumentException exception)
				{
					if (emitErrors)
					{
						base.WriteError(new ErrorRecord(exception, "GetCommandInvalidArgument", ErrorCategory.SyntaxError, null));
					}
					goto IL_1A;
				}
				catch (PathTooLongException exception2)
				{
					if (emitErrors)
					{
						base.WriteError(new ErrorRecord(exception2, "GetCommandInvalidArgument", ErrorCategory.SyntaxError, null));
					}
					goto IL_1A;
				}
				catch (FileLoadException exception3)
				{
					if (emitErrors)
					{
						base.WriteError(new ErrorRecord(exception3, "GetCommandFileLoadError", ErrorCategory.ReadError, null));
					}
					goto IL_1A;
				}
				catch (MetadataException exception4)
				{
					if (emitErrors)
					{
						base.WriteError(new ErrorRecord(exception4, "GetCommandMetadataError", ErrorCategory.MetadataError, null));
					}
					goto IL_1A;
				}
				catch (FormatException exception5)
				{
					if (emitErrors)
					{
						base.WriteError(new ErrorRecord(exception5, "GetCommandBadFileFormat", ErrorCategory.InvalidData, null));
					}
					goto IL_1A;
				}
				CommandInfo commandInfo = ((IEnumerator<CommandInfo>)commandSearcher).Current;
				CommandOrigin commandOrigin = base.MyInvocation.CommandOrigin;
				if (SessionState.IsVisible(commandOrigin, commandInfo))
				{
					bool flag = this.IsCommandMatch(ref commandInfo, out isDuplicate);
					if (flag && !this.IsCommandInResult(commandInfo))
					{
						result = true;
						if (this.IsParameterMatch(commandInfo))
						{
							currentCount++;
							if (this.TotalCount >= 0 && currentCount > this.TotalCount)
							{
								break;
							}
							this.accumulatedResults.Add(commandInfo);
							if (this.ArgumentList != null)
							{
								break;
							}
						}
						if (!isPattern && !this.All && this.totalCount == -1 && !this.isCommandTypeSpecified && !this.isModuleSpecified && !this.isFullyQualifiedModuleSpecified)
						{
							break;
						}
					}
				}
			}
			if (this.All)
			{
				foreach (CommandInfo commandInfo2 in this.GetMatchingCommandsFromModules(commandName))
				{
					CommandInfo commandInfo3 = commandInfo2;
					bool flag2 = this.IsCommandMatch(ref commandInfo3, out isDuplicate);
					if (flag2)
					{
						result = true;
						if (!this.IsCommandInResult(commandInfo2) && this.IsParameterMatch(commandInfo3))
						{
							currentCount++;
							if (this.TotalCount >= 0 && currentCount > this.TotalCount)
							{
								break;
							}
							this.accumulatedResults.Add(commandInfo3);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001B5D8 File Offset: 0x000197D8
		private bool IsDuplicate(CommandInfo info)
		{
			bool result = false;
			string text = null;
			ApplicationInfo applicationInfo = info as ApplicationInfo;
			if (applicationInfo != null)
			{
				text = applicationInfo.Path;
			}
			else
			{
				CmdletInfo cmdletInfo = info as CmdletInfo;
				if (cmdletInfo != null)
				{
					text = cmdletInfo.FullName;
				}
				else
				{
					ScriptInfo scriptInfo = info as ScriptInfo;
					if (scriptInfo != null)
					{
						text = scriptInfo.Definition;
					}
					else
					{
						ExternalScriptInfo externalScriptInfo = info as ExternalScriptInfo;
						if (externalScriptInfo != null)
						{
							text = externalScriptInfo.Path;
						}
					}
				}
			}
			if (text != null)
			{
				if (this.commandsWritten.ContainsKey(text))
				{
					result = true;
				}
				else
				{
					this.commandsWritten.Add(text, info);
				}
			}
			return result;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001B65C File Offset: 0x0001985C
		private bool IsParameterMatch(CommandInfo commandInfo)
		{
			if (this.ParameterName == null && this.ParameterType == null)
			{
				return true;
			}
			if (this._matchedParameterNames == null)
			{
				this._matchedParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			}
			IEnumerable<ParameterMetadata> enumerable = null;
			try
			{
				IDictionary<string, ParameterMetadata> parameters = commandInfo.Parameters;
				if (parameters != null)
				{
					enumerable = parameters.Values;
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			if (enumerable == null)
			{
				return false;
			}
			bool result = false;
			foreach (ParameterMetadata parameterMetadata in enumerable)
			{
				if (this.IsParameterMatch(parameterMetadata))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001B70C File Offset: 0x0001990C
		private bool IsParameterMatch(ParameterMetadata parameterMetadata)
		{
			bool flag = SessionStateUtilities.MatchesAnyWildcardPattern(parameterMetadata.Name, this._parameterNameWildcards, true);
			bool flag2 = false;
			foreach (string text in ((IEnumerable<string>)(parameterMetadata.Aliases ?? Enumerable.Empty<string>())))
			{
				if (SessionStateUtilities.MatchesAnyWildcardPattern(text, this._parameterNameWildcards, true))
				{
					this._matchedParameterNames.Add(text);
					flag2 = true;
				}
			}
			bool flag3 = flag || flag2;
			if (flag3)
			{
				this._matchedParameterNames.Add(parameterMetadata.Name);
			}
			bool flag4;
			if (this._parameterTypes == null || this._parameterTypes.Length == 0)
			{
				flag4 = true;
			}
			else
			{
				flag4 = false;
				if (this._parameterTypes != null && this._parameterTypes.Length > 0)
				{
					flag4 |= this._parameterTypes.Any(new Func<PSTypeName, bool>(parameterMetadata.IsMatchingType));
				}
			}
			return flag3 && flag4;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001B800 File Offset: 0x00019A00
		private bool IsCommandMatch(ref CommandInfo current, out bool isDuplicate)
		{
			bool flag = false;
			isDuplicate = false;
			if (!this.IsDuplicate(current))
			{
				if ((current.CommandType & this.CommandType) != (CommandTypes)0)
				{
					flag = true;
				}
				if (current.CommandType == CommandTypes.Cmdlet || ((this.verbs.Length > 0 || this.nouns.Length > 0) && (current.CommandType == CommandTypes.Function || current.CommandType == CommandTypes.Filter || current.CommandType == CommandTypes.Workflow || current.CommandType == CommandTypes.Configuration || current.CommandType == CommandTypes.Alias)))
				{
					if (!this.IsNounVerbMatch(current))
					{
						flag = false;
					}
				}
				else if (this.isFullyQualifiedModuleSpecified)
				{
					bool flag2 = false;
					foreach (ModuleSpecification moduleSpec in this._moduleSpecifications)
					{
						if (ModuleIntrinsics.IsModuleMatchingModuleSpec(current.Module, moduleSpec))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = false;
					}
				}
				else if (this._modulePatterns != null && this._modulePatterns.Count > 0 && !SessionStateUtilities.MatchesAnyWildcardPattern(current.ModuleName, this._modulePatterns, true))
				{
					flag = false;
				}
				if (!flag)
				{
					return flag;
				}
				if (this.ArgumentList != null)
				{
					AliasInfo aliasInfo = current as AliasInfo;
					if (aliasInfo != null)
					{
						current = aliasInfo.ResolvedCommand;
						if (current == null)
						{
							return false;
						}
					}
					else if (!(current is CmdletInfo) && !(current is IScriptCommandInfo))
					{
						base.ThrowTerminatingError(new ErrorRecord(PSTraceSource.NewArgumentException("ArgumentList", DiscoveryExceptions.CommandArgsOnlyForSingleCmdlet, new object[0]), "CommandArgsOnlyForSingleCmdlet", ErrorCategory.InvalidArgument, current));
					}
				}
				bool flag3 = false;
				try
				{
					flag3 = current.ImplementsDynamicParameters;
				}
				catch (PSSecurityException)
				{
				}
				catch (RuntimeException)
				{
				}
				if (!flag3)
				{
					return flag;
				}
				try
				{
					CommandInfo commandInfo = current.CreateGetCommandCopy(this.ArgumentList);
					if (this.ArgumentList != null)
					{
						ReadOnlyCollection<CommandParameterSetInfo> parameterSets = commandInfo.ParameterSets;
					}
					current = commandInfo;
					return flag;
				}
				catch (MetadataException exception)
				{
					base.WriteError(new ErrorRecord(exception, "GetCommandMetadataError", ErrorCategory.MetadataError, current));
					return flag;
				}
				catch (ParameterBindingException ex)
				{
					if (!ex.ErrorRecord.FullyQualifiedErrorId.StartsWith("GetDynamicParametersException", StringComparison.Ordinal))
					{
						throw;
					}
					return flag;
				}
			}
			isDuplicate = true;
			return flag;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001BF30 File Offset: 0x0001A130
		private IEnumerable<CommandInfo> GetMatchingCommandsFromModules(string commandName)
		{
			WildcardPattern matcher = new WildcardPattern(commandName, WildcardOptions.IgnoreCase);
			for (int i = base.Context.EngineSessionState.ModuleTableKeys.Count - 1; i >= 0; i--)
			{
				PSModuleInfo module = null;
				if (base.Context.EngineSessionState.ModuleTable.TryGetValue(base.Context.EngineSessionState.ModuleTableKeys[i], out module))
				{
					bool isModuleMatch = false;
					if (!this.isFullyQualifiedModuleSpecified)
					{
						isModuleMatch = SessionStateUtilities.MatchesAnyWildcardPattern(module.Name, this._modulePatterns, true);
					}
					else if (this._moduleSpecifications.Any((ModuleSpecification moduleSpecification) => ModuleIntrinsics.IsModuleMatchingModuleSpec(module, moduleSpecification)))
					{
						isModuleMatch = true;
					}
					if (isModuleMatch && module.SessionState != null)
					{
						if ((this.CommandType & (CommandTypes.Function | CommandTypes.Filter | CommandTypes.Configuration)) != (CommandTypes)0)
						{
							foreach (object obj in module.SessionState.Internal.GetFunctionTable())
							{
								DictionaryEntry function = (DictionaryEntry)obj;
								DictionaryEntry dictionaryEntry = function;
								FunctionInfo func = (FunctionInfo)dictionaryEntry.Value;
								WildcardPattern wildcardPattern = matcher;
								DictionaryEntry dictionaryEntry2 = function;
								if (wildcardPattern.IsMatch((string)dictionaryEntry2.Key) && func.IsImported && func.Module.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
								{
									DictionaryEntry dictionaryEntry3 = function;
									yield return (CommandInfo)dictionaryEntry3.Value;
								}
							}
						}
						if ((this.CommandType & CommandTypes.Alias) != (CommandTypes)0)
						{
							foreach (KeyValuePair<string, AliasInfo> alias in module.SessionState.Internal.GetAliasTable())
							{
								WildcardPattern wildcardPattern2 = matcher;
								KeyValuePair<string, AliasInfo> keyValuePair = alias;
								if (wildcardPattern2.IsMatch(keyValuePair.Key))
								{
									KeyValuePair<string, AliasInfo> keyValuePair2 = alias;
									if (keyValuePair2.Value.IsImported)
									{
										KeyValuePair<string, AliasInfo> keyValuePair3 = alias;
										if (keyValuePair3.Value.Module.Path.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
										{
											KeyValuePair<string, AliasInfo> keyValuePair4 = alias;
											yield return keyValuePair4.Value;
										}
									}
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001BF54 File Offset: 0x0001A154
		private bool IsCommandInResult(CommandInfo command)
		{
			bool result = false;
			bool flag = command.Module != null;
			foreach (CommandInfo commandInfo in this.accumulatedResults)
			{
				if (command.CommandType == commandInfo.CommandType && (string.Compare(command.Name, commandInfo.Name, StringComparison.CurrentCultureIgnoreCase) == 0 || string.Compare(ModuleCmdletBase.RemovePrefixFromCommandName(commandInfo.Name, commandInfo.Prefix), command.Name, StringComparison.CurrentCultureIgnoreCase) == 0) && commandInfo.Module != null && flag && ((commandInfo.IsImported && command.IsImported && commandInfo.Module.Equals(command.Module)) || ((!commandInfo.IsImported || !command.IsImported) && commandInfo.Module.Path.Equals(command.Module.Path, StringComparison.OrdinalIgnoreCase))))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001C05C File Offset: 0x0001A25C
		private static PSObject ConvertToShowCommandInfo(CommandInfo cmdInfo)
		{
			return new PSObject
			{
				Properties = 
				{
					new PSNoteProperty("Name", cmdInfo.Name),
					new PSNoteProperty("ModuleName", cmdInfo.ModuleName),
					new PSNoteProperty("Module", GetCommandCommand.GetModuleInfo(cmdInfo)),
					new PSNoteProperty("CommandType", cmdInfo.CommandType),
					new PSNoteProperty("Definition", cmdInfo.Definition),
					new PSNoteProperty("ParameterSets", GetCommandCommand.GetParameterSets(cmdInfo))
				}
			};
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001C118 File Offset: 0x0001A318
		private static PSObject GetModuleInfo(CommandInfo cmdInfo)
		{
			PSObject psobject = new PSObject();
			string value = (cmdInfo.Module != null) ? cmdInfo.Module.Name : string.Empty;
			psobject.Properties.Add(new PSNoteProperty("Name", value));
			return psobject;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001C160 File Offset: 0x0001A360
		private static PSObject[] GetParameterSets(CommandInfo cmdInfo)
		{
			ReadOnlyCollection<CommandParameterSetInfo> readOnlyCollection = null;
			try
			{
				if (cmdInfo.ParameterSets != null)
				{
					readOnlyCollection = cmdInfo.ParameterSets;
				}
			}
			catch (InvalidOperationException)
			{
			}
			catch (PSNotSupportedException)
			{
			}
			catch (PSNotImplementedException)
			{
			}
			if (readOnlyCollection == null)
			{
				return new PSObject[0];
			}
			List<PSObject> list = new List<PSObject>(cmdInfo.ParameterSets.Count);
			foreach (CommandParameterSetInfo commandParameterSetInfo in readOnlyCollection)
			{
				list.Add(new PSObject
				{
					Properties = 
					{
						new PSNoteProperty("Name", commandParameterSetInfo.Name),
						new PSNoteProperty("IsDefault", commandParameterSetInfo.IsDefault),
						new PSNoteProperty("Parameters", GetCommandCommand.GetParameterInfo(commandParameterSetInfo.Parameters))
					}
				});
			}
			return list.ToArray();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001C27C File Offset: 0x0001A47C
		private static PSObject[] GetParameterInfo(ReadOnlyCollection<CommandParameterInfo> parameters)
		{
			List<PSObject> list = new List<PSObject>(parameters.Count);
			foreach (CommandParameterInfo commandParameterInfo in parameters)
			{
				PSObject psobject = new PSObject();
				psobject.Properties.Add(new PSNoteProperty("Name", commandParameterInfo.Name));
				psobject.Properties.Add(new PSNoteProperty("IsMandatory", commandParameterInfo.IsMandatory));
				psobject.Properties.Add(new PSNoteProperty("ValueFromPipeline", commandParameterInfo.ValueFromPipeline));
				psobject.Properties.Add(new PSNoteProperty("Position", commandParameterInfo.Position));
				psobject.Properties.Add(new PSNoteProperty("ParameterType", GetCommandCommand.GetParameterType(commandParameterInfo.ParameterType)));
				bool flag = false;
				IList<string> value = new List<string>();
				ValidateSetAttribute validateSetAttribute = (from x in commandParameterInfo.Attributes
				where x is ValidateSetAttribute
				select x).Cast<ValidateSetAttribute>().LastOrDefault<ValidateSetAttribute>();
				if (validateSetAttribute != null)
				{
					flag = true;
					value = validateSetAttribute.ValidValues;
				}
				psobject.Properties.Add(new PSNoteProperty("HasParameterSet", flag));
				psobject.Properties.Add(new PSNoteProperty("ValidParamSetValues", value));
				list.Add(psobject);
			}
			return list.ToArray();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001C40C File Offset: 0x0001A60C
		private static PSObject GetParameterType(Type parameterType)
		{
			PSObject psobject = new PSObject();
			bool isEnum = parameterType.GetTypeInfo().IsEnum;
			bool isArray = parameterType.GetTypeInfo().IsArray;
			psobject.Properties.Add(new PSNoteProperty("FullName", parameterType.FullName));
			psobject.Properties.Add(new PSNoteProperty("IsEnum", isEnum));
			psobject.Properties.Add(new PSNoteProperty("IsArray", isArray));
			ArrayList value = isEnum ? new ArrayList(Enum.GetValues(parameterType)) : new ArrayList();
			psobject.Properties.Add(new PSNoteProperty("EnumValues", value));
			bool flag = isArray && parameterType.GetTypeInfo().GetCustomAttributes(typeof(FlagsAttribute), true).Count<object>() > 0;
			psobject.Properties.Add(new PSNoteProperty("HasFlagAttribute", flag));
			object value2 = isArray ? GetCommandCommand.GetParameterType(parameterType.GetElementType()) : null;
			psobject.Properties.Add(new PSNoteProperty("ElementType", value2));
			bool flag2 = !isEnum && !isArray && parameterType is IDictionary;
			psobject.Properties.Add(new PSNoteProperty("ImplementsDictionary", flag2));
			return psobject;
		}

		// Token: 0x04000254 RID: 596
		private string[] names;

		// Token: 0x04000255 RID: 597
		private bool nameContainsWildcard;

		// Token: 0x04000256 RID: 598
		private string[] verbs = new string[0];

		// Token: 0x04000257 RID: 599
		private string[] nouns = new string[0];

		// Token: 0x04000258 RID: 600
		private string[] _modules = new string[0];

		// Token: 0x04000259 RID: 601
		private bool isModuleSpecified;

		// Token: 0x0400025A RID: 602
		private ModuleSpecification[] _moduleSpecifications = new ModuleSpecification[0];

		// Token: 0x0400025B RID: 603
		private bool isFullyQualifiedModuleSpecified;

		// Token: 0x0400025C RID: 604
		private CommandTypes commandType = CommandTypes.All;

		// Token: 0x0400025D RID: 605
		private bool isCommandTypeSpecified;

		// Token: 0x0400025E RID: 606
		private int totalCount = -1;

		// Token: 0x0400025F RID: 607
		private bool usage;

		// Token: 0x04000260 RID: 608
		private object[] commandArgs;

		// Token: 0x04000261 RID: 609
		private bool all;

		// Token: 0x04000262 RID: 610
		private bool listImported;

		// Token: 0x04000263 RID: 611
		private Collection<WildcardPattern> _parameterNameWildcards;

		// Token: 0x04000264 RID: 612
		private string[] _parameterNames;

		// Token: 0x04000265 RID: 613
		private HashSet<string> _matchedParameterNames;

		// Token: 0x04000266 RID: 614
		private PSTypeName[] _parameterTypes;

		// Token: 0x04000267 RID: 615
		private Dictionary<string, CommandInfo> commandsWritten = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000268 RID: 616
		private List<CommandInfo> accumulatedResults = new List<CommandInfo>();

		// Token: 0x04000269 RID: 617
		private Collection<WildcardPattern> verbPatterns;

		// Token: 0x0400026A RID: 618
		private Collection<WildcardPattern> nounPatterns;

		// Token: 0x0400026B RID: 619
		private Collection<WildcardPattern> _modulePatterns;

		// Token: 0x0200006E RID: 110
		private class CommandInfoComparer : IComparer<CommandInfo>
		{
			// Token: 0x0600060E RID: 1550 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
			public int Compare(CommandInfo x, CommandInfo y)
			{
				if (x.CommandType < y.CommandType)
				{
					return -1;
				}
				if (x.CommandType > y.CommandType)
				{
					return 1;
				}
				return string.Compare(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
			}
		}
	}
}
