using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation.Tracing;
using System.Text;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x0200027F RID: 639
	public sealed class ContainerParentJob : Job2
	{
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000AD0ED File Offset: 0x000AB2ED
		// (set) Token: 0x06001E32 RID: 7730 RVA: 0x000AD0F5 File Offset: 0x000AB2F5
		internal PSEventManager EventManager
		{
			get
			{
				return this._eventManager;
			}
			set
			{
				this._tracer.WriteMessage("Setting event manager for Job ", base.InstanceId);
				this._eventManager = value;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x000AD118 File Offset: 0x000AB318
		private ManualResetEvent JobRunning
		{
			get
			{
				if (this._jobRunning == null)
				{
					lock (this._syncObject)
					{
						if (this._jobRunning == null)
						{
							base.AssertNotDisposed();
							this._jobRunning = new ManualResetEvent(false);
						}
					}
				}
				return this._jobRunning;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x000AD17C File Offset: 0x000AB37C
		private ManualResetEvent JobSuspendedOrAborted
		{
			get
			{
				if (this._jobSuspendedOrAborted == null)
				{
					lock (this._syncObject)
					{
						if (this._jobSuspendedOrAborted == null)
						{
							base.AssertNotDisposed();
							this._jobSuspendedOrAborted = new ManualResetEvent(false);
						}
					}
				}
				return this._jobSuspendedOrAborted;
			}
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000AD1E0 File Offset: 0x000AB3E0
		public ContainerParentJob(string command, string name) : base(command, name)
		{
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000AD230 File Offset: 0x000AB430
		public ContainerParentJob(string command) : base(command)
		{
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000AD280 File Offset: 0x000AB480
		public ContainerParentJob(string command, string name, JobIdentifier jobId) : base(command, name, jobId)
		{
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000AD2D0 File Offset: 0x000AB4D0
		public ContainerParentJob(string command, string name, Guid instanceId) : base(command, name, instanceId)
		{
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000AD320 File Offset: 0x000AB520
		public ContainerParentJob(string command, string name, JobIdentifier jobId, string jobType) : base(command, name, jobId)
		{
			base.PSJobTypeName = jobType;
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000AD378 File Offset: 0x000AB578
		public ContainerParentJob(string command, string name, Guid instanceId, string jobType) : base(command, name, instanceId)
		{
			base.PSJobTypeName = jobType;
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000AD3D0 File Offset: 0x000AB5D0
		public ContainerParentJob(string command, string name, string jobType) : base(command, name)
		{
			base.PSJobTypeName = jobType;
			base.StateChanged += this.HandleMyStateChanged;
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x000AD426 File Offset: 0x000AB626
		internal PSDataCollection<ErrorRecord> ExecutionError
		{
			get
			{
				return this._executionError;
			}
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000AD430 File Offset: 0x000AB630
		public void AddChildJob(Job2 childJob)
		{
			base.AssertNotDisposed();
			if (childJob == null)
			{
				throw new ArgumentNullException("childJob");
			}
			this._tracer.WriteMessage("ContainerParentJob", "AddChildJob", Guid.Empty, childJob, "Adding Child to Parent with InstanceId : ", new string[]
			{
				base.InstanceId.ToString()
			});
			JobStateInfo jobStateInfo;
			lock (childJob.syncObject)
			{
				jobStateInfo = childJob.JobStateInfo;
				childJob.StateChanged += this.HandleChildJobStateChanged;
			}
			base.ChildJobs.Add(childJob);
			this.ParentJobStateCalculation(new JobStateEventArgs(jobStateInfo, new JobStateInfo(JobState.NotStarted)));
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x000AD4F8 File Offset: 0x000AB6F8
		public override bool HasMoreData
		{
			get
			{
				if (this._moreData && base.IsFinishedState(base.JobStateInfo.State))
				{
					bool moreData = false;
					for (int i = 0; i < base.ChildJobs.Count; i++)
					{
						if (base.ChildJobs[i].HasMoreData)
						{
							moreData = true;
							break;
						}
					}
					this._moreData = moreData;
				}
				return this._moreData;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000AD55C File Offset: 0x000AB75C
		public override string StatusMessage
		{
			get
			{
				return this.ConstructStatusMessage();
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000AD6C4 File Offset: 0x000AB8C4
		public override void StartJob()
		{
			base.AssertNotDisposed();
			this._tracer.WriteMessage("ContainerParentJob", "StartJob", Guid.Empty, this, "Entering method", null);
			ContainerParentJob.StructuredTracer.BeginContainerParentJobExecution(base.InstanceId);
			if (base.ChildJobs.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNoChildJobs, new object[0]);
			}
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			if (base.ChildJobs.Count == 1)
			{
				Job2 job = base.ChildJobs[0] as Job2;
				try
				{
					this._tracer.WriteMessage("ContainerParentJob", "StartJob", Guid.Empty, this, "Single child job synchronously, child InstanceId: {0}", new string[]
					{
						job.InstanceId.ToString()
					});
					job.StartJob();
					this.JobRunning.WaitOne();
				}
				catch (Exception exception)
				{
					this.ExecutionError.Add(new ErrorRecord(exception, "ContainerParentJobStartError", ErrorCategory.InvalidResult, job));
					this._tracer.WriteMessage("ContainerParentJob", "StartJob", Guid.Empty, this, "Single child job threw exception, child InstanceId: {0}", new string[]
					{
						job.InstanceId.ToString()
					});
					this._tracer.TraceException(exception);
				}
				return;
			}
			AutoResetEvent completed = new AutoResetEvent(false);
			int startedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> value = delegate(object sender, AsyncCompletedEventArgs e)
			{
				Job2 job6 = sender as Job2;
				this._tracer.WriteMessage("ContainerParentJob", "StartJob-Handler", Guid.Empty, this, "Finished starting child job asynchronously, child InstanceId: {0}", new string[]
				{
					job6.InstanceId.ToString()
				});
				if (e.Error != null)
				{
					this.ExecutionError.Add(new ErrorRecord(e.Error, "ConainerParentJobStartError", ErrorCategory.InvalidResult, job6));
					this._tracer.WriteMessage("ContainerParentJob", "StartJob-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
					{
						job6.InstanceId.ToString()
					});
					this._tracer.TraceException(e.Error);
				}
				Interlocked.Increment(ref startedChildJobsCount);
				if (startedChildJobsCount == this.ChildJobs.Count)
				{
					this._tracer.WriteMessage("ContainerParentJob", "StartJob-Handler", Guid.Empty, this, "Finished starting all child jobs asynchronously", null);
					this.JobRunning.WaitOne();
					completed.Set();
				}
			};
			foreach (Job job2 in base.ChildJobs)
			{
				Job2 job3 = (Job2)job2;
				job3.StartJobCompleted += value;
				this._tracer.WriteMessage("ContainerParentJob", "StartJob", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
				{
					job3.InstanceId.ToString()
				});
				ScriptDebugger.SetDebugJobAsync(job3 as IJobDebugger, false);
				job3.StartJobAsync();
			}
			completed.WaitOne();
			foreach (Job job4 in base.ChildJobs)
			{
				Job2 job5 = (Job2)job4;
				job5.StartJobCompleted -= value;
			}
			this._tracer.WriteMessage("ContainerParentJob", "StartJob", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000ADB2C File Offset: 0x000ABD2C
		public override void StartJobAsync()
		{
			if (this._isDisposed == 1)
			{
				this.OnStartJobCompleted(new AsyncCompletedEventArgs(new ObjectDisposedException("ContainerParentJob"), false, null));
				return;
			}
			this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync", Guid.Empty, this, "Entering method", null);
			ContainerParentJob.StructuredTracer.BeginContainerParentJobExecution(base.InstanceId);
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			int startedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> eventHandler = null;
			eventHandler = delegate(object sender, AsyncCompletedEventArgs e)
			{
				Job2 job3 = sender as Job2;
				this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync-Handler", Guid.Empty, this, "Finished starting child job asynchronously, child InstanceId: {0}", new string[]
				{
					job3.InstanceId.ToString()
				});
				if (e.Error != null)
				{
					this.ExecutionError.Add(new ErrorRecord(e.Error, "ConainerParentJobStartAsyncError", ErrorCategory.InvalidResult, job3));
					this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
					{
						job3.InstanceId.ToString()
					});
					this._tracer.TraceException(e.Error);
				}
				Interlocked.Increment(ref startedChildJobsCount);
				job3.StartJobCompleted -= eventHandler;
				if (startedChildJobsCount == this.ChildJobs.Count)
				{
					this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync-Handler", Guid.Empty, this, "Finished starting all child jobs asynchronously", null);
					this.JobRunning.WaitOne();
					this.OnStartJobCompleted(new AsyncCompletedEventArgs(null, false, null));
				}
			};
			foreach (Job job in base.ChildJobs)
			{
				Job2 job2 = (Job2)job;
				job2.StartJobCompleted += eventHandler;
				this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
				{
					job2.InstanceId.ToString()
				});
				job2.StartJobAsync();
			}
			this._tracer.WriteMessage("ContainerParentJob", "StartJobAsync", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000ADE70 File Offset: 0x000AC070
		public override void ResumeJob()
		{
			base.AssertNotDisposed();
			this._tracer.WriteMessage("ContainerParentJob", "ResumeJob", Guid.Empty, this, "Entering method", null);
			if (base.ChildJobs.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNoChildJobs, new object[0]);
			}
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			if (base.ChildJobs.Count == 1)
			{
				Job2 job4 = base.ChildJobs[0] as Job2;
				try
				{
					this._tracer.WriteMessage("ContainerParentJob", "ResumeJob", Guid.Empty, this, "Single child job synchronously, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					job4.ResumeJob();
					this.JobRunning.WaitOne();
				}
				catch (Exception exception)
				{
					this.ExecutionError.Add(new ErrorRecord(exception, "ContainerParentJobResumeError", ErrorCategory.InvalidResult, job4));
					this._tracer.WriteMessage("ContainerParentJob", "ResumeJob", Guid.Empty, this, "Single child job threw exception, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					this._tracer.TraceException(exception);
				}
				return;
			}
			AutoResetEvent completed = new AutoResetEvent(false);
			int resumedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> value = null;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Job2 job = (Job2)enumerator2.Current;
					value = delegate(object sender, AsyncCompletedEventArgs e)
					{
						this._tracer.WriteMessage("ContainerParentJob", "ResumeJob-Handler", Guid.Empty, this, "Finished resuming child job asynchronously, child InstanceId: {0}", new string[]
						{
							job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ContainerParentJobResumeError", ErrorCategory.InvalidResult, job));
							this._tracer.WriteMessage("ContainerParentJob", "ResumeJob-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref resumedChildJobsCount);
						if (resumedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "ResumeJob-Handler", Guid.Empty, this, "Finished resuming all child jobs asynchronously", null);
							this.JobRunning.WaitOne();
							completed.Set();
						}
					};
					job.ResumeJobCompleted += value;
					this._tracer.WriteMessage("ContainerParentJob", "ResumeJob", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						job.InstanceId.ToString()
					});
					job.ResumeJobAsync();
				}
			}
			completed.WaitOne();
			foreach (Job job2 in base.ChildJobs)
			{
				Job2 job3 = (Job2)job2;
				job3.ResumeJobCompleted -= value;
			}
			this._tracer.WriteMessage("ContainerParentJob", "ResumeJob", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000AE364 File Offset: 0x000AC564
		public override void ResumeJobAsync()
		{
			if (this._isDisposed == 1)
			{
				this.OnResumeJobCompleted(new AsyncCompletedEventArgs(new ObjectDisposedException("ContainerParentJob"), false, null));
				return;
			}
			this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync", Guid.Empty, this, "Entering method", null);
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			int resumedChildJobsCount = 0;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ContainerParentJob.<>c__DisplayClass12 CS$<>8__locals2 = new ContainerParentJob.<>c__DisplayClass12();
					CS$<>8__locals2.job = (Job2)enumerator2.Current;
					EventHandler<AsyncCompletedEventArgs> eventHandler = null;
					eventHandler = delegate(object sender, AsyncCompletedEventArgs e)
					{
						Job2 job = sender as Job2;
						this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync-Handler", Guid.Empty, this, "Finished resuming child job asynchronously, child InstanceId: {0}", new string[]
						{
							CS$<>8__locals2.job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ContainerParentJobResumeAsyncError", ErrorCategory.InvalidResult, CS$<>8__locals2.job));
							this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								CS$<>8__locals2.job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref resumedChildJobsCount);
						job.ResumeJobCompleted -= eventHandler;
						if (resumedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync-Handler", Guid.Empty, this, "Finished resuming all child jobs asynchronously", null);
							this.JobRunning.WaitOne();
							this.OnResumeJobCompleted(new AsyncCompletedEventArgs(null, false, null));
						}
					};
					CS$<>8__locals2.job.ResumeJobCompleted += eventHandler;
					this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						CS$<>8__locals2.job.InstanceId.ToString()
					});
					CS$<>8__locals2.job.ResumeJobAsync();
				}
			}
			this._tracer.WriteMessage("ContainerParentJob", "ResumeJobAsync", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000AE520 File Offset: 0x000AC720
		public override void SuspendJob()
		{
			this.SuspendJobInternal(null, null);
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000AE53D File Offset: 0x000AC73D
		public override void SuspendJob(bool force, string reason)
		{
			this.SuspendJobInternal(new bool?(force), reason);
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000AE54C File Offset: 0x000AC74C
		public override void SuspendJobAsync()
		{
			this.SuspendJobAsyncInternal(null, null);
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x000AE569 File Offset: 0x000AC769
		public override void SuspendJobAsync(bool force, string reason)
		{
			this.SuspendJobAsyncInternal(new bool?(force), reason);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000AE578 File Offset: 0x000AC778
		public override void StopJob()
		{
			this.StopJobInternal(null, null);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000AE598 File Offset: 0x000AC798
		public override void StopJobAsync()
		{
			this.StopJobAsyncInternal(null, null);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000AE5B5 File Offset: 0x000AC7B5
		public override void StopJob(bool force, string reason)
		{
			this.StopJobInternal(new bool?(force), reason);
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000AE5C4 File Offset: 0x000AC7C4
		public override void StopJobAsync(bool force, string reason)
		{
			this.StopJobAsyncInternal(new bool?(force), reason);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000AE770 File Offset: 0x000AC970
		public override void UnblockJob()
		{
			base.AssertNotDisposed();
			this._tracer.WriteMessage("ContainerParentJob", "UnblockJob", Guid.Empty, this, "Entering method", null);
			if (base.ChildJobs.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNoChildJobs, new object[0]);
			}
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			if (base.ChildJobs.Count == 1)
			{
				Job2 job4 = base.ChildJobs[0] as Job2;
				try
				{
					this._tracer.WriteMessage("ContainerParentJob", "UnblockJob", Guid.Empty, this, "Single child job synchronously, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					job4.UnblockJob();
				}
				catch (Exception exception)
				{
					this.ExecutionError.Add(new ErrorRecord(exception, "ContainerParentJobUnblockError", ErrorCategory.InvalidResult, job4));
					this._tracer.WriteMessage("ContainerParentJob", "UnblockJob", Guid.Empty, this, "Single child job threw exception, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					this._tracer.TraceException(exception);
				}
				return;
			}
			AutoResetEvent completed = new AutoResetEvent(false);
			int unblockedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> value = null;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Job2 job = (Job2)enumerator2.Current;
					value = delegate(object sender, AsyncCompletedEventArgs e)
					{
						Job2 targetObject = sender as Job2;
						this._tracer.WriteMessage("ContainerParentJob", "UnblockJob-Handler", Guid.Empty, this, "Finished unblock child job asynchronously, child InstanceId: {0}", new string[]
						{
							job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ConainerParentJobUnblockError", ErrorCategory.InvalidResult, targetObject));
							this._tracer.WriteMessage("ContainerParentJob", "UnblockJob-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref unblockedChildJobsCount);
						if (unblockedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "UnblockJob-Handler", Guid.Empty, this, "Finished unblock all child jobs asynchronously", null);
							completed.Set();
						}
					};
					job.UnblockJobCompleted += value;
					this._tracer.WriteMessage("ContainerParentJob", "UnblockJob", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						job.InstanceId.ToString()
					});
					job.UnblockJobAsync();
				}
			}
			completed.WaitOne();
			foreach (Job job2 in base.ChildJobs)
			{
				Job2 job3 = (Job2)job2;
				job3.UnblockJobCompleted -= value;
			}
			this._tracer.WriteMessage("ContainerParentJob", "UnblockJob", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000AEC38 File Offset: 0x000ACE38
		public override void UnblockJobAsync()
		{
			if (this._isDisposed == 1)
			{
				this.OnUnblockJobCompleted(new AsyncCompletedEventArgs(new ObjectDisposedException("ContainerParentJob"), false, null));
				return;
			}
			this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync", Guid.Empty, this, "Entering method", null);
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			int unblockedChildJobsCount = 0;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ContainerParentJob.<>c__DisplayClass1f CS$<>8__locals2 = new ContainerParentJob.<>c__DisplayClass1f();
					CS$<>8__locals2.job = (Job2)enumerator2.Current;
					EventHandler<AsyncCompletedEventArgs> eventHandler = null;
					eventHandler = delegate(object sender, AsyncCompletedEventArgs e)
					{
						Job2 job = sender as Job2;
						this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync-Handler", Guid.Empty, this, "Finished unblock child job asynchronously, child InstanceId: {0}", new string[]
						{
							CS$<>8__locals2.job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ConainerParentJobUnblockError", ErrorCategory.InvalidResult, job));
							this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								CS$<>8__locals2.job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref unblockedChildJobsCount);
						job.UnblockJobCompleted -= eventHandler;
						if (unblockedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync-Handler", Guid.Empty, this, "Finished unblock all child jobs asynchronously", null);
							this.OnUnblockJobCompleted(new AsyncCompletedEventArgs(null, false, null));
						}
					};
					CS$<>8__locals2.job.UnblockJobCompleted += eventHandler;
					this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						CS$<>8__locals2.job.InstanceId.ToString()
					});
					CS$<>8__locals2.job.UnblockJobAsync();
				}
			}
			this._tracer.WriteMessage("ContainerParentJob", "UnblockJobAsync", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000AEFD8 File Offset: 0x000AD1D8
		private void SuspendJobInternal(bool? force, string reason)
		{
			base.AssertNotDisposed();
			this._tracer.WriteMessage("ContainerParentJob", "SuspendJob", Guid.Empty, this, "Entering method", null);
			if (base.ChildJobs.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNoChildJobs, new object[0]);
			}
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			if (base.ChildJobs.Count == 1)
			{
				Job2 job4 = base.ChildJobs[0] as Job2;
				try
				{
					this._tracer.WriteMessage("ContainerParentJob", "SuspendJob", Guid.Empty, this, "Single child job synchronously, child InstanceId: {0} force: {1}", new string[]
					{
						job4.InstanceId.ToString(),
						force.ToString()
					});
					if (force != null)
					{
						job4.SuspendJob(force.Value, reason);
					}
					else
					{
						job4.SuspendJob();
					}
					this.JobSuspendedOrAborted.WaitOne();
				}
				catch (Exception exception)
				{
					this.ExecutionError.Add(new ErrorRecord(exception, "ContainerParentJobSuspendError", ErrorCategory.InvalidResult, job4));
					this._tracer.WriteMessage("ContainerParentJob", "SuspendJob", Guid.Empty, this, "Single child job threw exception, child InstanceId: {0} force: {1}", new string[]
					{
						job4.InstanceId.ToString(),
						force.ToString()
					});
					this._tracer.TraceException(exception);
				}
				return;
			}
			AutoResetEvent completed = new AutoResetEvent(false);
			int suspendedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> value = null;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Job2 job = (Job2)enumerator2.Current;
					value = delegate(object sender, AsyncCompletedEventArgs e)
					{
						this._tracer.WriteMessage("ContainerParentJob", "SuspendJob-Handler", Guid.Empty, this, "Finished suspending child job asynchronously, child InstanceId: {0} force: {1}", new string[]
						{
							job.InstanceId.ToString(),
							force.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ContainerParentJobSuspendError", ErrorCategory.InvalidResult, job));
							this._tracer.WriteMessage("ContainerParentJob", "SuspendJob-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0} force: {1}", new string[]
							{
								job.InstanceId.ToString(),
								force.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref suspendedChildJobsCount);
						if (suspendedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "SuspendJob-Handler", Guid.Empty, this, "Finished suspending all child jobs asynchronously", null);
							this.JobSuspendedOrAborted.WaitOne();
							completed.Set();
						}
					};
					job.SuspendJobCompleted += value;
					this._tracer.WriteMessage("ContainerParentJob", "SuspendJob", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0} force: {1}", new string[]
					{
						job.InstanceId.ToString(),
						force.ToString()
					});
					if (force != null)
					{
						job.SuspendJobAsync(force.Value, reason);
					}
					else
					{
						job.SuspendJobAsync();
					}
				}
			}
			completed.WaitOne();
			foreach (Job job2 in base.ChildJobs)
			{
				Job2 job3 = (Job2)job2;
				job3.SuspendJobCompleted -= value;
			}
			this._tracer.WriteMessage("ContainerParentJob", "SuspendJob", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000AF594 File Offset: 0x000AD794
		private void SuspendJobAsyncInternal(bool? force, string reason)
		{
			if (this._isDisposed == 1)
			{
				this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(new ObjectDisposedException("ContainerParentJob"), false, null));
				return;
			}
			this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync", Guid.Empty, this, "Entering method", null);
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			int suspendedChildJobsCount = 0;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ContainerParentJob.<>c__DisplayClass2c CS$<>8__locals2 = new ContainerParentJob.<>c__DisplayClass2c();
					CS$<>8__locals2.job = (Job2)enumerator2.Current;
					EventHandler<AsyncCompletedEventArgs> eventHandler = null;
					eventHandler = delegate(object sender, AsyncCompletedEventArgs e)
					{
						Job2 job = sender as Job2;
						this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync-Handler", Guid.Empty, this, "Finished suspending child job asynchronously, child InstanceId: {0} force: {1}", new string[]
						{
							CS$<>8__locals2.job.InstanceId.ToString(),
							force.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ContainerParentJobSuspendAsyncError", ErrorCategory.InvalidResult, CS$<>8__locals2.job));
							this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0} force: {1}", new string[]
							{
								CS$<>8__locals2.job.InstanceId.ToString(),
								force.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref suspendedChildJobsCount);
						job.SuspendJobCompleted -= eventHandler;
						if (suspendedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync-Handler", Guid.Empty, this, "Finished suspending all child jobs asynchronously", null);
							this.JobSuspendedOrAborted.WaitOne();
							this.OnSuspendJobCompleted(new AsyncCompletedEventArgs(null, false, null));
						}
					};
					CS$<>8__locals2.job.SuspendJobCompleted += eventHandler;
					this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0} force: {1}", new string[]
					{
						CS$<>8__locals2.job.InstanceId.ToString(),
						force.ToString()
					});
					if (force != null)
					{
						CS$<>8__locals2.job.SuspendJobAsync(force.Value, reason);
					}
					else
					{
						CS$<>8__locals2.job.SuspendJobAsync();
					}
				}
			}
			this._tracer.WriteMessage("ContainerParentJob", "SuspendJobAsync", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000AF944 File Offset: 0x000ADB44
		private void StopJobInternal(bool? force, string reason)
		{
			base.AssertNotDisposed();
			this._tracer.WriteMessage("ContainerParentJob", "StopJob", Guid.Empty, this, "Entering method", null);
			if (base.ChildJobs.Count == 0)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNoChildJobs, new object[0]);
			}
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			if (base.ChildJobs.Count == 1)
			{
				Job2 job4 = base.ChildJobs[0] as Job2;
				try
				{
					this._tracer.WriteMessage("ContainerParentJob", "StopJob", Guid.Empty, this, "Single child job synchronously, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					if (force != null)
					{
						job4.StopJob(force.Value, reason);
					}
					else
					{
						job4.StopJob();
					}
					base.Finished.WaitOne();
				}
				catch (Exception exception)
				{
					this.ExecutionError.Add(new ErrorRecord(exception, "ContainerParentJobStopError", ErrorCategory.InvalidResult, job4));
					this._tracer.WriteMessage("ContainerParentJob", "StopJob", Guid.Empty, this, "Single child job threw exception, child InstanceId: {0}", new string[]
					{
						job4.InstanceId.ToString()
					});
					this._tracer.TraceException(exception);
				}
				return;
			}
			AutoResetEvent completed = new AutoResetEvent(false);
			int stoppedChildJobsCount = 0;
			EventHandler<AsyncCompletedEventArgs> value = null;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Job2 job = (Job2)enumerator2.Current;
					value = delegate(object sender, AsyncCompletedEventArgs e)
					{
						this._tracer.WriteMessage("ContainerParentJob", "StopJob-Handler", Guid.Empty, this, "Finished stopping child job asynchronously, child InstanceId: {0}", new string[]
						{
							job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ContainerParentJobStopError", ErrorCategory.InvalidResult, job));
							this._tracer.WriteMessage("ContainerParentJob", "StopJob-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref stoppedChildJobsCount);
						if (stoppedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "StopJob-Handler", Guid.Empty, this, "Finished stopping all child jobs asynchronously", null);
							this.Finished.WaitOne();
							completed.Set();
						}
					};
					job.StopJobCompleted += value;
					this._tracer.WriteMessage("ContainerParentJob", "StopJob", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						job.InstanceId.ToString()
					});
					if (force != null)
					{
						job.StopJobAsync(force.Value, reason);
					}
					else
					{
						job.StopJobAsync();
					}
				}
			}
			completed.WaitOne();
			foreach (Job job2 in base.ChildJobs)
			{
				Job2 job3 = (Job2)job2;
				job3.StopJobCompleted -= value;
			}
			this._tracer.WriteMessage("ContainerParentJob", "StopJob", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000AFE64 File Offset: 0x000AE064
		private void StopJobAsyncInternal(bool? force, string reason)
		{
			if (this._isDisposed == 1)
			{
				this.OnStopJobCompleted(new AsyncCompletedEventArgs(new ObjectDisposedException("ContainerParentJob"), false, null));
				return;
			}
			this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync", Guid.Empty, this, "Entering method", null);
			using (IEnumerator<Job> enumerator = base.ChildJobs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((Job2)enumerator.Current == null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.JobActionInvalidWithNullChild, new object[0]);
					}
				}
			}
			int stoppedChildJobsCount = 0;
			using (IEnumerator<Job> enumerator2 = base.ChildJobs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ContainerParentJob.<>c__DisplayClass39 CS$<>8__locals2 = new ContainerParentJob.<>c__DisplayClass39();
					CS$<>8__locals2.job = (Job2)enumerator2.Current;
					EventHandler<AsyncCompletedEventArgs> eventHandler = null;
					eventHandler = delegate(object sender, AsyncCompletedEventArgs e)
					{
						Job2 job = sender as Job2;
						this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync-Handler", Guid.Empty, this, "Finished stopping child job asynchronously, child InstanceId: {0}", new string[]
						{
							CS$<>8__locals2.job.InstanceId.ToString()
						});
						if (e.Error != null)
						{
							this.ExecutionError.Add(new ErrorRecord(e.Error, "ConainerParentJobStopAsyncError", ErrorCategory.InvalidResult, job));
							this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync-Handler", Guid.Empty, this, "Child job asynchronously had error, child InstanceId: {0}", new string[]
							{
								CS$<>8__locals2.job.InstanceId.ToString()
							});
							this._tracer.TraceException(e.Error);
						}
						Interlocked.Increment(ref stoppedChildJobsCount);
						job.StopJobCompleted -= eventHandler;
						if (stoppedChildJobsCount == this.ChildJobs.Count)
						{
							this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync-Handler", Guid.Empty, this, "Finished stopping all child jobs asynchronously", null);
							this.Finished.WaitOne();
							this.OnStopJobCompleted(new AsyncCompletedEventArgs(null, false, null));
						}
					};
					CS$<>8__locals2.job.StopJobCompleted += eventHandler;
					this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync", Guid.Empty, this, "Child job asynchronously, child InstanceId: {0}", new string[]
					{
						CS$<>8__locals2.job.InstanceId.ToString()
					});
					if (force != null)
					{
						CS$<>8__locals2.job.StopJobAsync(force.Value, reason);
					}
					else
					{
						CS$<>8__locals2.job.StopJobAsync();
					}
				}
			}
			this._tracer.WriteMessage("ContainerParentJob", "StopJobAsync", Guid.Empty, this, "Exiting method", null);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000B0040 File Offset: 0x000AE240
		private void HandleMyStateChanged(object sender, JobStateEventArgs e)
		{
			this._tracer.WriteMessage("ContainerParentJob", "HandleMyStateChanged", Guid.Empty, this, "NewState: {0}; OldState: {1}", new string[]
			{
				e.JobStateInfo.State.ToString(),
				e.PreviousJobStateInfo.State.ToString()
			});
			switch (e.JobStateInfo.State)
			{
			case JobState.Running:
				lock (this._syncObject)
				{
					this.JobRunning.Set();
					if (this._jobSuspendedOrAborted != null)
					{
						this.JobSuspendedOrAborted.Reset();
					}
					return;
				}
				break;
			case JobState.Completed:
			case JobState.Failed:
			case JobState.Stopped:
				goto IL_FD;
			case JobState.Blocked:
				return;
			case JobState.Suspended:
				break;
			default:
				return;
			}
			lock (this._syncObject)
			{
				this.JobSuspendedOrAborted.Set();
				this.JobRunning.Reset();
				return;
			}
			IL_FD:
			lock (this._syncObject)
			{
				this.JobSuspendedOrAborted.Set();
				this.JobRunning.Set();
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000B01AC File Offset: 0x000AE3AC
		private void HandleChildJobStateChanged(object sender, JobStateEventArgs e)
		{
			this.ParentJobStateCalculation(e);
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000B01B8 File Offset: 0x000AE3B8
		private void ParentJobStateCalculation(JobStateEventArgs e)
		{
			JobState jobState;
			if (ContainerParentJob.ComputeJobStateFromChildJobStates("ContainerParentJob", e, ref this._blockedChildJobsCount, ref this._suspendedChildJobsCount, ref this._suspendingChildJobsCount, ref this._finishedChildJobsCount, ref this._failedChildJobsCount, ref this._stoppedChildJobsCount, base.ChildJobs.Count, out jobState))
			{
				if (jobState != base.JobStateInfo.State)
				{
					if (base.JobStateInfo.State == JobState.NotStarted && jobState == JobState.Running)
					{
						base.PSBeginTime = new DateTime?(DateTime.Now);
					}
					if (!base.IsFinishedState(base.JobStateInfo.State) && base.IsPersistentState(jobState))
					{
						base.PSEndTime = new DateTime?(DateTime.Now);
					}
					base.SetJobState(jobState);
				}
				if (this._finishedChildJobsCount == base.ChildJobs.Count)
				{
					ContainerParentJob.StructuredTracer.EndContainerParentJobExecution(base.InstanceId);
				}
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000B028C File Offset: 0x000AE48C
		internal static bool ComputeJobStateFromChildJobStates(string traceClassName, JobStateEventArgs e, ref int blockedChildJobsCount, ref int suspendedChildJobsCount, ref int suspendingChildJobsCount, ref int finishedChildJobsCount, ref int failedChildJobsCount, ref int stoppedChildJobsCount, int childJobsCount, out JobState computedJobState)
		{
			computedJobState = JobState.NotStarted;
			using (PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource())
			{
				if (e.JobStateInfo.State == JobState.Blocked)
				{
					Interlocked.Increment(ref blockedChildJobsCount);
					traceSource.WriteMessage(traceClassName, ": JobState is Blocked, at least one child job is blocked.");
					computedJobState = JobState.Blocked;
					return true;
				}
				if (e.PreviousJobStateInfo.State == JobState.Blocked)
				{
					Interlocked.Decrement(ref blockedChildJobsCount);
					if (blockedChildJobsCount == 0)
					{
						traceSource.WriteMessage(traceClassName, ": JobState is unblocked, all child jobs are unblocked.");
						computedJobState = JobState.Running;
						return true;
					}
					return false;
				}
				else
				{
					if (e.PreviousJobStateInfo.State == JobState.Suspended)
					{
						Interlocked.Decrement(ref suspendedChildJobsCount);
					}
					if (e.PreviousJobStateInfo.State == JobState.Suspending)
					{
						Interlocked.Decrement(ref suspendingChildJobsCount);
					}
					if (e.JobStateInfo.State == JobState.Suspended)
					{
						Interlocked.Increment(ref suspendedChildJobsCount);
						if (suspendedChildJobsCount + finishedChildJobsCount == childJobsCount)
						{
							traceSource.WriteMessage(traceClassName, ": JobState is suspended, all child jobs are suspended.");
							computedJobState = JobState.Suspended;
							return true;
						}
						return false;
					}
					else if (e.JobStateInfo.State == JobState.Suspending)
					{
						Interlocked.Increment(ref suspendingChildJobsCount);
						if (suspendedChildJobsCount + finishedChildJobsCount + suspendingChildJobsCount == childJobsCount)
						{
							traceSource.WriteMessage(traceClassName, ": JobState is suspending, all child jobs are in suspending state.");
							computedJobState = JobState.Suspending;
							return true;
						}
						return false;
					}
					else if (e.JobStateInfo.State != JobState.Completed && e.JobStateInfo.State != JobState.Failed && e.JobStateInfo.State != JobState.Stopped)
					{
						if (e.JobStateInfo.State == JobState.Running)
						{
							computedJobState = JobState.Running;
							return true;
						}
						return false;
					}
					else
					{
						if (e.JobStateInfo.State == JobState.Failed)
						{
							Interlocked.Increment(ref failedChildJobsCount);
						}
						if (e.JobStateInfo.State == JobState.Stopped)
						{
							Interlocked.Increment(ref stoppedChildJobsCount);
						}
						bool flag = false;
						int num = Interlocked.Increment(ref finishedChildJobsCount);
						if (num == childJobsCount)
						{
							flag = true;
						}
						if (flag)
						{
							if (failedChildJobsCount > 0)
							{
								traceSource.WriteMessage(traceClassName, ": JobState is failed, at least one child job failed.");
								computedJobState = JobState.Failed;
								return true;
							}
							if (stoppedChildJobsCount > 0)
							{
								traceSource.WriteMessage(traceClassName, ": JobState is stopped, stop is called.");
								computedJobState = JobState.Stopped;
								return true;
							}
							traceSource.WriteMessage(traceClassName, ": JobState is completed.");
							computedJobState = JobState.Completed;
							return true;
						}
						else
						{
							if (suspendedChildJobsCount + num == childJobsCount)
							{
								traceSource.WriteMessage(traceClassName, ": JobState is suspended, all child jobs are suspended.");
								computedJobState = JobState.Suspended;
								return true;
							}
							if (suspendingChildJobsCount + suspendedChildJobsCount + num == childJobsCount)
							{
								traceSource.WriteMessage(traceClassName, ": JobState is suspending, all child jobs are in suspending state.");
								computedJobState = JobState.Suspending;
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000B04F4 File Offset: 0x000AE6F4
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (Interlocked.CompareExchange(ref this._isDisposed, 1, 0) == 1)
			{
				return;
			}
			try
			{
				this.UnregisterAllJobEvents();
				this._executionError.Dispose();
				base.StateChanged -= this.HandleMyStateChanged;
				foreach (Job job in base.ChildJobs)
				{
					this._tracer.WriteMessage("Disposing child job with id : " + job.Id);
					job.Dispose();
				}
				if (this._jobRunning != null)
				{
					this._jobRunning.Dispose();
				}
				if (this._jobSuspendedOrAborted != null)
				{
					this._jobSuspendedOrAborted.Dispose();
				}
			}
			finally
			{
				base.Dispose(true);
			}
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000B05F0 File Offset: 0x000AE7F0
		private string ConstructLocation()
		{
			if (base.ChildJobs == null || base.ChildJobs.Count == 0)
			{
				return string.Empty;
			}
			return (from job in base.ChildJobs
			select job.Location).Aggregate((string s1, string s2) => s1 + ',' + s2);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000B0664 File Offset: 0x000AE864
		private string ConstructStatusMessage()
		{
			if (base.ChildJobs == null || base.ChildJobs.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < base.ChildJobs.Count; i++)
			{
				if (!string.IsNullOrEmpty(base.ChildJobs[i].StatusMessage))
				{
					stringBuilder.Append(base.ChildJobs[i].StatusMessage);
				}
				if (i < base.ChildJobs.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x000B06FA File Offset: 0x000AE8FA
		public override string Location
		{
			get
			{
				return this.ConstructLocation();
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000B0720 File Offset: 0x000AE920
		private void UnregisterJobEvent(Job job)
		{
			string sourceIdentifier = job.InstanceId + ":StateChanged";
			this._tracer.WriteMessage("Unregistering StateChanged event for job ", job.InstanceId);
			using (IEnumerator<PSEventSubscriber> enumerator = (from subscriber in this.EventManager.Subscribers
			where string.Equals(subscriber.SourceIdentifier, sourceIdentifier, StringComparison.OrdinalIgnoreCase)
			select subscriber).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					PSEventSubscriber subscriber2 = enumerator.Current;
					this.EventManager.UnsubscribeEvent(subscriber2);
				}
			}
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000B07C4 File Offset: 0x000AE9C4
		private void UnregisterAllJobEvents()
		{
			if (this.EventManager == null)
			{
				this._tracer.WriteMessage("No events subscribed, skipping event unregistrations");
				return;
			}
			foreach (Job job in base.ChildJobs)
			{
				this.UnregisterJobEvent(job);
			}
			this.UnregisterJobEvent(this);
			this._tracer.WriteMessage("Setting event manager to null");
			this.EventManager = null;
		}

		// Token: 0x04000D54 RID: 3412
		private const string TraceClassName = "ContainerParentJob";

		// Token: 0x04000D55 RID: 3413
		private const int DisposedTrue = 1;

		// Token: 0x04000D56 RID: 3414
		private const int DisposedFalse = 0;

		// Token: 0x04000D57 RID: 3415
		private bool _moreData = true;

		// Token: 0x04000D58 RID: 3416
		private readonly object _syncObject = new object();

		// Token: 0x04000D59 RID: 3417
		private int _isDisposed;

		// Token: 0x04000D5A RID: 3418
		private int _finishedChildJobsCount;

		// Token: 0x04000D5B RID: 3419
		private int _blockedChildJobsCount;

		// Token: 0x04000D5C RID: 3420
		private int _suspendedChildJobsCount;

		// Token: 0x04000D5D RID: 3421
		private int _suspendingChildJobsCount;

		// Token: 0x04000D5E RID: 3422
		private int _failedChildJobsCount;

		// Token: 0x04000D5F RID: 3423
		private int _stoppedChildJobsCount;

		// Token: 0x04000D60 RID: 3424
		private readonly PowerShellTraceSource _tracer = PowerShellTraceSourceFactory.GetTraceSource();

		// Token: 0x04000D61 RID: 3425
		private readonly PSDataCollection<ErrorRecord> _executionError = new PSDataCollection<ErrorRecord>();

		// Token: 0x04000D62 RID: 3426
		private PSEventManager _eventManager;

		// Token: 0x04000D63 RID: 3427
		private ManualResetEvent _jobRunning;

		// Token: 0x04000D64 RID: 3428
		private ManualResetEvent _jobSuspendedOrAborted;

		// Token: 0x04000D65 RID: 3429
		private static Tracer StructuredTracer = new Tracer();
	}
}
