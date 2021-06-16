using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000354 RID: 852
	[Cmdlet("Enter", "PSHostProcess", DefaultParameterSetName = "ProcessIdParameterSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=403736")]
	public sealed class EnterPSHostProcessCommand : PSCmdlet
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000EA978 File Offset: 0x000E8B78
		// (set) Token: 0x06002A6A RID: 10858 RVA: 0x000EA980 File Offset: 0x000E8B80
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ProcessParameterSet")]
		[ValidateNotNull]
		public Process Process { get; set; }

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000EA989 File Offset: 0x000E8B89
		// (set) Token: 0x06002A6C RID: 10860 RVA: 0x000EA991 File Offset: 0x000E8B91
		[ValidateRange(0, 2147483647)]
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "ProcessIdParameterSet")]
		public int Id { get; set; }

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000EA99A File Offset: 0x000E8B9A
		// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000EA9A2 File Offset: 0x000E8BA2
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ParameterSetName = "ProcessNameParameterSet")]
		public string Name { get; set; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000EA9AB File Offset: 0x000E8BAB
		// (set) Token: 0x06002A70 RID: 10864 RVA: 0x000EA9B3 File Offset: 0x000E8BB3
		[ValidateNotNull]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "PSHostProcessInfoParameterSet")]
		public PSHostProcessInfo HostProcessInfo { get; set; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000EA9BC File Offset: 0x000E8BBC
		// (set) Token: 0x06002A72 RID: 10866 RVA: 0x000EA9C4 File Offset: 0x000E8BC4
		[Parameter(Position = 1, ParameterSetName = "ProcessNameParameterSet")]
		[Parameter(Position = 1, ParameterSetName = "PSHostProcessInfoParameterSet")]
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 1, ParameterSetName = "ProcessIdParameterSet")]
		[Parameter(Position = 1, ParameterSetName = "ProcessParameterSet")]
		public string AppDomainName { get; set; }

		// Token: 0x06002A73 RID: 10867 RVA: 0x000EA9D0 File Offset: 0x000E8BD0
		protected override void EndProcessing()
		{
			this._interactiveHost = (base.Host as IHostSupportsInteractiveSession);
			if (this._interactiveHost == null)
			{
				base.WriteError(new ErrorRecord(new ArgumentException(RemotingErrorIdStrings.HostDoesNotSupportIASession), "EnterPSHostProcessHostDoesNotSupportIASession", ErrorCategory.InvalidArgument, null));
				return;
			}
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (!(parameterSetName == "ProcessIdParameterSet"))
				{
					if (!(parameterSetName == "ProcessNameParameterSet"))
					{
						if (parameterSetName == "PSHostProcessInfoParameterSet")
						{
							this.Process = this.GetProcessByHostProcessInfo(this.HostProcessInfo);
						}
					}
					else
					{
						this.Process = this.GetProcessByName(this.Name);
					}
				}
				else
				{
					this.Process = this.GetProcessById(this.Id);
				}
			}
			this.VerifyProcess(this.Process);
			Runspace runspace = this.CreateNamedPipeRunspace(this.Process.Id, this.AppDomainName);
			this.PrepareRunspace(runspace);
			try
			{
				this._interactiveHost.PushRunspace(runspace);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				runspace.Close();
				base.ThrowTerminatingError(new ErrorRecord(ex, "EnterPSHostProcessCannotPushRunspace", ErrorCategory.InvalidOperation, this));
			}
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000EAAEC File Offset: 0x000E8CEC
		protected override void StopProcessing()
		{
			RemoteRunspace connectingRemoteRunspace = this._connectingRemoteRunspace;
			if (connectingRemoteRunspace != null)
			{
				connectingRemoteRunspace.AbortOpen();
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000EAB0C File Offset: 0x000E8D0C
		private Runspace CreateNamedPipeRunspace(int procId, string appDomainName)
		{
			NamedPipeConnectionInfo connectionInfo = new NamedPipeConnectionInfo(procId, appDomainName);
			TypeTable typeTable = TypeTable.LoadDefaultTypeFiles();
			RemoteRunspace remoteRunspace = RunspaceFactory.CreateRunspace(connectionInfo, base.Host, typeTable) as RemoteRunspace;
			remoteRunspace.Name = "PSAttachRunspace";
			remoteRunspace.ShouldCloseOnPop = true;
			this._connectingRemoteRunspace = remoteRunspace;
			try
			{
				remoteRunspace.Open();
				remoteRunspace.Debugger.SetDebugMode(DebugModes.LocalScript | DebugModes.RemoteScript);
			}
			catch (RuntimeException ex)
			{
				string o = (!string.IsNullOrEmpty(appDomainName)) ? appDomainName : "DefaultAppDomain";
				base.ThrowTerminatingError(new ErrorRecord(new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessCannotConnectToProcess, o, procId), ex.InnerException), "EnterPSHostProcessCannotConnectToProcess", ErrorCategory.OperationTimeout, this));
			}
			finally
			{
				this._connectingRemoteRunspace = null;
			}
			return remoteRunspace;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000EABD4 File Offset: 0x000E8DD4
		private void PrepareRunspace(Runspace runspace)
		{
			string script = StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessPrompt, new object[]
			{
				"function global:prompt { \"",
				"$($PID)",
				"PS $($executionContext.SessionState.Path.CurrentLocation)> \" }"
			});
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = runspace;
				try
				{
					powerShell.AddScript(script).Invoke();
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000EAC5C File Offset: 0x000E8E5C
		private Process GetProcessById(int procId)
		{
			Process result;
			try
			{
				result = Process.GetProcessById(procId);
			}
			catch (ArgumentException)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessNoProcessFoundWithId, procId)), "EnterPSHostProcessNoProcessFoundWithId", ErrorCategory.InvalidArgument, this));
				result = null;
			}
			return result;
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000EACB0 File Offset: 0x000E8EB0
		private Process GetProcessByHostProcessInfo(PSHostProcessInfo hostProcessInfo)
		{
			return this.GetProcessById(hostProcessInfo.ProcessId);
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000EACC0 File Offset: 0x000E8EC0
		private Process GetProcessByName(string name)
		{
			Collection<Process> collection;
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				powerShell.AddCommand("Get-Process").AddParameter("Name", name);
				collection = powerShell.Invoke<Process>();
			}
			if (collection.Count == 0)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessNoProcessFoundWithName, name)), "EnterPSHostProcessNoProcessFoundWithName", ErrorCategory.InvalidArgument, this));
			}
			else if (collection.Count > 1)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessMultipleProcessesFoundWithName, name)), "EnterPSHostProcessMultipleProcessesFoundWithName", ErrorCategory.InvalidArgument, this));
			}
			return collection[0];
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000EAD6C File Offset: 0x000E8F6C
		private void VerifyProcess(Process process)
		{
			if (process.Id == Process.GetCurrentProcess().Id)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(RemotingErrorIdStrings.EnterPSHostProcessCantEnterSameProcess), "EnterPSHostProcessCantEnterSameProcess", ErrorCategory.InvalidOperation, this));
			}
			bool flag = false;
			IReadOnlyCollection<PSHostProcessInfo> appDomainNamesFromProcessId = GetPSHostProcessInfoCommand.GetAppDomainNamesFromProcessId(null);
			foreach (PSHostProcessInfo pshostProcessInfo in appDomainNamesFromProcessId)
			{
				if (process.Id == pshostProcessInfo.ProcessId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				base.ThrowTerminatingError(new ErrorRecord(new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.EnterPSHostProcessNoPowerShell, this.Process.ProcessName)), "EnterPSHostProcessNoPowerShell", ErrorCategory.InvalidOperation, this));
			}
		}

		// Token: 0x040014F4 RID: 5364
		private const string ProcessParameterSet = "ProcessParameterSet";

		// Token: 0x040014F5 RID: 5365
		private const string ProcessNameParameterSet = "ProcessNameParameterSet";

		// Token: 0x040014F6 RID: 5366
		private const string ProcessIdParameterSet = "ProcessIdParameterSet";

		// Token: 0x040014F7 RID: 5367
		private const string PSHostProcessInfoParameterSet = "PSHostProcessInfoParameterSet";

		// Token: 0x040014F8 RID: 5368
		private const string NamedPipeRunspaceName = "PSAttachRunspace";

		// Token: 0x040014F9 RID: 5369
		private IHostSupportsInteractiveSession _interactiveHost;

		// Token: 0x040014FA RID: 5370
		private RemoteRunspace _connectingRemoteRunspace;
	}
}
