using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Language;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020000D9 RID: 217
	public abstract class Job : IDisposable
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x00043F94 File Offset: 0x00042194
		protected Job()
		{
			this.sessionId = Interlocked.Increment(ref Job._jobIdSeed);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00044098 File Offset: 0x00042298
		protected Job(string command) : this()
		{
			this.command = command;
			this.name = this.AutoGenerateJobName();
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000440B3 File Offset: 0x000422B3
		protected Job(string command, string name) : this(command)
		{
			if (!string.IsNullOrEmpty(name))
			{
				this.name = name;
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000440CB File Offset: 0x000422CB
		protected Job(string command, string name, IList<Job> childJobs) : this(command, name)
		{
			this.childJobs = childJobs;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000440DC File Offset: 0x000422DC
		protected Job(string command, string name, JobIdentifier token)
		{
			if (token == null)
			{
				throw PSTraceSource.NewArgumentNullException("token", RemotingErrorIdStrings.JobIdentifierNull, new object[0]);
			}
			if (token.Id > Job._jobIdSeed)
			{
				throw PSTraceSource.NewArgumentException("token", RemotingErrorIdStrings.JobIdNotYetAssigned, new object[]
				{
					token.Id
				});
			}
			this.command = command;
			this.sessionId = token.Id;
			this.guid = token.InstanceId;
			if (!string.IsNullOrEmpty(name))
			{
				this.name = name;
				return;
			}
			this.name = this.AutoGenerateJobName();
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00044257 File Offset: 0x00042457
		protected Job(string command, string name, Guid instanceId) : this(command, name)
		{
			this.guid = instanceId;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00044268 File Offset: 0x00042468
		internal static string GetCommandTextFromInvocationInfo(InvocationInfo invocationInfo)
		{
			if (invocationInfo == null)
			{
				return null;
			}
			IScriptExtent scriptPosition = invocationInfo.ScriptPosition;
			if (scriptPosition != null && scriptPosition.StartScriptPosition != null && !string.IsNullOrWhiteSpace(scriptPosition.StartScriptPosition.Line))
			{
				return scriptPosition.StartScriptPosition.Line.Substring(scriptPosition.StartScriptPosition.ColumnNumber - 1).Trim();
			}
			return invocationInfo.InvocationName;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x000442C7 File Offset: 0x000424C7
		public string Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x000442CF File Offset: 0x000424CF
		public JobStateInfo JobStateInfo
		{
			get
			{
				return this.stateInfo;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000442D8 File Offset: 0x000424D8
		public WaitHandle Finished
		{
			get
			{
				WaitHandle result;
				lock (this.syncObject)
				{
					if (this.finished != null)
					{
						result = this.finished;
					}
					else
					{
						result = new ManualResetEvent(true);
					}
				}
				return result;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0004432C File Offset: 0x0004252C
		public Guid InstanceId
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00044334 File Offset: 0x00042534
		public int Id
		{
			get
			{
				return this.sessionId;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0004433C File Offset: 0x0004253C
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x00044344 File Offset: 0x00042544
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.AssertNotDisposed();
				this.name = value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00044354 File Offset: 0x00042554
		public IList<Job> ChildJobs
		{
			get
			{
				if (this.childJobs == null)
				{
					lock (this.syncObject)
					{
						if (this.childJobs == null)
						{
							this.childJobs = new List<Job>();
						}
					}
				}
				return this.childJobs;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000C0F RID: 3087
		public abstract string StatusMessage { get; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000C10 RID: 3088
		public abstract bool HasMoreData { get; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x000443B0 File Offset: 0x000425B0
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x000443B8 File Offset: 0x000425B8
		public DateTime? PSBeginTime
		{
			get
			{
				return this._beginTime;
			}
			protected set
			{
				this._beginTime = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x000443C1 File Offset: 0x000425C1
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x000443C9 File Offset: 0x000425C9
		public DateTime? PSEndTime
		{
			get
			{
				return this._endTime;
			}
			protected set
			{
				this._endTime = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x000443D2 File Offset: 0x000425D2
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x000443DA File Offset: 0x000425DA
		public string PSJobTypeName
		{
			get
			{
				return this._jobTypeName;
			}
			protected internal set
			{
				this._jobTypeName = ((value != null) ? value : base.GetType().ToString());
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x000443F3 File Offset: 0x000425F3
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x000443FC File Offset: 0x000425FC
		internal PSDataCollection<PSStreamObject> Results
		{
			get
			{
				return this.results;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Results");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.resultsOwner = false;
					this.results = value;
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x00044458 File Offset: 0x00042658
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x00044460 File Offset: 0x00042660
		internal bool UsesResultsCollection { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00044469 File Offset: 0x00042669
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x00044471 File Offset: 0x00042671
		internal bool SuppressOutputForwarding
		{
			get
			{
				return this.suppressOutputForwarding;
			}
			set
			{
				this.suppressOutputForwarding = value;
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0004447C File Offset: 0x0004267C
		internal virtual void WriteObject(object outputObject)
		{
			PSObject psobject = (outputObject == null) ? null : PSObject.AsPSObject(outputObject);
			this.Output.Add(psobject);
			if (!this.suppressOutputForwarding)
			{
				this.Results.Add(new PSStreamObject(PSStreamObjectType.Output, psobject));
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x000444BC File Offset: 0x000426BC
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x000444C4 File Offset: 0x000426C4
		internal bool PropagateThrows
		{
			get
			{
				return this.propagateThrows;
			}
			set
			{
				this.propagateThrows = value;
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000444D0 File Offset: 0x000426D0
		private void WriteError(Cmdlet cmdlet, ErrorRecord errorRecord)
		{
			if (this.PropagateThrows)
			{
				Exception exceptionFromErrorRecord = Job.GetExceptionFromErrorRecord(errorRecord);
				if (exceptionFromErrorRecord != null)
				{
					throw exceptionFromErrorRecord;
				}
			}
			errorRecord.PreserveInvocationInfoOnce = true;
			cmdlet.WriteError(errorRecord);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00044500 File Offset: 0x00042700
		private static Exception GetExceptionFromErrorRecord(ErrorRecord errorRecord)
		{
			RuntimeException ex = errorRecord.Exception as RuntimeException;
			if (ex == null)
			{
				return null;
			}
			RemoteException ex2 = ex as RemoteException;
			if (ex2 == null)
			{
				return null;
			}
			PSPropertyInfo pspropertyInfo = ex2.SerializedRemoteException.Properties["WasThrownFromThrowStatement"];
			if (pspropertyInfo == null || !(bool)pspropertyInfo.Value)
			{
				return null;
			}
			ex.WasThrownFromThrowStatement = true;
			return ex;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0004455C File Offset: 0x0004275C
		internal virtual void WriteError(ErrorRecord errorRecord)
		{
			this.Error.Add(errorRecord);
			if (this.PropagateThrows)
			{
				Exception exceptionFromErrorRecord = Job.GetExceptionFromErrorRecord(errorRecord);
				if (exceptionFromErrorRecord != null)
				{
					this.Results.Add(new PSStreamObject(PSStreamObjectType.Exception, exceptionFromErrorRecord));
					return;
				}
			}
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Error, errorRecord));
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000445CC File Offset: 0x000427CC
		internal void WriteError(ErrorRecord errorRecord, out Exception exceptionThrownOnCmdletThread)
		{
			this.Error.Add(errorRecord);
			this.InvokeCmdletMethodAndWaitForResults<object>(delegate(Cmdlet cmdlet)
			{
				this.WriteError(cmdlet, errorRecord);
				return null;
			}, out exceptionThrownOnCmdletThread);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00044612 File Offset: 0x00042812
		internal virtual void WriteWarning(string message)
		{
			this.Warning.Add(new WarningRecord(message));
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Warning, message));
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00044637 File Offset: 0x00042837
		internal virtual void WriteVerbose(string message)
		{
			this.Verbose.Add(new VerboseRecord(message));
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Verbose, message));
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0004465D File Offset: 0x0004285D
		internal virtual void WriteDebug(string message)
		{
			this.Debug.Add(new DebugRecord(message));
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Debug, message));
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00044684 File Offset: 0x00042884
		internal virtual void WriteProgress(ProgressRecord progressRecord)
		{
			if (progressRecord.ParentActivityId == -1 && this.parentActivityId != null)
			{
				progressRecord = new ProgressRecord(progressRecord)
				{
					ParentActivityId = this.parentActivityId.Value
				};
			}
			this.Progress.Add(progressRecord);
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Progress, progressRecord));
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000446DC File Offset: 0x000428DC
		internal virtual void WriteInformation(InformationRecord informationRecord)
		{
			this.Information.Add(informationRecord);
			this.Results.Add(new PSStreamObject(PSStreamObjectType.Information, informationRecord));
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000446FD File Offset: 0x000428FD
		internal void SetParentActivityIdGetter(Func<int> parentActivityIdGetter)
		{
			this.parentActivityId = new Lazy<int>(parentActivityIdGetter);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0004470C File Offset: 0x0004290C
		internal bool ShouldContinue(string query, string caption)
		{
			Exception ex;
			return this.ShouldContinue(query, caption, out ex);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00044740 File Offset: 0x00042940
		internal bool ShouldContinue(string query, string caption, out Exception exceptionThrownOnCmdletThread)
		{
			return this.InvokeCmdletMethodAndWaitForResults<bool>((Cmdlet cmdlet) => cmdlet.ShouldContinue(query, caption), out exceptionThrownOnCmdletThread);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000447A8 File Offset: 0x000429A8
		internal virtual void NonblockingShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			this.InvokeCmdletMethodAndIgnoreResults(delegate(Cmdlet cmdlet)
			{
				ShouldProcessReason shouldProcessReason;
				cmdlet.ShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
			});
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0004480C File Offset: 0x00042A0C
		internal virtual bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason, out Exception exceptionThrownOnCmdletThread)
		{
			ShouldProcessReason closureSafeShouldProcessReason = ShouldProcessReason.None;
			bool result = this.InvokeCmdletMethodAndWaitForResults<bool>((Cmdlet cmdlet) => cmdlet.ShouldProcess(verboseDescription, verboseWarning, caption, out closureSafeShouldProcessReason), out exceptionThrownOnCmdletThread);
			shouldProcessReason = closureSafeShouldProcessReason;
			return result;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00044874 File Offset: 0x00042A74
		private void InvokeCmdletMethodAndIgnoreResults(Action<Cmdlet> invokeCmdletMethod)
		{
			object obj = new object();
			CmdletMethodInvoker<object> value = new CmdletMethodInvoker<object>
			{
				Action = delegate(Cmdlet cmdlet)
				{
					invokeCmdletMethod(cmdlet);
					return null;
				},
				Finished = null,
				SyncObject = obj
			};
			this.Results.Add(new PSStreamObject(PSStreamObjectType.BlockingError, value));
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00044968 File Offset: 0x00042B68
		private T InvokeCmdletMethodAndWaitForResults<T>(Func<Cmdlet, T> invokeCmdletMethodAndReturnResult, out Exception exceptionThrownOnCmdletThread)
		{
			Job.<>c__DisplayClass19<T> CS$<>8__locals1 = new Job.<>c__DisplayClass19<T>();
			CS$<>8__locals1.<>4__this = this;
			T t = default(T);
			CS$<>8__locals1.closureSafeExceptionThrownOnCmdletThread = null;
			CS$<>8__locals1.resultsLock = new object();
			using (ManualResetEventSlim gotResultEvent = new ManualResetEventSlim(false))
			{
				EventHandler<JobStateEventArgs> eventHandler = delegate(object sender, JobStateEventArgs eventArgs)
				{
					if (CS$<>8__locals1.<>4__this.IsFinishedState(eventArgs.JobStateInfo.State) || eventArgs.JobStateInfo.State == JobState.Stopping)
					{
						lock (CS$<>8__locals1.resultsLock)
						{
							CS$<>8__locals1.closureSafeExceptionThrownOnCmdletThread = new OperationCanceledException();
						}
						gotResultEvent.Set();
					}
				};
				this.StateChanged += eventHandler;
				Interlocked.MemoryBarrier();
				try
				{
					eventHandler(null, new JobStateEventArgs(this.JobStateInfo));
					if (!gotResultEvent.IsSet)
					{
						this.SetJobState(JobState.Blocked);
						CmdletMethodInvoker<T> cmdletMethodInvoker = new CmdletMethodInvoker<T>
						{
							Action = invokeCmdletMethodAndReturnResult,
							Finished = gotResultEvent,
							SyncObject = CS$<>8__locals1.resultsLock
						};
						PSStreamObjectType objectType = PSStreamObjectType.ShouldMethod;
						if (typeof(T) == typeof(object))
						{
							objectType = PSStreamObjectType.BlockingError;
						}
						this.Results.Add(new PSStreamObject(objectType, cmdletMethodInvoker));
						gotResultEvent.Wait();
						this.SetJobState(JobState.Running);
						lock (CS$<>8__locals1.resultsLock)
						{
							if (CS$<>8__locals1.closureSafeExceptionThrownOnCmdletThread == null)
							{
								CS$<>8__locals1.closureSafeExceptionThrownOnCmdletThread = cmdletMethodInvoker.ExceptionThrownOnCmdletThread;
								t = cmdletMethodInvoker.MethodResult;
							}
						}
					}
				}
				finally
				{
					this.StateChanged -= eventHandler;
				}
			}
			T result;
			lock (CS$<>8__locals1.resultsLock)
			{
				exceptionThrownOnCmdletThread = CS$<>8__locals1.closureSafeExceptionThrownOnCmdletThread;
				result = t;
			}
			return result;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00044B74 File Offset: 0x00042D74
		internal virtual void ForwardAvailableResultsToCmdlet(Cmdlet cmdlet)
		{
			foreach (PSStreamObject psstreamObject in this.Results.ReadAll())
			{
				psstreamObject.WriteStreamObject(cmdlet, false);
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00044BC8 File Offset: 0x00042DC8
		internal virtual void ForwardAllResultsToCmdlet(Cmdlet cmdlet)
		{
			foreach (PSStreamObject psstreamObject in this.Results)
			{
				psstreamObject.WriteStreamObject(cmdlet, false);
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00044C18 File Offset: 0x00042E18
		protected virtual void DoLoadJobStreams()
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00044C1A File Offset: 0x00042E1A
		protected virtual void DoUnloadJobStreams()
		{
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00044C1C File Offset: 0x00042E1C
		public void LoadJobStreams()
		{
			if (this._jobStreamsLoaded)
			{
				return;
			}
			lock (this.syncObject)
			{
				if (this._jobStreamsLoaded)
				{
					return;
				}
				this._jobStreamsLoaded = true;
			}
			try
			{
				this.DoLoadJobStreams();
			}
			catch (Exception exception)
			{
				using (PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource())
				{
					traceSource.TraceException(exception);
				}
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00044CB0 File Offset: 0x00042EB0
		public void UnloadJobStreams()
		{
			if (!this._jobStreamsLoaded)
			{
				return;
			}
			lock (this.syncObject)
			{
				if (!this._jobStreamsLoaded)
				{
					return;
				}
				this._jobStreamsLoaded = false;
			}
			try
			{
				this.DoUnloadJobStreams();
			}
			catch (Exception exception)
			{
				using (PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource())
				{
					traceSource.TraceException(exception);
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00044D44 File Offset: 0x00042F44
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x00044D54 File Offset: 0x00042F54
		public PSDataCollection<PSObject> Output
		{
			get
			{
				this.LoadJobStreams();
				return this.output;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Output");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.outputOwner = false;
					this.output = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00044DB8 File Offset: 0x00042FB8
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00044DC8 File Offset: 0x00042FC8
		public PSDataCollection<ErrorRecord> Error
		{
			get
			{
				this.LoadJobStreams();
				return this.error;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Error");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.errorOwner = false;
					this.error = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00044E2C File Offset: 0x0004302C
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00044E3C File Offset: 0x0004303C
		public PSDataCollection<ProgressRecord> Progress
		{
			get
			{
				this.LoadJobStreams();
				return this.progress;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Progress");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.progressOwner = false;
					this.progress = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00044EA0 File Offset: 0x000430A0
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00044EB0 File Offset: 0x000430B0
		public PSDataCollection<VerboseRecord> Verbose
		{
			get
			{
				this.LoadJobStreams();
				return this.verbose;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Verbose");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.verboseOwner = false;
					this.verbose = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00044F14 File Offset: 0x00043114
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x00044F24 File Offset: 0x00043124
		public PSDataCollection<DebugRecord> Debug
		{
			get
			{
				this.LoadJobStreams();
				return this.debug;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Debug");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.debugOwner = false;
					this.debug = value;
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00044F80 File Offset: 0x00043180
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00044F90 File Offset: 0x00043190
		public PSDataCollection<WarningRecord> Warning
		{
			get
			{
				this.LoadJobStreams();
				return this.warning;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Warning");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.warningOwner = false;
					this.warning = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00044FF4 File Offset: 0x000431F4
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00045004 File Offset: 0x00043204
		public PSDataCollection<InformationRecord> Information
		{
			get
			{
				this.LoadJobStreams();
				return this.information;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Information");
				}
				lock (this.syncObject)
				{
					this.AssertChangesAreAccepted();
					this.informationOwner = false;
					this.information = value;
					this._jobStreamsLoaded = true;
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000C44 RID: 3140
		public abstract string Location { get; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00045068 File Offset: 0x00043268
		internal virtual bool CanDisconnect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0004506B File Offset: 0x0004326B
		internal virtual IEnumerable<RemoteRunspace> GetRunspaces()
		{
			return null;
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000C47 RID: 3143 RVA: 0x00045070 File Offset: 0x00043270
		// (remove) Token: 0x06000C48 RID: 3144 RVA: 0x000450A8 File Offset: 0x000432A8
		public event EventHandler<JobStateEventArgs> StateChanged;

		// Token: 0x06000C49 RID: 3145 RVA: 0x000450DD File Offset: 0x000432DD
		protected void SetJobState(JobState state)
		{
			this.AssertNotDisposed();
			this.SetJobState(state, null);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000450F0 File Offset: 0x000432F0
		internal void SetJobState(JobState state, Exception reason)
		{
			using (PowerShellTraceSource traceSource = PowerShellTraceSourceFactory.GetTraceSource())
			{
				this.AssertNotDisposed();
				bool flag = false;
				JobStateInfo previousJobStateInfo = this.stateInfo;
				lock (this.syncObject)
				{
					this.stateInfo = new JobStateInfo(state, reason);
					if (state == JobState.Running)
					{
						if (this.PSBeginTime == null)
						{
							this.PSBeginTime = new DateTime?(DateTime.Now);
						}
					}
					else if (this.IsFinishedState(state))
					{
						flag = true;
						if (this.PSEndTime == null)
						{
							this.PSEndTime = new DateTime?(DateTime.Now);
						}
					}
				}
				if (flag)
				{
					this.CloseAllStreams();
					if (this._processingOutput)
					{
						try
						{
							this.HandleOutputProcessingStateChanged(this, new OutputProcessingStateEventArgs(false));
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
						}
					}
				}
				try
				{
					traceSource.WriteMessage("Job", "SetJobState", Guid.Empty, this, "Invoking StateChanged event", null);
					this.StateChanged.SafeInvoke(this, new JobStateEventArgs(this.stateInfo.Clone(), previousJobStateInfo));
				}
				catch (Exception ex)
				{
					traceSource.WriteMessage("Job", "SetJobState", Guid.Empty, this, "Some Job StateChange event handler threw an unhandled exception.", null);
					traceSource.TraceException(ex);
					CommandProcessorBase.CheckForSevereException(ex);
				}
				if (flag)
				{
					lock (this.syncObject)
					{
						if (this.finished != null)
						{
							this.finished.Set();
						}
					}
				}
			}
		}

		// Token: 0x06000C4B RID: 3147
		public abstract void StopJob();

		// Token: 0x06000C4C RID: 3148 RVA: 0x000452E8 File Offset: 0x000434E8
		internal Collection<PSStreamObject> ReadAll()
		{
			this.Output.Clear();
			this.Error.Clear();
			this.Debug.Clear();
			this.Warning.Clear();
			this.Verbose.Clear();
			this.Progress.Clear();
			return this.Results.ReadAll();
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00045344 File Offset: 0x00043544
		internal bool IsFinishedState(JobState state)
		{
			bool result;
			lock (this.syncObject)
			{
				result = (state == JobState.Completed || state == JobState.Failed || state == JobState.Stopped);
			}
			return result;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00045390 File Offset: 0x00043590
		internal bool IsPersistentState(JobState state)
		{
			bool result;
			lock (this.syncObject)
			{
				result = (this.IsFinishedState(state) || state == JobState.Disconnected || state == JobState.Suspended);
			}
			return result;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x000453E0 File Offset: 0x000435E0
		private void AssertChangesAreAccepted()
		{
			this.AssertNotDisposed();
			lock (this.syncObject)
			{
				if (this.JobStateInfo.State == JobState.Running)
				{
					throw new InvalidJobStateException(JobState.Running);
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00045438 File Offset: 0x00043638
		protected string AutoGenerateJobName()
		{
			return "Job" + this.sessionId.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00045462 File Offset: 0x00043662
		internal void AssertNotDisposed()
		{
			if (this.isDisposed)
			{
				throw PSTraceSource.NewObjectDisposedException("PSJob");
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00045478 File Offset: 0x00043678
		internal void CloseAllStreams()
		{
			if (this.resultsOwner)
			{
				this.results.Complete();
			}
			if (this.outputOwner)
			{
				this.output.Complete();
			}
			if (this.errorOwner)
			{
				this.error.Complete();
			}
			if (this.progressOwner)
			{
				this.progress.Complete();
			}
			if (this.verboseOwner)
			{
				this.verbose.Complete();
			}
			if (this.warningOwner)
			{
				this.warning.Complete();
			}
			if (this.debugOwner)
			{
				this.debug.Complete();
			}
			if (this.informationOwner)
			{
				this.information.Complete();
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00045520 File Offset: 0x00043720
		internal List<Job> GetJobsForLocation(string location)
		{
			List<Job> list = new List<Job>();
			foreach (Job job in this.ChildJobs)
			{
				if (string.Equals(job.Location, location, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(job);
				}
			}
			return list;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00045584 File Offset: 0x00043784
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00045594 File Offset: 0x00043794
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.isDisposed)
			{
				this.CloseAllStreams();
				lock (this.syncObject)
				{
					if (this.finished != null)
					{
						this.finished.Dispose();
						this.finished = null;
					}
				}
				if (this.resultsOwner)
				{
					this.results.Dispose();
				}
				if (this.outputOwner)
				{
					this.output.Dispose();
				}
				if (this.errorOwner)
				{
					this.error.Dispose();
				}
				if (this.debugOwner)
				{
					this.debug.Dispose();
				}
				if (this.informationOwner)
				{
					this.information.Dispose();
				}
				if (this.verboseOwner)
				{
					this.verbose.Dispose();
				}
				if (this.warningOwner)
				{
					this.warning.Dispose();
				}
				if (this.progressOwner)
				{
					this.progress.Dispose();
				}
				this.isDisposed = true;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000C56 RID: 3158 RVA: 0x000456A0 File Offset: 0x000438A0
		// (remove) Token: 0x06000C57 RID: 3159 RVA: 0x000456D8 File Offset: 0x000438D8
		internal event EventHandler<OutputProcessingStateEventArgs> OutputProcessingStateChanged;

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0004570D File Offset: 0x0004390D
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00045715 File Offset: 0x00043915
		internal bool MonitorOutputProcessing { get; set; }

		// Token: 0x06000C5A RID: 3162 RVA: 0x0004571E File Offset: 0x0004391E
		internal void SetMonitorOutputProcessing(IOutputProcessingState outputProcessingState)
		{
			if (outputProcessingState != null)
			{
				outputProcessingState.OutputProcessingStateChanged += this.HandleOutputProcessingStateChanged;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00045735 File Offset: 0x00043935
		internal void RemoveMonitorOutputProcessing(IOutputProcessingState outputProcessingState)
		{
			if (outputProcessingState != null)
			{
				outputProcessingState.OutputProcessingStateChanged -= this.HandleOutputProcessingStateChanged;
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0004574C File Offset: 0x0004394C
		private void HandleOutputProcessingStateChanged(object sender, OutputProcessingStateEventArgs e)
		{
			this._processingOutput = e.ProcessingOutput;
			this.OutputProcessingStateChanged.SafeInvoke(this, e);
		}

		// Token: 0x04000557 RID: 1367
		private readonly string command;

		// Token: 0x04000558 RID: 1368
		private JobStateInfo stateInfo = new JobStateInfo(JobState.NotStarted);

		// Token: 0x04000559 RID: 1369
		private ManualResetEvent finished = new ManualResetEvent(false);

		// Token: 0x0400055A RID: 1370
		private Guid guid = Guid.NewGuid();

		// Token: 0x0400055B RID: 1371
		private readonly int sessionId;

		// Token: 0x0400055C RID: 1372
		private string name;

		// Token: 0x0400055D RID: 1373
		private IList<Job> childJobs;

		// Token: 0x0400055E RID: 1374
		internal readonly object syncObject = new object();

		// Token: 0x0400055F RID: 1375
		private PSDataCollection<PSStreamObject> results = new PSDataCollection<PSStreamObject>();

		// Token: 0x04000560 RID: 1376
		private bool resultsOwner = true;

		// Token: 0x04000561 RID: 1377
		private PSDataCollection<ErrorRecord> error = new PSDataCollection<ErrorRecord>();

		// Token: 0x04000562 RID: 1378
		private bool errorOwner = true;

		// Token: 0x04000563 RID: 1379
		private PSDataCollection<ProgressRecord> progress = new PSDataCollection<ProgressRecord>();

		// Token: 0x04000564 RID: 1380
		private bool progressOwner = true;

		// Token: 0x04000565 RID: 1381
		private PSDataCollection<VerboseRecord> verbose = new PSDataCollection<VerboseRecord>();

		// Token: 0x04000566 RID: 1382
		private bool verboseOwner = true;

		// Token: 0x04000567 RID: 1383
		private PSDataCollection<WarningRecord> warning = new PSDataCollection<WarningRecord>();

		// Token: 0x04000568 RID: 1384
		private bool warningOwner = true;

		// Token: 0x04000569 RID: 1385
		private PSDataCollection<DebugRecord> debug = new PSDataCollection<DebugRecord>();

		// Token: 0x0400056A RID: 1386
		private bool debugOwner = true;

		// Token: 0x0400056B RID: 1387
		private PSDataCollection<InformationRecord> information = new PSDataCollection<InformationRecord>();

		// Token: 0x0400056C RID: 1388
		private bool informationOwner = true;

		// Token: 0x0400056D RID: 1389
		private PSDataCollection<PSObject> output = new PSDataCollection<PSObject>();

		// Token: 0x0400056E RID: 1390
		private bool outputOwner = true;

		// Token: 0x0400056F RID: 1391
		private static int _jobIdSeed;

		// Token: 0x04000570 RID: 1392
		private DateTime? _beginTime = null;

		// Token: 0x04000571 RID: 1393
		private DateTime? _endTime = null;

		// Token: 0x04000572 RID: 1394
		private string _jobTypeName = string.Empty;

		// Token: 0x04000573 RID: 1395
		private bool suppressOutputForwarding;

		// Token: 0x04000574 RID: 1396
		private bool propagateThrows;

		// Token: 0x04000575 RID: 1397
		private Lazy<int> parentActivityId;

		// Token: 0x04000576 RID: 1398
		private bool _jobStreamsLoaded;

		// Token: 0x04000578 RID: 1400
		private bool isDisposed;

		// Token: 0x0400057A RID: 1402
		private bool _processingOutput;
	}
}
