using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x020001CB RID: 459
	internal class HelpErrorTracer
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x0008451E File Offset: 0x0008271E
		internal HelpSystem HelpSystem
		{
			get
			{
				return this._helpSystem;
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00084526 File Offset: 0x00082726
		internal HelpErrorTracer(HelpSystem helpSystem)
		{
			if (helpSystem == null)
			{
				throw PSTraceSource.NewArgumentNullException("HelpSystem");
			}
			this._helpSystem = helpSystem;
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00084550 File Offset: 0x00082750
		internal IDisposable Trace(string helpFile)
		{
			HelpErrorTracer.TraceFrame traceFrame = new HelpErrorTracer.TraceFrame(this, helpFile);
			this._traceFrames.Add(traceFrame);
			return traceFrame;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00084574 File Offset: 0x00082774
		internal void TraceError(ErrorRecord errorRecord)
		{
			if (this._traceFrames.Count <= 0)
			{
				return;
			}
			HelpErrorTracer.TraceFrame traceFrame = (HelpErrorTracer.TraceFrame)this._traceFrames[this._traceFrames.Count - 1];
			traceFrame.TraceError(errorRecord);
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x000845B8 File Offset: 0x000827B8
		internal void TraceErrors(Collection<ErrorRecord> errorRecords)
		{
			if (this._traceFrames.Count <= 0)
			{
				return;
			}
			HelpErrorTracer.TraceFrame traceFrame = (HelpErrorTracer.TraceFrame)this._traceFrames[this._traceFrames.Count - 1];
			traceFrame.TraceErrors(errorRecords);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000845FC File Offset: 0x000827FC
		internal void PopFrame(HelpErrorTracer.TraceFrame traceFrame)
		{
			if (this._traceFrames.Count <= 0)
			{
				return;
			}
			HelpErrorTracer.TraceFrame traceFrame2 = (HelpErrorTracer.TraceFrame)this._traceFrames[this._traceFrames.Count - 1];
			if (traceFrame2 == traceFrame)
			{
				this._traceFrames.RemoveAt(this._traceFrames.Count - 1);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x00084652 File Offset: 0x00082852
		internal bool IsOn
		{
			get
			{
				return this._traceFrames.Count > 0 && this.HelpSystem.VerboseHelpErrors;
			}
		}

		// Token: 0x040008F9 RID: 2297
		private HelpSystem _helpSystem;

		// Token: 0x040008FA RID: 2298
		private ArrayList _traceFrames = new ArrayList();

		// Token: 0x020001CC RID: 460
		internal sealed class TraceFrame : IDisposable
		{
			// Token: 0x06001532 RID: 5426 RVA: 0x0008466F File Offset: 0x0008286F
			internal TraceFrame(HelpErrorTracer helpTracer, string helpFile)
			{
				this._helpTracer = helpTracer;
				this._helpFile = helpFile;
			}

			// Token: 0x06001533 RID: 5427 RVA: 0x0008469B File Offset: 0x0008289B
			internal void TraceError(ErrorRecord errorRecord)
			{
				if (this._helpTracer.HelpSystem.VerboseHelpErrors)
				{
					this._errors.Add(errorRecord);
				}
			}

			// Token: 0x06001534 RID: 5428 RVA: 0x000846BC File Offset: 0x000828BC
			internal void TraceErrors(Collection<ErrorRecord> errorRecords)
			{
				if (this._helpTracer.HelpSystem.VerboseHelpErrors)
				{
					foreach (ErrorRecord item in errorRecords)
					{
						this._errors.Add(item);
					}
				}
			}

			// Token: 0x06001535 RID: 5429 RVA: 0x0008471C File Offset: 0x0008291C
			public void Dispose()
			{
				if (this._helpTracer.HelpSystem.VerboseHelpErrors && this._errors.Count > 0)
				{
					ErrorRecord errorRecord = new ErrorRecord(new ParentContainsErrorRecordException("Help Load Error"), "HelpLoadError", ErrorCategory.SyntaxError, null);
					errorRecord.ErrorDetails = new ErrorDetails(typeof(HelpErrorTracer).GetTypeInfo().Assembly, "HelpErrors", "HelpLoadError", new object[]
					{
						this._helpFile,
						this._errors.Count
					});
					this._helpTracer.HelpSystem.LastErrors.Add(errorRecord);
					foreach (ErrorRecord item in this._errors)
					{
						this._helpTracer.HelpSystem.LastErrors.Add(item);
					}
				}
				this._helpTracer.PopFrame(this);
			}

			// Token: 0x040008FB RID: 2299
			private string _helpFile = "";

			// Token: 0x040008FC RID: 2300
			private Collection<ErrorRecord> _errors = new Collection<ErrorRecord>();

			// Token: 0x040008FD RID: 2301
			private HelpErrorTracer _helpTracer;
		}
	}
}
