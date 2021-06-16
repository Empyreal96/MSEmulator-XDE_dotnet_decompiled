using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Remoting;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000045 RID: 69
	internal class MshCommandRuntime : ICommandRuntime2, ICommandRuntime
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000BD0B File Offset: 0x00009F0B
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000BD13 File Offset: 0x00009F13
		internal ExecutionContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000BD1C File Offset: 0x00009F1C
		public PSHost Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000BD24 File Offset: 0x00009F24
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000BD2C File Offset: 0x00009F2C
		internal bool IsClosed
		{
			get
			{
				return this.isClosed;
			}
			set
			{
				this.isClosed = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000BD35 File Offset: 0x00009F35
		internal bool IsPipelineInputExpected
		{
			get
			{
				return !this.isClosed || (this.inputPipe != null && !this.inputPipe.Empty);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000BD57 File Offset: 0x00009F57
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000BD5F File Offset: 0x00009F5F
		internal string OutVariable { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000BD68 File Offset: 0x00009F68
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000BD70 File Offset: 0x00009F70
		internal IList OutVarList
		{
			get
			{
				return this.outVarList;
			}
			set
			{
				this.outVarList = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000BD79 File Offset: 0x00009F79
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000BD81 File Offset: 0x00009F81
		internal PipelineProcessor PipelineProcessor { get; set; }

		// Token: 0x06000350 RID: 848 RVA: 0x0000BD8C File Offset: 0x00009F8C
		internal MshCommandRuntime(ExecutionContext context, CommandInfo commandInfo, InternalCommand thisCommand)
		{
			this.context = context;
			this.host = context.EngineHostInterface;
			this.CBhost = context.EngineHostInterface;
			this.commandInfo = commandInfo;
			this.thisCommand = thisCommand;
			this.shouldLogPipelineExecutionDetail = this.InitShouldLogPipelineExecutionDetail();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000BDFB File Offset: 0x00009FFB
		public override string ToString()
		{
			if (this.commandInfo != null)
			{
				return this.commandInfo.ToString();
			}
			return "<NullCommandInfo>";
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000BE16 File Offset: 0x0000A016
		internal InvocationInfo MyInvocation
		{
			get
			{
				if (this.myInvocation == null)
				{
					this.myInvocation = this.thisCommand.MyInvocation;
				}
				return this.myInvocation;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000BE37 File Offset: 0x0000A037
		internal bool IsStopping
		{
			get
			{
				return this.PipelineProcessor != null && this.PipelineProcessor.Stopping;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000BE50 File Offset: 0x0000A050
		public void WriteObject(object sendToPipeline)
		{
			this.ThrowIfStopping();
			if (!this.UseSecurityContextRun)
			{
				this.DoWriteObject(sendToPipeline);
				return;
			}
			if (this.PipelineProcessor == null || this.PipelineProcessor.SecurityContext == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.WriteNotPermitted, new object[0]);
			}
			ContextCallback callback = new ContextCallback(this.DoWriteObject);
			SecurityContext.Run(this.PipelineProcessor.SecurityContext.CreateCopy(), callback, sendToPipeline);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000BEBD File Offset: 0x0000A0BD
		private void DoWriteObject(object sendToPipeline)
		{
			this.ThrowIfWriteNotPermitted(true);
			this._WriteObjectSkipAllowCheck(sendToPipeline);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public void WriteObject(object sendToPipeline, bool enumerateCollection)
		{
			if (!enumerateCollection)
			{
				this.WriteObject(sendToPipeline);
				return;
			}
			this.ThrowIfStopping();
			if (!this.UseSecurityContextRun)
			{
				this.DoWriteObjects(sendToPipeline);
				return;
			}
			if (this.PipelineProcessor == null || this.PipelineProcessor.SecurityContext == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.WriteNotPermitted, new object[0]);
			}
			ContextCallback callback = new ContextCallback(this.DoWriteObjects);
			SecurityContext.Run(this.PipelineProcessor.SecurityContext.CreateCopy(), callback, sendToPipeline);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000BF48 File Offset: 0x0000A148
		private void DoWriteObjects(object sendToPipeline)
		{
			this.ThrowIfWriteNotPermitted(true);
			this._WriteObjectsSkipAllowCheck(sendToPipeline);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BF58 File Offset: 0x0000A158
		public void WriteProgress(ProgressRecord progressRecord)
		{
			this.WriteProgress(progressRecord, false);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000BF62 File Offset: 0x0000A162
		internal void WriteProgress(ProgressRecord progressRecord, bool overrideInquire)
		{
			this.ThrowIfStopping();
			this.ThrowIfWriteNotPermitted(false);
			if (0L == this._sourceId)
			{
				this._sourceId = Interlocked.Increment(ref MshCommandRuntime._lastUsedSourceId);
			}
			this.WriteProgress(this._sourceId, progressRecord, overrideInquire);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BF99 File Offset: 0x0000A199
		public void WriteProgress(long sourceId, ProgressRecord progressRecord)
		{
			this.WriteProgress(sourceId, progressRecord, false);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		internal void WriteProgress(long sourceId, ProgressRecord progressRecord, bool overrideInquire)
		{
			if (progressRecord == null)
			{
				throw PSTraceSource.NewArgumentNullException("progressRecord");
			}
			if (this.Host == null || this.Host.UI == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			InternalHostUserInterface internalHostUserInterface = this.Host.UI as InternalHostUserInterface;
			ActionPreference actionPreference = this.ProgressPreference;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (this.WriteHelper_ShouldWrite(actionPreference, this.lastProgressContinueStatus))
			{
				internalHostUserInterface.WriteProgress(sourceId, progressRecord);
			}
			this.lastProgressContinueStatus = this.WriteHelper(null, null, actionPreference, this.lastProgressContinueStatus, "ProgressPreference", progressRecord.Activity);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000C032 File Offset: 0x0000A232
		public void WriteDebug(string text)
		{
			this.WriteDebug(new DebugRecord(text), false);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000C044 File Offset: 0x0000A244
		internal void WriteDebug(DebugRecord record, bool overrideInquire = false)
		{
			ActionPreference actionPreference = this.DebugPreference;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (this.WriteHelper_ShouldWrite(actionPreference, this.lastDebugContinueStatus))
			{
				if (record.InvocationInfo == null)
				{
					record.SetInvocationInfo(this.MyInvocation);
				}
				if (this.DebugOutputPipe != null)
				{
					if (this.CBhost != null && this.CBhost.InternalUI != null && this.DebugOutputPipe.NullPipe)
					{
						this.CBhost.InternalUI.WriteDebugInfoBuffers(record);
					}
					PSObject psobject = PSObject.AsPSObject(record);
					if (psobject.Members["WriteDebugStream"] == null)
					{
						psobject.Properties.Add(new PSNoteProperty("WriteDebugStream", true));
					}
					this.DebugOutputPipe.Add(psobject);
				}
				else
				{
					if (this.Host == null || this.Host.UI == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					this.CBhost.InternalUI.TranscribeResult(StringUtil.Format(InternalHostUserInterfaceStrings.DebugFormatString, record.Message));
					this.CBhost.InternalUI.WriteDebugRecord(record);
				}
			}
			this.lastDebugContinueStatus = this.WriteHelper(null, null, actionPreference, this.lastDebugContinueStatus, "DebugPreference", record.Message);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C170 File Offset: 0x0000A370
		public void WriteVerbose(string text)
		{
			this.WriteVerbose(new VerboseRecord(text), false);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C180 File Offset: 0x0000A380
		internal void WriteVerbose(VerboseRecord record, bool overrideInquire = false)
		{
			ActionPreference actionPreference = this.VerbosePreference;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (this.WriteHelper_ShouldWrite(actionPreference, this.lastVerboseContinueStatus))
			{
				if (record.InvocationInfo == null)
				{
					record.SetInvocationInfo(this.MyInvocation);
				}
				if (this.VerboseOutputPipe != null)
				{
					if (this.CBhost != null && this.CBhost.InternalUI != null && this.VerboseOutputPipe.NullPipe)
					{
						this.CBhost.InternalUI.WriteVerboseInfoBuffers(record);
					}
					PSObject psobject = PSObject.AsPSObject(record);
					if (psobject.Members["WriteVerboseStream"] == null)
					{
						psobject.Properties.Add(new PSNoteProperty("WriteVerboseStream", true));
					}
					this.VerboseOutputPipe.Add(psobject);
				}
				else
				{
					if (this.Host == null || this.Host.UI == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					this.CBhost.InternalUI.TranscribeResult(StringUtil.Format(InternalHostUserInterfaceStrings.VerboseFormatString, record.Message));
					this.CBhost.InternalUI.WriteVerboseRecord(record);
				}
			}
			this.lastVerboseContinueStatus = this.WriteHelper(null, null, actionPreference, this.lastVerboseContinueStatus, "VerbosePreference", record.Message);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		public void WriteWarning(string text)
		{
			this.WriteWarning(new WarningRecord(text), false);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		internal void WriteWarning(WarningRecord record, bool overrideInquire = false)
		{
			ActionPreference actionPreference = this.WarningPreference;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (this.WriteHelper_ShouldWrite(actionPreference, this.lastWarningContinueStatus))
			{
				if (record.InvocationInfo == null)
				{
					record.SetInvocationInfo(this.MyInvocation);
				}
				if (this.WarningOutputPipe != null)
				{
					if (this.CBhost != null && this.CBhost.InternalUI != null && this.WarningOutputPipe.NullPipe)
					{
						this.CBhost.InternalUI.WriteWarningInfoBuffers(record);
					}
					PSObject psobject = PSObject.AsPSObject(record);
					if (psobject.Members["WriteWarningStream"] == null)
					{
						psobject.Properties.Add(new PSNoteProperty("WriteWarningStream", true));
					}
					this.WarningOutputPipe.AddWithoutAppendingOutVarList(psobject);
				}
				else
				{
					if (this.Host == null || this.Host.UI == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					this.CBhost.InternalUI.TranscribeResult(StringUtil.Format(InternalHostUserInterfaceStrings.WarningFormatString, record.Message));
					this.CBhost.InternalUI.WriteWarningRecord(record);
				}
			}
			this.AppendWarningVarList(record);
			this.lastWarningContinueStatus = this.WriteHelper(null, null, actionPreference, this.lastWarningContinueStatus, "WarningPreference", record.Message);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C3EF File Offset: 0x0000A5EF
		public void WriteInformation(InformationRecord informationRecord)
		{
			this.WriteInformation(informationRecord, false);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		internal void WriteInformation(InformationRecord record, bool overrideInquire = false)
		{
			ActionPreference actionPreference = this.InformationPreference;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (actionPreference != ActionPreference.Ignore)
			{
				if (this.InformationOutputPipe != null)
				{
					if (this.CBhost != null && this.CBhost.InternalUI != null && this.InformationOutputPipe.NullPipe)
					{
						this.CBhost.InternalUI.WriteInformationInfoBuffers(record);
					}
					PSObject psobject = PSObject.AsPSObject(record);
					if (psobject.Members["WriteInformationStream"] == null)
					{
						psobject.Properties.Add(new PSNoteProperty("WriteInformationStream", true));
					}
					this.InformationOutputPipe.Add(psobject);
				}
				else
				{
					if (this.Host == null || this.Host.UI == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					this.CBhost.InternalUI.WriteInformationRecord(record);
					if ((record.Tags.Contains("PSHOST") && !record.Tags.Contains("FORWARDED")) || actionPreference == ActionPreference.Continue)
					{
						HostInformationMessage hostInformationMessage = record.MessageData as HostInformationMessage;
						if (hostInformationMessage != null)
						{
							string message = hostInformationMessage.Message;
							ConsoleColor? consoleColor = null;
							ConsoleColor? consoleColor2 = null;
							bool flag = false;
							if (hostInformationMessage.ForegroundColor != null)
							{
								consoleColor = new ConsoleColor?(hostInformationMessage.ForegroundColor.Value);
							}
							if (hostInformationMessage.BackgroundColor != null)
							{
								consoleColor2 = new ConsoleColor?(hostInformationMessage.BackgroundColor.Value);
							}
							if (hostInformationMessage.NoNewLine != null)
							{
								flag = hostInformationMessage.NoNewLine.Value;
							}
							if (consoleColor != null || consoleColor2 != null)
							{
								if (consoleColor == null)
								{
									consoleColor = new ConsoleColor?(ConsoleColor.Gray);
								}
								if (consoleColor2 == null)
								{
									consoleColor2 = new ConsoleColor?(ConsoleColor.Black);
								}
								if (flag)
								{
									this.CBhost.InternalUI.Write(consoleColor.Value, consoleColor2.Value, message);
								}
								else
								{
									this.CBhost.InternalUI.WriteLine(consoleColor.Value, consoleColor2.Value, message);
								}
							}
							else if (flag)
							{
								this.CBhost.InternalUI.Write(message);
							}
							else
							{
								this.CBhost.InternalUI.WriteLine(message);
							}
						}
						else
						{
							this.CBhost.InternalUI.WriteLine(record.ToString());
						}
					}
					else
					{
						this.CBhost.InternalUI.TranscribeResult(StringUtil.Format(InternalHostUserInterfaceStrings.InformationFormatString, record.ToString()));
					}
				}
			}
			this.AppendInformationVarList(record);
			this.lastInformationContinueStatus = this.WriteHelper(null, null, actionPreference, this.lastInformationContinueStatus, "InformationPreference", record.ToString());
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C699 File Offset: 0x0000A899
		public void WriteCommandDetail(string text)
		{
			this.PipelineProcessor.LogExecutionInfo(this.thisCommand.MyInvocation, text);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000C6B2 File Offset: 0x0000A8B2
		internal bool LogPipelineExecutionDetail
		{
			get
			{
				return this.shouldLogPipelineExecutionDetail;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		private bool InitShouldLogPipelineExecutionDetail()
		{
			CmdletInfo cmdletInfo = this.commandInfo as CmdletInfo;
			if (cmdletInfo == null)
			{
				FunctionInfo functionInfo = this.commandInfo as FunctionInfo;
				return functionInfo != null && functionInfo.Module != null && functionInfo.Module.LogPipelineExecutionDetails;
			}
			if (string.Equals("Add-Type", cmdletInfo.Name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (cmdletInfo.Module == null && cmdletInfo.PSSnapIn != null)
			{
				return cmdletInfo.PSSnapIn.LogPipelineExecutionDetails;
			}
			return cmdletInfo.PSSnapIn == null && cmdletInfo.Module != null && cmdletInfo.Module.LogPipelineExecutionDetails;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000C74B File Offset: 0x0000A94B
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000C753 File Offset: 0x0000A953
		internal string PipelineVariable { get; set; }

		// Token: 0x06000369 RID: 873 RVA: 0x0000C75C File Offset: 0x0000A95C
		internal void SetupOutVariable()
		{
			if (string.IsNullOrEmpty(this.OutVariable))
			{
				return;
			}
			this.EnsureVariableParameterAllowed();
			if (!string.IsNullOrEmpty(this.OutVariable) && !this.OutVariable.StartsWith("+", StringComparison.Ordinal) && string.Equals("Out-Default", this.thisCommand.CommandInfo.Name, StringComparison.OrdinalIgnoreCase))
			{
				if (this.state == null)
				{
					this.state = new SessionState(this.context.EngineSessionState);
				}
				IList list = PSObject.Base(this.state.PSVariable.GetValue(this.OutVariable)) as IList;
				if (list == null)
				{
					this.outVarList = new ArrayList();
				}
				else
				{
					this.outVarList = list;
				}
				if (!(this.thisCommand is PSScriptCmdlet))
				{
					this.OutputPipe.AddVariableList(VariableStreamKind.Output, this.outVarList);
				}
				this.state.PSVariable.Set(this.OutVariable, this.outVarList);
				return;
			}
			this.SetupVariable(VariableStreamKind.Output, this.OutVariable, ref this.outVarList);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C86C File Offset: 0x0000AA6C
		internal void SetupPipelineVariable()
		{
			if (string.IsNullOrEmpty(this.PipelineVariable))
			{
				return;
			}
			this.EnsureVariableParameterAllowed();
			if (this.state == null)
			{
				this.state = new SessionState(this.context.EngineSessionState);
			}
			this.pipelineVarReference = new PSVariable(this.PipelineVariable);
			this.state.PSVariable.Set(this.pipelineVarReference);
			this.pipelineVarReference = this.state.PSVariable.Get(this.PipelineVariable);
			if (!(this.thisCommand is PSScriptCmdlet))
			{
				this.OutputPipe.SetPipelineVariable(this.pipelineVarReference);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000C90C File Offset: 0x0000AB0C
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000C919 File Offset: 0x0000AB19
		internal int OutBuffer
		{
			get
			{
				return this.OutputPipe.OutBufferCount;
			}
			set
			{
				this.OutputPipe.OutBufferCount = value;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C928 File Offset: 0x0000AB28
		public bool ShouldProcess(string target)
		{
			string verboseDescription = StringUtil.Format(CommandBaseStrings.ShouldProcessMessage, this.MyInvocation.MyCommand.Name, target);
			ShouldProcessReason shouldProcessReason;
			return this.DoShouldProcess(verboseDescription, null, null, out shouldProcessReason);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C95C File Offset: 0x0000AB5C
		public bool ShouldProcess(string target, string action)
		{
			string shouldProcessMessage = CommandBaseStrings.ShouldProcessMessage;
			object[] array = new object[3];
			array[0] = action;
			array[1] = target;
			string verboseDescription = StringUtil.Format(shouldProcessMessage, array);
			ShouldProcessReason shouldProcessReason;
			return this.DoShouldProcess(verboseDescription, null, null, out shouldProcessReason);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C990 File Offset: 0x0000AB90
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			ShouldProcessReason shouldProcessReason;
			return this.DoShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			return this.DoShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		private bool CanShouldProcessAutoConfirm()
		{
			CommandMetadata commandMetadata = this.commandInfo.CommandMetadata;
			if (commandMetadata == null)
			{
				return true;
			}
			ConfirmImpact confirmImpact = commandMetadata.ConfirmImpact;
			ConfirmImpact confirmImpact2 = this.ConfirmPreference;
			return confirmImpact2 == ConfirmImpact.None || confirmImpact2 > confirmImpact;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		private bool DoShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			this.ThrowIfStopping();
			shouldProcessReason = ShouldProcessReason.None;
			switch (this.lastShouldProcessContinueStatus)
			{
			case MshCommandRuntime.ContinueStatus.YesToAll:
				return true;
			case MshCommandRuntime.ContinueStatus.NoToAll:
				return false;
			default:
				if (this.WhatIf)
				{
					this.ThrowIfWriteNotPermitted(false);
					shouldProcessReason = ShouldProcessReason.WhatIf;
					string text = StringUtil.Format(CommandBaseStrings.ShouldProcessWhatIfMessage, verboseDescription);
					this.CBhost.InternalUI.TranscribeResult(text);
					this.CBhost.UI.WriteLine(text);
					return false;
				}
				if (this.CanShouldProcessAutoConfirm())
				{
					if (this.Verbose)
					{
						this.ThrowIfWriteNotPermitted(false);
						this.WriteVerbose(verboseDescription);
					}
					return true;
				}
				if (string.IsNullOrEmpty(verboseWarning))
				{
					verboseWarning = StringUtil.Format(CommandBaseStrings.ShouldProcessWarningFallback, verboseDescription);
				}
				this.ThrowIfWriteNotPermitted(false);
				this.lastShouldProcessContinueStatus = this.InquireHelper(verboseWarning, caption, true, true, false, false);
				switch (this.lastShouldProcessContinueStatus)
				{
				case MshCommandRuntime.ContinueStatus.No:
				case MshCommandRuntime.ContinueStatus.NoToAll:
					return false;
				}
				return true;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000CADA File Offset: 0x0000ACDA
		internal MshCommandRuntime.ShouldProcessPossibleOptimization CalculatePossibleShouldProcessOptimization()
		{
			if (this.WhatIf)
			{
				return MshCommandRuntime.ShouldProcessPossibleOptimization.AutoNo_CanCallShouldProcessAsynchronously;
			}
			if (!this.CanShouldProcessAutoConfirm())
			{
				return MshCommandRuntime.ShouldProcessPossibleOptimization.NoOptimizationPossible;
			}
			if (this.Verbose)
			{
				return MshCommandRuntime.ShouldProcessPossibleOptimization.AutoYes_CanCallShouldProcessAsynchronously;
			}
			return MshCommandRuntime.ShouldProcessPossibleOptimization.AutoYes_CanSkipShouldProcessCall;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000CB00 File Offset: 0x0000AD00
		public bool ShouldContinue(string query, string caption)
		{
			bool flag = false;
			bool flag2 = false;
			bool hasSecurityImpact = false;
			return this.DoShouldContinue(query, caption, hasSecurityImpact, false, ref flag, ref flag2);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000CB21 File Offset: 0x0000AD21
		public bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
		{
			return this.DoShouldContinue(query, caption, hasSecurityImpact, true, ref yesToAll, ref noToAll);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000CB31 File Offset: 0x0000AD31
		public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			return this.DoShouldContinue(query, caption, false, true, ref yesToAll, ref noToAll);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000CB40 File Offset: 0x0000AD40
		private bool DoShouldContinue(string query, string caption, bool hasSecurityImpact, bool supportsToAllOptions, ref bool yesToAll, ref bool noToAll)
		{
			this.ThrowIfStopping();
			this.ThrowIfWriteNotPermitted(false);
			if (noToAll)
			{
				return false;
			}
			if (yesToAll)
			{
				return true;
			}
			switch (this.InquireHelper(query, caption, supportsToAllOptions, supportsToAllOptions, false, hasSecurityImpact))
			{
			case MshCommandRuntime.ContinueStatus.No:
				return false;
			case MshCommandRuntime.ContinueStatus.YesToAll:
				yesToAll = true;
				break;
			case MshCommandRuntime.ContinueStatus.NoToAll:
				noToAll = true;
				return false;
			}
			return true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		public bool TransactionAvailable()
		{
			return this.UseTransactionFlagSet && this.Context.TransactionManager.HasTransaction;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
		public PSTransactionContext CurrentPSTransaction
		{
			get
			{
				if (!this.TransactionAvailable())
				{
					string message;
					if (!this.UseTransactionFlagSet)
					{
						message = TransactionStrings.CmdletRequiresUseTx;
					}
					else
					{
						message = TransactionStrings.NoTransactionAvailable;
					}
					throw new InvalidOperationException(message);
				}
				return new PSTransactionContext(this.Context.TransactionManager);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			this.ThrowIfStopping();
			if (errorRecord == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorRecord");
			}
			errorRecord.SetInvocationInfo(this.MyInvocation);
			if (errorRecord.ErrorDetails != null && errorRecord.ErrorDetails.TextLookupError != null)
			{
				Exception textLookupError = errorRecord.ErrorDetails.TextLookupError;
				errorRecord.ErrorDetails.TextLookupError = null;
				MshLog.LogCommandHealthEvent(this.context, textLookupError, Severity.Warning);
			}
			if (errorRecord.Exception != null && string.IsNullOrEmpty(errorRecord.Exception.StackTrace))
			{
				try
				{
					throw errorRecord.Exception;
				}
				catch (Exception)
				{
				}
			}
			CmdletInvocationException e = new CmdletInvocationException(errorRecord);
			throw this.ManageException(e);
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		internal MshCommandRuntime.MergeDataStream ErrorMergeTo
		{
			get
			{
				return this.errorMergeTo;
			}
			set
			{
				this.errorMergeTo = value;
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000CCBC File Offset: 0x0000AEBC
		internal void SetMergeFromRuntime(MshCommandRuntime fromRuntime)
		{
			this.ErrorMergeTo = fromRuntime.ErrorMergeTo;
			if (fromRuntime.WarningOutputPipe != null)
			{
				this.WarningOutputPipe = fromRuntime.WarningOutputPipe;
			}
			if (fromRuntime.VerboseOutputPipe != null)
			{
				this.VerboseOutputPipe = fromRuntime.VerboseOutputPipe;
			}
			if (fromRuntime.DebugOutputPipe != null)
			{
				this.DebugOutputPipe = fromRuntime.DebugOutputPipe;
			}
			if (fromRuntime.InformationOutputPipe != null)
			{
				this.InformationOutputPipe = fromRuntime.InformationOutputPipe;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000CD25 File Offset: 0x0000AF25
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000CD2D File Offset: 0x0000AF2D
		internal bool MergeUnclaimedPreviousErrorResults
		{
			get
			{
				return this.mergeUnclaimedPreviousErrorResults;
			}
			set
			{
				this.mergeUnclaimedPreviousErrorResults = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000CD36 File Offset: 0x0000AF36
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000CD51 File Offset: 0x0000AF51
		internal Pipe InputPipe
		{
			get
			{
				if (this.inputPipe == null)
				{
					this.inputPipe = new Pipe();
				}
				return this.inputPipe;
			}
			set
			{
				this.inputPipe = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000CD5A File Offset: 0x0000AF5A
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000CD75 File Offset: 0x0000AF75
		internal Pipe OutputPipe
		{
			get
			{
				if (this.outputPipe == null)
				{
					this.outputPipe = new Pipe();
				}
				return this.outputPipe;
			}
			set
			{
				this.outputPipe = value;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000CD7E File Offset: 0x0000AF7E
		internal object[] GetResultsAsArray()
		{
			if (this.outputPipe == null)
			{
				return MshCommandRuntime.StaticEmptyArray;
			}
			return this.outputPipe.ToArray();
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000CD99 File Offset: 0x0000AF99
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
		internal Pipe ErrorOutputPipe
		{
			get
			{
				if (this.errorOutputPipe == null)
				{
					this.errorOutputPipe = new Pipe();
				}
				return this.errorOutputPipe;
			}
			set
			{
				this.errorOutputPipe = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000CDBD File Offset: 0x0000AFBD
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0000CDC5 File Offset: 0x0000AFC5
		internal Pipe WarningOutputPipe
		{
			get
			{
				return this.warningOutputPipe;
			}
			set
			{
				this.warningOutputPipe = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000CDCE File Offset: 0x0000AFCE
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000CDD6 File Offset: 0x0000AFD6
		internal Pipe VerboseOutputPipe
		{
			get
			{
				return this.verboseOutputPipe;
			}
			set
			{
				this.verboseOutputPipe = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000CDDF File Offset: 0x0000AFDF
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000CDE7 File Offset: 0x0000AFE7
		internal Pipe DebugOutputPipe
		{
			get
			{
				return this.debugOutputPipe;
			}
			set
			{
				this.debugOutputPipe = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		internal Pipe InformationOutputPipe
		{
			get
			{
				return this.informationOutputPipe;
			}
			set
			{
				this.informationOutputPipe = value;
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000CE01 File Offset: 0x0000B001
		internal void ThrowIfStopping()
		{
			if (this.IsStopping)
			{
				throw new PipelineStoppedException();
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000CE14 File Offset: 0x0000B014
		internal void ThrowIfWriteNotPermitted(bool needsToWriteToPipeline)
		{
			if ((this.PipelineProcessor == null || this.thisCommand != this.PipelineProcessor._permittedToWrite || (needsToWriteToPipeline && !this.PipelineProcessor._permittedToWriteToPipeline) || Thread.CurrentThread != this.PipelineProcessor._permittedToWriteThread) && this.PipelineProcessor._permittedToWrite != null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.WriteNotPermitted, new object[0]);
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000CE7C File Offset: 0x0000B07C
		internal IDisposable AllowThisCommandToWrite(bool permittedToWriteToPipeline)
		{
			return new MshCommandRuntime.AllowWrite(this.thisCommand, permittedToWriteToPipeline);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000CE8C File Offset: 0x0000B08C
		public Exception ManageException(Exception e)
		{
			if (e == null)
			{
				throw PSTraceSource.NewArgumentNullException("e");
			}
			if (this.PipelineProcessor != null)
			{
				this.PipelineProcessor.RecordFailure(e, this.thisCommand);
			}
			if (!(e is HaltCommandException) && !(e is PipelineStoppedException) && !(e is ExitNestedPromptException))
			{
				try
				{
					this.AppendErrorToVariables(e);
				}
				catch
				{
				}
				MshLog.LogCommandHealthEvent(this.context, e, Severity.Warning);
			}
			return new PipelineStoppedException();
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000CF08 File Offset: 0x0000B108
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0000CF10 File Offset: 0x0000B110
		internal string ErrorVariable { get; set; }

		// Token: 0x06000395 RID: 917 RVA: 0x0000CF19 File Offset: 0x0000B119
		internal void SetupErrorVariable()
		{
			this.SetupVariable(VariableStreamKind.Error, this.ErrorVariable, ref this.errorVarList);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000CF2E File Offset: 0x0000B12E
		private void EnsureVariableParameterAllowed()
		{
			if (this.context.LanguageMode == PSLanguageMode.NoLanguage || this.context.LanguageMode == PSLanguageMode.RestrictedLanguage)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "VariableReferenceNotSupportedInDataSection", ParserStrings.VariableReferenceNotSupportedInDataSection, new object[0]);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000CF6E File Offset: 0x0000B16E
		internal void AppendErrorToVariables(object obj)
		{
			if (obj == null)
			{
				return;
			}
			this.AppendDollarError(obj);
			this.OutputPipe.AppendVariableList(VariableStreamKind.Error, obj);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000CF88 File Offset: 0x0000B188
		private void AppendDollarError(object obj)
		{
			if (obj is Exception && (this.PipelineProcessor == null || !this.PipelineProcessor.TopLevel))
			{
				return;
			}
			this.context.AppendDollarError(obj);
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		internal string WarningVariable { get; set; }

		// Token: 0x0600039B RID: 923 RVA: 0x0000CFC5 File Offset: 0x0000B1C5
		internal void SetupWarningVariable()
		{
			this.SetupVariable(VariableStreamKind.Warning, this.WarningVariable, ref this.warningVarList);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000CFDA File Offset: 0x0000B1DA
		internal void AppendWarningVarList(object obj)
		{
			this.OutputPipe.AppendVariableList(VariableStreamKind.Warning, obj);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0000CFE9 File Offset: 0x0000B1E9
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0000CFF1 File Offset: 0x0000B1F1
		internal string InformationVariable
		{
			get
			{
				return this.informationVariable;
			}
			set
			{
				this.informationVariable = value;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CFFA File Offset: 0x0000B1FA
		internal void SetupInformationVariable()
		{
			this.SetupVariable(VariableStreamKind.Information, this.InformationVariable, ref this.informationVarList);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000D010 File Offset: 0x0000B210
		internal void SetupVariable(VariableStreamKind streamKind, string variableName, ref IList varList)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				return;
			}
			this.EnsureVariableParameterAllowed();
			if (this.state == null)
			{
				this.state = new SessionState(this.context.EngineSessionState);
			}
			if (variableName.StartsWith("+", StringComparison.Ordinal))
			{
				variableName = variableName.Substring(1);
				object obj = PSObject.Base(this.state.PSVariable.GetValue(variableName));
				varList = (obj as IList);
				if (varList == null)
				{
					varList = new ArrayList();
					if (obj != null && AutomationNull.Value != obj)
					{
						IEnumerable enumerable = LanguagePrimitives.GetEnumerable(obj);
						if (enumerable != null)
						{
							using (IEnumerator enumerator = enumerable.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object value = enumerator.Current;
									varList.Add(value);
								}
								goto IL_EF;
							}
						}
						varList.Add(obj);
					}
				}
				else if (varList.IsFixedSize)
				{
					ArrayList arrayList = new ArrayList();
					arrayList.AddRange(varList);
					varList = arrayList;
				}
			}
			else
			{
				varList = new ArrayList();
			}
			IL_EF:
			if (!(this.thisCommand is PSScriptCmdlet))
			{
				this.OutputPipe.AddVariableList(streamKind, varList);
			}
			this.state.PSVariable.Set(variableName, varList);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D14C File Offset: 0x0000B34C
		internal void AppendInformationVarList(object obj)
		{
			this.OutputPipe.AppendVariableList(VariableStreamKind.Information, obj);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D15B File Offset: 0x0000B35B
		internal void _WriteObjectSkipAllowCheck(object sendToPipeline)
		{
			this.ThrowIfStopping();
			if (AutomationNull.Value == sendToPipeline)
			{
				return;
			}
			sendToPipeline = LanguagePrimitives.AsPSObjectOrNull(sendToPipeline);
			this.OutputPipe.Add(sendToPipeline);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D180 File Offset: 0x0000B380
		internal void _WriteObjectsSkipAllowCheck(object sendToPipeline)
		{
			IEnumerable enumerable = LanguagePrimitives.GetEnumerable(sendToPipeline);
			if (enumerable == null)
			{
				this._WriteObjectSkipAllowCheck(sendToPipeline);
				return;
			}
			this.ThrowIfStopping();
			ArrayList arrayList = new ArrayList();
			foreach (object obj in enumerable)
			{
				if (AutomationNull.Value != obj)
				{
					object value = LanguagePrimitives.AsPSObjectOrNull(obj);
					arrayList.Add(value);
				}
			}
			this.OutputPipe.AddItems(arrayList);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D210 File Offset: 0x0000B410
		public void WriteError(ErrorRecord errorRecord)
		{
			this.WriteError(errorRecord, false);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D21C File Offset: 0x0000B41C
		internal void WriteError(ErrorRecord errorRecord, bool overrideInquire)
		{
			this.ThrowIfStopping();
			ActionPreference actionPreference = this.ErrorAction;
			if (overrideInquire && actionPreference == ActionPreference.Inquire)
			{
				actionPreference = ActionPreference.Continue;
			}
			if (!this.UseSecurityContextRun)
			{
				this.DoWriteError(new KeyValuePair<ErrorRecord, ActionPreference>(errorRecord, actionPreference));
				return;
			}
			if (this.PipelineProcessor == null || this.PipelineProcessor.SecurityContext == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.WriteNotPermitted, new object[0]);
			}
			ContextCallback callback = new ContextCallback(this.DoWriteError);
			SecurityContext.Run(this.PipelineProcessor.SecurityContext.CreateCopy(), callback, new KeyValuePair<ErrorRecord, ActionPreference>(errorRecord, actionPreference));
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		private void DoWriteError(object obj)
		{
			KeyValuePair<ErrorRecord, ActionPreference> keyValuePair = (KeyValuePair<ErrorRecord, ActionPreference>)obj;
			ErrorRecord key = keyValuePair.Key;
			ActionPreference value = keyValuePair.Value;
			if (key == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorRecord");
			}
			if (this.UseTransaction && this.context.TransactionManager.RollbackPreference != RollbackSeverity.TerminatingError && this.context.TransactionManager.RollbackPreference != RollbackSeverity.Never)
			{
				this.context.TransactionManager.Rollback(true);
			}
			if (key.PreserveInvocationInfoOnce)
			{
				key.PreserveInvocationInfoOnce = false;
			}
			else
			{
				key.SetInvocationInfo(this.MyInvocation);
			}
			this.ThrowIfWriteNotPermitted(true);
			this._WriteErrorSkipAllowCheck(key, new ActionPreference?(value));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D358 File Offset: 0x0000B558
		internal void _WriteErrorSkipAllowCheck(ErrorRecord errorRecord, ActionPreference? actionPreference = null)
		{
			this.ThrowIfStopping();
			if (errorRecord.ErrorDetails != null && errorRecord.ErrorDetails.TextLookupError != null)
			{
				Exception textLookupError = errorRecord.ErrorDetails.TextLookupError;
				errorRecord.ErrorDetails.TextLookupError = null;
				MshLog.LogCommandHealthEvent(this.context, textLookupError, Severity.Warning);
			}
			this.PipelineProcessor.ExecutionFailed = true;
			if (this.shouldLogPipelineExecutionDetail)
			{
				this.PipelineProcessor.LogExecutionError(this.thisCommand.MyInvocation, errorRecord);
			}
			ActionPreference actionPreference2 = this.ErrorAction;
			if (actionPreference != null)
			{
				actionPreference2 = actionPreference.Value;
			}
			if (ActionPreference.Ignore == actionPreference2)
			{
				return;
			}
			if (actionPreference2 == ActionPreference.SilentlyContinue)
			{
				this.AppendErrorToVariables(errorRecord);
				return;
			}
			if (MshCommandRuntime.ContinueStatus.YesToAll == this.lastErrorContinueStatus)
			{
				actionPreference2 = ActionPreference.Continue;
			}
			switch (actionPreference2)
			{
			case ActionPreference.Stop:
			{
				ActionPreferenceStopException e = new ActionPreferenceStopException(this.MyInvocation, errorRecord, StringUtil.Format(CommandBaseStrings.ErrorPreferenceStop, "ErrorActionPreference", errorRecord.ToString()));
				throw this.ManageException(e);
			}
			case ActionPreference.Inquire:
				this.lastErrorContinueStatus = this.InquireHelper(RuntimeException.RetrieveMessage(errorRecord), null, true, false, true, false);
				break;
			}
			this.AppendErrorToVariables(errorRecord);
			PSObject psobject = PSObject.AsPSObject(errorRecord);
			if (psobject.Members["writeErrorStream"] == null)
			{
				PSNoteProperty member = new PSNoteProperty("writeErrorStream", true);
				psobject.Properties.Add(member);
			}
			if (this.ErrorMergeTo != MshCommandRuntime.MergeDataStream.None)
			{
				this.OutputPipe.AddWithoutAppendingOutVarList(psobject);
				return;
			}
			if (this.context.InternalHost.UI.IsTranscribing)
			{
				this.context.InternalHost.UI.TranscribeError(this.context, errorRecord.InvocationInfo, psobject);
			}
			this.ErrorOutputPipe.AddWithoutAppendingOutVarList(psobject);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		internal ConfirmImpact ConfirmPreference
		{
			get
			{
				if (this.Confirm)
				{
					return ConfirmImpact.Low;
				}
				if (this.Debug)
				{
					if (this.isConfirmFlagSet)
					{
						return ConfirmImpact.None;
					}
					return ConfirmImpact.Low;
				}
				else
				{
					if (this.isConfirmFlagSet)
					{
						return ConfirmImpact.None;
					}
					if (!this.isConfirmPreferenceCached)
					{
						bool flag = false;
						this.confirmPreference = this.Context.GetEnumPreference<ConfirmImpact>(SpecialVariables.ConfirmPreferenceVarPath, this.confirmPreference, out flag);
						this.isConfirmPreferenceCached = true;
					}
					return this.confirmPreference;
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000D568 File Offset: 0x0000B768
		// (set) Token: 0x060003AA RID: 938 RVA: 0x0000D603 File Offset: 0x0000B803
		internal ActionPreference DebugPreference
		{
			get
			{
				if (this.isDebugPreferenceSet)
				{
					return this.debugPreference;
				}
				if (!this.isDebugFlagSet)
				{
					if (!this.isDebugPreferenceCached)
					{
						bool flag = false;
						this.debugPreference = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.DebugPreferenceVarPath, this.debugPreference, out flag);
						if (this.CBhost.ExternalHost.UI == null && this.debugPreference == ActionPreference.Inquire)
						{
							this.debugPreference = ActionPreference.Continue;
						}
						this.isDebugPreferenceCached = true;
					}
					return this.debugPreference;
				}
				if (!this.Debug)
				{
					return ActionPreference.SilentlyContinue;
				}
				if (this.CBhost.ExternalHost.UI == null)
				{
					return ActionPreference.Continue;
				}
				return ActionPreference.Inquire;
			}
			set
			{
				this.debugPreference = value;
				this.isDebugPreferenceSet = true;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000D614 File Offset: 0x0000B814
		internal ActionPreference VerbosePreference
		{
			get
			{
				if (this.isVerboseFlagSet)
				{
					if (this.Verbose)
					{
						return ActionPreference.Continue;
					}
					return ActionPreference.SilentlyContinue;
				}
				else
				{
					if (!this.Debug)
					{
						if (!this.isVerbosePreferenceCached)
						{
							bool flag = false;
							this.verbosePreference = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.VerbosePreferenceVarPath, this.verbosePreference, out flag);
						}
						return this.verbosePreference;
					}
					if (this.CBhost.ExternalHost.UI == null)
					{
						return ActionPreference.Continue;
					}
					return ActionPreference.Inquire;
				}
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000D681 File Offset: 0x0000B881
		internal bool IsWarningActionSet
		{
			get
			{
				return this.isWarningPreferenceSet;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000D68C File Offset: 0x0000B88C
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000D6EA File Offset: 0x0000B8EA
		internal ActionPreference WarningPreference
		{
			get
			{
				if (this.isWarningPreferenceSet)
				{
					return this.warningPreference;
				}
				if (this.Debug)
				{
					return ActionPreference.Inquire;
				}
				if (this.Verbose)
				{
					return ActionPreference.Continue;
				}
				if (!this.isWarningPreferenceCached)
				{
					bool flag = false;
					this.warningPreference = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.WarningPreferenceVarPath, this.warningPreference, out flag);
				}
				return this.warningPreference;
			}
			set
			{
				if (value == ActionPreference.Suspend)
				{
					throw PSTraceSource.NewNotSupportedException(ErrorPackage.SuspendActionPreferenceErrorActionOnly, new object[0]);
				}
				this.warningPreference = value;
				this.isWarningPreferenceSet = true;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000D70F File Offset: 0x0000B90F
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000D717 File Offset: 0x0000B917
		internal bool Verbose
		{
			get
			{
				return this.verboseFlag;
			}
			set
			{
				this.verboseFlag = value;
				this.isVerboseFlagSet = true;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000D727 File Offset: 0x0000B927
		internal bool IsVerboseFlagSet
		{
			get
			{
				return this.isVerboseFlagSet;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000D72F File Offset: 0x0000B92F
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000D73C File Offset: 0x0000B93C
		internal SwitchParameter Confirm
		{
			get
			{
				return this.confirmFlag;
			}
			set
			{
				this.confirmFlag = value;
				this.isConfirmFlagSet = true;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000D751 File Offset: 0x0000B951
		internal bool IsConfirmFlagSet
		{
			get
			{
				return this.isConfirmFlagSet;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000D759 File Offset: 0x0000B959
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000D766 File Offset: 0x0000B966
		internal SwitchParameter UseTransaction
		{
			get
			{
				return this.useTransactionFlag;
			}
			set
			{
				this.useTransactionFlag = value;
				this.useTransactionFlagSet = true;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000D77B File Offset: 0x0000B97B
		internal bool UseTransactionFlagSet
		{
			get
			{
				return this.useTransactionFlagSet;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000D783 File Offset: 0x0000B983
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000D78B File Offset: 0x0000B98B
		internal bool Debug
		{
			get
			{
				return this.debugFlag;
			}
			set
			{
				this.debugFlag = value;
				this.isDebugFlagSet = true;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000D79B File Offset: 0x0000B99B
		internal bool IsDebugFlagSet
		{
			get
			{
				return this.isDebugFlagSet;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000D7F3 File Offset: 0x0000B9F3
		internal SwitchParameter WhatIf
		{
			get
			{
				if (!this.isWhatIfFlagSet && !this.isWhatIfPreferenceCached)
				{
					bool flag = false;
					this.whatIfFlag = this.context.GetBooleanPreference(SpecialVariables.WhatIfPreferenceVarPath, this.whatIfFlag, out flag);
					this.isWhatIfPreferenceCached = true;
				}
				return this.whatIfFlag;
			}
			set
			{
				this.whatIfFlag = value;
				this.isWhatIfFlagSet = true;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000D808 File Offset: 0x0000BA08
		internal bool IsWhatIfFlagSet
		{
			get
			{
				return this.isWhatIfFlagSet;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000D810 File Offset: 0x0000BA10
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000D875 File Offset: 0x0000BA75
		internal ActionPreference ErrorAction
		{
			get
			{
				if (this.isErrorActionSet)
				{
					return this.errorAction;
				}
				if (this.Debug)
				{
					return ActionPreference.Inquire;
				}
				if (this.Verbose)
				{
					return ActionPreference.Continue;
				}
				if (!this.isErrorActionPreferenceCached)
				{
					bool flag = false;
					this.errorAction = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.ErrorActionPreferenceVarPath, this.errorAction, out flag);
					this.isErrorActionPreferenceCached = true;
				}
				return this.errorAction;
			}
			set
			{
				if (!(this.commandInfo is WorkflowInfo) && value == ActionPreference.Suspend)
				{
					throw PSTraceSource.NewNotSupportedException(ErrorPackage.SuspendActionPreferenceSupportedOnlyOnWorkflow, new object[0]);
				}
				this.errorAction = value;
				this.isErrorActionSet = true;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000D8A7 File Offset: 0x0000BAA7
		internal bool IsErrorActionSet
		{
			get
			{
				return this.isErrorActionSet;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0000D901 File Offset: 0x0000BB01
		internal ActionPreference ProgressPreference
		{
			get
			{
				if (this.isProgressPreferenceSet)
				{
					return this.progressPreference;
				}
				if (!this.isProgressPreferenceCached)
				{
					bool flag = false;
					this.progressPreference = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.ProgressPreferenceVarPath, this.progressPreference, out flag);
					this.isProgressPreferenceCached = true;
				}
				return this.progressPreference;
			}
			set
			{
				this.progressPreference = value;
				this.isProgressPreferenceSet = true;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000D914 File Offset: 0x0000BB14
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0000D965 File Offset: 0x0000BB65
		internal ActionPreference InformationPreference
		{
			get
			{
				if (this.isInformationPreferenceSet)
				{
					return this.informationPreference;
				}
				if (!this.isInformationPreferenceCached)
				{
					bool flag = false;
					this.informationPreference = this.context.GetEnumPreference<ActionPreference>(SpecialVariables.InformationPreferenceVarPath, this.informationPreference, out flag);
					this.isInformationPreferenceCached = true;
				}
				return this.informationPreference;
			}
			set
			{
				if (value == ActionPreference.Suspend)
				{
					throw PSTraceSource.NewNotSupportedException(ErrorPackage.SuspendActionPreferenceErrorActionOnly, new object[0]);
				}
				this.informationPreference = value;
				this.isInformationPreferenceSet = true;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000D98A File Offset: 0x0000BB8A
		internal bool IsInformationActionSet
		{
			get
			{
				return this.isInformationPreferenceSet;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000D992 File Offset: 0x0000BB92
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000D99A File Offset: 0x0000BB9A
		internal PagingParameters PagingParameters
		{
			get
			{
				return this.pagingParameters;
			}
			set
			{
				this.pagingParameters = value;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		internal bool WriteHelper_ShouldWrite(ActionPreference preference, MshCommandRuntime.ContinueStatus lastContinueStatus)
		{
			this.ThrowIfStopping();
			this.ThrowIfWriteNotPermitted(false);
			switch (lastContinueStatus)
			{
			case MshCommandRuntime.ContinueStatus.YesToAll:
				return true;
			case MshCommandRuntime.ContinueStatus.NoToAll:
				return false;
			default:
				switch (preference)
				{
				case ActionPreference.SilentlyContinue:
				case ActionPreference.Ignore:
					return false;
				case ActionPreference.Stop:
				case ActionPreference.Continue:
				case ActionPreference.Inquire:
					return true;
				default:
					return true;
				}
				break;
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000D9FC File Offset: 0x0000BBFC
		internal MshCommandRuntime.ContinueStatus WriteHelper(string inquireCaption, string inquireMessage, ActionPreference preference, MshCommandRuntime.ContinueStatus lastContinueStatus, string preferenceVariableName, string message)
		{
			switch (lastContinueStatus)
			{
			case MshCommandRuntime.ContinueStatus.YesToAll:
				return MshCommandRuntime.ContinueStatus.YesToAll;
			case MshCommandRuntime.ContinueStatus.NoToAll:
				return MshCommandRuntime.ContinueStatus.NoToAll;
			default:
				switch (preference)
				{
				case ActionPreference.SilentlyContinue:
				case ActionPreference.Continue:
				case ActionPreference.Ignore:
					return MshCommandRuntime.ContinueStatus.Yes;
				case ActionPreference.Stop:
				{
					ActionPreferenceStopException e = new ActionPreferenceStopException(this.MyInvocation, StringUtil.Format(CommandBaseStrings.ErrorPreferenceStop, preferenceVariableName, message));
					throw this.ManageException(e);
				}
				case ActionPreference.Inquire:
					return this.InquireHelper(inquireMessage, inquireCaption, true, false, true, false);
				default:
				{
					ActionPreferenceStopException e2 = new ActionPreferenceStopException(this.MyInvocation, StringUtil.Format(CommandBaseStrings.PreferenceInvalid, preferenceVariableName, preference));
					throw this.ManageException(e2);
				}
				}
				break;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000DA98 File Offset: 0x0000BC98
		internal MshCommandRuntime.ContinueStatus InquireHelper(string inquireMessage, string inquireCaption, bool allowYesToAll, bool allowNoToAll, bool replaceNoWithHalt, bool hasSecurityImpact)
		{
			Collection<ChoiceDescription> collection = new Collection<ChoiceDescription>();
			int num = 0;
			int num2 = int.MaxValue;
			int num3 = int.MaxValue;
			int num4 = int.MaxValue;
			int num5 = int.MaxValue;
			int num6 = int.MaxValue;
			int num7 = int.MaxValue;
			string continueOneLabel = CommandBaseStrings.ContinueOneLabel;
			string continueOneHelpMessage = CommandBaseStrings.ContinueOneHelpMessage;
			collection.Add(new ChoiceDescription(continueOneLabel, continueOneHelpMessage));
			num2 = num++;
			if (allowYesToAll)
			{
				string continueAllLabel = CommandBaseStrings.ContinueAllLabel;
				string continueAllHelpMessage = CommandBaseStrings.ContinueAllHelpMessage;
				collection.Add(new ChoiceDescription(continueAllLabel, continueAllHelpMessage));
				num3 = num++;
			}
			if (replaceNoWithHalt)
			{
				string haltLabel = CommandBaseStrings.HaltLabel;
				string haltHelpMessage = CommandBaseStrings.HaltHelpMessage;
				collection.Add(new ChoiceDescription(haltLabel, haltHelpMessage));
				num4 = num++;
			}
			else
			{
				string skipOneLabel = CommandBaseStrings.SkipOneLabel;
				string skipOneHelpMessage = CommandBaseStrings.SkipOneHelpMessage;
				collection.Add(new ChoiceDescription(skipOneLabel, skipOneHelpMessage));
				num5 = num++;
			}
			if (allowNoToAll)
			{
				string skipAllLabel = CommandBaseStrings.SkipAllLabel;
				string skipAllHelpMessage = CommandBaseStrings.SkipAllHelpMessage;
				collection.Add(new ChoiceDescription(skipAllLabel, skipAllHelpMessage));
				num6 = num++;
			}
			if (this.IsSuspendPromptAllowed())
			{
				string pauseLabel = CommandBaseStrings.PauseLabel;
				string helpMessage = StringUtil.Format(CommandBaseStrings.PauseHelpMessage, "exit");
				collection.Add(new ChoiceDescription(pauseLabel, helpMessage));
				num7 = num++;
			}
			if (string.IsNullOrEmpty(inquireMessage))
			{
				inquireMessage = CommandBaseStrings.ShouldContinuePromptCaption;
			}
			if (string.IsNullOrEmpty(inquireCaption))
			{
				inquireCaption = CommandBaseStrings.InquireCaptionDefault;
			}
			int num8;
			for (;;)
			{
				this.CBhost.InternalUI.TranscribeResult(inquireCaption);
				this.CBhost.InternalUI.TranscribeResult(inquireMessage);
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ChoiceDescription choiceDescription in collection)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("  ");
					}
					stringBuilder.Append(choiceDescription.Label);
				}
				this.CBhost.InternalUI.TranscribeResult(stringBuilder.ToString());
				int defaultChoice = 0;
				if (hasSecurityImpact)
				{
					defaultChoice = num5;
				}
				num8 = this.CBhost.UI.PromptForChoice(inquireCaption, inquireMessage, collection, defaultChoice);
				string text = collection[num8].Label;
				int num9 = text.IndexOf('&');
				if (num9 > -1)
				{
					text = text[num9 + 1].ToString();
				}
				this.CBhost.InternalUI.TranscribeResult(text);
				if (num2 == num8)
				{
					break;
				}
				if (num3 == num8)
				{
					return MshCommandRuntime.ContinueStatus.YesToAll;
				}
				if (num4 == num8)
				{
					goto Block_12;
				}
				if (num5 == num8)
				{
					return MshCommandRuntime.ContinueStatus.No;
				}
				if (num6 == num8)
				{
					return MshCommandRuntime.ContinueStatus.NoToAll;
				}
				if (num7 != num8)
				{
					goto IL_297;
				}
				this.CBhost.EnterNestedPrompt(this.thisCommand);
			}
			return MshCommandRuntime.ContinueStatus.Yes;
			Block_12:
			ActionPreferenceStopException e = new ActionPreferenceStopException(this.MyInvocation, CommandBaseStrings.InquireHalt);
			throw this.ManageException(e);
			IL_297:
			if (-1 == num8)
			{
				ActionPreferenceStopException e2 = new ActionPreferenceStopException(this.MyInvocation, CommandBaseStrings.InquireCtrlC);
				throw this.ManageException(e2);
			}
			InvalidOperationException e3 = PSTraceSource.NewInvalidOperationException();
			throw this.ManageException(e3);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000DD7C File Offset: 0x0000BF7C
		private bool IsSuspendPromptAllowed()
		{
			return !(this.CBhost.ExternalHost is ServerRemoteHost);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000DD94 File Offset: 0x0000BF94
		internal void SetVariableListsInPipe()
		{
			if (this.outVarList != null)
			{
				this.OutputPipe.AddVariableList(VariableStreamKind.Output, this.outVarList);
			}
			if (this.errorVarList != null)
			{
				this.OutputPipe.AddVariableList(VariableStreamKind.Error, this.errorVarList);
			}
			if (this.warningVarList != null)
			{
				this.OutputPipe.AddVariableList(VariableStreamKind.Warning, this.warningVarList);
			}
			if (this.informationVarList != null)
			{
				this.OutputPipe.AddVariableList(VariableStreamKind.Information, this.informationVarList);
			}
			if (this.PipelineVariable != null)
			{
				this.OutputPipe.SetPipelineVariable(this.pipelineVarReference);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000DE24 File Offset: 0x0000C024
		internal void RemoveVariableListsInPipe()
		{
			if (this.outVarList != null)
			{
				this.OutputPipe.RemoveVariableList(VariableStreamKind.Output, this.outVarList);
			}
			if (this.errorVarList != null)
			{
				this.OutputPipe.RemoveVariableList(VariableStreamKind.Error, this.errorVarList);
			}
			if (this.warningVarList != null)
			{
				this.OutputPipe.RemoveVariableList(VariableStreamKind.Warning, this.warningVarList);
			}
			if (this.informationVarList != null)
			{
				this.OutputPipe.RemoveVariableList(VariableStreamKind.Information, this.informationVarList);
			}
			if (this.PipelineVariable != null)
			{
				this.OutputPipe.RemovePipelineVariable();
				this.state.PSVariable.Remove(this.PipelineVariable);
			}
		}

		// Token: 0x0400010C RID: 268
		private ExecutionContext context;

		// Token: 0x0400010D RID: 269
		private SessionState state;

		// Token: 0x0400010E RID: 270
		internal InternalHost CBhost;

		// Token: 0x0400010F RID: 271
		private PSHost host;

		// Token: 0x04000110 RID: 272
		private Pipe inputPipe;

		// Token: 0x04000111 RID: 273
		private Pipe outputPipe;

		// Token: 0x04000112 RID: 274
		private Pipe errorOutputPipe;

		// Token: 0x04000113 RID: 275
		private Pipe warningOutputPipe;

		// Token: 0x04000114 RID: 276
		private Pipe verboseOutputPipe;

		// Token: 0x04000115 RID: 277
		private Pipe debugOutputPipe;

		// Token: 0x04000116 RID: 278
		private Pipe informationOutputPipe;

		// Token: 0x04000117 RID: 279
		private bool isClosed;

		// Token: 0x04000118 RID: 280
		private IList outVarList;

		// Token: 0x04000119 RID: 281
		private CommandInfo commandInfo;

		// Token: 0x0400011A RID: 282
		private InternalCommand thisCommand;

		// Token: 0x0400011B RID: 283
		private InvocationInfo myInvocation;

		// Token: 0x0400011C RID: 284
		private static long _lastUsedSourceId;

		// Token: 0x0400011D RID: 285
		private long _sourceId;

		// Token: 0x0400011E RID: 286
		private bool shouldLogPipelineExecutionDetail;

		// Token: 0x0400011F RID: 287
		private PSVariable pipelineVarReference;

		// Token: 0x04000120 RID: 288
		private MshCommandRuntime.MergeDataStream errorMergeTo;

		// Token: 0x04000121 RID: 289
		private bool mergeUnclaimedPreviousErrorResults;

		// Token: 0x04000122 RID: 290
		internal static object[] StaticEmptyArray = new object[0];

		// Token: 0x04000123 RID: 291
		private IList errorVarList;

		// Token: 0x04000124 RID: 292
		private IList warningVarList;

		// Token: 0x04000125 RID: 293
		private IList informationVarList;

		// Token: 0x04000126 RID: 294
		private string informationVariable;

		// Token: 0x04000127 RID: 295
		internal bool UseSecurityContextRun = true;

		// Token: 0x04000128 RID: 296
		private bool isConfirmPreferenceCached;

		// Token: 0x04000129 RID: 297
		private ConfirmImpact confirmPreference = ConfirmImpact.High;

		// Token: 0x0400012A RID: 298
		private bool isDebugPreferenceSet;

		// Token: 0x0400012B RID: 299
		private ActionPreference debugPreference;

		// Token: 0x0400012C RID: 300
		private bool isDebugPreferenceCached;

		// Token: 0x0400012D RID: 301
		private bool isVerbosePreferenceCached;

		// Token: 0x0400012E RID: 302
		private ActionPreference verbosePreference;

		// Token: 0x0400012F RID: 303
		private bool isWarningPreferenceSet;

		// Token: 0x04000130 RID: 304
		private bool isWarningPreferenceCached;

		// Token: 0x04000131 RID: 305
		private ActionPreference warningPreference = ActionPreference.Continue;

		// Token: 0x04000132 RID: 306
		private bool verboseFlag;

		// Token: 0x04000133 RID: 307
		private bool isVerboseFlagSet;

		// Token: 0x04000134 RID: 308
		private bool confirmFlag;

		// Token: 0x04000135 RID: 309
		private bool isConfirmFlagSet;

		// Token: 0x04000136 RID: 310
		private bool useTransactionFlag;

		// Token: 0x04000137 RID: 311
		private bool useTransactionFlagSet;

		// Token: 0x04000138 RID: 312
		private bool debugFlag;

		// Token: 0x04000139 RID: 313
		private bool isDebugFlagSet;

		// Token: 0x0400013A RID: 314
		private bool whatIfFlag;

		// Token: 0x0400013B RID: 315
		private bool isWhatIfFlagSet;

		// Token: 0x0400013C RID: 316
		private bool isWhatIfPreferenceCached;

		// Token: 0x0400013D RID: 317
		private bool isErrorActionSet;

		// Token: 0x0400013E RID: 318
		private ActionPreference errorAction = ActionPreference.Continue;

		// Token: 0x0400013F RID: 319
		private bool isErrorActionPreferenceCached;

		// Token: 0x04000140 RID: 320
		private ActionPreference progressPreference = ActionPreference.Continue;

		// Token: 0x04000141 RID: 321
		private bool isProgressPreferenceSet;

		// Token: 0x04000142 RID: 322
		private bool isProgressPreferenceCached;

		// Token: 0x04000143 RID: 323
		private ActionPreference informationPreference;

		// Token: 0x04000144 RID: 324
		private bool isInformationPreferenceSet;

		// Token: 0x04000145 RID: 325
		private bool isInformationPreferenceCached;

		// Token: 0x04000146 RID: 326
		private PagingParameters pagingParameters;

		// Token: 0x04000147 RID: 327
		internal MshCommandRuntime.ContinueStatus lastShouldProcessContinueStatus;

		// Token: 0x04000148 RID: 328
		internal MshCommandRuntime.ContinueStatus lastErrorContinueStatus;

		// Token: 0x04000149 RID: 329
		internal MshCommandRuntime.ContinueStatus lastDebugContinueStatus;

		// Token: 0x0400014A RID: 330
		internal MshCommandRuntime.ContinueStatus lastVerboseContinueStatus;

		// Token: 0x0400014B RID: 331
		internal MshCommandRuntime.ContinueStatus lastWarningContinueStatus;

		// Token: 0x0400014C RID: 332
		internal MshCommandRuntime.ContinueStatus lastProgressContinueStatus;

		// Token: 0x0400014D RID: 333
		internal MshCommandRuntime.ContinueStatus lastInformationContinueStatus;

		// Token: 0x02000046 RID: 70
		internal enum ShouldProcessPossibleOptimization
		{
			// Token: 0x04000154 RID: 340
			AutoYes_CanSkipShouldProcessCall,
			// Token: 0x04000155 RID: 341
			AutoYes_CanCallShouldProcessAsynchronously,
			// Token: 0x04000156 RID: 342
			AutoNo_CanCallShouldProcessAsynchronously,
			// Token: 0x04000157 RID: 343
			NoOptimizationPossible
		}

		// Token: 0x02000047 RID: 71
		internal enum MergeDataStream
		{
			// Token: 0x04000159 RID: 345
			None,
			// Token: 0x0400015A RID: 346
			All,
			// Token: 0x0400015B RID: 347
			Output,
			// Token: 0x0400015C RID: 348
			Error,
			// Token: 0x0400015D RID: 349
			Warning,
			// Token: 0x0400015E RID: 350
			Verbose,
			// Token: 0x0400015F RID: 351
			Debug,
			// Token: 0x04000160 RID: 352
			Host,
			// Token: 0x04000161 RID: 353
			Information
		}

		// Token: 0x02000048 RID: 72
		private class AllowWrite : IDisposable
		{
			// Token: 0x060003CF RID: 975 RVA: 0x0000DED0 File Offset: 0x0000C0D0
			internal AllowWrite(InternalCommand permittedToWrite, bool permittedToWriteToPipeline)
			{
				if (permittedToWrite == null)
				{
					throw PSTraceSource.NewArgumentNullException("permittedToWrite");
				}
				MshCommandRuntime mshCommandRuntime = permittedToWrite.commandRuntime as MshCommandRuntime;
				if (mshCommandRuntime == null)
				{
					throw PSTraceSource.NewArgumentNullException("permittedToWrite.CommandRuntime");
				}
				this._pp = mshCommandRuntime.PipelineProcessor;
				if (this._pp == null)
				{
					throw PSTraceSource.NewArgumentNullException("permittedToWrite.CommandRuntime.PipelineProcessor");
				}
				this._wasPermittedToWrite = this._pp._permittedToWrite;
				this._wasPermittedToWriteToPipeline = this._pp._permittedToWriteToPipeline;
				this._wasPermittedToWriteThread = this._pp._permittedToWriteThread;
				this._pp._permittedToWrite = permittedToWrite;
				this._pp._permittedToWriteToPipeline = permittedToWriteToPipeline;
				this._pp._permittedToWriteThread = Thread.CurrentThread;
			}

			// Token: 0x060003D0 RID: 976 RVA: 0x0000DF85 File Offset: 0x0000C185
			public void Dispose()
			{
				this._pp._permittedToWrite = this._wasPermittedToWrite;
				this._pp._permittedToWriteToPipeline = this._wasPermittedToWriteToPipeline;
				this._pp._permittedToWriteThread = this._wasPermittedToWriteThread;
				GC.SuppressFinalize(this);
			}

			// Token: 0x04000162 RID: 354
			private PipelineProcessor _pp;

			// Token: 0x04000163 RID: 355
			private InternalCommand _wasPermittedToWrite;

			// Token: 0x04000164 RID: 356
			private bool _wasPermittedToWriteToPipeline;

			// Token: 0x04000165 RID: 357
			private Thread _wasPermittedToWriteThread;
		}

		// Token: 0x02000049 RID: 73
		internal enum ContinueStatus
		{
			// Token: 0x04000167 RID: 359
			Yes,
			// Token: 0x04000168 RID: 360
			No,
			// Token: 0x04000169 RID: 361
			YesToAll,
			// Token: 0x0400016A RID: 362
			NoToAll
		}
	}
}
