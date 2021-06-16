using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;

namespace System.Management.Automation
{
	// Token: 0x0200027D RID: 637
	public abstract class Job2 : Job
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000ACC80 File Offset: 0x000AAE80
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x000ACCDC File Offset: 0x000AAEDC
		public List<CommandParameterCollection> StartParameters
		{
			get
			{
				if (this._parameters == null)
				{
					lock (this._syncobject)
					{
						if (this._parameters == null)
						{
							this._parameters = new List<CommandParameterCollection>();
						}
					}
				}
				return this._parameters;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				lock (this._syncobject)
				{
					this._parameters = value;
				}
			}
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x000ACD2C File Offset: 0x000AAF2C
		protected object SyncRoot
		{
			get
			{
				return this.syncObject;
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000ACD34 File Offset: 0x000AAF34
		protected Job2()
		{
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000ACD52 File Offset: 0x000AAF52
		protected Job2(string command) : base(command)
		{
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000ACD71 File Offset: 0x000AAF71
		protected Job2(string command, string name) : base(command, name)
		{
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000ACD91 File Offset: 0x000AAF91
		protected Job2(string command, string name, IList<Job> childJobs) : base(command, name, childJobs)
		{
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000ACDB2 File Offset: 0x000AAFB2
		protected Job2(string command, string name, JobIdentifier token) : base(command, name, token)
		{
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000ACDD3 File Offset: 0x000AAFD3
		protected Job2(string command, string name, Guid instanceId) : base(command, name, instanceId)
		{
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000ACDF4 File Offset: 0x000AAFF4
		protected new void SetJobState(JobState state, Exception reason)
		{
			base.SetJobState(state, reason);
		}

		// Token: 0x06001E14 RID: 7700
		public abstract void StartJob();

		// Token: 0x06001E15 RID: 7701
		public abstract void StartJobAsync();

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06001E16 RID: 7702 RVA: 0x000ACE00 File Offset: 0x000AB000
		// (remove) Token: 0x06001E17 RID: 7703 RVA: 0x000ACE38 File Offset: 0x000AB038
		public event EventHandler<AsyncCompletedEventArgs> StartJobCompleted;

		// Token: 0x06001E18 RID: 7704 RVA: 0x000ACE6D File Offset: 0x000AB06D
		protected virtual void OnStartJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			this.RaiseCompletedHandler(1, eventArgs);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000ACE77 File Offset: 0x000AB077
		protected virtual void OnStopJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			this.RaiseCompletedHandler(2, eventArgs);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000ACE81 File Offset: 0x000AB081
		protected virtual void OnSuspendJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			this.RaiseCompletedHandler(3, eventArgs);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000ACE8B File Offset: 0x000AB08B
		protected virtual void OnResumeJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			this.RaiseCompletedHandler(4, eventArgs);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000ACE95 File Offset: 0x000AB095
		protected virtual void OnUnblockJobCompleted(AsyncCompletedEventArgs eventArgs)
		{
			this.RaiseCompletedHandler(5, eventArgs);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000ACEA0 File Offset: 0x000AB0A0
		private void RaiseCompletedHandler(int operation, AsyncCompletedEventArgs eventArgs)
		{
			EventHandler<AsyncCompletedEventArgs> eventHandler = null;
			switch (operation)
			{
			case 1:
				eventHandler = this.StartJobCompleted;
				break;
			case 2:
				eventHandler = this.StopJobCompleted;
				break;
			case 3:
				eventHandler = this.SuspendJobCompleted;
				break;
			case 4:
				eventHandler = this.ResumeJobCompleted;
				break;
			case 5:
				eventHandler = this.UnblockJobCompleted;
				break;
			}
			try
			{
				if (eventHandler != null)
				{
					eventHandler(this, eventArgs);
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				this._tracer.TraceException(ex);
			}
		}

		// Token: 0x06001E1E RID: 7710
		public abstract void StopJobAsync();

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06001E1F RID: 7711 RVA: 0x000ACF30 File Offset: 0x000AB130
		// (remove) Token: 0x06001E20 RID: 7712 RVA: 0x000ACF68 File Offset: 0x000AB168
		public event EventHandler<AsyncCompletedEventArgs> StopJobCompleted;

		// Token: 0x06001E21 RID: 7713
		public abstract void SuspendJob();

		// Token: 0x06001E22 RID: 7714
		public abstract void SuspendJobAsync();

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06001E23 RID: 7715 RVA: 0x000ACFA0 File Offset: 0x000AB1A0
		// (remove) Token: 0x06001E24 RID: 7716 RVA: 0x000ACFD8 File Offset: 0x000AB1D8
		public event EventHandler<AsyncCompletedEventArgs> SuspendJobCompleted;

		// Token: 0x06001E25 RID: 7717
		public abstract void ResumeJob();

		// Token: 0x06001E26 RID: 7718
		public abstract void ResumeJobAsync();

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06001E27 RID: 7719 RVA: 0x000AD010 File Offset: 0x000AB210
		// (remove) Token: 0x06001E28 RID: 7720 RVA: 0x000AD048 File Offset: 0x000AB248
		public event EventHandler<AsyncCompletedEventArgs> ResumeJobCompleted;

		// Token: 0x06001E29 RID: 7721
		public abstract void UnblockJob();

		// Token: 0x06001E2A RID: 7722
		public abstract void UnblockJobAsync();

		// Token: 0x06001E2B RID: 7723
		public abstract void StopJob(bool force, string reason);

		// Token: 0x06001E2C RID: 7724
		public abstract void StopJobAsync(bool force, string reason);

		// Token: 0x06001E2D RID: 7725
		public abstract void SuspendJob(bool force, string reason);

		// Token: 0x06001E2E RID: 7726
		public abstract void SuspendJobAsync(bool force, string reason);

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06001E2F RID: 7727 RVA: 0x000AD080 File Offset: 0x000AB280
		// (remove) Token: 0x06001E30 RID: 7728 RVA: 0x000AD0B8 File Offset: 0x000AB2B8
		public event EventHandler<AsyncCompletedEventArgs> UnblockJobCompleted;

		// Token: 0x04000D43 RID: 3395
		private const int StartJobOperation = 1;

		// Token: 0x04000D44 RID: 3396
		private const int StopJobOperation = 2;

		// Token: 0x04000D45 RID: 3397
		private const int SuspendJobOperation = 3;

		// Token: 0x04000D46 RID: 3398
		private const int ResumeJobOperation = 4;

		// Token: 0x04000D47 RID: 3399
		private const int UnblockJobOperation = 5;

		// Token: 0x04000D48 RID: 3400
		private List<CommandParameterCollection> _parameters;

		// Token: 0x04000D49 RID: 3401
		private readonly object _syncobject = new object();

		// Token: 0x04000D4A RID: 3402
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();
	}
}
