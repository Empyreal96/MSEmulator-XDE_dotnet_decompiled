using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020000EB RID: 235
	public abstract class Debugger
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000CD2 RID: 3282 RVA: 0x0004655C File Offset: 0x0004475C
		// (remove) Token: 0x06000CD3 RID: 3283 RVA: 0x00046594 File Offset: 0x00044794
		public event EventHandler<DebuggerStopEventArgs> DebuggerStop;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000CD4 RID: 3284 RVA: 0x000465CC File Offset: 0x000447CC
		// (remove) Token: 0x06000CD5 RID: 3285 RVA: 0x00046604 File Offset: 0x00044804
		public event EventHandler<BreakpointUpdatedEventArgs> BreakpointUpdated;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000CD6 RID: 3286 RVA: 0x0004663C File Offset: 0x0004483C
		// (remove) Token: 0x06000CD7 RID: 3287 RVA: 0x00046674 File Offset: 0x00044874
		internal event EventHandler<EventArgs> NestedDebuggingCancelledEvent;

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000466A9 File Offset: 0x000448A9
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x000466B1 File Offset: 0x000448B1
		private protected bool DebuggerStopped { protected get; private set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000466BA File Offset: 0x000448BA
		internal virtual bool IsPushed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000466BD File Offset: 0x000448BD
		internal virtual bool IsRemote
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000466C0 File Offset: 0x000448C0
		internal virtual bool IsPendingDebugStopEvent
		{
			get
			{
				throw new PSNotImplementedException();
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x000466C7 File Offset: 0x000448C7
		internal virtual bool IsDebuggerSteppingEnabled
		{
			get
			{
				throw new PSNotImplementedException();
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000466CE File Offset: 0x000448CE
		internal bool IsDebugHandlerSubscribed
		{
			get
			{
				return this.DebuggerStop != null;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x000466DC File Offset: 0x000448DC
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x000466E3 File Offset: 0x000448E3
		internal virtual UnhandledBreakpointProcessingMode UnhandledBreakpointMode
		{
			get
			{
				throw new PSNotImplementedException();
			}
			set
			{
				throw new PSNotImplementedException();
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x000466EA File Offset: 0x000448EA
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x000466F2 File Offset: 0x000448F2
		public DebugModes DebugMode
		{
			get
			{
				return this._debugMode;
			}
			protected set
			{
				this._debugMode = value;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x000466FB File Offset: 0x000448FB
		public virtual bool IsActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000466FE File Offset: 0x000448FE
		public virtual Guid InstanceId
		{
			get
			{
				return Debugger._instanceId;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00046705 File Offset: 0x00044905
		public virtual bool InBreakpoint
		{
			get
			{
				return this.DebuggerStopped;
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00046710 File Offset: 0x00044910
		protected void RaiseDebuggerStopEvent(DebuggerStopEventArgs args)
		{
			try
			{
				this.DebuggerStopped = true;
				this.DebuggerStop.SafeInvoke(this, args);
			}
			finally
			{
				this.DebuggerStopped = false;
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0004674C File Offset: 0x0004494C
		protected bool IsDebuggerStopEventSubscribed()
		{
			return this.DebuggerStop != null;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0004675A File Offset: 0x0004495A
		protected void RaiseBreakpointUpdatedEvent(BreakpointUpdatedEventArgs args)
		{
			this.BreakpointUpdated.SafeInvoke(this, args);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00046769 File Offset: 0x00044969
		protected bool IsDebuggerBreakpointUpdatedEventSubscribed()
		{
			return this.BreakpointUpdated != null;
		}

		// Token: 0x06000CEA RID: 3306
		public abstract DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output);

		// Token: 0x06000CEB RID: 3307
		public abstract void SetDebuggerAction(DebuggerResumeAction resumeAction);

		// Token: 0x06000CEC RID: 3308
		public abstract void StopProcessCommand();

		// Token: 0x06000CED RID: 3309
		public abstract DebuggerStopEventArgs GetDebuggerStopArgs();

		// Token: 0x06000CEE RID: 3310 RVA: 0x00046777 File Offset: 0x00044977
		public virtual void SetParent(Debugger parent, IEnumerable<Breakpoint> breakPoints, DebuggerResumeAction? startAction, PSHost host, PathInfo path)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0004677E File Offset: 0x0004497E
		public virtual void SetParent(Debugger parent, IEnumerable<Breakpoint> breakPoints, DebuggerResumeAction? startAction, PSHost host, PathInfo path, Dictionary<string, DebugSource> functionSourceMap)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00046785 File Offset: 0x00044985
		public virtual void SetDebugMode(DebugModes mode)
		{
			if (mode != DebugModes.None)
			{
				this.DebugMode = mode;
				return;
			}
			this.DebugMode = mode;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00046799 File Offset: 0x00044999
		public virtual IEnumerable<CallStackFrame> GetCallStack()
		{
			return new Collection<CallStackFrame>();
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000467A0 File Offset: 0x000449A0
		public virtual void SetBreakpoints(IEnumerable<Breakpoint> breakpoints)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000467A7 File Offset: 0x000449A7
		public virtual void ResetCommandProcessorSource()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000467AE File Offset: 0x000449AE
		public virtual void SetDebuggerStepMode(bool enabled)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x000467B5 File Offset: 0x000449B5
		internal virtual DebuggerCommand InternalProcessCommand(string command, IList<PSObject> output)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000467BC File Offset: 0x000449BC
		internal virtual bool InternalProcessListCommand(int lineNum, IList<PSObject> output)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000467C3 File Offset: 0x000449C3
		internal virtual void DebugJob(Job job)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000467CA File Offset: 0x000449CA
		internal virtual void StopDebugJob(Job job)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000467D1 File Offset: 0x000449D1
		internal virtual CallStackFrame[] GetActiveDebuggerCallStack()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000467D8 File Offset: 0x000449D8
		internal virtual void StartMonitoringRunspace(PSMonitorRunspaceInfo args)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000467DF File Offset: 0x000449DF
		internal virtual void EndMonitoringRunspace(PSMonitorRunspaceInfo args)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000467E6 File Offset: 0x000449E6
		internal virtual void ReleaseSavedDebugStop()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x000467ED File Offset: 0x000449ED
		internal virtual void DebugRunspace(Runspace runspace)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x000467F4 File Offset: 0x000449F4
		internal virtual void StopDebugRunspace(Runspace runspace)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00046834 File Offset: 0x00044A34
		internal void RaiseNestedDebuggingCancelEvent()
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				try
				{
					this.NestedDebuggingCancelledEvent.SafeInvoke(this, null);
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			});
		}

		// Token: 0x040005C1 RID: 1473
		internal const string CannotProcessCommandNotStopped = "Debugger:CannotProcessCommandNotStopped";

		// Token: 0x040005C2 RID: 1474
		internal const string CannotEnableDebuggerSteppingInvalidMode = "Debugger:CannotEnableDebuggerSteppingInvalidMode";

		// Token: 0x040005C6 RID: 1478
		private DebugModes _debugMode = DebugModes.Default;

		// Token: 0x040005C7 RID: 1479
		private static readonly Guid _instanceId = default(Guid);
	}
}
