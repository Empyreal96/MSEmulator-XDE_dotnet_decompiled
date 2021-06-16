using System;
using System.Management.Automation.Tracing;

namespace System.Management.Automation
{
	// Token: 0x02000289 RID: 649
	public sealed class PSChildJobProxy : Job2
	{
		// Token: 0x06001F0F RID: 7951 RVA: 0x000B447C File Offset: 0x000B267C
		internal PSChildJobProxy(string command, PSObject o) : base(command)
		{
			PSJobProxy.TryGetJobPropertyValue<string>(o, "StatusMessage", out this._statusMessage);
			PSJobProxy.TryGetJobPropertyValue<string>(o, "Location", out this._location);
			string name;
			PSJobProxy.TryGetJobPropertyValue<string>(o, "Name", out name);
			base.Name = name;
			base.Output.DataAdded += this.OutputAdded;
			base.Error.DataAdded += this.ErrorAdded;
			base.Warning.DataAdded += this.WarningAdded;
			base.Verbose.DataAdded += this.VerboseAdded;
			base.Progress.DataAdded += this.ProgressAdded;
			base.Debug.DataAdded += this.DebugAdded;
			base.Information.DataAdded += this.InformationAdded;
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x000B4580 File Offset: 0x000B2780
		internal void AssignDisconnectedState()
		{
			this.DoSetJobState(JobState.Disconnected, null);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x000B458C File Offset: 0x000B278C
		private void OnJobDataAdded(JobDataAddedEventArgs eventArgs)
		{
			try
			{
				this._tracer.WriteMessage("PSChildJobProxy", "OnJobDataAdded", Guid.Empty, this, "BEGIN call event handlers", new string[0]);
				this.JobDataAdded.SafeInvoke(this, eventArgs);
				this._tracer.WriteMessage("PSChildJobProxy", "OnJobDataAdded", Guid.Empty, this, "END call event handlers", new string[0]);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.WriteMessage("PSChildJobProxy", "OnJobDataAdded", Guid.Empty, this, "END Exception thrown in JobDataAdded handler", new string[0]);
				this._tracer.TraceException(ex);
			}
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000B4640 File Offset: 0x000B2840
		private void OutputAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Output, e.Index));
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x000B4655 File Offset: 0x000B2855
		private void ErrorAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Error, e.Index));
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x000B466A File Offset: 0x000B286A
		private void WarningAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Warning, e.Index));
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x000B467F File Offset: 0x000B287F
		private void VerboseAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Verbose, e.Index));
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000B4694 File Offset: 0x000B2894
		private void ProgressAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Progress, e.Index));
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000B46A9 File Offset: 0x000B28A9
		private void DebugAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Debug, e.Index));
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000B46BE File Offset: 0x000B28BE
		private void InformationAdded(object sender, DataAddedEventArgs e)
		{
			this.OnJobDataAdded(new JobDataAddedEventArgs(this, PowerShellStreamType.Information, e.Index));
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000B46D4 File Offset: 0x000B28D4
		internal void DoSetJobState(JobState state, Exception reason = null)
		{
			if (this._disposed)
			{
				return;
			}
			try
			{
				this._tracer.WriteMessage("PSChildJobProxy", "DoSetJobState", Guid.Empty, this, "BEGIN Set job state to {0} and call event handlers", new string[]
				{
					state.ToString()
				});
				PSChildJobProxy.StructuredTracer.BeginProxyChildJobEventHandler(base.InstanceId);
				base.SetJobState(state, reason);
				PSChildJobProxy.StructuredTracer.EndProxyJobEventHandler(base.InstanceId);
				this._tracer.WriteMessage("PSChildJobProxy", "DoSetJobState", Guid.Empty, this, "END Set job state to {0} and call event handlers", new string[]
				{
					state.ToString()
				});
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000B4794 File Offset: 0x000B2994
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			lock (this._syncObject)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				base.Output.DataAdded -= this.OutputAdded;
				base.Error.DataAdded -= this.ErrorAdded;
				base.Warning.DataAdded -= this.WarningAdded;
				base.Verbose.DataAdded -= this.VerboseAdded;
				base.Progress.DataAdded -= this.ProgressAdded;
				base.Debug.DataAdded -= this.DebugAdded;
				base.Information.DataAdded -= this.InformationAdded;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000B4894 File Offset: 0x000B2A94
		public override void StartJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000B48A6 File Offset: 0x000B2AA6
		public override void StartJobAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000B48B8 File Offset: 0x000B2AB8
		public override void StopJob(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000B48CA File Offset: 0x000B2ACA
		public override void StopJobAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000B48DC File Offset: 0x000B2ADC
		public override void StopJobAsync(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000B48EE File Offset: 0x000B2AEE
		public override void SuspendJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000B4900 File Offset: 0x000B2B00
		public override void SuspendJobAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000B4912 File Offset: 0x000B2B12
		public override void SuspendJob(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000B4924 File Offset: 0x000B2B24
		public override void SuspendJobAsync(bool force, string reason)
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000B4936 File Offset: 0x000B2B36
		public override void ResumeJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000B4948 File Offset: 0x000B2B48
		public override void ResumeJobAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000B495A File Offset: 0x000B2B5A
		public override void UnblockJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000B496C File Offset: 0x000B2B6C
		public override void UnblockJobAsync()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000B497E File Offset: 0x000B2B7E
		public override void StopJob()
		{
			throw PSTraceSource.NewNotSupportedException(PowerShellStrings.ProxyChildJobControlNotSupported, new object[0]);
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001F29 RID: 7977 RVA: 0x000B4990 File Offset: 0x000B2B90
		// (remove) Token: 0x06001F2A RID: 7978 RVA: 0x000B49C8 File Offset: 0x000B2BC8
		public event EventHandler<JobDataAddedEventArgs> JobDataAdded;

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000B49FD File Offset: 0x000B2BFD
		public override string StatusMessage
		{
			get
			{
				return this._statusMessage;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000B4A08 File Offset: 0x000B2C08
		public override bool HasMoreData
		{
			get
			{
				return base.Output.IsOpen || base.Output.Count > 0 || base.Error.IsOpen || base.Error.Count > 0 || base.Verbose.IsOpen || base.Verbose.Count > 0 || base.Debug.IsOpen || base.Debug.Count > 0 || base.Warning.IsOpen || base.Warning.Count > 0 || base.Progress.IsOpen || base.Progress.Count > 0 || base.Information.IsOpen || base.Information.Count > 0;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x000B4AE0 File Offset: 0x000B2CE0
		public override string Location
		{
			get
			{
				return this._location;
			}
		}

		// Token: 0x04000DAC RID: 3500
		private const string ClassNameTrace = "PSChildJobProxy";

		// Token: 0x04000DAD RID: 3501
		private static Tracer StructuredTracer = new Tracer();

		// Token: 0x04000DAF RID: 3503
		private string _statusMessage;

		// Token: 0x04000DB0 RID: 3504
		private string _location;

		// Token: 0x04000DB1 RID: 3505
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04000DB2 RID: 3506
		private readonly object _syncObject = new object();

		// Token: 0x04000DB3 RID: 3507
		private bool _disposed;
	}
}
