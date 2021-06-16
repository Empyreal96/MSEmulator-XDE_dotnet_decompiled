using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200062C RID: 1580
	internal class FileRedirection : CommandRedirection, IDisposable
	{
		// Token: 0x060044A2 RID: 17570 RVA: 0x0016F6BD File Offset: 0x0016D8BD
		internal FileRedirection(RedirectionStream from, bool appending, string file) : base(from)
		{
			this.File = file;
			this.Appending = appending;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0016F6D4 File Offset: 0x0016D8D4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}> {1}", new object[]
			{
				(base.FromStream == RedirectionStream.All) ? "*" : ((int)base.FromStream).ToString(CultureInfo.InvariantCulture),
				this.File
			});
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x0016F726 File Offset: 0x0016D926
		// (set) Token: 0x060044A5 RID: 17573 RVA: 0x0016F72E File Offset: 0x0016D92E
		internal string File { get; private set; }

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x0016F737 File Offset: 0x0016D937
		// (set) Token: 0x060044A7 RID: 17575 RVA: 0x0016F73F File Offset: 0x0016D93F
		internal bool Appending { get; private set; }

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0016F748 File Offset: 0x0016D948
		// (set) Token: 0x060044A9 RID: 17577 RVA: 0x0016F750 File Offset: 0x0016D950
		private PipelineProcessor PipelineProcessor { get; set; }

		// Token: 0x060044AA RID: 17578 RVA: 0x0016F75C File Offset: 0x0016D95C
		internal override void Bind(PipelineProcessor pipelineProcessor, CommandProcessorBase commandProcessor, ExecutionContext context)
		{
			Pipe redirectionPipe = this.GetRedirectionPipe(context, pipelineProcessor);
			switch (base.FromStream)
			{
			case RedirectionStream.All:
				if (context.CurrentCommandProcessor != null)
				{
					context.CurrentCommandProcessor.CommandRuntime.OutputPipe.SetVariableListForTemporaryPipe(redirectionPipe);
				}
				commandProcessor.CommandRuntime.OutputPipe = redirectionPipe;
				commandProcessor.CommandRuntime.ErrorOutputPipe = redirectionPipe;
				commandProcessor.CommandRuntime.WarningOutputPipe = redirectionPipe;
				commandProcessor.CommandRuntime.VerboseOutputPipe = redirectionPipe;
				commandProcessor.CommandRuntime.DebugOutputPipe = redirectionPipe;
				commandProcessor.CommandRuntime.InformationOutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Output:
				if (context.CurrentCommandProcessor != null)
				{
					context.CurrentCommandProcessor.CommandRuntime.OutputPipe.SetVariableListForTemporaryPipe(redirectionPipe);
				}
				commandProcessor.CommandRuntime.OutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Error:
				commandProcessor.CommandRuntime.ErrorOutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Warning:
				commandProcessor.CommandRuntime.WarningOutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Verbose:
				commandProcessor.CommandRuntime.VerboseOutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Debug:
				commandProcessor.CommandRuntime.DebugOutputPipe = redirectionPipe;
				return;
			case RedirectionStream.Information:
				commandProcessor.CommandRuntime.InformationOutputPipe = redirectionPipe;
				return;
			default:
				return;
			}
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0016F870 File Offset: 0x0016DA70
		internal Pipe[] BindForExpression(FunctionContext funcContext)
		{
			ExecutionContext executionContext = funcContext._executionContext;
			Pipe redirectionPipe = this.GetRedirectionPipe(executionContext, null);
			Pipe[] array = new Pipe[7];
			switch (base.FromStream)
			{
			case RedirectionStream.All:
				array[1] = funcContext._outputPipe;
				array[2] = executionContext.ShellFunctionErrorOutputPipe;
				array[3] = executionContext.ExpressionWarningOutputPipe;
				array[4] = executionContext.ExpressionVerboseOutputPipe;
				array[5] = executionContext.ExpressionDebugOutputPipe;
				array[6] = executionContext.ExpressionInformationOutputPipe;
				funcContext._outputPipe.SetVariableListForTemporaryPipe(redirectionPipe);
				funcContext._outputPipe = redirectionPipe;
				executionContext.ShellFunctionErrorOutputPipe = redirectionPipe;
				executionContext.ExpressionWarningOutputPipe = redirectionPipe;
				executionContext.ExpressionVerboseOutputPipe = redirectionPipe;
				executionContext.ExpressionDebugOutputPipe = redirectionPipe;
				executionContext.ExpressionInformationOutputPipe = redirectionPipe;
				break;
			case RedirectionStream.Output:
				array[1] = funcContext._outputPipe;
				funcContext._outputPipe.SetVariableListForTemporaryPipe(redirectionPipe);
				funcContext._outputPipe = redirectionPipe;
				break;
			case RedirectionStream.Error:
				array[(int)base.FromStream] = executionContext.ShellFunctionErrorOutputPipe;
				executionContext.ShellFunctionErrorOutputPipe = redirectionPipe;
				break;
			case RedirectionStream.Warning:
				array[(int)base.FromStream] = executionContext.ExpressionWarningOutputPipe;
				executionContext.ExpressionWarningOutputPipe = redirectionPipe;
				break;
			case RedirectionStream.Verbose:
				array[(int)base.FromStream] = executionContext.ExpressionVerboseOutputPipe;
				executionContext.ExpressionVerboseOutputPipe = redirectionPipe;
				break;
			case RedirectionStream.Debug:
				array[(int)base.FromStream] = executionContext.ExpressionDebugOutputPipe;
				executionContext.ExpressionDebugOutputPipe = redirectionPipe;
				break;
			case RedirectionStream.Information:
				array[(int)base.FromStream] = executionContext.ExpressionInformationOutputPipe;
				executionContext.ExpressionInformationOutputPipe = redirectionPipe;
				break;
			}
			return array;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0016F9C4 File Offset: 0x0016DBC4
		internal Pipe GetRedirectionPipe(ExecutionContext context, PipelineProcessor parentPipelineProcessor)
		{
			if (string.IsNullOrWhiteSpace(this.File))
			{
				return new Pipe
				{
					NullPipe = true
				};
			}
			CommandProcessorBase commandProcessorBase = context.CreateCommand("out-file", false);
			CommandParameterInternal parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, "Encoding", "-Encoding:", PositionUtilities.EmptyExtent, "Unicode", false, false);
			commandProcessorBase.AddParameter(parameter);
			if (this.Appending)
			{
				parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, "Append", "-Append:", PositionUtilities.EmptyExtent, true, false, false);
				commandProcessorBase.AddParameter(parameter);
			}
			parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, "Filepath", "-Filepath:", PositionUtilities.EmptyExtent, this.File, false, false);
			commandProcessorBase.AddParameter(parameter);
			this.PipelineProcessor = new PipelineProcessor();
			this.PipelineProcessor.Add(commandProcessorBase);
			try
			{
				this.PipelineProcessor.StartStepping(true);
			}
			catch (RuntimeException ex)
			{
				if (ex.ErrorRecord.Exception is ArgumentException)
				{
					throw InterpreterError.NewInterpreterExceptionWithInnerException(null, typeof(RuntimeException), null, "RedirectionFailed", ParserStrings.RedirectionFailed, ex.ErrorRecord.Exception, new object[]
					{
						this.File,
						ex.ErrorRecord.Exception.Message
					});
				}
				throw;
			}
			if (parentPipelineProcessor != null)
			{
				parentPipelineProcessor.AddRedirectionPipe(this.PipelineProcessor);
			}
			return new Pipe(context, this.PipelineProcessor);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0016FB34 File Offset: 0x0016DD34
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0016FB43 File Offset: 0x0016DD43
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && this.PipelineProcessor != null)
			{
				this.PipelineProcessor.Dispose();
			}
			this._disposed = true;
		}

		// Token: 0x04002215 RID: 8725
		private bool _disposed;
	}
}
