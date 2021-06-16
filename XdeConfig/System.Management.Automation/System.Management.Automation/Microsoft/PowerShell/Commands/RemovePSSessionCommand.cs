using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000339 RID: 825
	[Cmdlet("Remove", "PSSession", SupportsShouldProcess = true, DefaultParameterSetName = "Id", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135250", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class RemovePSSessionCommand : PSRunspaceCmdlet
	{
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x000DF61F File Offset: 0x000DD81F
		// (set) Token: 0x060027F4 RID: 10228 RVA: 0x000DF627 File Offset: 0x000DD827
		[Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Session")]
		public PSSession[] Session
		{
			get
			{
				return this.remoteRunspaceInfos;
			}
			set
			{
				this.remoteRunspaceInfos = value;
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000DF630 File Offset: 0x000DD830
		protected override void ProcessRecord()
		{
			string parameterSetName;
			ICollection<PSSession> collection;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "ComputerName" || parameterSetName == "Name" || parameterSetName == "InstanceId" || parameterSetName == "Id")
				{
					Dictionary<Guid, PSSession> matchingRunspaces = base.GetMatchingRunspaces(false, true);
					collection = matchingRunspaces.Values;
					goto IL_76;
				}
				if (parameterSetName == "Session")
				{
					collection = this.remoteRunspaceInfos;
					goto IL_76;
				}
			}
			collection = new Collection<PSSession>();
			IL_76:
			foreach (PSSession pssession in collection)
			{
				RemoteRunspace remoteRunspace = (RemoteRunspace)pssession.Runspace;
				if (base.ShouldProcess(remoteRunspace.ConnectionInfo.ComputerName, "Remove"))
				{
					if (pssession.Runspace.RunspaceStateInfo.State == RunspaceState.Disconnected)
					{
						bool flag;
						try
						{
							pssession.Runspace.Connect();
							flag = true;
						}
						catch (InvalidRunspaceStateException)
						{
							flag = false;
						}
						catch (PSRemotingTransportException)
						{
							flag = false;
						}
						if (!flag)
						{
							string message = StringUtil.Format(RemotingErrorIdStrings.RemoveRunspaceNotConnected, remoteRunspace.PSSessionName);
							Exception exception = new RuntimeException(message);
							ErrorRecord errorRecord = new ErrorRecord(exception, "RemoveSessionCannotConnectToServer", ErrorCategory.InvalidOperation, remoteRunspace);
							base.WriteError(errorRecord);
						}
					}
					try
					{
						remoteRunspace.Dispose();
					}
					catch (PSRemotingTransportException)
					{
					}
					try
					{
						base.RunspaceRepository.Remove(pssession);
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x040013C8 RID: 5064
		private PSSession[] remoteRunspaceInfos;
	}
}
