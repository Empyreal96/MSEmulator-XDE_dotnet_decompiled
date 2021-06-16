using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200062B RID: 1579
	internal class MergingRedirection : CommandRedirection
	{
		// Token: 0x0600449E RID: 17566 RVA: 0x0016F465 File Offset: 0x0016D665
		internal MergingRedirection(RedirectionStream from, RedirectionStream to) : base(from)
		{
			if (to != RedirectionStream.Output)
			{
				throw InterpreterError.NewInterpreterException(to, typeof(ArgumentException), null, "RedirectionStreamCanOnlyMergeToOutputStream", ParserStrings.RedirectionStreamCanOnlyMergeToOutputStream, new object[0]);
			}
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0016F49C File Offset: 0x0016D69C
		public override string ToString()
		{
			if (base.FromStream != RedirectionStream.All)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}>&1", new object[]
				{
					(int)base.FromStream
				});
			}
			return "*>&1";
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0016F4DC File Offset: 0x0016D6DC
		internal override void Bind(PipelineProcessor pipelineProcessor, CommandProcessorBase commandProcessor, ExecutionContext context)
		{
			Pipe outputPipe = commandProcessor.CommandRuntime.OutputPipe;
			switch (base.FromStream)
			{
			case RedirectionStream.All:
				commandProcessor.CommandRuntime.ErrorMergeTo = MshCommandRuntime.MergeDataStream.Output;
				commandProcessor.CommandRuntime.WarningOutputPipe = outputPipe;
				commandProcessor.CommandRuntime.VerboseOutputPipe = outputPipe;
				commandProcessor.CommandRuntime.DebugOutputPipe = outputPipe;
				commandProcessor.CommandRuntime.InformationOutputPipe = outputPipe;
				return;
			case RedirectionStream.Output:
				break;
			case RedirectionStream.Error:
				commandProcessor.CommandRuntime.ErrorMergeTo = MshCommandRuntime.MergeDataStream.Output;
				return;
			case RedirectionStream.Warning:
				commandProcessor.CommandRuntime.WarningOutputPipe = outputPipe;
				return;
			case RedirectionStream.Verbose:
				commandProcessor.CommandRuntime.VerboseOutputPipe = outputPipe;
				return;
			case RedirectionStream.Debug:
				commandProcessor.CommandRuntime.DebugOutputPipe = outputPipe;
				return;
			case RedirectionStream.Information:
				commandProcessor.CommandRuntime.InformationOutputPipe = outputPipe;
				break;
			default:
				return;
			}
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x0016F59C File Offset: 0x0016D79C
		internal Pipe[] BindForExpression(ExecutionContext context, FunctionContext funcContext)
		{
			Pipe[] array = new Pipe[7];
			Pipe outputPipe = funcContext._outputPipe;
			switch (base.FromStream)
			{
			case RedirectionStream.All:
				array[1] = funcContext._outputPipe;
				array[2] = context.ShellFunctionErrorOutputPipe;
				context.ShellFunctionErrorOutputPipe = outputPipe;
				array[3] = context.ExpressionWarningOutputPipe;
				context.ExpressionWarningOutputPipe = outputPipe;
				array[4] = context.ExpressionVerboseOutputPipe;
				context.ExpressionVerboseOutputPipe = outputPipe;
				array[5] = context.ExpressionDebugOutputPipe;
				context.ExpressionDebugOutputPipe = outputPipe;
				array[6] = context.ExpressionInformationOutputPipe;
				context.ExpressionInformationOutputPipe = outputPipe;
				break;
			case RedirectionStream.Output:
				array[1] = funcContext._outputPipe;
				break;
			case RedirectionStream.Error:
				array[(int)base.FromStream] = context.ShellFunctionErrorOutputPipe;
				context.ShellFunctionErrorOutputPipe = outputPipe;
				break;
			case RedirectionStream.Warning:
				array[(int)base.FromStream] = context.ExpressionWarningOutputPipe;
				context.ExpressionWarningOutputPipe = outputPipe;
				break;
			case RedirectionStream.Verbose:
				array[(int)base.FromStream] = context.ExpressionVerboseOutputPipe;
				context.ExpressionVerboseOutputPipe = outputPipe;
				break;
			case RedirectionStream.Debug:
				array[(int)base.FromStream] = context.ExpressionDebugOutputPipe;
				context.ExpressionDebugOutputPipe = outputPipe;
				break;
			case RedirectionStream.Information:
				array[(int)base.FromStream] = context.ExpressionInformationOutputPipe;
				context.ExpressionInformationOutputPipe = outputPipe;
				break;
			}
			return array;
		}
	}
}
