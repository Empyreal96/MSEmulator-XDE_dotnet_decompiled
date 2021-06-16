using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x020000F3 RID: 243
	internal sealed class StandaloneRunspaceDebugger : NestedRunspaceDebugger
	{
		// Token: 0x06000DA6 RID: 3494 RVA: 0x0004A7C9 File Offset: 0x000489C9
		public StandaloneRunspaceDebugger(Runspace runspace) : base(runspace, PSMonitorRunspaceType.Standalone, Guid.Empty)
		{
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0004A7D8 File Offset: 0x000489D8
		protected override DebuggerCommandResults HandleCallStack(PSDataCollection<PSObject> output)
		{
			PSCommand pscommand = new PSCommand();
			pscommand.AddCommand("Get-PSCallStack");
			PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
			this._wrappedDebugger.ProcessCommand(pscommand, psdataCollection);
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("Out-String").AddParameter("Stream", true);
				powerShell.Invoke<PSObject>(psdataCollection, output);
			}
			return new DebuggerCommandResults(null, true);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0004A860 File Offset: 0x00048A60
		protected override void HandleDebuggerStop(object sender, DebuggerStopEventArgs e)
		{
			PowerShell runningCmd = null;
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

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0004A898 File Offset: 0x00048A98
		private PowerShell DrainAndBlockRemoteOutput()
		{
			RemoteRunspace remoteRunspace = this._runspace as RemoteRunspace;
			if (remoteRunspace == null)
			{
				return null;
			}
			PowerShell currentBasePowerShell = remoteRunspace.GetCurrentBasePowerShell();
			if (currentBasePowerShell != null)
			{
				currentBasePowerShell.WaitForServicingComplete();
				currentBasePowerShell.SuspendIncomingData();
				return currentBasePowerShell;
			}
			return null;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0004A8CF File Offset: 0x00048ACF
		private void RestoreRemoteOutput(PowerShell runningCmd)
		{
			if (runningCmd != null)
			{
				runningCmd.ResumeIncomingData();
			}
		}
	}
}
