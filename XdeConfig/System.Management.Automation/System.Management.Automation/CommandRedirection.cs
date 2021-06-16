using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200062A RID: 1578
	internal abstract class CommandRedirection
	{
		// Token: 0x06004499 RID: 17561 RVA: 0x0016F375 File Offset: 0x0016D575
		protected CommandRedirection(RedirectionStream from)
		{
			this.FromStream = from;
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x0016F384 File Offset: 0x0016D584
		// (set) Token: 0x0600449B RID: 17563 RVA: 0x0016F38C File Offset: 0x0016D58C
		internal RedirectionStream FromStream { get; private set; }

		// Token: 0x0600449C RID: 17564
		internal abstract void Bind(PipelineProcessor pipelineProcessor, CommandProcessorBase commandProcessor, ExecutionContext context);

		// Token: 0x0600449D RID: 17565 RVA: 0x0016F398 File Offset: 0x0016D598
		internal void UnbindForExpression(FunctionContext funcContext, Pipe[] pipes)
		{
			if (pipes == null)
			{
				return;
			}
			ExecutionContext executionContext = funcContext._executionContext;
			switch (this.FromStream)
			{
			case RedirectionStream.All:
				funcContext._outputPipe = pipes[1];
				executionContext.ShellFunctionErrorOutputPipe = pipes[2];
				executionContext.ExpressionWarningOutputPipe = pipes[3];
				executionContext.ExpressionVerboseOutputPipe = pipes[4];
				executionContext.ExpressionDebugOutputPipe = pipes[5];
				executionContext.ExpressionInformationOutputPipe = pipes[6];
				return;
			case RedirectionStream.Output:
				funcContext._outputPipe = pipes[1];
				return;
			case RedirectionStream.Error:
				executionContext.ShellFunctionErrorOutputPipe = pipes[(int)this.FromStream];
				return;
			case RedirectionStream.Warning:
				executionContext.ExpressionWarningOutputPipe = pipes[(int)this.FromStream];
				return;
			case RedirectionStream.Verbose:
				executionContext.ExpressionVerboseOutputPipe = pipes[(int)this.FromStream];
				return;
			case RedirectionStream.Debug:
				executionContext.ExpressionDebugOutputPipe = pipes[(int)this.FromStream];
				return;
			case RedirectionStream.Information:
				executionContext.ExpressionInformationOutputPipe = pipes[(int)this.FromStream];
				return;
			default:
				return;
			}
		}
	}
}
