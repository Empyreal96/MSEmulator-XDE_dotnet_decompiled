using System;
using System.Collections.Generic;
using System.Management.Automation.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000279 RID: 633
	internal sealed class RemotingJobDebugger : Debugger
	{
		// Token: 0x06001DE5 RID: 7653 RVA: 0x000AC455 File Offset: 0x000AA655
		private RemotingJobDebugger()
		{
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000AC460 File Offset: 0x000AA660
		public RemotingJobDebugger(Debugger debugger, Runspace runspace, string jobName)
		{
			if (debugger == null)
			{
				throw new PSArgumentNullException("debugger");
			}
			if (runspace == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			this._wrappedDebugger = debugger;
			this._runspace = runspace;
			this._jobName = (jobName ?? string.Empty);
			this._wrappedDebugger.BreakpointUpdated += this.HandleBreakpointUpdated;
			this._wrappedDebugger.DebuggerStop += this.HandleDebuggerStop;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000AC4DB File Offset: 0x000AA6DB
		public override DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output)
		{
			if (command.Commands[0].CommandText.Trim().Equals("prompt", StringComparison.OrdinalIgnoreCase))
			{
				return this.HandlePromptCommand(output);
			}
			return this._wrappedDebugger.ProcessCommand(command, output);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000AC515 File Offset: 0x000AA715
		public override void SetDebuggerAction(DebuggerResumeAction resumeAction)
		{
			this._wrappedDebugger.SetDebuggerAction(resumeAction);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000AC523 File Offset: 0x000AA723
		public override void StopProcessCommand()
		{
			this._wrappedDebugger.StopProcessCommand();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000AC530 File Offset: 0x000AA730
		public override DebuggerStopEventArgs GetDebuggerStopArgs()
		{
			return this._wrappedDebugger.GetDebuggerStopArgs();
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000AC53D File Offset: 0x000AA73D
		public override void SetParent(Debugger parent, IEnumerable<Breakpoint> breakPoints, DebuggerResumeAction? startAction, PSHost host, PathInfo path, Dictionary<string, DebugSource> functionSourceMap)
		{
			this.SetDebuggerStepMode(true);
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000AC546 File Offset: 0x000AA746
		public override void SetParent(Debugger parent, IEnumerable<Breakpoint> breakPoints, DebuggerResumeAction? startAction, PSHost host, PathInfo path)
		{
			this.SetDebuggerStepMode(true);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000AC54F File Offset: 0x000AA74F
		public override void SetDebugMode(DebugModes mode)
		{
			this._wrappedDebugger.SetDebugMode(mode);
			base.SetDebugMode(mode);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000AC564 File Offset: 0x000AA764
		public override IEnumerable<CallStackFrame> GetCallStack()
		{
			return this._wrappedDebugger.GetCallStack();
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000AC571 File Offset: 0x000AA771
		public override void SetDebuggerStepMode(bool enabled)
		{
			this._wrappedDebugger.SetDebuggerStepMode(enabled);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000AC580 File Offset: 0x000AA780
		internal void CheckStateAndRaiseStopEvent()
		{
			RemoteDebugger remoteDebugger = this._wrappedDebugger as RemoteDebugger;
			if (remoteDebugger != null)
			{
				remoteDebugger.CheckStateAndRaiseStopEvent();
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x000AC5A2 File Offset: 0x000AA7A2
		public override bool InBreakpoint
		{
			get
			{
				return this._wrappedDebugger.InBreakpoint;
			}
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000AC5B0 File Offset: 0x000AA7B0
		private void HandleDebuggerStop(object sender, DebuggerStopEventArgs e)
		{
			Pipeline runningCmd = null;
			try
			{
				runningCmd = this.DrainAndBlockRemoteOutput();
				base.RaiseDebuggerStopEvent(e);
			}
			finally
			{
				this.RestoreRemoteOutput(runningCmd);
			}
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000AC5E8 File Offset: 0x000AA7E8
		private Pipeline DrainAndBlockRemoteOutput()
		{
			if (!(this._runspace is RemoteRunspace))
			{
				return null;
			}
			Pipeline currentlyRunningPipeline = this._runspace.GetCurrentlyRunningPipeline();
			if (currentlyRunningPipeline != null)
			{
				currentlyRunningPipeline.DrainIncomingData();
				currentlyRunningPipeline.SuspendIncomingData();
				return currentlyRunningPipeline;
			}
			return null;
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000AC622 File Offset: 0x000AA822
		private void RestoreRemoteOutput(Pipeline runningCmd)
		{
			if (runningCmd != null)
			{
				runningCmd.ResumeIncomingData();
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000AC62D File Offset: 0x000AA82D
		private void HandleBreakpointUpdated(object sender, BreakpointUpdatedEventArgs e)
		{
			base.RaiseBreakpointUpdatedEvent(e);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000AC638 File Offset: 0x000AA838
		private DebuggerCommandResults HandlePromptCommand(PSDataCollection<PSObject> output)
		{
			string script = "'[DBG]: ' + '[" + CodeGeneration.EscapeSingleQuotedStringContent(this._jobName) + "]: ' + \"PS $($executionContext.SessionState.Path.CurrentLocation)>> \"";
			PSCommand pscommand = new PSCommand();
			pscommand.AddScript(script);
			this._wrappedDebugger.ProcessCommand(pscommand, output);
			return new DebuggerCommandResults(null, true);
		}

		// Token: 0x04000D38 RID: 3384
		private Debugger _wrappedDebugger;

		// Token: 0x04000D39 RID: 3385
		private Runspace _runspace;

		// Token: 0x04000D3A RID: 3386
		private string _jobName;
	}
}
