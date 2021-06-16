using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000013 RID: 19
	internal abstract class CommandProcessorBase : IDisposable
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004473 File Offset: 0x00002673
		internal CommandProcessorBase()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004498 File Offset: 0x00002698
		internal CommandProcessorBase(CommandInfo commandInfo)
		{
			if (commandInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandInfo");
			}
			this.commandInfo = commandInfo;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000044D2 File Offset: 0x000026D2
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000044DA File Offset: 0x000026DA
		internal bool AddedToPipelineAlready
		{
			get
			{
				return this._addedToPipelineAlready;
			}
			set
			{
				this._addedToPipelineAlready = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000044E3 File Offset: 0x000026E3
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000044EB File Offset: 0x000026EB
		internal CommandInfo CommandInfo
		{
			get
			{
				return this.commandInfo;
			}
			set
			{
				this.commandInfo = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000044F4 File Offset: 0x000026F4
		public bool FromScriptFile
		{
			get
			{
				return this._fromScriptFile;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000044FC File Offset: 0x000026FC
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004504 File Offset: 0x00002704
		internal bool RedirectShellErrorOutputPipe
		{
			get
			{
				return this._redirectShellErrorOutputPipe;
			}
			set
			{
				this._redirectShellErrorOutputPipe = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000450D File Offset: 0x0000270D
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004518 File Offset: 0x00002718
		internal InternalCommand Command
		{
			get
			{
				return this.command;
			}
			set
			{
				if (value != null)
				{
					value.commandRuntime = this.commandRuntime;
					if (this.command != null)
					{
						value.CommandInfo = this.command.CommandInfo;
					}
					if (value.Context == null && this._context != null)
					{
						value.Context = this._context;
					}
				}
				this.command = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004570 File Offset: 0x00002770
		internal virtual ObsoleteAttribute ObsoleteAttribute
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004573 File Offset: 0x00002773
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x0000457B File Offset: 0x0000277B
		internal MshCommandRuntime CommandRuntime
		{
			get
			{
				return this.commandRuntime;
			}
			set
			{
				this.commandRuntime = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004584 File Offset: 0x00002784
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000458C File Offset: 0x0000278C
		internal bool UseLocalScope
		{
			get
			{
				return this._useLocalScope;
			}
			set
			{
				this._useLocalScope = value;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004598 File Offset: 0x00002798
		protected static void ValidateCompatibleLanguageMode(ScriptBlock scriptBlock, PSLanguageMode languageMode, InvocationInfo invocationInfo)
		{
			if (scriptBlock.LanguageMode != null && scriptBlock.LanguageMode != languageMode && (languageMode == PSLanguageMode.RestrictedLanguage || languageMode == PSLanguageMode.ConstrainedLanguage))
			{
				ErrorRecord errorRecord = new ErrorRecord(new NotSupportedException(DiscoveryExceptions.DotSourceNotSupported), "DotSourceNotSupported", ErrorCategory.InvalidOperation, null);
				errorRecord.SetInvocationInfo(invocationInfo);
				throw new CmdletInvocationException(errorRecord);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004604 File Offset: 0x00002804
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x0000460C File Offset: 0x0000280C
		internal ExecutionContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004615 File Offset: 0x00002815
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x0000461D File Offset: 0x0000281D
		internal Guid PipelineActivityId
		{
			get
			{
				return this._pipelineActivityId;
			}
			set
			{
				this._pipelineActivityId = value;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004626 File Offset: 0x00002826
		internal virtual bool IsHelpRequested(out string helpTarget, out HelpCategory helpCategory)
		{
			helpTarget = null;
			helpCategory = HelpCategory.None;
			return false;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004630 File Offset: 0x00002830
		internal static CommandProcessorBase CreateGetHelpCommandProcessor(ExecutionContext context, string helpTarget, HelpCategory helpCategory)
		{
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			if (string.IsNullOrEmpty(helpTarget))
			{
				throw PSTraceSource.NewArgumentNullException("helpTarget");
			}
			CommandProcessorBase commandProcessorBase = context.CreateCommand("get-help", false);
			CommandParameterInternal parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, "Name", "-Name:", PositionUtilities.EmptyExtent, helpTarget, false, false);
			commandProcessorBase.AddParameter(parameter);
			parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, "Category", "-Category:", PositionUtilities.EmptyExtent, helpCategory.ToString(), false, false);
			commandProcessorBase.AddParameter(parameter);
			return commandProcessorBase;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000046BE File Offset: 0x000028BE
		internal bool IsPipelineInputExpected()
		{
			return this.commandRuntime.IsPipelineInputExpected;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000046CB File Offset: 0x000028CB
		// (set) Token: 0x060000CE RID: 206 RVA: 0x000046D3 File Offset: 0x000028D3
		internal SessionStateInternal CommandSessionState
		{
			get
			{
				return this._commandSessionState;
			}
			set
			{
				this._commandSessionState = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000046DC File Offset: 0x000028DC
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x000046E4 File Offset: 0x000028E4
		protected internal SessionStateScope CommandScope { get; protected set; }

		// Token: 0x060000D1 RID: 209 RVA: 0x000046ED File Offset: 0x000028ED
		protected virtual void OnSetCurrentScope()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000046EF File Offset: 0x000028EF
		protected virtual void OnRestorePreviousScope()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000046F4 File Offset: 0x000028F4
		internal void SetCurrentScopeToExecutionScope()
		{
			if (this._commandSessionState == null)
			{
				this._commandSessionState = this.Context.EngineSessionState;
			}
			this._previousScope = this._commandSessionState.CurrentScope;
			this._previousCommandSessionState = this.Context.EngineSessionState;
			this.Context.EngineSessionState = this._commandSessionState;
			this._commandSessionState.CurrentScope = this.CommandScope;
			this.OnSetCurrentScope();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004764 File Offset: 0x00002964
		internal void RestorePreviousScope()
		{
			this.OnRestorePreviousScope();
			this.Context.EngineSessionState = this._previousCommandSessionState;
			if (this._previousScope != null)
			{
				this._commandSessionState.CurrentScope = this._previousScope;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004796 File Offset: 0x00002996
		internal void AddParameter(CommandParameterInternal parameter)
		{
			this.arguments.Add(parameter);
		}

		// Token: 0x060000D6 RID: 214
		internal abstract void Prepare(IDictionary psDefaultParameterValues);

		// Token: 0x060000D7 RID: 215 RVA: 0x000047A4 File Offset: 0x000029A4
		private void HandleObsoleteCommand(ObsoleteAttribute obsoleteAttr)
		{
			string text = string.IsNullOrEmpty(this.commandInfo.Name) ? "script block" : string.Format(CultureInfo.InvariantCulture, CommandBaseStrings.ObsoleteCommand, new object[]
			{
				this.commandInfo.Name
			});
			string message = string.Format(CultureInfo.InvariantCulture, CommandBaseStrings.UseOfDeprecatedCommandWarning, new object[]
			{
				text,
				obsoleteAttr.Message
			});
			using (this.CommandRuntime.AllowThisCommandToWrite(false))
			{
				this.CommandRuntime.WriteWarning(new WarningRecord("CommandObsolete", message), false);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004858 File Offset: 0x00002A58
		internal void DoPrepare(IDictionary psDefaultParameterValues)
		{
			CommandProcessorBase currentCommandProcessor = this._context.CurrentCommandProcessor;
			try
			{
				this.Context.CurrentCommandProcessor = this;
				this.SetCurrentScopeToExecutionScope();
				this.Prepare(psDefaultParameterValues);
				if (this.ObsoleteAttribute != null)
				{
					this.HandleObsoleteCommand(this.ObsoleteAttribute);
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				if (this._useLocalScope)
				{
					this._commandSessionState.RemoveScope(this.CommandScope);
				}
				throw;
			}
			finally
			{
				this.Context.CurrentCommandProcessor = currentCommandProcessor;
				this.RestorePreviousScope();
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000048F4 File Offset: 0x00002AF4
		internal virtual void DoBegin()
		{
			if (!this.RanBeginAlready)
			{
				this.RanBeginAlready = true;
				Pipe shellFunctionErrorOutputPipe = this._context.ShellFunctionErrorOutputPipe;
				CommandProcessorBase currentCommandProcessor = this._context.CurrentCommandProcessor;
				try
				{
					if (this.RedirectShellErrorOutputPipe || this._context.ShellFunctionErrorOutputPipe != null)
					{
						this._context.ShellFunctionErrorOutputPipe = this.commandRuntime.ErrorOutputPipe;
					}
					this._context.CurrentCommandProcessor = this;
					using (this.commandRuntime.AllowThisCommandToWrite(true))
					{
						using (ParameterBinderBase.bindingTracer.TraceScope("CALLING BeginProcessing", new object[0]))
						{
							this.SetCurrentScopeToExecutionScope();
							if (this.Context._debuggingMode > 0 && !(this.Command is PSScriptCmdlet))
							{
								this.Context.Debugger.CheckCommand(this.Command.MyInvocation);
							}
							this.Command.DoBeginProcessing();
						}
					}
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
					throw this.ManageInvocationException(e);
				}
				finally
				{
					this._context.ShellFunctionErrorOutputPipe = shellFunctionErrorOutputPipe;
					this._context.CurrentCommandProcessor = currentCommandProcessor;
					this.RestorePreviousScope();
				}
			}
		}

		// Token: 0x060000DA RID: 218
		internal abstract void ProcessRecord();

		// Token: 0x060000DB RID: 219 RVA: 0x00004A50 File Offset: 0x00002C50
		internal void DoExecute()
		{
			ExecutionContext.CheckStackDepth();
			CommandProcessorBase currentCommandProcessor = this._context.CurrentCommandProcessor;
			try
			{
				this.Context.CurrentCommandProcessor = this;
				this.SetCurrentScopeToExecutionScope();
				this.ProcessRecord();
			}
			finally
			{
				this.Context.CurrentCommandProcessor = currentCommandProcessor;
				this.RestorePreviousScope();
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004AAC File Offset: 0x00002CAC
		internal virtual void Complete()
		{
			this.ProcessRecord();
			try
			{
				using (this.commandRuntime.AllowThisCommandToWrite(true))
				{
					using (ParameterBinderBase.bindingTracer.TraceScope("CALLING EndProcessing", new object[0]))
					{
						this.Command.DoEndProcessing();
					}
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw this.ManageInvocationException(e);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B40 File Offset: 0x00002D40
		internal void DoComplete()
		{
			Pipe shellFunctionErrorOutputPipe = this._context.ShellFunctionErrorOutputPipe;
			CommandProcessorBase currentCommandProcessor = this._context.CurrentCommandProcessor;
			try
			{
				if (this.RedirectShellErrorOutputPipe || this._context.ShellFunctionErrorOutputPipe != null)
				{
					this._context.ShellFunctionErrorOutputPipe = this.commandRuntime.ErrorOutputPipe;
				}
				this._context.CurrentCommandProcessor = this;
				this.SetCurrentScopeToExecutionScope();
				this.Complete();
			}
			finally
			{
				this.OnRestorePreviousScope();
				this._context.ShellFunctionErrorOutputPipe = shellFunctionErrorOutputPipe;
				this._context.CurrentCommandProcessor = currentCommandProcessor;
				if (this._useLocalScope && this.CommandScope != null)
				{
					this._commandSessionState.RemoveScope(this.CommandScope);
				}
				if (this._previousScope != null)
				{
					this._commandSessionState.CurrentScope = this._previousScope;
				}
				if (this._previousCommandSessionState != null)
				{
					this.Context.EngineSessionState = this._previousCommandSessionState;
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004C2C File Offset: 0x00002E2C
		public override string ToString()
		{
			if (this.commandInfo != null)
			{
				return this.commandInfo.ToString();
			}
			return "<NullCommandInfo>";
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004C48 File Offset: 0x00002E48
		internal virtual bool Read()
		{
			if (this.firstCallToRead)
			{
				this.firstCallToRead = false;
			}
			object obj = this.commandRuntime.InputPipe.Retrieve();
			if (obj == AutomationNull.Value)
			{
				return false;
			}
			if (this.Command.MyInvocation.PipelinePosition == 1)
			{
				this.Command.MyInvocation.PipelineIterationInfo[0]++;
			}
			this.Command.CurrentPipelineObject = LanguagePrimitives.AsPSObjectOrNull(obj);
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004CC8 File Offset: 0x00002EC8
		internal static void CheckForSevereException(Exception e)
		{
			if (!(e is AccessViolationException))
			{
				if (!(e is StackOverflowException))
				{
					return;
				}
			}
			try
			{
				if (!CommandProcessorBase.alreadyFailing)
				{
					CommandProcessorBase.alreadyFailing = true;
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					MshLog.LogCommandHealthEvent(executionContextFromTLS, e, Severity.Critical);
				}
			}
			finally
			{
				WindowsErrorReporting.FailFast(e);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004D1C File Offset: 0x00002F1C
		internal PipelineStoppedException ManageInvocationException(Exception e)
		{
			PipelineStoppedException result;
			try
			{
				if (this.Command != null)
				{
					ProviderInvocationException ex = e as ProviderInvocationException;
					if (ex != null)
					{
						e = new CmdletProviderInvocationException(ex, this.Command.MyInvocation);
					}
					else if (!(e is PipelineStoppedException) && !(e is CmdletInvocationException) && !(e is ActionPreferenceStopException) && !(e is HaltCommandException) && !(e is FlowControlException) && !(e is ScriptCallDepthException))
					{
						RuntimeException ex2 = e as RuntimeException;
						if (ex2 == null || !ex2.WasThrownFromThrowStatement)
						{
							e = new CmdletInvocationException(e, this.Command.MyInvocation);
						}
					}
					if (this.commandRuntime.UseTransaction)
					{
						bool flag = false;
						for (Exception ex3 = e; ex3 != null; ex3 = ex3.InnerException)
						{
							if (ex3 is TimeoutException)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(TransactionStrings.TransactionTimedOut), "TRANSACTION_TIMEOUT", ErrorCategory.InvalidOperation, e);
							errorRecord.SetInvocationInfo(this.Command.MyInvocation);
							e = new CmdletInvocationException(errorRecord);
						}
						if (this._context.TransactionManager.HasTransaction && this._context.TransactionManager.RollbackPreference != RollbackSeverity.Never)
						{
							this.Context.TransactionManager.Rollback(true);
						}
					}
					result = (PipelineStoppedException)this.commandRuntime.ManageException(e);
				}
				else
				{
					result = new PipelineStoppedException();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004E84 File Offset: 0x00003084
		internal void ManageScriptException(RuntimeException e)
		{
			if (this.Command != null && this.commandRuntime.PipelineProcessor != null)
			{
				this.commandRuntime.PipelineProcessor.RecordFailure(e, this.Command);
				if (!(e is PipelineStoppedException) && !e.WasThrownFromThrowStatement)
				{
					this.commandRuntime.AppendErrorToVariables(e);
				}
			}
			throw new PipelineStoppedException();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004EDF File Offset: 0x000030DF
		internal void ForgetScriptException()
		{
			if (this.Command != null && this.commandRuntime.PipelineProcessor != null)
			{
				this.commandRuntime.PipelineProcessor.ForgetFailure();
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004F06 File Offset: 0x00003106
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004F18 File Offset: 0x00003118
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				IDisposable disposable = this.Command as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			this.disposed = true;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004F50 File Offset: 0x00003150
		~CommandProcessorBase()
		{
			this.Dispose(false);
		}

		// Token: 0x04000043 RID: 67
		private const string FQIDCommandObsolete = "CommandObsolete";

		// Token: 0x04000044 RID: 68
		private InternalCommand command;

		// Token: 0x04000045 RID: 69
		internal bool RanBeginAlready;

		// Token: 0x04000046 RID: 70
		internal bool _addedToPipelineAlready;

		// Token: 0x04000047 RID: 71
		private CommandInfo commandInfo;

		// Token: 0x04000048 RID: 72
		protected bool _fromScriptFile;

		// Token: 0x04000049 RID: 73
		private bool _redirectShellErrorOutputPipe;

		// Token: 0x0400004A RID: 74
		protected MshCommandRuntime commandRuntime;

		// Token: 0x0400004B RID: 75
		protected bool _useLocalScope;

		// Token: 0x0400004C RID: 76
		protected ExecutionContext _context;

		// Token: 0x0400004D RID: 77
		private Guid _pipelineActivityId = Guid.Empty;

		// Token: 0x0400004E RID: 78
		private SessionStateInternal _commandSessionState;

		// Token: 0x0400004F RID: 79
		private SessionStateScope _previousScope;

		// Token: 0x04000050 RID: 80
		private SessionStateInternal _previousCommandSessionState;

		// Token: 0x04000051 RID: 81
		internal Collection<CommandParameterInternal> arguments = new Collection<CommandParameterInternal>();

		// Token: 0x04000052 RID: 82
		private bool firstCallToRead = true;

		// Token: 0x04000053 RID: 83
		private static bool alreadyFailing;

		// Token: 0x04000054 RID: 84
		private bool disposed;
	}
}
