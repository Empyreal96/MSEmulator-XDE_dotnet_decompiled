using System;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000307 RID: 775
	internal class ServerDriverRemoteHost : ServerRemoteHost
	{
		// Token: 0x060024A3 RID: 9379 RVA: 0x000CD7A2 File Offset: 0x000CB9A2
		internal ServerDriverRemoteHost(Guid clientRunspacePoolId, Guid clientPowerShellId, HostInfo hostInfo, AbstractServerSessionTransportManager transportManager, ServerRemoteDebugger debugger) : base(clientRunspacePoolId, clientPowerShellId, hostInfo, transportManager, null, null)
		{
			this._debugger = debugger;
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x000CD7B9 File Offset: 0x000CB9B9
		public override bool IsRunspacePushed
		{
			get
			{
				return this._pushedRunspace != null;
			}
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000CD7C8 File Offset: 0x000CB9C8
		public override void PushRunspace(Runspace runspace)
		{
			if (this._transportManager is OutOfProcessServerSessionTransportManager || (!(runspace.ConnectionInfo is NamedPipeConnectionInfo) && !(runspace.ConnectionInfo is VMConnectionInfo) && !(runspace.ConnectionInfo is ContainerConnectionInfo)))
			{
				throw new PSNotSupportedException();
			}
			if (this._debugger == null)
			{
				throw new PSInvalidOperationException(RemotingErrorIdStrings.ServerDriverRemoteHostNoDebuggerToPush);
			}
			if (this._pushedRunspace != null)
			{
				throw new PSInvalidOperationException(RemotingErrorIdStrings.ServerDriverRemoteHostAlreadyPushed);
			}
			RemoteRunspace remoteRunspace = runspace as RemoteRunspace;
			if (remoteRunspace == null)
			{
				throw new PSInvalidOperationException(RemotingErrorIdStrings.ServerDriverRemoteHostNotRemoteRunspace);
			}
			this._hostSupportsPSEdit = false;
			PSEventManager pseventManager = (base.Runspace != null) ? base.Runspace.Events : null;
			this._hostSupportsPSEdit = (pseventManager != null && pseventManager.GetEventSubscribers("PSISERemoteSessionOpenFile").GetEnumerator().MoveNext());
			if (this._hostSupportsPSEdit)
			{
				this.AddPSEditForRunspace(remoteRunspace);
			}
			this._debugger.PushDebugger(runspace.Debugger);
			this._pushedRunspace = remoteRunspace;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000CD8B0 File Offset: 0x000CBAB0
		public override void PopRunspace()
		{
			if (this._pushedRunspace != null)
			{
				if (this._debugger != null)
				{
					this._debugger.PopDebugger();
				}
				if (this._hostSupportsPSEdit)
				{
					this.RemovePSEditFromRunspace(this._pushedRunspace);
				}
				if (this._pushedRunspace.ShouldCloseOnPop)
				{
					this._pushedRunspace.Close();
				}
				this._pushedRunspace = null;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x000CD90B File Offset: 0x000CBB0B
		// (set) Token: 0x060024A8 RID: 9384 RVA: 0x000CD913 File Offset: 0x000CBB13
		internal Debugger ServerDebugger
		{
			get
			{
				return this._debugger;
			}
			set
			{
				this._debugger = (value as ServerRemoteDebugger);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000CD921 File Offset: 0x000CBB21
		internal Runspace PushedRunspace
		{
			get
			{
				return this._pushedRunspace;
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000CD92C File Offset: 0x000CBB2C
		private void AddPSEditForRunspace(RemoteRunspace remoteRunspace)
		{
			if (remoteRunspace.Events == null)
			{
				return;
			}
			remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.HandleRemoteSessionForwardedEvent;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = remoteRunspace;
				powerShell.AddScript("\r\n            param (\r\n                [string] $PSEditFunction\r\n            )\r\n\r\n            if ($PSVersionTable.PSVersion -lt ([version] '3.0'))\r\n            {\r\n                throw (new-object System.NotSupportedException)\r\n            }\r\n\r\n            Register-EngineEvent -SourceIdentifier PSISERemoteSessionOpenFile -Forward\r\n\r\n            if ((Test-Path -Path 'function:\\global:PSEdit') -eq $false)\r\n            {\r\n                Set-Item -Path 'function:\\global:PSEdit' -Value $PSEditFunction\r\n            }\r\n        ").AddParameter("PSEditFunction", "\r\n            param (\r\n                [Parameter(Mandatory=$true)] [String[]] $FileName\r\n            )\r\n\r\n            foreach ($file in $FileName)\r\n            {\r\n                dir $file -File | foreach {\r\n                    $filePathName = $_.FullName\r\n\r\n                    # Get file contents\r\n                    $contentBytes = Get-Content -Path $filePathName -Raw -Encoding Byte\r\n\r\n                    # Notify client for file open.\r\n                    New-Event -SourceIdentifier PSISERemoteSessionOpenFile -EventArguments @($filePathName, $contentBytes) > $null\r\n                }\r\n            }\r\n        ");
				try
				{
					powerShell.Invoke();
				}
				catch (RemoteException)
				{
				}
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000CD9BC File Offset: 0x000CBBBC
		private void RemovePSEditFromRunspace(RemoteRunspace remoteRunspace)
		{
			if (remoteRunspace.Events == null)
			{
				return;
			}
			if (remoteRunspace.RunspaceStateInfo.State != RunspaceState.Opened || remoteRunspace.RunspaceAvailability != RunspaceAvailability.Available)
			{
				return;
			}
			remoteRunspace.Events.ReceivedEvents.PSEventReceived -= this.HandleRemoteSessionForwardedEvent;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = remoteRunspace;
				powerShell.AddScript("\r\n            if ($PSVersionTable.PSVersion -lt ([version] '3.0'))\r\n            {\r\n                throw (new-object System.NotSupportedException)\r\n            }\r\n\r\n            if ((Test-Path -Path 'function:\\global:PSEdit') -eq $true)\r\n            {\r\n                Remove-Item -Path 'function:\\global:PSEdit' -Force\r\n            }\r\n\r\n            Get-EventSubscriber -SourceIdentifier PSISERemoteSessionOpenFile -EA Ignore | Remove-Event\r\n        ");
				try
				{
					powerShell.Invoke();
				}
				catch (RemoteException)
				{
				}
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000CDA54 File Offset: 0x000CBC54
		private void HandleRemoteSessionForwardedEvent(object sender, PSEventArgs args)
		{
			if (base.Runspace == null || base.Runspace.Events == null)
			{
				return;
			}
			try
			{
				base.Runspace.Events.GenerateEvent(args.SourceIdentifier, null, args.SourceArgs, null);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x04001206 RID: 4614
		private RemoteRunspace _pushedRunspace;

		// Token: 0x04001207 RID: 4615
		private ServerRemoteDebugger _debugger;

		// Token: 0x04001208 RID: 4616
		private bool _hostSupportsPSEdit;
	}
}
