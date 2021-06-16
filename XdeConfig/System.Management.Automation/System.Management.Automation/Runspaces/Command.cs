using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001EA RID: 490
	public sealed class Command
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x0008F398 File Offset: 0x0008D598
		public Command(string command) : this(command, false, null)
		{
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0008F3B8 File Offset: 0x0008D5B8
		public Command(string command, bool isScript) : this(command, isScript, null)
		{
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0008F3D8 File Offset: 0x0008D5D8
		public Command(string command, bool isScript, bool useLocalScope)
		{
			this._mergeInstructions = new PipelineResultTypes[5];
			this._parameters = new CommandParameterCollection();
			this._command = string.Empty;
			base..ctor();
			this.IsEndOfStatement = false;
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this._command = command;
			this._isScript = isScript;
			this._useLocalScope = new bool?(useLocalScope);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0008F43C File Offset: 0x0008D63C
		internal Command(string command, bool isScript, bool? useLocalScope)
		{
			this._mergeInstructions = new PipelineResultTypes[5];
			this._parameters = new CommandParameterCollection();
			this._command = string.Empty;
			base..ctor();
			this.IsEndOfStatement = false;
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this._command = command;
			this._isScript = isScript;
			this._useLocalScope = useLocalScope;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0008F49B File Offset: 0x0008D69B
		internal Command(string command, bool isScript, bool? useLocalScope, bool mergeUnclaimedPreviousErrorResults) : this(command, isScript, useLocalScope)
		{
			if (mergeUnclaimedPreviousErrorResults)
			{
				this._mergeUnclaimedPreviousCommandResults = PipelineResultTypes.Warning;
			}
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0008F4B1 File Offset: 0x0008D6B1
		internal Command(CommandInfo commandInfo) : this(commandInfo, false)
		{
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0008F4BC File Offset: 0x0008D6BC
		internal Command(CommandInfo commandInfo, bool isScript)
		{
			this._mergeInstructions = new PipelineResultTypes[5];
			this._parameters = new CommandParameterCollection();
			this._command = string.Empty;
			base..ctor();
			this.IsEndOfStatement = false;
			this._commandInfo = commandInfo;
			this._command = this._commandInfo.Name;
			this._isScript = isScript;
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0008F518 File Offset: 0x0008D718
		internal Command(Command command)
		{
			this._mergeInstructions = new PipelineResultTypes[5];
			this._parameters = new CommandParameterCollection();
			this._command = string.Empty;
			base..ctor();
			this._isScript = command._isScript;
			this._useLocalScope = command._useLocalScope;
			this._command = command._command;
			this._mergeInstructions = command._mergeInstructions;
			this._mergeMyResult = command._mergeMyResult;
			this._mergeToResult = command._mergeToResult;
			this._mergeUnclaimedPreviousCommandResults = command._mergeUnclaimedPreviousCommandResults;
			this.IsEndOfStatement = command.IsEndOfStatement;
			foreach (CommandParameter commandParameter in command.Parameters)
			{
				this.Parameters.Add(new CommandParameter(commandParameter.Name, commandParameter.Value));
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x0008F604 File Offset: 0x0008D804
		public CommandParameterCollection Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x0008F60C File Offset: 0x0008D80C
		public string CommandText
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x0008F614 File Offset: 0x0008D814
		internal CommandInfo CommandInfo
		{
			get
			{
				return this._commandInfo;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x0008F61C File Offset: 0x0008D81C
		public bool IsScript
		{
			get
			{
				return this._isScript;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x0008F624 File Offset: 0x0008D824
		public bool UseLocalScope
		{
			get
			{
				return this._useLocalScope ?? false;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x0008F64A File Offset: 0x0008D84A
		// (set) Token: 0x06001672 RID: 5746 RVA: 0x0008F652 File Offset: 0x0008D852
		public CommandOrigin CommandOrigin
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

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x0008F65B File Offset: 0x0008D85B
		internal bool? UseLocalScopeNullable
		{
			get
			{
				return this._useLocalScope;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x0008F663 File Offset: 0x0008D863
		// (set) Token: 0x06001675 RID: 5749 RVA: 0x0008F66B File Offset: 0x0008D86B
		public bool IsEndOfStatement { get; internal set; }

		// Token: 0x06001676 RID: 5750 RVA: 0x0008F674 File Offset: 0x0008D874
		internal Command Clone()
		{
			return new Command(this);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0008F67C File Offset: 0x0008D87C
		public override string ToString()
		{
			return this._command;
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x0008F684 File Offset: 0x0008D884
		// (set) Token: 0x06001679 RID: 5753 RVA: 0x0008F68C File Offset: 0x0008D88C
		public PipelineResultTypes MergeUnclaimedPreviousCommandResults
		{
			get
			{
				return this._mergeUnclaimedPreviousCommandResults;
			}
			set
			{
				if (value == PipelineResultTypes.None)
				{
					this._mergeUnclaimedPreviousCommandResults = value;
					return;
				}
				if (value != PipelineResultTypes.Warning)
				{
					throw PSTraceSource.NewNotSupportedException();
				}
				this._mergeUnclaimedPreviousCommandResults = value;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x0008F6AA File Offset: 0x0008D8AA
		internal PipelineResultTypes MergeMyResult
		{
			get
			{
				return this._mergeMyResult;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x0008F6B2 File Offset: 0x0008D8B2
		internal PipelineResultTypes MergeToResult
		{
			get
			{
				return this._mergeToResult;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x0008F6BA File Offset: 0x0008D8BA
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x0008F6C2 File Offset: 0x0008D8C2
		internal PipelineResultTypes[] MergeInstructions
		{
			get
			{
				return this._mergeInstructions;
			}
			set
			{
				this._mergeInstructions = value;
			}
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0008F6CC File Offset: 0x0008D8CC
		public void MergeMyResults(PipelineResultTypes myResult, PipelineResultTypes toResult)
		{
			if (myResult == PipelineResultTypes.None && toResult == PipelineResultTypes.None)
			{
				this._mergeMyResult = myResult;
				this._mergeToResult = toResult;
				for (int i = 0; i < 5; i++)
				{
					this._mergeInstructions[i] = PipelineResultTypes.None;
				}
				return;
			}
			if (myResult == PipelineResultTypes.None || myResult == PipelineResultTypes.Output)
			{
				throw PSTraceSource.NewArgumentException("myResult", RunspaceStrings.InvalidMyResultError, new object[0]);
			}
			if (myResult == PipelineResultTypes.Error && toResult != PipelineResultTypes.Output)
			{
				throw PSTraceSource.NewArgumentException("toResult", RunspaceStrings.InvalidValueToResultError, new object[0]);
			}
			if (toResult != PipelineResultTypes.Output && toResult != PipelineResultTypes.Null)
			{
				throw PSTraceSource.NewArgumentException("toResult", RunspaceStrings.InvalidValueToResult, new object[0]);
			}
			if (myResult == PipelineResultTypes.Error)
			{
				this._mergeMyResult = myResult;
				this._mergeToResult = toResult;
			}
			if (myResult == PipelineResultTypes.Error || myResult == PipelineResultTypes.All)
			{
				this._mergeInstructions[0] = toResult;
			}
			if (myResult == PipelineResultTypes.Warning || myResult == PipelineResultTypes.All)
			{
				this._mergeInstructions[1] = toResult;
			}
			if (myResult == PipelineResultTypes.Verbose || myResult == PipelineResultTypes.All)
			{
				this._mergeInstructions[2] = toResult;
			}
			if (myResult == PipelineResultTypes.Debug || myResult == PipelineResultTypes.All)
			{
				this._mergeInstructions[3] = toResult;
			}
			if (myResult == PipelineResultTypes.Information || myResult == PipelineResultTypes.All)
			{
				this._mergeInstructions[4] = toResult;
			}
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0008F7C4 File Offset: 0x0008D9C4
		private void SetMergeSettingsOnCommandProcessor(CommandProcessorBase commandProcessor)
		{
			MshCommandRuntime mshCommandRuntime = commandProcessor.Command.commandRuntime as MshCommandRuntime;
			if (this._mergeUnclaimedPreviousCommandResults != PipelineResultTypes.None && mshCommandRuntime != null)
			{
				mshCommandRuntime.MergeUnclaimedPreviousErrorResults = true;
			}
			if (this._mergeInstructions[0] == PipelineResultTypes.Output)
			{
				mshCommandRuntime.ErrorMergeTo = MshCommandRuntime.MergeDataStream.Output;
			}
			PipelineResultTypes pipelineResultTypes = this._mergeInstructions[1];
			if (pipelineResultTypes != PipelineResultTypes.None)
			{
				mshCommandRuntime.WarningOutputPipe = this.GetRedirectionPipe(pipelineResultTypes, mshCommandRuntime);
			}
			pipelineResultTypes = this._mergeInstructions[2];
			if (pipelineResultTypes != PipelineResultTypes.None)
			{
				mshCommandRuntime.VerboseOutputPipe = this.GetRedirectionPipe(pipelineResultTypes, mshCommandRuntime);
			}
			pipelineResultTypes = this._mergeInstructions[3];
			if (pipelineResultTypes != PipelineResultTypes.None)
			{
				mshCommandRuntime.DebugOutputPipe = this.GetRedirectionPipe(pipelineResultTypes, mshCommandRuntime);
			}
			pipelineResultTypes = this._mergeInstructions[4];
			if (pipelineResultTypes != PipelineResultTypes.None)
			{
				mshCommandRuntime.InformationOutputPipe = this.GetRedirectionPipe(pipelineResultTypes, mshCommandRuntime);
			}
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x0008F870 File Offset: 0x0008DA70
		private Pipe GetRedirectionPipe(PipelineResultTypes toType, MshCommandRuntime mcr)
		{
			if (toType == PipelineResultTypes.Output)
			{
				return mcr.OutputPipe;
			}
			return new Pipe
			{
				NullPipe = true
			};
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x0008F898 File Offset: 0x0008DA98
		internal CommandProcessorBase CreateCommandProcessor(ExecutionContext executionContext, CommandFactory commandFactory, bool addToHistory, CommandOrigin origin)
		{
			CommandProcessorBase commandProcessorBase;
			if (this.IsScript)
			{
				if (executionContext.LanguageMode == PSLanguageMode.NoLanguage && origin == CommandOrigin.Runspace)
				{
					throw InterpreterError.NewInterpreterException(this.CommandText, typeof(ParseException), null, "ScriptsNotAllowed", ParserStrings.ScriptsNotAllowed, new object[0]);
				}
				ScriptBlock scriptBlock = executionContext.Engine.ParseScriptBlock(this.CommandText, addToHistory);
				if (origin == CommandOrigin.Internal)
				{
					scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
				}
				PSLanguageMode valueOrDefault = scriptBlock.LanguageMode.GetValueOrDefault();
				PSLanguageMode? pslanguageMode;
				if (pslanguageMode != null)
				{
					switch (valueOrDefault)
					{
					case PSLanguageMode.FullLanguage:
					case PSLanguageMode.ConstrainedLanguage:
						break;
					case PSLanguageMode.RestrictedLanguage:
						scriptBlock.CheckRestrictedLanguage(null, null, false);
						break;
					case PSLanguageMode.NoLanguage:
						goto IL_A0;
					default:
						goto IL_A0;
					}
					if (scriptBlock.UsesCmdletBinding)
					{
						FunctionInfo scriptCommandInfo = new FunctionInfo("", scriptBlock, executionContext);
						commandProcessorBase = new CommandProcessor(scriptCommandInfo, executionContext, this._useLocalScope ?? false, false, executionContext.EngineSessionState);
						goto IL_1AE;
					}
					commandProcessorBase = new DlrScriptCommandProcessor(scriptBlock, executionContext, this._useLocalScope ?? false, origin, executionContext.EngineSessionState);
					goto IL_1AE;
				}
				IL_A0:
				throw new InvalidOperationException("Invalid langage mode was set when building a ScriptCommandProcessor");
			}
			else
			{
				if (this._useLocalScope != null && !this._useLocalScope.Value)
				{
					switch (executionContext.LanguageMode)
					{
					case PSLanguageMode.RestrictedLanguage:
					case PSLanguageMode.NoLanguage:
					{
						string message = StringUtil.Format(RunspaceStrings.UseLocalScopeNotAllowed, new object[]
						{
							"UseLocalScope",
							PSLanguageMode.RestrictedLanguage.ToString(),
							PSLanguageMode.NoLanguage.ToString()
						});
						throw new RuntimeException(message);
					}
					}
				}
				commandProcessorBase = commandFactory.CreateCommand(this.CommandText, origin, this._useLocalScope);
			}
			IL_1AE:
			CommandParameterCollection parameters = this.Parameters;
			if (parameters != null)
			{
				bool forNativeCommand = commandProcessorBase is NativeCommandProcessor;
				foreach (CommandParameter publicParameter in parameters)
				{
					CommandParameterInternal parameter = CommandParameter.ToCommandParameterInternal(publicParameter, forNativeCommand);
					commandProcessorBase.AddParameter(parameter);
				}
			}
			string helpTarget;
			HelpCategory helpCategory;
			if (commandProcessorBase.IsHelpRequested(out helpTarget, out helpCategory))
			{
				commandProcessorBase = CommandProcessorBase.CreateGetHelpCommandProcessor(executionContext, helpTarget, helpCategory);
			}
			this.SetMergeSettingsOnCommandProcessor(commandProcessorBase);
			return commandProcessorBase;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x0008FAD8 File Offset: 0x0008DCD8
		internal static Command FromPSObjectForRemoting(PSObject commandAsPSObject)
		{
			if (commandAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandAsPSObject");
			}
			string propertyValue = RemotingDecoder.GetPropertyValue<string>(commandAsPSObject, "Cmd");
			bool propertyValue2 = RemotingDecoder.GetPropertyValue<bool>(commandAsPSObject, "IsScript");
			bool? propertyValue3 = RemotingDecoder.GetPropertyValue<bool?>(commandAsPSObject, "UseLocalScope");
			Command command = new Command(propertyValue, propertyValue2, propertyValue3);
			PipelineResultTypes propertyValue4 = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeMyResult");
			PipelineResultTypes propertyValue5 = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeToResult");
			command.MergeMyResults(propertyValue4, propertyValue5);
			command.MergeUnclaimedPreviousCommandResults = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergePreviousResults");
			if (commandAsPSObject.Properties["MergeError"] != null)
			{
				command.MergeInstructions[0] = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeError");
			}
			if (commandAsPSObject.Properties["MergeWarning"] != null)
			{
				command.MergeInstructions[1] = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeWarning");
			}
			if (commandAsPSObject.Properties["MergeVerbose"] != null)
			{
				command.MergeInstructions[2] = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeVerbose");
			}
			if (commandAsPSObject.Properties["MergeDebug"] != null)
			{
				command.MergeInstructions[3] = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeDebug");
			}
			if (commandAsPSObject.Properties["MergeInformation"] != null)
			{
				command.MergeInstructions[4] = RemotingDecoder.GetPropertyValue<PipelineResultTypes>(commandAsPSObject, "MergeInformation");
			}
			foreach (PSObject parameterAsPSObject in RemotingDecoder.EnumerateListProperty<PSObject>(commandAsPSObject, "Args"))
			{
				command.Parameters.Add(CommandParameter.FromPSObjectForRemoting(parameterAsPSObject));
			}
			return command;
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0008FC68 File Offset: 0x0008DE68
		internal PSObject ToPSObjectForRemoting(Version psRPVersion)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("Cmd", this.CommandText));
			psobject.Properties.Add(new PSNoteProperty("IsScript", this.IsScript));
			psobject.Properties.Add(new PSNoteProperty("UseLocalScope", this.UseLocalScopeNullable));
			psobject.Properties.Add(new PSNoteProperty("MergeMyResult", this.MergeMyResult));
			psobject.Properties.Add(new PSNoteProperty("MergeToResult", this.MergeToResult));
			psobject.Properties.Add(new PSNoteProperty("MergePreviousResults", this.MergeUnclaimedPreviousCommandResults));
			if (psRPVersion != null && psRPVersion >= RemotingConstants.ProtocolVersionWin10RTM)
			{
				psobject.Properties.Add(new PSNoteProperty("MergeError", this._mergeInstructions[0]));
				psobject.Properties.Add(new PSNoteProperty("MergeWarning", this._mergeInstructions[1]));
				psobject.Properties.Add(new PSNoteProperty("MergeVerbose", this._mergeInstructions[2]));
				psobject.Properties.Add(new PSNoteProperty("MergeDebug", this._mergeInstructions[3]));
				psobject.Properties.Add(new PSNoteProperty("MergeInformation", this._mergeInstructions[4]));
			}
			else if (psRPVersion != null && psRPVersion >= RemotingConstants.ProtocolVersionWin8RTM)
			{
				psobject.Properties.Add(new PSNoteProperty("MergeError", this._mergeInstructions[0]));
				psobject.Properties.Add(new PSNoteProperty("MergeWarning", this._mergeInstructions[1]));
				psobject.Properties.Add(new PSNoteProperty("MergeVerbose", this._mergeInstructions[2]));
				psobject.Properties.Add(new PSNoteProperty("MergeDebug", this._mergeInstructions[3]));
				if (this._mergeInstructions[4] == PipelineResultTypes.Output && this._mergeInstructions.Length != 5)
				{
					throw new RuntimeException(StringUtil.Format(RunspaceStrings.InformationRedirectionNotSupported, new object[0]));
				}
			}
			else if (this._mergeInstructions.Length != 5)
			{
				if (this._mergeInstructions[1] == PipelineResultTypes.Output)
				{
					throw new RuntimeException(StringUtil.Format(RunspaceStrings.WarningRedirectionNotSupported, new object[0]));
				}
				if (this._mergeInstructions[2] == PipelineResultTypes.Output)
				{
					throw new RuntimeException(StringUtil.Format(RunspaceStrings.VerboseRedirectionNotSupported, new object[0]));
				}
				if (this._mergeInstructions[3] == PipelineResultTypes.Output)
				{
					throw new RuntimeException(StringUtil.Format(RunspaceStrings.DebugRedirectionNotSupported, new object[0]));
				}
				if (this._mergeInstructions[4] == PipelineResultTypes.Output)
				{
					throw new RuntimeException(StringUtil.Format(RunspaceStrings.InformationRedirectionNotSupported, new object[0]));
				}
			}
			List<PSObject> list = new List<PSObject>(this.Parameters.Count);
			foreach (CommandParameter commandParameter in this.Parameters)
			{
				list.Add(commandParameter.ToPSObjectForRemoting());
			}
			psobject.Properties.Add(new PSNoteProperty("Args", list));
			return psobject;
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x0008FFD8 File Offset: 0x0008E1D8
		internal CimInstance ToCimInstance()
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance("PS_Command");
			CimProperty newItem = InternalMISerializer.CreateCimProperty("CommandText", this.CommandText, CimType.String);
			cimInstance.CimInstanceProperties.Add(newItem);
			CimProperty newItem2 = InternalMISerializer.CreateCimProperty("IsScript", this.IsScript, CimType.Boolean);
			cimInstance.CimInstanceProperties.Add(newItem2);
			if (this.Parameters != null && this.Parameters.Count > 0)
			{
				List<CimInstance> list = new List<CimInstance>();
				foreach (CommandParameter commandParameter in this.Parameters)
				{
					list.Add(commandParameter.ToCimInstance());
				}
				if (list.Count > 0)
				{
					CimProperty newItem3 = InternalMISerializer.CreateCimProperty("Parameters", list.ToArray(), CimType.ReferenceArray);
					cimInstance.CimInstanceProperties.Add(newItem3);
				}
			}
			return cimInstance;
		}

		// Token: 0x04000988 RID: 2440
		internal const int MaxMergeType = 5;

		// Token: 0x04000989 RID: 2441
		private CommandOrigin _commandOrigin;

		// Token: 0x0400098A RID: 2442
		private PipelineResultTypes _mergeUnclaimedPreviousCommandResults;

		// Token: 0x0400098B RID: 2443
		private PipelineResultTypes _mergeMyResult;

		// Token: 0x0400098C RID: 2444
		private PipelineResultTypes _mergeToResult;

		// Token: 0x0400098D RID: 2445
		private PipelineResultTypes[] _mergeInstructions;

		// Token: 0x0400098E RID: 2446
		private readonly CommandParameterCollection _parameters;

		// Token: 0x0400098F RID: 2447
		private readonly string _command;

		// Token: 0x04000990 RID: 2448
		private readonly CommandInfo _commandInfo;

		// Token: 0x04000991 RID: 2449
		private readonly bool _isScript;

		// Token: 0x04000992 RID: 2450
		private bool? _useLocalScope;

		// Token: 0x020001EB RID: 491
		internal enum MergeType
		{
			// Token: 0x04000995 RID: 2453
			Error,
			// Token: 0x04000996 RID: 2454
			Warning,
			// Token: 0x04000997 RID: 2455
			Verbose,
			// Token: 0x04000998 RID: 2456
			Debug,
			// Token: 0x04000999 RID: 2457
			Information
		}
	}
}
