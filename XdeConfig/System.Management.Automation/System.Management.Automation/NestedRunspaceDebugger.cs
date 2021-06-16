using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020000F2 RID: 242
	internal abstract class NestedRunspaceDebugger : Debugger, IDisposable
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0004A3D4 File Offset: 0x000485D4
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0004A3DC File Offset: 0x000485DC
		public PSMonitorRunspaceType RunspaceType { get; private set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0004A3E5 File Offset: 0x000485E5
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x0004A3ED File Offset: 0x000485ED
		public Guid ParentDebuggerId { get; private set; }

		// Token: 0x06000D95 RID: 3477 RVA: 0x0004A3F8 File Offset: 0x000485F8
		public NestedRunspaceDebugger(Runspace runspace, PSMonitorRunspaceType runspaceType, Guid parentDebuggerId)
		{
			if (runspace == null || runspace.Debugger == null)
			{
				throw new PSArgumentNullException("runspace");
			}
			this._runspace = runspace;
			this._wrappedDebugger = runspace.Debugger;
			base.SetDebugMode(this._wrappedDebugger.DebugMode);
			this.RunspaceType = runspaceType;
			this.ParentDebuggerId = parentDebuggerId;
			this._wrappedDebugger.BreakpointUpdated += this.HandleBreakpointUpdated;
			this._wrappedDebugger.DebuggerStop += this.HandleDebuggerStop;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0004A484 File Offset: 0x00048684
		public override DebuggerCommandResults ProcessCommand(PSCommand command, PSDataCollection<PSObject> output)
		{
			string text = command.Commands[0].CommandText.Trim();
			if (text.Equals("prompt", StringComparison.OrdinalIgnoreCase))
			{
				return this.HandlePromptCommand(output);
			}
			if (text.Equals("k", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Get-PSCallStack", StringComparison.OrdinalIgnoreCase))
			{
				return this.HandleCallStack(output);
			}
			if ((text.Equals("l", StringComparison.OrdinalIgnoreCase) || text.Equals("list", StringComparison.OrdinalIgnoreCase)) && this.HandleListCommand(output))
			{
				return new DebuggerCommandResults(null, true);
			}
			return this._wrappedDebugger.ProcessCommand(command, output);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0004A524 File Offset: 0x00048724
		public override void SetDebuggerAction(DebuggerResumeAction resumeAction)
		{
			this._wrappedDebugger.SetDebuggerAction(resumeAction);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0004A532 File Offset: 0x00048732
		public override void StopProcessCommand()
		{
			this._wrappedDebugger.StopProcessCommand();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0004A53F File Offset: 0x0004873F
		public override DebuggerStopEventArgs GetDebuggerStopArgs()
		{
			return this._wrappedDebugger.GetDebuggerStopArgs();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0004A54C File Offset: 0x0004874C
		public override void SetDebugMode(DebugModes mode)
		{
			this._wrappedDebugger.SetDebugMode(mode);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004A55A File Offset: 0x0004875A
		public override void SetDebuggerStepMode(bool enabled)
		{
			this._wrappedDebugger.SetDebuggerStepMode(enabled);
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0004A568 File Offset: 0x00048768
		public override bool IsActive
		{
			get
			{
				return this._wrappedDebugger.IsActive;
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0004A578 File Offset: 0x00048778
		public virtual void Dispose()
		{
			if (this._wrappedDebugger != null)
			{
				this._wrappedDebugger.BreakpointUpdated -= this.HandleBreakpointUpdated;
				this._wrappedDebugger.DebuggerStop -= this.HandleDebuggerStop;
			}
			this._wrappedDebugger = null;
			this._runspace = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0004A5D1 File Offset: 0x000487D1
		protected virtual void HandleDebuggerStop(object sender, DebuggerStopEventArgs e)
		{
			base.RaiseDebuggerStopEvent(e);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004A5DA File Offset: 0x000487DA
		protected virtual void HandleBreakpointUpdated(object sender, BreakpointUpdatedEventArgs e)
		{
			base.RaiseBreakpointUpdatedEvent(e);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0004A5E4 File Offset: 0x000487E4
		protected virtual DebuggerCommandResults HandlePromptCommand(PSDataCollection<PSObject> output)
		{
			string text = (this._runspace.ConnectionInfo != null) ? this._runspace.ConnectionInfo.ComputerName : null;
			string formatSpec = "{0}[{1}:{2}]:{3}";
			string text2 = StringUtil.Format(formatSpec, new object[]
			{
				"\"",
				DebuggerStrings.NestedRunspaceDebuggerPromptProcessName,
				"$($PID)",
				"\""
			});
			string text3 = "\"PS $($executionContext.SessionState.Path.CurrentLocation)> \"";
			string script = string.Concat(new string[]
			{
				"'[DBG]: ' + ",
				text2,
				" + ' [",
				CodeGeneration.EscapeSingleQuotedStringContent(this._runspace.Name),
				"]: ' + ",
				text3
			});
			PSCommand pscommand = new PSCommand();
			pscommand.AddScript(script);
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			this._wrappedDebugger.ProcessCommand(pscommand, psdataCollection);
			string value = (psdataCollection.Count == 1) ? (psdataCollection[0].BaseObject as string) : string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("[" + text + "]:");
			}
			stringBuilder.Append(value);
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Insert(stringBuilder.Length - 1, ">");
			}
			output.Add(stringBuilder.ToString());
			return new DebuggerCommandResults(null, true);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0004A75B File Offset: 0x0004895B
		protected virtual DebuggerCommandResults HandleCallStack(PSDataCollection<PSObject> output)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004A762 File Offset: 0x00048962
		protected virtual bool HandleListCommand(PSDataCollection<PSObject> output)
		{
			return false;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004A765 File Offset: 0x00048965
		internal virtual InvocationInfo FixupInvocationInfo(InvocationInfo debugStopInvocationInfo)
		{
			return debugStopInvocationInfo;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0004A768 File Offset: 0x00048968
		internal bool IsSameDebugger(Debugger testDebugger)
		{
			return this._wrappedDebugger.Equals(testDebugger);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004A778 File Offset: 0x00048978
		internal void CheckStateAndRaiseStopEvent()
		{
			RemoteDebugger remoteDebugger = this._wrappedDebugger as RemoteDebugger;
			if (remoteDebugger != null)
			{
				remoteDebugger.CheckStateAndRaiseStopEvent();
				return;
			}
			if (this._wrappedDebugger.IsPendingDebugStopEvent)
			{
				this._wrappedDebugger.ReleaseSavedDebugStop();
				return;
			}
			ServerRemoteDebugger serverRemoteDebugger = this._wrappedDebugger as ServerRemoteDebugger;
			if (serverRemoteDebugger != null)
			{
				serverRemoteDebugger.ReleaseAndRaiseDebugStopLocal();
			}
		}

		// Token: 0x04000603 RID: 1539
		protected Runspace _runspace;

		// Token: 0x04000604 RID: 1540
		protected Debugger _wrappedDebugger;
	}
}
