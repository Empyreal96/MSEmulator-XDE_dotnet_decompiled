using System;

namespace System.Management.Automation
{
	// Token: 0x0200028C RID: 652
	public sealed class PowerShellStreams<TInput, TOutput> : IDisposable
	{
		// Token: 0x06001F33 RID: 7987 RVA: 0x000B4B2C File Offset: 0x000B2D2C
		public PowerShellStreams()
		{
			this.inputStream = null;
			this.outputStream = null;
			this.errorStream = null;
			this.warningStream = null;
			this.progressStream = null;
			this.verboseStream = null;
			this.debugStream = null;
			this.informationStream = null;
			this.disposed = false;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000B4B8C File Offset: 0x000B2D8C
		public PowerShellStreams(PSDataCollection<TInput> pipelineInput)
		{
			if (pipelineInput == null)
			{
				this.inputStream = new PSDataCollection<TInput>();
			}
			else
			{
				this.inputStream = pipelineInput;
			}
			this.inputStream.Complete();
			this.outputStream = new PSDataCollection<TOutput>();
			this.errorStream = new PSDataCollection<ErrorRecord>();
			this.warningStream = new PSDataCollection<WarningRecord>();
			this.progressStream = new PSDataCollection<ProgressRecord>();
			this.verboseStream = new PSDataCollection<VerboseRecord>();
			this.debugStream = new PSDataCollection<DebugRecord>();
			this.informationStream = new PSDataCollection<InformationRecord>();
			this.disposed = false;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000B4C20 File Offset: 0x000B2E20
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000B4C30 File Offset: 0x000B2E30
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			lock (this.syncLock)
			{
				if (!this.disposed)
				{
					if (disposing)
					{
						this.inputStream.Dispose();
						this.outputStream.Dispose();
						this.errorStream.Dispose();
						this.warningStream.Dispose();
						this.progressStream.Dispose();
						this.verboseStream.Dispose();
						this.debugStream.Dispose();
						this.informationStream.Dispose();
						this.inputStream = null;
						this.outputStream = null;
						this.errorStream = null;
						this.warningStream = null;
						this.progressStream = null;
						this.verboseStream = null;
						this.debugStream = null;
						this.informationStream = null;
					}
					this.disposed = true;
				}
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x000B4D1C File Offset: 0x000B2F1C
		// (set) Token: 0x06001F38 RID: 7992 RVA: 0x000B4D24 File Offset: 0x000B2F24
		public PSDataCollection<TInput> InputStream
		{
			get
			{
				return this.inputStream;
			}
			set
			{
				this.inputStream = value;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x000B4D2D File Offset: 0x000B2F2D
		// (set) Token: 0x06001F3A RID: 7994 RVA: 0x000B4D35 File Offset: 0x000B2F35
		public PSDataCollection<TOutput> OutputStream
		{
			get
			{
				return this.outputStream;
			}
			set
			{
				this.outputStream = value;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000B4D3E File Offset: 0x000B2F3E
		// (set) Token: 0x06001F3C RID: 7996 RVA: 0x000B4D46 File Offset: 0x000B2F46
		public PSDataCollection<ErrorRecord> ErrorStream
		{
			get
			{
				return this.errorStream;
			}
			set
			{
				this.errorStream = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000B4D4F File Offset: 0x000B2F4F
		// (set) Token: 0x06001F3E RID: 7998 RVA: 0x000B4D57 File Offset: 0x000B2F57
		public PSDataCollection<WarningRecord> WarningStream
		{
			get
			{
				return this.warningStream;
			}
			set
			{
				this.warningStream = value;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000B4D60 File Offset: 0x000B2F60
		// (set) Token: 0x06001F40 RID: 8000 RVA: 0x000B4D68 File Offset: 0x000B2F68
		public PSDataCollection<ProgressRecord> ProgressStream
		{
			get
			{
				return this.progressStream;
			}
			set
			{
				this.progressStream = value;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000B4D71 File Offset: 0x000B2F71
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x000B4D79 File Offset: 0x000B2F79
		public PSDataCollection<VerboseRecord> VerboseStream
		{
			get
			{
				return this.verboseStream;
			}
			set
			{
				this.verboseStream = value;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x000B4D82 File Offset: 0x000B2F82
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x000B4D8A File Offset: 0x000B2F8A
		public PSDataCollection<DebugRecord> DebugStream
		{
			get
			{
				return this.debugStream;
			}
			set
			{
				this.debugStream = value;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000B4D93 File Offset: 0x000B2F93
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000B4D9B File Offset: 0x000B2F9B
		public PSDataCollection<InformationRecord> InformationStream
		{
			get
			{
				return this.informationStream;
			}
			set
			{
				this.informationStream = value;
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000B4DA4 File Offset: 0x000B2FA4
		public void CloseAll()
		{
			if (!this.disposed)
			{
				lock (this.syncLock)
				{
					if (!this.disposed)
					{
						this.outputStream.Complete();
						this.errorStream.Complete();
						this.warningStream.Complete();
						this.progressStream.Complete();
						this.verboseStream.Complete();
						this.debugStream.Complete();
						this.informationStream.Complete();
					}
				}
			}
		}

		// Token: 0x04000DC0 RID: 3520
		private PSDataCollection<TInput> inputStream;

		// Token: 0x04000DC1 RID: 3521
		private PSDataCollection<TOutput> outputStream;

		// Token: 0x04000DC2 RID: 3522
		private PSDataCollection<ErrorRecord> errorStream;

		// Token: 0x04000DC3 RID: 3523
		private PSDataCollection<WarningRecord> warningStream;

		// Token: 0x04000DC4 RID: 3524
		private PSDataCollection<ProgressRecord> progressStream;

		// Token: 0x04000DC5 RID: 3525
		private PSDataCollection<VerboseRecord> verboseStream;

		// Token: 0x04000DC6 RID: 3526
		private PSDataCollection<DebugRecord> debugStream;

		// Token: 0x04000DC7 RID: 3527
		private PSDataCollection<InformationRecord> informationStream;

		// Token: 0x04000DC8 RID: 3528
		private bool disposed;

		// Token: 0x04000DC9 RID: 3529
		private readonly object syncLock = new object();
	}
}
