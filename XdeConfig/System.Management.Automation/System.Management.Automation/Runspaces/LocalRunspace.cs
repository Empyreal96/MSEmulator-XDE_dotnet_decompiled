using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000212 RID: 530
	internal sealed class LocalRunspace : RunspaceBase
	{
		// Token: 0x060018CE RID: 6350 RVA: 0x00096F82 File Offset: 0x00095182
		internal LocalRunspace(PSHost host, RunspaceConfiguration runspaceConfig) : base(host, runspaceConfig)
		{
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00096F8C File Offset: 0x0009518C
		internal LocalRunspace(PSHost host, InitialSessionState initialSessionState, bool suppressClone) : base(host, initialSessionState, suppressClone)
		{
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00096F97 File Offset: 0x00095197
		internal LocalRunspace(PSHost host, InitialSessionState initialSessionState) : base(host, initialSessionState)
		{
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00096FA4 File Offset: 0x000951A4
		public override PSPrimitiveDictionary GetApplicationPrivateData()
		{
			if (this.applicationPrivateData == null)
			{
				lock (base.SyncRoot)
				{
					if (this.applicationPrivateData == null)
					{
						this.applicationPrivateData = new PSPrimitiveDictionary();
					}
				}
			}
			return this.applicationPrivateData;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00097000 File Offset: 0x00095200
		internal override void SetApplicationPrivateData(PSPrimitiveDictionary applicationPrivateData)
		{
			this.applicationPrivateData = applicationPrivateData;
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x0009700C File Offset: 0x0009520C
		public override PSEventManager Events
		{
			get
			{
				ExecutionContext getExecutionContext = this.GetExecutionContext;
				if (getExecutionContext == null)
				{
					return null;
				}
				return getExecutionContext.Events;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0009702B File Offset: 0x0009522B
		// (set) Token: 0x060018D5 RID: 6357 RVA: 0x00097034 File Offset: 0x00095234
		public override PSThreadOptions ThreadOptions
		{
			get
			{
				return this.createThreadOptions;
			}
			set
			{
				lock (base.SyncRoot)
				{
					if (value != this.createThreadOptions)
					{
						if (this.RunspaceStateInfo.State != RunspaceState.BeforeOpen && ((base.ApartmentState != ApartmentState.MTA && base.ApartmentState != ApartmentState.Unknown) || value != PSThreadOptions.ReuseThread))
						{
							throw new InvalidOperationException(StringUtil.Format(RunspaceStrings.InvalidThreadOptionsChange, new object[0]));
						}
						this.createThreadOptions = value;
					}
				}
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000970C0 File Offset: 0x000952C0
		public override void ResetRunspaceState()
		{
			PSInvalidOperationException ex = null;
			if (this.InitialSessionState == null)
			{
				ex = PSTraceSource.NewInvalidOperationException();
			}
			else if (base.RunspaceState != RunspaceState.Opened)
			{
				ex = PSTraceSource.NewInvalidOperationException(RunspaceStrings.RunspaceNotInOpenedState, new object[]
				{
					base.RunspaceState
				});
			}
			else if (this.RunspaceAvailability != RunspaceAvailability.Available)
			{
				ex = PSTraceSource.NewInvalidOperationException(RunspaceStrings.ConcurrentInvokeNotAllowed, new object[0]);
			}
			if (ex != null)
			{
				ex.Source = "ResetRunspaceState";
				throw ex;
			}
			this.InitialSessionState.ResetRunspaceState(base.ExecutionContext);
			this._history = new History(base.ExecutionContext);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00097157 File Offset: 0x00095357
		protected override Pipeline CoreCreatePipeline(string command, bool addToHistory, bool isNested)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			return new LocalPipeline(this, command, addToHistory, isNested);
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00097175 File Offset: 0x00095375
		internal override ExecutionContext GetExecutionContext
		{
			get
			{
				if (this._engine == null)
				{
					return null;
				}
				return this._engine.Context;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x0009718C File Offset: 0x0009538C
		internal override bool InNestedPrompt
		{
			get
			{
				ExecutionContext getExecutionContext = this.GetExecutionContext;
				return getExecutionContext != null && getExecutionContext.InternalHost.HostInNestedPrompt();
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x000971B0 File Offset: 0x000953B0
		internal CommandFactory CommandFactory
		{
			get
			{
				return this._commandFactory;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x000971B8 File Offset: 0x000953B8
		internal History History
		{
			get
			{
				return this._history;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x000971C0 File Offset: 0x000953C0
		internal TranscriptionData TranscriptionData
		{
			get
			{
				return this._transcriptionData;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x000971C8 File Offset: 0x000953C8
		internal JobRepository JobRepository
		{
			get
			{
				return this._jobRepository;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x000971D0 File Offset: 0x000953D0
		public override JobManager JobManager
		{
			get
			{
				return this._jobManager;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x000971D8 File Offset: 0x000953D8
		internal RunspaceRepository RunspaceRepository
		{
			get
			{
				return this._runspaceRepository;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x000971E0 File Offset: 0x000953E0
		public override Debugger Debugger
		{
			get
			{
				return base.InternalDebugger ?? base.Debugger;
			}
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x000971F4 File Offset: 0x000953F4
		private static LocalRunspace.DebugPreference CreateDebugPreference(string[] AppDomainNames)
		{
			return new LocalRunspace.DebugPreference
			{
				AppDomainNames = AppDomainNames
			};
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00097210 File Offset: 0x00095410
		internal static void SetDebugPreference(string processName, List<string> appDomainName, bool enable)
		{
			lock (LocalRunspace.DebugPreferenceLockObject)
			{
				bool flag2 = false;
				Hashtable hashtable = null;
				string[] appDomainNames = null;
				if (appDomainName != null)
				{
					appDomainNames = appDomainName.ToArray();
				}
				if (!File.Exists(LocalRunspace.DebugPreferenceCachePath))
				{
					if (enable)
					{
						LocalRunspace.DebugPreference value = LocalRunspace.CreateDebugPreference(appDomainNames);
						hashtable = new Hashtable();
						hashtable.Add(processName, value);
						flag2 = true;
					}
				}
				else
				{
					hashtable = LocalRunspace.GetDebugPreferenceCache(null);
					if (hashtable != null)
					{
						if (enable)
						{
							if (!hashtable.ContainsKey(processName))
							{
								LocalRunspace.DebugPreference value2 = LocalRunspace.CreateDebugPreference(appDomainNames);
								hashtable.Add(processName, value2);
								flag2 = true;
							}
							else
							{
								LocalRunspace.DebugPreference processSpecificDebugPreference = LocalRunspace.GetProcessSpecificDebugPreference(hashtable[processName]);
								if (processSpecificDebugPreference != null)
								{
									List<string> list = null;
									if (processSpecificDebugPreference.AppDomainNames != null && processSpecificDebugPreference.AppDomainNames.Length > 0)
									{
										list = new List<string>(processSpecificDebugPreference.AppDomainNames);
										foreach (string text in appDomainName)
										{
											if (!list.Contains(text, StringComparer.OrdinalIgnoreCase))
											{
												list.Add(text);
												flag2 = true;
											}
										}
									}
									if (flag2)
									{
										LocalRunspace.DebugPreference value3 = LocalRunspace.CreateDebugPreference(list.ToArray());
										hashtable[processName] = value3;
									}
								}
							}
						}
						else if (hashtable.ContainsKey(processName))
						{
							if (appDomainName == null)
							{
								hashtable.Remove(processName);
								flag2 = true;
							}
							else
							{
								LocalRunspace.DebugPreference processSpecificDebugPreference2 = LocalRunspace.GetProcessSpecificDebugPreference(hashtable[processName]);
								if (processSpecificDebugPreference2 != null)
								{
									List<string> list2 = null;
									if (processSpecificDebugPreference2.AppDomainNames != null && processSpecificDebugPreference2.AppDomainNames.Length > 0)
									{
										list2 = new List<string>(processSpecificDebugPreference2.AppDomainNames);
										foreach (string text2 in appDomainName)
										{
											if (list2.Contains(text2, StringComparer.OrdinalIgnoreCase))
											{
												list2.Remove(text2);
												flag2 = true;
											}
										}
									}
									if (flag2)
									{
										LocalRunspace.DebugPreference value4 = LocalRunspace.CreateDebugPreference(list2.ToArray());
										hashtable[processName] = value4;
									}
								}
							}
						}
					}
					else if (enable)
					{
						hashtable = new Hashtable();
						LocalRunspace.DebugPreference value5 = LocalRunspace.CreateDebugPreference(appDomainNames);
						hashtable.Add(processName, value5);
						flag2 = true;
					}
				}
				if (flag2)
				{
					using (PowerShell powerShell = PowerShell.Create())
					{
						powerShell.AddCommand("Export-Clixml").AddParameter("Path", LocalRunspace.DebugPreferenceCachePath).AddParameter("InputObject", hashtable);
						powerShell.Invoke();
					}
				}
			}
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x000974D8 File Offset: 0x000956D8
		private static Hashtable GetDebugPreferenceCache(Runspace runspace)
		{
			Hashtable result = null;
			using (PowerShell powerShell = PowerShell.Create())
			{
				if (runspace != null)
				{
					powerShell.Runspace = runspace;
				}
				powerShell.AddCommand("Import-Clixml").AddParameter("Path", LocalRunspace.DebugPreferenceCachePath);
				Collection<PSObject> collection = powerShell.Invoke();
				if (collection != null && collection.Count == 1)
				{
					result = (collection[0].BaseObject as Hashtable);
				}
			}
			return result;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00097554 File Offset: 0x00095754
		private static LocalRunspace.DebugPreference GetProcessSpecificDebugPreference(object debugPreference)
		{
			LocalRunspace.DebugPreference result = null;
			if (debugPreference != null)
			{
				PSObject psobject = debugPreference as PSObject;
				if (psobject != null)
				{
					result = LanguagePrimitives.ConvertTo<LocalRunspace.DebugPreference>(psobject);
				}
			}
			return result;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00097578 File Offset: 0x00095778
		protected override void OpenHelper(bool syncCall)
		{
			if (syncCall)
			{
				this.DoOpenHelper();
				return;
			}
			Thread thread = new Thread(new ThreadStart(this.OpenThreadProc));
			thread.Start();
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x000975A8 File Offset: 0x000957A8
		private void OpenThreadProc()
		{
			try
			{
				this.DoOpenHelper();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x000975D8 File Offset: 0x000957D8
		private void DoOpenHelper()
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			bool flag = false;
			LocalRunspace.runspaceInitTracer.WriteLine("begin open runspace", new object[0]);
			try
			{
				this._transcriptionData = new TranscriptionData();
				if (this.InitialSessionState != null)
				{
					this._engine = new AutomationEngine(base.Host, null, this.InitialSessionState);
				}
				else
				{
					this._engine = new AutomationEngine(base.Host, this.RunspaceConfiguration, null);
				}
				this._engine.Context.CurrentRunspace = this;
				MshLog.LogEngineLifecycleEvent(this._engine.Context, EngineState.Available);
				flag = true;
				this._commandFactory = new CommandFactory(this._engine.Context);
				this._history = new History(this._engine.Context);
				this._jobRepository = new JobRepository();
				this._jobManager = new JobManager();
				this._runspaceRepository = new RunspaceRepository();
				LocalRunspace.runspaceInitTracer.WriteLine("initializing built-in aliases and variable information", new object[0]);
				this.InitializeDefaults();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				LocalRunspace.runspaceInitTracer.WriteLine("Runspace open failed", new object[0]);
				this.LogEngineHealthEvent(ex);
				if (flag)
				{
					MshLog.LogEngineLifecycleEvent(this._engine.Context, EngineState.Stopped);
				}
				base.SetRunspaceState(RunspaceState.Broken, ex);
				base.RaiseRunspaceStateEvents();
				throw;
			}
			base.SetRunspaceState(RunspaceState.Opened);
			this.RunspaceOpening.Set();
			base.RaiseRunspaceStateEvents();
			LocalRunspace.runspaceInitTracer.WriteLine("runspace opened successfully", new object[0]);
			if (this.InitialSessionState != null)
			{
				Exception ex2 = this.InitialSessionState.BindRunspace(this, LocalRunspace.runspaceInitTracer);
				if (ex2 != null)
				{
					this.LogEngineHealthEvent(ex2);
					MshLog.LogEngineLifecycleEvent(this._engine.Context, EngineState.Stopped);
					base.SetRunspaceState(RunspaceState.Broken, ex2);
					base.RaiseRunspaceStateEvents();
					throw ex2;
				}
			}
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x000977AC File Offset: 0x000959AC
		internal void LogEngineHealthEvent(Exception exception)
		{
			this.LogEngineHealthEvent(exception, Severity.Error, 103, null);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x000977BC File Offset: 0x000959BC
		internal void LogEngineHealthEvent(Exception exception, Severity severity, int id, Dictionary<string, string> additionalInfo)
		{
			LogContext logContext = new LogContext();
			logContext.EngineVersion = this.Version.ToString();
			logContext.HostId = base.Host.InstanceId.ToString();
			logContext.HostName = base.Host.Name;
			logContext.HostVersion = base.Host.Version.ToString();
			logContext.RunspaceId = base.InstanceId.ToString();
			logContext.Severity = severity.ToString();
			if (this.RunspaceConfiguration == null)
			{
				logContext.ShellId = Utils.DefaultPowerShellShellID;
			}
			else
			{
				logContext.ShellId = this.RunspaceConfiguration.ShellId;
			}
			MshLog.LogEngineHealthEvent(logContext, id, exception, additionalInfo);
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00097881 File Offset: 0x00095A81
		internal PipelineThread GetPipelineThread()
		{
			if (this.pipelineThread == null)
			{
				this.pipelineThread = new PipelineThread(base.ApartmentState);
			}
			return this.pipelineThread;
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x000978A4 File Offset: 0x00095AA4
		protected override void CloseHelper(bool syncCall)
		{
			if (syncCall)
			{
				this.DoCloseHelper();
				return;
			}
			Thread thread = new Thread(new ThreadStart(this.CloseThreadProc));
			thread.Start();
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x000978D4 File Offset: 0x00095AD4
		private void CloseThreadProc()
		{
			try
			{
				this.DoCloseHelper();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00097970 File Offset: 0x00095B70
		private void DoCloseHelper()
		{
			ExecutionContext getExecutionContext = this.GetExecutionContext;
			if (getExecutionContext != null)
			{
				Runspace runspace = null;
				try
				{
					runspace = getExecutionContext.EngineHostInterface.Runspace;
				}
				catch (PSNotImplementedException)
				{
				}
				if (runspace == null || this == runspace)
				{
					PSHostUserInterface ui = getExecutionContext.EngineHostInterface.UI;
					if (ui != null)
					{
						ui.StopAllTranscribing();
					}
				}
			}
			if (this.Events != null)
			{
				this.Events.GenerateEvent("PowerShell.Exiting", null, new object[0], null, true, false);
			}
			base.StopPipelines();
			this.StopOrDisconnectAllJobs();
			this.CloseOrDisconnectAllRemoteRunspaces(delegate
			{
				List<RemoteRunspace> list = new List<RemoteRunspace>();
				foreach (PSSession pssession in this.RunspaceRepository.Runspaces)
				{
					list.Add(pssession.Runspace as RemoteRunspace);
				}
				return list;
			});
			this._engine.Context.RunspaceClosingNotification();
			MshLog.LogEngineLifecycleEvent(this._engine.Context, EngineState.Stopped);
			AmsiUtils.Uninitialize();
			this._engine = null;
			this._commandFactory = null;
			base.SetRunspaceState(RunspaceState.Closed);
			base.RaiseRunspaceStateEvents();
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00097A60 File Offset: 0x00095C60
		private void CloseOrDisconnectAllRemoteRunspaces(Func<List<RemoteRunspace>> getRunspaces)
		{
			List<RemoteRunspace> list = getRunspaces();
			if (list.Count == 0)
			{
				return;
			}
			using (ManualResetEvent remoteRunspaceCloseCompleted = new ManualResetEvent(false))
			{
				ThrottleManager throttleManager = new ThrottleManager();
				throttleManager.ThrottleComplete += delegate(object sender, EventArgs e)
				{
					remoteRunspaceCloseCompleted.Set();
				};
				foreach (RemoteRunspace remoteRunspace in list)
				{
					IThrottleOperation operation = new CloseOrDisconnectRunspaceOperationHelper(remoteRunspace);
					throttleManager.AddOperation(operation);
				}
				throttleManager.EndSubmitOperations();
				remoteRunspaceCloseCompleted.WaitOne();
			}
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00097B60 File Offset: 0x00095D60
		private void StopOrDisconnectAllJobs()
		{
			LocalRunspace.<>c__DisplayClassa CS$<>8__locals1 = new LocalRunspace.<>c__DisplayClassa();
			if (this.JobRepository.Jobs.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.disconnectRunspaces = new List<RemoteRunspace>();
			using (ManualResetEvent jobsStopCompleted = new ManualResetEvent(false))
			{
				ThrottleManager throttleManager = new ThrottleManager();
				throttleManager.ThrottleComplete += delegate(object sender, EventArgs e)
				{
					jobsStopCompleted.Set();
				};
				foreach (Job job in this.JobRepository.Jobs)
				{
					if (job is PSRemotingJob)
					{
						if (!job.CanDisconnect)
						{
							throttleManager.AddOperation(new StopJobOperationHelper(job));
						}
						else if (job.JobStateInfo.State == JobState.Running)
						{
							IEnumerable<RemoteRunspace> runspaces = job.GetRunspaces();
							if (runspaces != null)
							{
								CS$<>8__locals1.disconnectRunspaces.AddRange(runspaces);
							}
						}
					}
				}
				throttleManager.EndSubmitOperations();
				jobsStopCompleted.WaitOne();
			}
			this.CloseOrDisconnectAllRemoteRunspaces(() => CS$<>8__locals1.disconnectRunspaces);
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00097CA4 File Offset: 0x00095EA4
		internal void ReleaseDebugger()
		{
			Debugger debugger = this.Debugger;
			if (debugger != null)
			{
				try
				{
					if (debugger.UnhandledBreakpointMode == UnhandledBreakpointProcessingMode.Wait)
					{
						debugger.UnhandledBreakpointMode = UnhandledBreakpointProcessingMode.Ignore;
					}
				}
				catch (PSNotImplementedException)
				{
				}
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00097CE0 File Offset: 0x00095EE0
		protected override void DoSetVariable(string name, object value)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			this._engine.Context.EngineSessionState.SetVariableValue(name, value, CommandOrigin.Internal);
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00097D0D File Offset: 0x00095F0D
		protected override object DoGetVariable(string name)
		{
			if (this._disposed)
			{
				throw PSTraceSource.NewObjectDisposedException("runspace");
			}
			return this._engine.Context.EngineSessionState.GetVariableValue(name);
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00097D38 File Offset: 0x00095F38
		protected override List<string> DoApplications
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.EngineSessionState.Applications;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00097D62 File Offset: 0x00095F62
		protected override List<string> DoScripts
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.EngineSessionState.Scripts;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00097D8C File Offset: 0x00095F8C
		protected override DriveManagementIntrinsics DoDrive
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.SessionState.Drive;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00097DB6 File Offset: 0x00095FB6
		// (set) Token: 0x060018F7 RID: 6391 RVA: 0x00097DE0 File Offset: 0x00095FE0
		protected override PSLanguageMode DoLanguageMode
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.SessionState.LanguageMode;
			}
			set
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				this._engine.Context.SessionState.LanguageMode = value;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00097E0B File Offset: 0x0009600B
		protected override PSModuleInfo DoModule
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.EngineSessionState.Module;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x00097E35 File Offset: 0x00096035
		protected override PathIntrinsics DoPath
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.SessionState.Path;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x00097E5F File Offset: 0x0009605F
		protected override CmdletProviderManagementIntrinsics DoProvider
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.SessionState.Provider;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x00097E89 File Offset: 0x00096089
		protected override PSVariableIntrinsics DoPSVariable
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.SessionState.PSVariable;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x00097EB3 File Offset: 0x000960B3
		protected override CommandInvocationIntrinsics DoInvokeCommand
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.EngineIntrinsics.InvokeCommand;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x00097EDD File Offset: 0x000960DD
		protected override ProviderIntrinsics DoInvokeProvider
		{
			get
			{
				if (this._disposed)
				{
					throw PSTraceSource.NewObjectDisposedException("runspace");
				}
				return this._engine.Context.EngineIntrinsics.InvokeProvider;
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00097F08 File Offset: 0x00096108
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					lock (base.SyncRoot)
					{
						if (this._disposed)
						{
							return;
						}
						this._disposed = true;
					}
					if (disposing)
					{
						this.Close();
						this._engine = null;
						this._history = null;
						this._transcriptionData = null;
						this._jobManager = null;
						this._jobRepository = null;
						this._runspaceRepository = null;
						if (this.RunspaceOpening != null)
						{
							this.RunspaceOpening.Dispose();
							this.RunspaceOpening = null;
						}
						if (base.ExecutionContext != null && base.ExecutionContext.Events != null)
						{
							try
							{
								base.ExecutionContext.Events.Dispose();
							}
							catch (ObjectDisposedException)
							{
							}
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00097FFC File Offset: 0x000961FC
		public override void Close()
		{
			base.Close();
			if (this.pipelineThread != null)
			{
				this.pipelineThread.Close();
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00098017 File Offset: 0x00096217
		internal AutomationEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x00098020 File Offset: 0x00096220
		private void InitializeDefaults()
		{
			SessionStateInternal engineSessionState = this._engine.Context.EngineSessionState;
			engineSessionState.InitializeFixedVariables();
			if (this.RunspaceConfiguration != null)
			{
				bool addSetStrictMode = true;
				foreach (RunspaceConfigurationEntry runspaceConfigurationEntry in ((IEnumerable<CmdletConfigurationEntry>)this.RunspaceConfiguration.Cmdlets))
				{
					if (runspaceConfigurationEntry.Name.Equals("Set-StrictMode", StringComparison.OrdinalIgnoreCase))
					{
						addSetStrictMode = false;
						break;
					}
				}
				engineSessionState.AddBuiltInEntries(addSetStrictMode);
			}
		}

		// Token: 0x04000A41 RID: 2625
		private PSPrimitiveDictionary applicationPrivateData;

		// Token: 0x04000A42 RID: 2626
		private PSThreadOptions createThreadOptions;

		// Token: 0x04000A43 RID: 2627
		private TranscriptionData _transcriptionData;

		// Token: 0x04000A44 RID: 2628
		private JobRepository _jobRepository;

		// Token: 0x04000A45 RID: 2629
		private JobManager _jobManager;

		// Token: 0x04000A46 RID: 2630
		private RunspaceRepository _runspaceRepository;

		// Token: 0x04000A47 RID: 2631
		private static string DebugPreferenceCachePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsPowerShell"), "DebugPreference.clixml");

		// Token: 0x04000A48 RID: 2632
		private static object DebugPreferenceLockObject = new object();

		// Token: 0x04000A49 RID: 2633
		private PipelineThread pipelineThread;

		// Token: 0x04000A4A RID: 2634
		private bool _disposed;

		// Token: 0x04000A4B RID: 2635
		private CommandFactory _commandFactory;

		// Token: 0x04000A4C RID: 2636
		private AutomationEngine _engine;

		// Token: 0x04000A4D RID: 2637
		private History _history;

		// Token: 0x04000A4E RID: 2638
		[TraceSource("RunspaceInit", "Initialization code for Runspace")]
		private static PSTraceSource runspaceInitTracer = PSTraceSource.GetTracer("RunspaceInit", "Initialization code for Runspace", false);

		// Token: 0x04000A4F RID: 2639
		private static RemoteSessionNamedPipeServer IPCNamedPipeServer = RemoteSessionNamedPipeServer.IPCNamedPipeServer;

		// Token: 0x02000213 RID: 531
		public class DebugPreference
		{
			// Token: 0x04000A50 RID: 2640
			public string[] AppDomainNames;
		}
	}
}
