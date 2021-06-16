using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Microsoft.PowerShell.Telemetry.Internal;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200075F RID: 1887
	internal class PipelineProcessor : IDisposable
	{
		// Token: 0x06004B3F RID: 19263 RVA: 0x00189AA1 File Offset: 0x00187CA1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x00189AB0 File Offset: 0x00187CB0
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				this.DisposeCommands();
				this.localPipeline = null;
				this.externalSuccessOutput = null;
				this.externalErrorOutput = null;
				this.executionScope = null;
				this.eventLogBuffer = null;
				this.SecurityContext.Dispose();
				this.SecurityContext = null;
			}
			this.disposed = true;
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x00189B0C File Offset: 0x00187D0C
		~PipelineProcessor()
		{
			this.Dispose(false);
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x00189B3C File Offset: 0x00187D3C
		internal List<CommandProcessorBase> Commands
		{
			get
			{
				return this._commands;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06004B43 RID: 19267 RVA: 0x00189B44 File Offset: 0x00187D44
		// (set) Token: 0x06004B44 RID: 19268 RVA: 0x00189B4C File Offset: 0x00187D4C
		internal bool ExecutionFailed
		{
			get
			{
				return this.executionFailed;
			}
			set
			{
				this.executionFailed = value;
			}
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x00189B58 File Offset: 0x00187D58
		internal void LogExecutionInfo(InvocationInfo invocationInfo, string text)
		{
			string logElement = StringUtil.Format(PipelineStrings.PipelineExecutionInformation, this.GetCommand(invocationInfo), text);
			this.Log(logElement, invocationInfo, PipelineProcessor.PipelineExecutionStatus.Started);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x00189B84 File Offset: 0x00187D84
		internal void LogExecutionComplete(InvocationInfo invocationInfo, string text)
		{
			string logElement = StringUtil.Format(PipelineStrings.PipelineExecutionInformation, this.GetCommand(invocationInfo), text);
			this.Log(logElement, invocationInfo, PipelineProcessor.PipelineExecutionStatus.Complete);
			TelemetryAPI.TraceExecutedCommand(invocationInfo.MyCommand, invocationInfo.CommandOrigin, !this.ExecutionFailed);
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00189BC7 File Offset: 0x00187DC7
		internal void LogPipelineComplete()
		{
			this.Log(null, null, PipelineProcessor.PipelineExecutionStatus.PipelineComplete);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x00189BD4 File Offset: 0x00187DD4
		internal void LogExecutionParameterBinding(InvocationInfo invocationInfo, string parameterName, string parameterValue)
		{
			string logElement = StringUtil.Format(PipelineStrings.PipelineExecutionParameterBinding, new object[]
			{
				this.GetCommand(invocationInfo),
				parameterName,
				parameterValue
			});
			this.Log(logElement, invocationInfo, PipelineProcessor.PipelineExecutionStatus.ParameterBinding);
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x00189C10 File Offset: 0x00187E10
		internal void LogExecutionError(InvocationInfo invocationInfo, ErrorRecord errorRecord)
		{
			if (errorRecord == null)
			{
				return;
			}
			string logElement = StringUtil.Format(PipelineStrings.PipelineExecutionNonTerminatingError, this.GetCommand(invocationInfo), errorRecord.ToString());
			this.Log(logElement, invocationInfo, PipelineProcessor.PipelineExecutionStatus.Error);
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x00189C44 File Offset: 0x00187E44
		internal void LogExecutionException(Exception exception)
		{
			this.executionFailed = true;
			if (this.terminatingErrorLogged)
			{
				return;
			}
			this.terminatingErrorLogged = true;
			if (exception == null)
			{
				return;
			}
			string logElement = StringUtil.Format(PipelineStrings.PipelineExecutionTerminatingError, this.GetCommand(exception), exception.Message);
			this.Log(logElement, null, PipelineProcessor.PipelineExecutionStatus.Error);
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x00189C8D File Offset: 0x00187E8D
		private string GetCommand(InvocationInfo invocationInfo)
		{
			if (invocationInfo == null)
			{
				return "";
			}
			if (invocationInfo.MyCommand != null)
			{
				return invocationInfo.MyCommand.Name;
			}
			return "";
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x00189CB4 File Offset: 0x00187EB4
		private string GetCommand(Exception exception)
		{
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				return this.GetCommand(containsErrorRecord.ErrorRecord.InvocationInfo);
			}
			return "";
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00189CEC File Offset: 0x00187EEC
		private void Log(string logElement, InvocationInfo invocation, PipelineProcessor.PipelineExecutionStatus pipelineExecutionStatus)
		{
			PSHostUserInterface pshostUserInterface = null;
			if (this.LocalPipeline != null)
			{
				pshostUserInterface = this.LocalPipeline.Runspace.GetExecutionContext.EngineHostInterface.UI;
			}
			if (pshostUserInterface != null)
			{
				if (pipelineExecutionStatus == PipelineProcessor.PipelineExecutionStatus.Complete)
				{
					pshostUserInterface.TranscribeCommandComplete(invocation);
					return;
				}
				if (pipelineExecutionStatus == PipelineProcessor.PipelineExecutionStatus.PipelineComplete)
				{
					pshostUserInterface.TranscribePipelineComplete();
					return;
				}
			}
			if ((invocation == null || string.IsNullOrEmpty(invocation.Line)) && pshostUserInterface != null)
			{
				pshostUserInterface.TranscribeCommand(logElement, invocation);
			}
			if (!string.IsNullOrEmpty(logElement))
			{
				this.eventLogBuffer.Add(logElement);
			}
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00189D68 File Offset: 0x00187F68
		internal void LogToEventLog()
		{
			if (this.NeedToLog())
			{
				if (this._commands == null || this._commands.Count <= 0 || this.eventLogBuffer.Count == 0)
				{
					return;
				}
				MshLog.LogPipelineExecutionDetailEvent(this._commands[0].Command.Context, this.eventLogBuffer, this._commands[0].Command.MyInvocation);
			}
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00189DD8 File Offset: 0x00187FD8
		private bool NeedToLog()
		{
			if (this._commands == null)
			{
				return false;
			}
			foreach (CommandProcessorBase commandProcessorBase in this._commands)
			{
				MshCommandRuntime mshCommandRuntime = commandProcessorBase.Command.commandRuntime as MshCommandRuntime;
				if (mshCommandRuntime != null && mshCommandRuntime.LogPipelineExecutionDetail)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00189E54 File Offset: 0x00188054
		internal int Add(CommandProcessorBase commandProcessor)
		{
			commandProcessor.CommandRuntime.PipelineProcessor = this;
			return this.AddCommand(commandProcessor, this._commands.Count, false);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00189E75 File Offset: 0x00188075
		internal void AddRedirectionPipe(PipelineProcessor pipelineProcessor)
		{
			if (pipelineProcessor == null)
			{
				throw PSTraceSource.NewArgumentNullException("pipelineProcessor");
			}
			if (this._redirectionPipes == null)
			{
				this._redirectionPipes = new List<PipelineProcessor>();
			}
			this._redirectionPipes.Add(pipelineProcessor);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00189EA4 File Offset: 0x001880A4
		internal Array Execute()
		{
			return this.Execute(null);
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x00189EAD File Offset: 0x001880AD
		internal Array Execute(Array input)
		{
			return this.SynchronousExecute(input, null);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00189EB8 File Offset: 0x001880B8
		internal int AddCommand(CommandProcessorBase commandProcessor, int readFromCommand, bool readErrorQueue)
		{
			if (commandProcessor == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandProcessor");
			}
			if (this._commands == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			if (this.disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("PipelineProcessor");
			}
			if (this.executionStarted)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.ExecutionAlreadyStarted, new object[0]);
			}
			if (commandProcessor.AddedToPipelineAlready)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.CommandProcessorAlreadyUsed, new object[0]);
			}
			if (this._commands.Count == 0)
			{
				if (readFromCommand != 0)
				{
					throw PSTraceSource.NewArgumentException("readFromCommand", PipelineStrings.FirstCommandCannotHaveInput, new object[0]);
				}
				commandProcessor.AddedToPipelineAlready = true;
			}
			else
			{
				if (readFromCommand > this._commands.Count || readFromCommand <= 0)
				{
					throw PSTraceSource.NewArgumentException("readFromCommand", PipelineStrings.InvalidCommandNumber, new object[0]);
				}
				CommandProcessorBase commandProcessorBase = this._commands[readFromCommand - 1];
				if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				Pipe pipe = readErrorQueue ? commandProcessorBase.CommandRuntime.ErrorOutputPipe : commandProcessorBase.CommandRuntime.OutputPipe;
				if (pipe == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				if (pipe.DownstreamCmdlet != null)
				{
					throw PSTraceSource.NewInvalidOperationException(PipelineStrings.PipeAlreadyTaken, new object[0]);
				}
				commandProcessor.AddedToPipelineAlready = true;
				commandProcessor.CommandRuntime.InputPipe = pipe;
				pipe.DownstreamCmdlet = commandProcessor;
				if (commandProcessor.CommandRuntime.MergeUnclaimedPreviousErrorResults)
				{
					for (int i = 0; i < this._commands.Count; i++)
					{
						commandProcessorBase = this._commands[i];
						if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
						{
							throw PSTraceSource.NewInvalidOperationException();
						}
						if (commandProcessorBase.CommandRuntime.ErrorOutputPipe.DownstreamCmdlet == null && commandProcessorBase.CommandRuntime.ErrorOutputPipe.ExternalWriter == null)
						{
							commandProcessorBase.CommandRuntime.ErrorOutputPipe = pipe;
						}
					}
				}
			}
			this._commands.Add(commandProcessor);
			commandProcessor.CommandRuntime.PipelineProcessor = this;
			return this._commands.Count;
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x0018A08C File Offset: 0x0018828C
		internal Array SynchronousExecute(Array input, Hashtable errorResults)
		{
			if (input == null)
			{
				return this.SynchronousExecuteEnumerate(AutomationNull.Value, errorResults, true);
			}
			return this.SynchronousExecuteEnumerate(input, errorResults, true);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x0018A0A8 File Offset: 0x001882A8
		internal Array SynchronousExecuteEnumerate(object input, Hashtable errorResults, bool enumerate)
		{
			if (this.Stopping)
			{
				throw new PipelineStoppedException();
			}
			Exception ex = null;
			try
			{
				CommandProcessorBase commandRequestingUpstreamCommandsToStop = null;
				try
				{
					this.Start(input != AutomationNull.Value);
					CommandProcessorBase commandProcessorBase = this._commands[0];
					if (this.ExternalInput != null)
					{
						commandProcessorBase.CommandRuntime.InputPipe.ExternalReader = this.ExternalInput;
					}
					this.Inject(input, enumerate);
				}
				catch (PipelineStoppedException)
				{
					StopUpstreamCommandsException ex2 = this.firstTerminatingError as StopUpstreamCommandsException;
					if (ex2 == null)
					{
						throw;
					}
					this.firstTerminatingError = null;
					commandRequestingUpstreamCommandsToStop = ex2.RequestingCommandProcessor;
				}
				this.DoCompleteCore(commandRequestingUpstreamCommandsToStop);
				if (this._redirectionPipes != null)
				{
					foreach (PipelineProcessor pipelineProcessor in this._redirectionPipes)
					{
						pipelineProcessor.DoCompleteCore(null);
					}
				}
				return this.RetrieveResults(errorResults);
			}
			catch (RuntimeException ex3)
			{
				if (this.firstTerminatingError != null)
				{
					ex = this.firstTerminatingError;
				}
				else
				{
					ex = ex3;
				}
				this.LogExecutionException(ex);
			}
			catch (InvalidComObjectException ex4)
			{
				if (this.firstTerminatingError != null)
				{
					ex = this.firstTerminatingError;
				}
				else
				{
					string message = StringUtil.Format(ParserStrings.InvalidComObjectException, ex4.Message);
					ex = new RuntimeException(message, ex4);
					((RuntimeException)ex).SetErrorId("InvalidComObjectException");
				}
				this.LogExecutionException(ex);
			}
			finally
			{
				this.DisposeCommands();
			}
			RuntimeException.LockStackTrace(ex);
			throw ex;
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x0018A27C File Offset: 0x0018847C
		private void DoCompleteCore(CommandProcessorBase commandRequestingUpstreamCommandsToStop)
		{
			MshCommandRuntime mshCommandRuntime = null;
			if (this._commands != null)
			{
				for (int i = 0; i < this._commands.Count; i++)
				{
					CommandProcessorBase commandProcessorBase = this._commands[i];
					if (commandProcessorBase == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					if (object.ReferenceEquals(commandRequestingUpstreamCommandsToStop, commandProcessorBase))
					{
						commandRequestingUpstreamCommandsToStop = null;
					}
					else if (commandRequestingUpstreamCommandsToStop == null)
					{
						try
						{
							commandProcessorBase.DoComplete();
						}
						catch (PipelineStoppedException)
						{
							StopUpstreamCommandsException ex = this.firstTerminatingError as StopUpstreamCommandsException;
							if (ex == null)
							{
								throw;
							}
							this.firstTerminatingError = null;
							commandRequestingUpstreamCommandsToStop = ex.RequestingCommandProcessor;
						}
						EtwActivity.SetActivityId(commandProcessorBase.PipelineActivityId);
						MshLog.LogCommandLifecycleEvent(commandProcessorBase.Command.Context, CommandState.Stopped, commandProcessorBase.Command.MyInvocation);
						if (commandProcessorBase.CommandInfo.CommandType != CommandTypes.Script)
						{
							commandProcessorBase.CommandRuntime.PipelineProcessor.LogExecutionComplete(commandProcessorBase.Command.MyInvocation, commandProcessorBase.CommandInfo.Name);
						}
						mshCommandRuntime = commandProcessorBase.CommandRuntime;
					}
				}
			}
			if (mshCommandRuntime != null && (this.LocalPipeline == null || !this.LocalPipeline.IsNested))
			{
				mshCommandRuntime.PipelineProcessor.LogPipelineComplete();
			}
			if (this.firstTerminatingError != null)
			{
				this.LogExecutionException(this.firstTerminatingError);
				throw this.firstTerminatingError;
			}
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x0018A3BC File Offset: 0x001885BC
		internal Array DoComplete()
		{
			if (this.Stopping)
			{
				throw new PipelineStoppedException();
			}
			if (!this.executionStarted)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.PipelineNotStarted, new object[0]);
			}
			Exception ex = null;
			try
			{
				this.DoCompleteCore(null);
				Hashtable errorResults = new Hashtable();
				return this.RetrieveResults(errorResults);
			}
			catch (RuntimeException ex2)
			{
				if (this.firstTerminatingError != null)
				{
					ex = this.firstTerminatingError;
				}
				else
				{
					ex = ex2;
				}
				this.LogExecutionException(ex);
			}
			catch (InvalidComObjectException ex3)
			{
				if (this.firstTerminatingError != null)
				{
					ex = this.firstTerminatingError;
				}
				else
				{
					string message = StringUtil.Format(ParserStrings.InvalidComObjectException, ex3.Message);
					ex = new RuntimeException(message, ex3);
					((RuntimeException)ex).SetErrorId("InvalidComObjectException");
				}
				this.LogExecutionException(ex);
			}
			finally
			{
				this.DisposeCommands();
			}
			RuntimeException.LockStackTrace(ex);
			throw ex;
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x0018A4A8 File Offset: 0x001886A8
		internal void StartStepping(bool expectInput)
		{
			try
			{
				this.Start(expectInput);
				if (this.firstTerminatingError != null)
				{
					throw this.firstTerminatingError;
				}
			}
			catch (PipelineStoppedException)
			{
				this.DisposeCommands();
				if (this.firstTerminatingError != null)
				{
					throw this.firstTerminatingError;
				}
				throw;
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0018A4F8 File Offset: 0x001886F8
		internal Array Step(object input)
		{
			return this.DoStepItems(input, null, false);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x0018A503 File Offset: 0x00188703
		internal Array Step(object input, Hashtable errorResults)
		{
			if (errorResults == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorResults");
			}
			return this.DoStepItems(input, errorResults, false);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x0018A51C File Offset: 0x0018871C
		internal Array StepArray(Array input)
		{
			return this.DoStepItems(input, null, true);
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x0018A527 File Offset: 0x00188727
		internal Array StepArray(Array input, Hashtable errorResults)
		{
			if (errorResults == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorResults");
			}
			return this.DoStepItems(input, errorResults, true);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x0018A540 File Offset: 0x00188740
		internal void Stop()
		{
			if (!this.RecordFailure(new PipelineStoppedException(), null))
			{
				return;
			}
			List<CommandProcessorBase> commands = this._commands;
			if (commands == null)
			{
				return;
			}
			for (int i = 0; i < commands.Count; i++)
			{
				CommandProcessorBase commandProcessorBase = commands[i];
				if (commandProcessorBase == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				try
				{
					commandProcessorBase.Command.DoStopProcessing();
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x0018A5B0 File Offset: 0x001887B0
		private Array DoStepItems(object input, Hashtable errorResults, bool enumerate)
		{
			if (this.Stopping)
			{
				throw new PipelineStoppedException();
			}
			Array result;
			try
			{
				this.Start(true);
				this.Inject(input, enumerate);
				if (this.firstTerminatingError != null)
				{
					throw this.firstTerminatingError;
				}
				result = this.RetrieveResults(errorResults);
			}
			catch (PipelineStoppedException)
			{
				this.DisposeCommands();
				if (this.firstTerminatingError != null)
				{
					throw this.firstTerminatingError;
				}
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this.DisposeCommands();
				throw;
			}
			return result;
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x0018A638 File Offset: 0x00188838
		private void Start(bool incomingStream)
		{
			if (this.disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("PipelineProcessor");
			}
			if (this.Stopping)
			{
				throw new PipelineStoppedException();
			}
			if (this.executionStarted)
			{
				return;
			}
			if (this._commands == null || this._commands.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.PipelineExecuteRequiresAtLeastOneCommand, new object[0]);
			}
			CommandProcessorBase commandProcessorBase = this._commands[0];
			if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.PipelineExecuteRequiresAtLeastOneCommand, new object[0]);
			}
			if (this.executionScope == null)
			{
				this.executionScope = commandProcessorBase.Context.EngineSessionState.CurrentScope;
			}
			CommandProcessorBase commandProcessorBase2 = this._commands[this._commands.Count - 1];
			if (commandProcessorBase2 == null || commandProcessorBase2.CommandRuntime == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			if (this.ExternalSuccessOutput != null)
			{
				commandProcessorBase2.CommandRuntime.OutputPipe.ExternalWriter = this.ExternalSuccessOutput;
			}
			this.SetExternalErrorOutput();
			if (this.ExternalInput == null && !incomingStream)
			{
				commandProcessorBase.CommandRuntime.IsClosed = true;
			}
			IDictionary psDefaultParameterValues = commandProcessorBase.Context.GetVariableValue(SpecialVariables.PSDefaultParameterValuesVarPath, false) as IDictionary;
			this.executionStarted = true;
			int[] pipelineIterationInfo = new int[this._commands.Count + 1];
			for (int i = 0; i < this._commands.Count; i++)
			{
				CommandProcessorBase commandProcessorBase3 = this._commands[i];
				if (commandProcessorBase3 == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				Guid guid = EtwActivity.CreateActivityId();
				EtwActivity.SetActivityId(guid);
				commandProcessorBase3.PipelineActivityId = guid;
				MshLog.LogCommandLifecycleEvent(commandProcessorBase3.Context, CommandState.Started, commandProcessorBase3.Command.MyInvocation);
				if (commandProcessorBase3.CommandInfo.CommandType != CommandTypes.Script)
				{
					commandProcessorBase3.CommandRuntime.PipelineProcessor.LogExecutionInfo(commandProcessorBase3.Command.MyInvocation, commandProcessorBase3.CommandInfo.Name);
				}
				InvocationInfo myInvocation = commandProcessorBase3.Command.MyInvocation;
				myInvocation.PipelinePosition = i + 1;
				myInvocation.PipelineLength = this._commands.Count;
				myInvocation.PipelineIterationInfo = pipelineIterationInfo;
				commandProcessorBase3.DoPrepare(psDefaultParameterValues);
				myInvocation.ExpectingInput = commandProcessorBase3.IsPipelineInputExpected();
			}
			this.SetupParameterVariables();
			for (int j = 0; j < this._commands.Count; j++)
			{
				CommandProcessorBase commandProcessorBase4 = this._commands[j];
				commandProcessorBase4.DoBegin();
			}
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x0018A89C File Offset: 0x00188A9C
		private void SetExternalErrorOutput()
		{
			if (this.ExternalErrorOutput != null)
			{
				for (int i = 0; i < this._commands.Count; i++)
				{
					CommandProcessorBase commandProcessorBase = this._commands[i];
					Pipe errorOutputPipe = commandProcessorBase.CommandRuntime.ErrorOutputPipe;
					if (!errorOutputPipe.IsRedirected)
					{
						errorOutputPipe.ExternalWriter = this.ExternalErrorOutput;
					}
				}
			}
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x0018A8F4 File Offset: 0x00188AF4
		private void SetupParameterVariables()
		{
			for (int i = 0; i < this._commands.Count; i++)
			{
				CommandProcessorBase commandProcessorBase = this._commands[i];
				if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				commandProcessorBase.CommandRuntime.SetupOutVariable();
				commandProcessorBase.CommandRuntime.SetupErrorVariable();
				commandProcessorBase.CommandRuntime.SetupWarningVariable();
				commandProcessorBase.CommandRuntime.SetupPipelineVariable();
				commandProcessorBase.CommandRuntime.SetupInformationVariable();
			}
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x0018A96C File Offset: 0x00188B6C
		private void Inject(object input, bool enumerate)
		{
			CommandProcessorBase commandProcessorBase = this._commands[0];
			if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
			{
				throw PSTraceSource.NewInvalidOperationException(PipelineStrings.PipelineExecuteRequiresAtLeastOneCommand, new object[0]);
			}
			if (input != AutomationNull.Value)
			{
				if (enumerate)
				{
					IEnumerator enumerator = LanguagePrimitives.GetEnumerator(input);
					if (enumerator != null)
					{
						commandProcessorBase.CommandRuntime.InputPipe = new Pipe(enumerator);
					}
					else
					{
						commandProcessorBase.CommandRuntime.InputPipe.Add(input);
					}
				}
				else
				{
					commandProcessorBase.CommandRuntime.InputPipe.Add(input);
				}
			}
			commandProcessorBase.DoExecute();
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x0018A9F4 File Offset: 0x00188BF4
		private Array RetrieveResults(Hashtable errorResults)
		{
			if (!this._linkedErrorOutput)
			{
				for (int i = 0; i < this._commands.Count; i++)
				{
					CommandProcessorBase commandProcessorBase = this._commands[i];
					if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
					{
						throw PSTraceSource.NewInvalidOperationException();
					}
					Pipe errorOutputPipe = commandProcessorBase.CommandRuntime.ErrorOutputPipe;
					if (errorOutputPipe.DownstreamCmdlet == null && !errorOutputPipe.Empty)
					{
						if (errorResults != null)
						{
							errorResults.Add(i + 1, errorOutputPipe.ToArray());
						}
						errorOutputPipe.Clear();
					}
				}
			}
			if (this._linkedSuccessOutput)
			{
				return MshCommandRuntime.StaticEmptyArray;
			}
			CommandProcessorBase commandProcessorBase2 = this._commands[this._commands.Count - 1];
			if (commandProcessorBase2 == null || commandProcessorBase2.CommandRuntime == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			Array resultsAsArray = commandProcessorBase2.CommandRuntime.GetResultsAsArray();
			commandProcessorBase2.CommandRuntime.OutputPipe.Clear();
			if (resultsAsArray == null)
			{
				return MshCommandRuntime.StaticEmptyArray;
			}
			return resultsAsArray;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x0018AAD8 File Offset: 0x00188CD8
		internal void LinkPipelineSuccessOutput(Pipe pipeToUse)
		{
			CommandProcessorBase commandProcessorBase = this._commands[this._commands.Count - 1];
			if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			commandProcessorBase.CommandRuntime.OutputPipe = pipeToUse;
			this._linkedSuccessOutput = true;
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0018AB24 File Offset: 0x00188D24
		internal void LinkPipelineErrorOutput(Pipe pipeToUse)
		{
			for (int i = 0; i < this._commands.Count; i++)
			{
				CommandProcessorBase commandProcessorBase = this._commands[i];
				if (commandProcessorBase == null || commandProcessorBase.CommandRuntime == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				Pipe errorOutputPipe = commandProcessorBase.CommandRuntime.ErrorOutputPipe;
				if (commandProcessorBase.CommandRuntime.ErrorOutputPipe.DownstreamCmdlet == null)
				{
					commandProcessorBase.CommandRuntime.ErrorOutputPipe = pipeToUse;
				}
			}
			this._linkedErrorOutput = true;
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x0018AB98 File Offset: 0x00188D98
		private void DisposeCommands()
		{
			this.stopping = true;
			this.LogToEventLog();
			if (this._commands != null)
			{
				for (int i = 0; i < this._commands.Count; i++)
				{
					CommandProcessorBase commandProcessorBase = this._commands[i];
					if (commandProcessorBase != null)
					{
						try
						{
							commandProcessorBase.CommandRuntime.RemoveVariableListsInPipe();
							commandProcessorBase.Dispose();
						}
						catch (Exception ex)
						{
							CommandProcessorBase.CheckForSevereException(ex);
							InvocationInfo invocationInfo = null;
							if (commandProcessorBase.Command != null)
							{
								invocationInfo = commandProcessorBase.Command.MyInvocation;
							}
							ProviderInvocationException ex2 = ex as ProviderInvocationException;
							if (ex2 != null)
							{
								ex = new CmdletProviderInvocationException(ex2, invocationInfo);
							}
							else
							{
								ex = new CmdletInvocationException(ex, invocationInfo);
								MshLog.LogCommandHealthEvent(commandProcessorBase.Command.Context, ex, Severity.Warning);
							}
							this.RecordFailure(ex, commandProcessorBase.Command);
						}
					}
				}
			}
			this._commands = null;
			if (this._redirectionPipes != null)
			{
				foreach (PipelineProcessor pipelineProcessor in this._redirectionPipes)
				{
					try
					{
						if (pipelineProcessor != null)
						{
							pipelineProcessor.Dispose();
						}
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			}
			this._redirectionPipes = null;
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x0018ACE4 File Offset: 0x00188EE4
		internal bool RecordFailure(Exception e, InternalCommand command)
		{
			bool flag = false;
			lock (this.StopReasonLock)
			{
				if (this.firstTerminatingError == null)
				{
					RuntimeException.LockStackTrace(e);
					this.firstTerminatingError = e;
				}
				else if (!(this.firstTerminatingError is PipelineStoppedException) && command != null && command.Context != null)
				{
					Exception ex = e;
					while ((ex is TargetInvocationException || ex is CmdletInvocationException) && ex.InnerException != null)
					{
						ex = ex.InnerException;
					}
					if (!(ex is PipelineStoppedException))
					{
						string message = StringUtil.Format(PipelineStrings.SecondFailure, new object[]
						{
							this.firstTerminatingError.GetType().Name,
							this.firstTerminatingError.StackTrace,
							ex.GetType().Name,
							ex.StackTrace
						});
						InvalidOperationException exception = new InvalidOperationException(message, ex);
						MshLog.LogCommandHealthEvent(command.Context, exception, Severity.Warning);
					}
				}
				flag = this.stopping;
				this.stopping = true;
			}
			return !flag;
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x0018AE04 File Offset: 0x00189004
		internal void ForgetFailure()
		{
			this.firstTerminatingError = null;
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06004B6A RID: 19306 RVA: 0x0018AE0D File Offset: 0x0018900D
		// (set) Token: 0x06004B6B RID: 19307 RVA: 0x0018AE15 File Offset: 0x00189015
		internal PipelineReader<object> ExternalInput
		{
			get
			{
				return this.externalInputPipe;
			}
			set
			{
				if (this.executionStarted)
				{
					throw PSTraceSource.NewInvalidOperationException(PipelineStrings.ExecutionAlreadyStarted, new object[0]);
				}
				this.externalInputPipe = value;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06004B6C RID: 19308 RVA: 0x0018AE37 File Offset: 0x00189037
		// (set) Token: 0x06004B6D RID: 19309 RVA: 0x0018AE3F File Offset: 0x0018903F
		internal PipelineWriter ExternalSuccessOutput
		{
			get
			{
				return this.externalSuccessOutput;
			}
			set
			{
				if (this.executionStarted)
				{
					throw PSTraceSource.NewInvalidOperationException(PipelineStrings.ExecutionAlreadyStarted, new object[0]);
				}
				this.externalSuccessOutput = value;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x0018AE61 File Offset: 0x00189061
		// (set) Token: 0x06004B6F RID: 19311 RVA: 0x0018AE69 File Offset: 0x00189069
		internal PipelineWriter ExternalErrorOutput
		{
			get
			{
				return this.externalErrorOutput;
			}
			set
			{
				if (this.executionStarted)
				{
					throw PSTraceSource.NewInvalidOperationException(PipelineStrings.ExecutionAlreadyStarted, new object[0]);
				}
				this.externalErrorOutput = value;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06004B70 RID: 19312 RVA: 0x0018AE8B File Offset: 0x0018908B
		internal bool ExecutionStarted
		{
			get
			{
				return this.executionStarted;
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06004B71 RID: 19313 RVA: 0x0018AE93 File Offset: 0x00189093
		internal bool Stopping
		{
			get
			{
				return this.localPipeline != null && this.localPipeline.IsStopping;
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06004B72 RID: 19314 RVA: 0x0018AEAA File Offset: 0x001890AA
		// (set) Token: 0x06004B73 RID: 19315 RVA: 0x0018AEB2 File Offset: 0x001890B2
		internal LocalPipeline LocalPipeline
		{
			get
			{
				return this.localPipeline;
			}
			set
			{
				this.localPipeline = value;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x0018AEBB File Offset: 0x001890BB
		// (set) Token: 0x06004B75 RID: 19317 RVA: 0x0018AEC3 File Offset: 0x001890C3
		internal bool TopLevel
		{
			get
			{
				return this.topLevel;
			}
			set
			{
				this.topLevel = value;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06004B76 RID: 19318 RVA: 0x0018AECC File Offset: 0x001890CC
		// (set) Token: 0x06004B77 RID: 19319 RVA: 0x0018AED4 File Offset: 0x001890D4
		internal SessionStateScope ExecutionScope
		{
			get
			{
				return this.executionScope;
			}
			set
			{
				this.executionScope = value;
			}
		}

		// Token: 0x0400245F RID: 9311
		private List<CommandProcessorBase> _commands = new List<CommandProcessorBase>();

		// Token: 0x04002460 RID: 9312
		private List<PipelineProcessor> _redirectionPipes;

		// Token: 0x04002461 RID: 9313
		private PipelineReader<object> externalInputPipe;

		// Token: 0x04002462 RID: 9314
		private PipelineWriter externalSuccessOutput;

		// Token: 0x04002463 RID: 9315
		private PipelineWriter externalErrorOutput;

		// Token: 0x04002464 RID: 9316
		private bool executionStarted;

		// Token: 0x04002465 RID: 9317
		private bool topLevel;

		// Token: 0x04002466 RID: 9318
		private bool stopping;

		// Token: 0x04002467 RID: 9319
		private SessionStateScope executionScope;

		// Token: 0x04002468 RID: 9320
		private Exception firstTerminatingError;

		// Token: 0x04002469 RID: 9321
		private bool _linkedSuccessOutput;

		// Token: 0x0400246A RID: 9322
		private bool _linkedErrorOutput;

		// Token: 0x0400246B RID: 9323
		internal SecurityContext SecurityContext = SecurityContext.Capture();

		// Token: 0x0400246C RID: 9324
		private bool disposed;

		// Token: 0x0400246D RID: 9325
		private bool executionFailed;

		// Token: 0x0400246E RID: 9326
		private bool terminatingErrorLogged;

		// Token: 0x0400246F RID: 9327
		private List<string> eventLogBuffer = new List<string>();

		// Token: 0x04002470 RID: 9328
		private object StopReasonLock = new object();

		// Token: 0x04002471 RID: 9329
		internal InternalCommand _permittedToWrite;

		// Token: 0x04002472 RID: 9330
		internal bool _permittedToWriteToPipeline;

		// Token: 0x04002473 RID: 9331
		internal Thread _permittedToWriteThread;

		// Token: 0x04002474 RID: 9332
		private LocalPipeline localPipeline;

		// Token: 0x02000760 RID: 1888
		internal enum PipelineExecutionStatus
		{
			// Token: 0x04002476 RID: 9334
			Started,
			// Token: 0x04002477 RID: 9335
			ParameterBinding,
			// Token: 0x04002478 RID: 9336
			Complete,
			// Token: 0x04002479 RID: 9337
			Error,
			// Token: 0x0400247A RID: 9338
			PipelineComplete
		}
	}
}
