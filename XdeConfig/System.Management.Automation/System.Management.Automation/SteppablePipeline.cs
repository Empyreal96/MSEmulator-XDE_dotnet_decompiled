using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000490 RID: 1168
	public sealed class SteppablePipeline : IDisposable
	{
		// Token: 0x0600341F RID: 13343 RVA: 0x0011C8F1 File Offset: 0x0011AAF1
		internal SteppablePipeline(ExecutionContext context, PipelineProcessor pipeline)
		{
			if (pipeline == null)
			{
				throw new ArgumentNullException("pipeline");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this._pipeline = pipeline;
			this._context = context;
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x0011C923 File Offset: 0x0011AB23
		public void Begin(bool expectInput)
		{
			this.Begin(expectInput, null);
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x0011C930 File Offset: 0x0011AB30
		public void Begin(bool expectInput, EngineIntrinsics contextToRedirectTo)
		{
			if (contextToRedirectTo == null)
			{
				throw new ArgumentNullException("contextToRedirectTo");
			}
			ExecutionContext executionContext = contextToRedirectTo.SessionState.Internal.ExecutionContext;
			CommandProcessorBase currentCommandProcessor = executionContext.CurrentCommandProcessor;
			ICommandRuntime commandRuntime = (currentCommandProcessor == null) ? null : currentCommandProcessor.CommandRuntime;
			this.Begin(expectInput, commandRuntime);
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0011C978 File Offset: 0x0011AB78
		public void Begin(InternalCommand command)
		{
			if (command == null || command.MyInvocation == null)
			{
				throw new ArgumentNullException("command");
			}
			this.Begin(command.MyInvocation.ExpectingInput, command.commandRuntime);
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x0011C9A8 File Offset: 0x0011ABA8
		private void Begin(bool expectInput, ICommandRuntime commandRuntime)
		{
			try
			{
				this._pipeline.ExecutionScope = this._context.EngineSessionState.CurrentScope;
				this._context.PushPipelineProcessor(this._pipeline);
				this._expectInput = expectInput;
				MshCommandRuntime mshCommandRuntime = commandRuntime as MshCommandRuntime;
				if (mshCommandRuntime != null)
				{
					if (mshCommandRuntime.OutputPipe != null)
					{
						this._pipeline.LinkPipelineSuccessOutput(mshCommandRuntime.OutputPipe);
					}
					if (mshCommandRuntime.ErrorOutputPipe != null)
					{
						this._pipeline.LinkPipelineErrorOutput(mshCommandRuntime.ErrorOutputPipe);
					}
				}
				this._pipeline.StartStepping(this._expectInput);
			}
			finally
			{
				this._context.PopPipelineProcessor(true);
			}
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x0011CA54 File Offset: 0x0011AC54
		public Array Process(object input)
		{
			Array result;
			try
			{
				this._context.PushPipelineProcessor(this._pipeline);
				if (this._expectInput)
				{
					result = this._pipeline.Step(input);
				}
				else
				{
					result = this._pipeline.Step(AutomationNull.Value);
				}
			}
			finally
			{
				this._context.PopPipelineProcessor(true);
			}
			return result;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0011CABC File Offset: 0x0011ACBC
		public Array Process(PSObject input)
		{
			Array result;
			try
			{
				this._context.PushPipelineProcessor(this._pipeline);
				if (this._expectInput)
				{
					result = this._pipeline.Step(input);
				}
				else
				{
					result = this._pipeline.Step(AutomationNull.Value);
				}
			}
			finally
			{
				this._context.PopPipelineProcessor(true);
			}
			return result;
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x0011CB24 File Offset: 0x0011AD24
		public Array Process()
		{
			Array result;
			try
			{
				this._context.PushPipelineProcessor(this._pipeline);
				result = this._pipeline.Step(AutomationNull.Value);
			}
			finally
			{
				this._context.PopPipelineProcessor(true);
			}
			return result;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x0011CB74 File Offset: 0x0011AD74
		public Array End()
		{
			Array result;
			try
			{
				this._context.PushPipelineProcessor(this._pipeline);
				result = this._pipeline.DoComplete();
			}
			finally
			{
				this._context.PopPipelineProcessor(true);
				this._pipeline.Dispose();
			}
			return result;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x0011CBCC File Offset: 0x0011ADCC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x0011CBDB File Offset: 0x0011ADDB
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._pipeline.Dispose();
			}
			this._disposed = true;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x0011CBFC File Offset: 0x0011ADFC
		~SteppablePipeline()
		{
			this.Dispose(false);
		}

		// Token: 0x04001AD4 RID: 6868
		private PipelineProcessor _pipeline;

		// Token: 0x04001AD5 RID: 6869
		private ExecutionContext _context;

		// Token: 0x04001AD6 RID: 6870
		private bool _expectInput;

		// Token: 0x04001AD7 RID: 6871
		private bool _disposed;
	}
}
