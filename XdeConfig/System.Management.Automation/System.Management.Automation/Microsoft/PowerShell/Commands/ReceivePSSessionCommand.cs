using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Remoting.Internal;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000332 RID: 818
	[Cmdlet("Receive", "PSSession", SupportsShouldProcess = true, DefaultParameterSetName = "Session", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=217037", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class ReceivePSSessionCommand : PSRemotingCmdlet
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x000DD828 File Offset: 0x000DBA28
		// (set) Token: 0x06002785 RID: 10117 RVA: 0x000DD830 File Offset: 0x000DBA30
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Session")]
		[ValidateNotNullOrEmpty]
		public PSSession Session
		{
			get
			{
				return this.remotePSSessionInfo;
			}
			set
			{
				this.remotePSSessionInfo = value;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06002786 RID: 10118 RVA: 0x000DD839 File Offset: 0x000DBA39
		// (set) Token: 0x06002787 RID: 10119 RVA: 0x000DD841 File Offset: 0x000DBA41
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Id")]
		public int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000DD84A File Offset: 0x000DBA4A
		// (set) Token: 0x06002789 RID: 10121 RVA: 0x000DD852 File Offset: 0x000DBA52
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerSessionName")]
		[Alias(new string[]
		{
			"Cn"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
		public string ComputerName
		{
			get
			{
				return this.computerName;
			}
			set
			{
				this.computerName = value;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x000DD85B File Offset: 0x000DBA5B
		// (set) Token: 0x0600278B RID: 10123 RVA: 0x000DD863 File Offset: 0x000DBA63
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerSessionName")]
		public string ApplicationName
		{
			get
			{
				return this.appName;
			}
			set
			{
				this.appName = base.ResolveAppName(value);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x000DD872 File Offset: 0x000DBA72
		// (set) Token: 0x0600278D RID: 10125 RVA: 0x000DD87A File Offset: 0x000DBA7A
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerSessionName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriInstanceId")]
		public string ConfigurationName
		{
			get
			{
				return this.shell;
			}
			set
			{
				this.shell = base.ResolveShell(value);
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x000DD889 File Offset: 0x000DBA89
		// (set) Token: 0x0600278F RID: 10127 RVA: 0x000DD891 File Offset: 0x000DBA91
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriInstanceId")]
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriSessionName")]
		public Uri ConnectionUri
		{
			get
			{
				return this.uris;
			}
			set
			{
				this.uris = value;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x000DD89A File Offset: 0x000DBA9A
		// (set) Token: 0x06002791 RID: 10129 RVA: 0x000DD8A7 File Offset: 0x000DBAA7
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		public SwitchParameter AllowRedirection
		{
			get
			{
				return this.allowRedirection;
			}
			set
			{
				this.allowRedirection = value;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x000DD8B5 File Offset: 0x000DBAB5
		// (set) Token: 0x06002793 RID: 10131 RVA: 0x000DD8BD File Offset: 0x000DBABD
		[Parameter(Mandatory = true, ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "InstanceId")]
		[Parameter(Mandatory = true, ParameterSetName = "ComputerInstanceId")]
		[ValidateNotNullOrEmpty]
		public Guid InstanceId
		{
			get
			{
				return this.instanceId;
			}
			set
			{
				this.instanceId = value;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06002794 RID: 10132 RVA: 0x000DD8C6 File Offset: 0x000DBAC6
		// (set) Token: 0x06002795 RID: 10133 RVA: 0x000DD8CE File Offset: 0x000DBACE
		[Parameter(Mandatory = true, ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "SessionName")]
		[Parameter(Mandatory = true, ParameterSetName = "ComputerSessionName")]
		[ValidateNotNullOrEmpty]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06002796 RID: 10134 RVA: 0x000DD8D7 File Offset: 0x000DBAD7
		// (set) Token: 0x06002797 RID: 10135 RVA: 0x000DD8DF File Offset: 0x000DBADF
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "Id")]
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "InstanceId")]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ParameterSetName = "SessionName")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		public OutTarget OutTarget
		{
			get
			{
				return this.outputMode;
			}
			set
			{
				this.outputMode = value;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x000DD8E8 File Offset: 0x000DBAE8
		// (set) Token: 0x06002799 RID: 10137 RVA: 0x000DD8F0 File Offset: 0x000DBAF0
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "InstanceId")]
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "Id")]
		[Parameter(ParameterSetName = "SessionName")]
		[Parameter(ParameterSetName = "Session")]
		public string JobName
		{
			get
			{
				return this.jobName;
			}
			set
			{
				this.jobName = value;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000DD8F9 File Offset: 0x000DBAF9
		// (set) Token: 0x0600279B RID: 10139 RVA: 0x000DD901 File Offset: 0x000DBB01
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Credential]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		public PSCredential Credential
		{
			get
			{
				return this.psCredential;
			}
			set
			{
				this.psCredential = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000DD921 File Offset: 0x000DBB21
		// (set) Token: 0x0600279D RID: 10141 RVA: 0x000DD929 File Offset: 0x000DBB29
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		public AuthenticationMechanism Authentication
		{
			get
			{
				return this.authentication;
			}
			set
			{
				this.authentication = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x000DD949 File Offset: 0x000DBB49
		// (set) Token: 0x0600279F RID: 10143 RVA: 0x000DD951 File Offset: 0x000DBB51
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		public string CertificateThumbprint
		{
			get
			{
				return this.thumbprint;
			}
			set
			{
				this.thumbprint = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000DD971 File Offset: 0x000DBB71
		// (set) Token: 0x060027A1 RID: 10145 RVA: 0x000DD979 File Offset: 0x000DBB79
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[ValidateRange(1, 65535)]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060027A2 RID: 10146 RVA: 0x000DD982 File Offset: 0x000DBB82
		// (set) Token: 0x060027A3 RID: 10147 RVA: 0x000DD98A File Offset: 0x000DBB8A
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ComputerSessionName")]
		public SwitchParameter UseSSL
		{
			get
			{
				return this.useSSL;
			}
			set
			{
				this.useSSL = value;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x000DD993 File Offset: 0x000DBB93
		// (set) Token: 0x060027A5 RID: 10149 RVA: 0x000DD99B File Offset: 0x000DBB9B
		[Parameter(ParameterSetName = "ComputerSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriSessionName")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		public PSSessionOption SessionOption
		{
			get
			{
				return this.sessionOption;
			}
			set
			{
				this.sessionOption = value;
			}
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000DD9A4 File Offset: 0x000DBBA4
		protected override void ProcessRecord()
		{
			if (base.ParameterSetName == "ComputerSessionName" || base.ParameterSetName == "ConnectionUriSessionName")
			{
				this.QueryForAndConnectCommands(this.Name, Guid.Empty);
				return;
			}
			if (base.ParameterSetName == "ComputerInstanceId" || base.ParameterSetName == "ConnectionUriInstanceId")
			{
				this.QueryForAndConnectCommands(string.Empty, this.InstanceId);
				return;
			}
			this.GetAndConnectSessionCommand();
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000DDA24 File Offset: 0x000DBC24
		protected override void StopProcessing()
		{
			RemotePipeline remotePipeline;
			Job job;
			lock (this._syncObject)
			{
				this._stopProcessing = true;
				remotePipeline = this._remotePipeline;
				job = this._job;
			}
			if (remotePipeline != null)
			{
				remotePipeline.StopAsync();
			}
			if (job != null)
			{
				job.StopJob();
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000DDA88 File Offset: 0x000DBC88
		private void QueryForAndConnectCommands(string name, Guid instanceId)
		{
			WSManConnectionInfo connectionObject = this.GetConnectionObject();
			Runspace[] runspaces;
			try
			{
				runspaces = Runspace.GetRunspaces(connectionObject, base.Host, QueryRunspaces.BuiltInTypesTable);
			}
			catch (RuntimeException ex)
			{
				int transportErrorCode;
				string message = StringUtil.Format(RemotingErrorIdStrings.QueryForRunspacesFailed, connectionObject.ComputerName, QueryRunspaces.ExtractMessage(ex.InnerException, out transportErrorCode));
				string fqeidfromTransportError = WSManTransportManagerUtils.GetFQEIDFromTransportError(transportErrorCode, "ReceivePSSessionQueryForSessionFailed");
				Exception exception = new RuntimeException(message, ex.InnerException);
				ErrorRecord errorRecord = new ErrorRecord(exception, fqeidfromTransportError, ErrorCategory.InvalidOperation, connectionObject);
				base.WriteError(errorRecord);
				return;
			}
			string text = null;
			if (!string.IsNullOrEmpty(this.ConfigurationName))
			{
				text = ((this.ConfigurationName.IndexOf("http://schemas.microsoft.com/powershell/", StringComparison.OrdinalIgnoreCase) != -1) ? this.ConfigurationName : ("http://schemas.microsoft.com/powershell/" + this.ConfigurationName));
			}
			Runspace[] array = runspaces;
			int i = 0;
			while (i < array.Length)
			{
				Runspace runspace = array[i];
				if (this._stopProcessing)
				{
					return;
				}
				if (text == null)
				{
					goto IL_F4;
				}
				WSManConnectionInfo wsmanConnectionInfo = runspace.ConnectionInfo as WSManConnectionInfo;
				if (wsmanConnectionInfo == null || text.Equals(wsmanConnectionInfo.ShellUri, StringComparison.OrdinalIgnoreCase))
				{
					goto IL_F4;
				}
				IL_24C:
				i++;
				continue;
				IL_F4:
				bool flag = false;
				if (!string.IsNullOrEmpty(name) && string.Compare(name, ((RemoteRunspace)runspace).RunspacePool.RemoteRunspacePoolInternal.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
				}
				else if (instanceId.Equals(runspace.InstanceId))
				{
					flag = true;
				}
				if (!flag || !base.ShouldProcess(((RemoteRunspace)runspace).PSSessionName, "Receive"))
				{
					goto IL_24C;
				}
				PSSession item = base.RunspaceRepository.GetItem(runspace.InstanceId);
				Exception innerException;
				PSSession pssession = this.ConnectSession(item, out innerException);
				if (pssession != null)
				{
					base.RunspaceRepository.AddOrReplace(pssession);
					PSRemotingJob job = this.FindJobForSession(pssession);
					if (this.OutTarget == OutTarget.Host)
					{
						this.ConnectSessionToHost(pssession, job);
						return;
					}
					this.ConnectSessionToJob(pssession, job);
					return;
				}
				else
				{
					PSSession pssession2 = new PSSession(runspace as RemoteRunspace);
					pssession = this.ConnectSession(pssession2, out innerException);
					if (pssession == null)
					{
						string message2 = StringUtil.Format(RemotingErrorIdStrings.RunspaceCannotBeConnected, pssession2.Name);
						base.WriteError(new ErrorRecord(new ArgumentException(message2, innerException), "ReceivePSSessionCannotConnectSession", ErrorCategory.InvalidOperation, pssession2));
						return;
					}
					if (item != null)
					{
						pssession = (item.InsertRunspace(pssession.Runspace as RemoteRunspace) ? item : pssession);
					}
					base.RunspaceRepository.AddOrReplace(pssession);
					if (this.OutTarget == OutTarget.Job)
					{
						this.ConnectSessionToJob(pssession, null);
						return;
					}
					this.ConnectSessionToHost(pssession, null);
					return;
				}
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000DDD04 File Offset: 0x000DBF04
		private WSManConnectionInfo GetConnectionObject()
		{
			WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
			if (base.ParameterSetName == "ComputerSessionName" || base.ParameterSetName == "ComputerInstanceId")
			{
				string scheme = this.UseSSL.IsPresent ? "https" : "http";
				wsmanConnectionInfo.Scheme = scheme;
				wsmanConnectionInfo.ComputerName = base.ResolveComputerName(this.ComputerName);
				wsmanConnectionInfo.AppName = this.ApplicationName;
				wsmanConnectionInfo.ShellUri = this.ConfigurationName;
				wsmanConnectionInfo.Port = this.Port;
				if (this.CertificateThumbprint != null)
				{
					wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
				}
				else
				{
					wsmanConnectionInfo.Credential = this.Credential;
				}
				wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
				this.UpdateConnectionInfo(wsmanConnectionInfo);
			}
			else
			{
				wsmanConnectionInfo.ConnectionUri = this.ConnectionUri;
				wsmanConnectionInfo.ShellUri = this.ConfigurationName;
				if (this.CertificateThumbprint != null)
				{
					wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
				}
				else
				{
					wsmanConnectionInfo.Credential = this.Credential;
				}
				wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
				this.UpdateConnectionInfo(wsmanConnectionInfo);
			}
			return wsmanConnectionInfo;
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000DDE20 File Offset: 0x000DC020
		private void UpdateConnectionInfo(WSManConnectionInfo connectionInfo)
		{
			if (base.ParameterSetName != "ConnectionUriInstanceId" && base.ParameterSetName != "ConnectionUriSessionName")
			{
				connectionInfo.MaximumConnectionRedirectionCount = 0;
			}
			if (!this.allowRedirection)
			{
				connectionInfo.MaximumConnectionRedirectionCount = 0;
			}
			if (this.SessionOption != null)
			{
				connectionInfo.SetSessionOptions(this.SessionOption);
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000DDE7C File Offset: 0x000DC07C
		private void GetAndConnectSessionCommand()
		{
			PSSession pssession = null;
			if (base.ParameterSetName == "Session")
			{
				pssession = this.Session;
			}
			else if (base.ParameterSetName == "Id")
			{
				pssession = this.GetSessionById(this.Id);
				if (pssession == null)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedSessionId, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedSessionId, this.Id);
					return;
				}
			}
			else if (base.ParameterSetName == "SessionName")
			{
				pssession = this.GetSessionByName(this.Name);
				if (pssession == null)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedName, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedName, this.Name);
					return;
				}
			}
			else if (base.ParameterSetName == "InstanceId")
			{
				pssession = this.GetSessionByInstanceId(this.InstanceId);
				if (pssession == null)
				{
					this.WriteInvalidArgumentError(PSRemotingErrorId.RemoteRunspaceNotAvailableForSpecifiedRunspaceId, RemotingErrorIdStrings.RemoteRunspaceNotAvailableForSpecifiedRunspaceId, this.InstanceId);
					return;
				}
			}
			if (base.ShouldProcess(pssession.Name, "Receive"))
			{
				Exception innerException;
				if (this.ConnectSession(pssession, out innerException) == null)
				{
					PSSession pssession2 = pssession;
					pssession = this.TryGetSessionFromServer(pssession2);
					if (pssession == null)
					{
						string message = StringUtil.Format(RemotingErrorIdStrings.RunspaceCannotBeConnected, pssession2.Name);
						base.WriteError(new ErrorRecord(new ArgumentException(message, innerException), "ReceivePSSessionCannotConnectSession", ErrorCategory.InvalidOperation, pssession2));
						return;
					}
				}
				PSRemotingJob psremotingJob = this.FindJobForSession(pssession);
				if (psremotingJob != null)
				{
					if (this.OutTarget == OutTarget.Host)
					{
						this.ConnectSessionToHost(pssession, psremotingJob);
					}
					else
					{
						this.ConnectSessionToJob(pssession, psremotingJob);
					}
				}
				else if (this.OutTarget == OutTarget.Job)
				{
					this.ConnectSessionToJob(pssession, null);
				}
				else
				{
					this.ConnectSessionToHost(pssession, null);
				}
				if (pssession.Runspace.RunspaceStateInfo.State != RunspaceState.Disconnected)
				{
					base.RunspaceRepository.AddOrReplace(pssession);
				}
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000DE020 File Offset: 0x000DC220
		private bool CheckForDebugMode(PSSession session, bool monitorAvailabilityChange)
		{
			RemoteRunspace remoteRunspace = session.Runspace as RemoteRunspace;
			if (remoteRunspace.RunspaceAvailability == RunspaceAvailability.RemoteDebug)
			{
				this.DisconnectAndStopRunningCmds(remoteRunspace);
				this.WriteDebugStopWarning();
				return true;
			}
			if (monitorAvailabilityChange)
			{
				remoteRunspace.AvailabilityChanged += this.HandleRunspaceAvailabilityChanged;
			}
			return false;
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000DE068 File Offset: 0x000DC268
		private void HandleRunspaceAvailabilityChanged(object sender, RunspaceAvailabilityEventArgs e)
		{
			if (e.RunspaceAvailability == RunspaceAvailability.RemoteDebug)
			{
				RemoteRunspace remoteRunspace = sender as RemoteRunspace;
				remoteRunspace.AvailabilityChanged -= this.HandleRunspaceAvailabilityChanged;
				this.DisconnectAndStopRunningCmds(remoteRunspace);
			}
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000DE0A0 File Offset: 0x000DC2A0
		private void DisconnectAndStopRunningCmds(RemoteRunspace remoteRunspace)
		{
			if (remoteRunspace.RunspaceStateInfo.State == RunspaceState.Opened)
			{
				Job job;
				ManualResetEvent stopPipelineReceive;
				lock (this._syncObject)
				{
					job = this._job;
					stopPipelineReceive = this._stopPipelineReceive;
				}
				remoteRunspace.Disconnect();
				if (stopPipelineReceive != null)
				{
					try
					{
						stopPipelineReceive.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				if (job != null)
				{
					job.StopJob();
				}
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000DE120 File Offset: 0x000DC320
		private void WriteDebugStopWarning()
		{
			base.WriteWarning(base.GetMessage(RemotingErrorIdStrings.ReceivePSSessionInDebugMode));
			base.WriteObject("");
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000DE188 File Offset: 0x000DC388
		private void ConnectSessionToHost(PSSession session, PSRemotingJob job = null)
		{
			RemoteRunspace remoteRunspace = session.Runspace as RemoteRunspace;
			if (job != null)
			{
				lock (this._syncObject)
				{
					this._job = job;
					this._stopPipelineReceive = new ManualResetEvent(false);
				}
				using (this._stopPipelineReceive)
				{
					try
					{
						Job job2 = job.ChildJobs[0];
						job.ConnectJobs();
						if (this.CheckForDebugMode(session, true))
						{
							return;
						}
						for (;;)
						{
							int num = WaitHandle.WaitAny(new WaitHandle[]
							{
								this._stopPipelineReceive,
								job2.Results.WaitHandle
							});
							foreach (PSStreamObject psstreamObject in job2.ReadAll())
							{
								if (psstreamObject != null)
								{
									psstreamObject.WriteStreamObject(this, false);
								}
							}
							if (num == 0)
							{
								break;
							}
							if (job.IsFinishedState(job.JobStateInfo.State))
							{
								goto Block_17;
							}
						}
						this.WriteDebugStopWarning();
						return;
						Block_17:;
					}
					finally
					{
						if (job != null)
						{
							((IDisposable)job).Dispose();
						}
					}
				}
				lock (this._syncObject)
				{
					this._job = null;
					this._stopPipelineReceive = null;
				}
			}
			else
			{
				if (remoteRunspace.RemoteCommand == null)
				{
					this.CheckForDebugMode(session, false);
					return;
				}
				lock (this._syncObject)
				{
					this._remotePipeline = (RemotePipeline)session.Runspace.CreateDisconnectedPipeline();
					this._stopPipelineReceive = new ManualResetEvent(false);
				}
				using (this._stopPipelineReceive)
				{
					using (this._remotePipeline)
					{
						ManualResetEvent pipelineConnectedEvent = new ManualResetEvent(false);
						using (pipelineConnectedEvent)
						{
							this._remotePipeline.StateChanged += delegate(object sender, PipelineStateEventArgs args)
							{
								if (pipelineConnectedEvent != null && (args.PipelineStateInfo.State == PipelineState.Running || args.PipelineStateInfo.State == PipelineState.Stopped || args.PipelineStateInfo.State == PipelineState.Failed))
								{
									pipelineConnectedEvent.Set();
								}
							};
							this._remotePipeline.ConnectAsync();
							pipelineConnectedEvent.WaitOne();
						}
						pipelineConnectedEvent = null;
						if (this.CheckForDebugMode(session, true))
						{
							return;
						}
						while (!this._remotePipeline.Output.EndOfPipeline && !this._stopProcessing)
						{
							if (WaitHandle.WaitAny(new WaitHandle[]
							{
								this._stopPipelineReceive,
								this._remotePipeline.Output.WaitHandle
							}) == 0)
							{
								this.WriteDebugStopWarning();
								return;
							}
							while (this._remotePipeline.Output.Count > 0 && !this._stopProcessing)
							{
								PSObject psObject = this._remotePipeline.Output.Read();
								this.WriteRemoteObject(psObject, session);
							}
						}
						if (this._remotePipeline.Error.Count > 0)
						{
							while (!this._remotePipeline.Error.EndOfPipeline)
							{
								object obj = this._remotePipeline.Error.Read();
								if (obj is Collection<ErrorRecord>)
								{
									Collection<ErrorRecord> collection = (Collection<ErrorRecord>)obj;
									using (IEnumerator<ErrorRecord> enumerator2 = collection.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											ErrorRecord errorRecord = enumerator2.Current;
											base.WriteError(errorRecord);
										}
										continue;
									}
								}
								if (obj is ErrorRecord)
								{
									base.WriteError((ErrorRecord)obj);
								}
							}
						}
						if (WaitHandle.WaitAny(new WaitHandle[]
						{
							this._stopPipelineReceive,
							this._remotePipeline.PipelineFinishedEvent
						}) == 0)
						{
							this.WriteDebugStopWarning();
							return;
						}
						remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.ConnectCommands = null;
						if (this._remotePipeline.PipelineStateInfo.State == PipelineState.Failed)
						{
							Exception reason = this._remotePipeline.PipelineStateInfo.Reason;
							string message;
							if (reason != null && !string.IsNullOrEmpty(reason.Message))
							{
								message = StringUtil.Format(RemotingErrorIdStrings.PipelineFailedWithReason, reason.Message);
							}
							else
							{
								message = RemotingErrorIdStrings.PipelineFailedWithoutReason;
							}
							ErrorRecord errorRecord2 = new ErrorRecord(new RuntimeException(message, reason), "ReceivePSSessionPipelineFailed", ErrorCategory.OperationStopped, this._remotePipeline);
							base.WriteError(errorRecord2);
						}
					}
				}
				lock (this._syncObject)
				{
					this._remotePipeline = null;
					this._stopPipelineReceive = null;
				}
				return;
			}
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000DE700 File Offset: 0x000DC900
		private void WriteRemoteObject(PSObject psObject, PSSession session)
		{
			if (psObject == null)
			{
				return;
			}
			if (psObject.Properties[RemotingConstants.ComputerNameNoteProperty] == null)
			{
				psObject.Properties.Add(new PSNoteProperty(RemotingConstants.ComputerNameNoteProperty, session.ComputerName));
			}
			if (psObject.Properties[RemotingConstants.RunspaceIdNoteProperty] == null)
			{
				psObject.Properties.Add(new PSNoteProperty(RemotingConstants.RunspaceIdNoteProperty, session.InstanceId));
			}
			if (psObject.Properties[RemotingConstants.ShowComputerNameNoteProperty] == null)
			{
				psObject.Properties.Add(new PSNoteProperty(RemotingConstants.ShowComputerNameNoteProperty, true));
			}
			base.WriteObject(psObject);
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000DE7A4 File Offset: 0x000DC9A4
		private void ConnectSessionToJob(PSSession session, PSRemotingJob job = null)
		{
			bool flag = false;
			if (job == null)
			{
				List<IThrottleOperation> list = new List<IThrottleOperation>();
				Pipeline pipeline = session.Runspace.CreateDisconnectedPipeline();
				list.Add(new DisconnectedJobOperation(pipeline));
				job = new PSRemotingJob(list, 0, this.JobName, false);
				job.PSJobTypeName = InvokeCommandCommand.RemoteJobType;
				job.HideComputerName = false;
				flag = true;
			}
			if (job.JobStateInfo.State == JobState.Disconnected)
			{
				job.ConnectJob(session.Runspace.InstanceId);
				if (flag)
				{
					base.JobRepository.Add(job);
				}
			}
			if (this.CheckForDebugMode(session, true))
			{
				return;
			}
			base.WriteObject(job);
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000DE838 File Offset: 0x000DCA38
		private PSSession ConnectSession(PSSession session, out Exception ex)
		{
			ex = null;
			if (session == null || (session.Runspace.RunspaceStateInfo.State != RunspaceState.Opened && session.Runspace.RunspaceStateInfo.State != RunspaceState.Disconnected))
			{
				return null;
			}
			if (session.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
			{
				return session;
			}
			try
			{
				session.Runspace.Connect();
			}
			catch (PSInvalidOperationException ex2)
			{
				ex = ex2;
			}
			catch (InvalidRunspaceStateException ex3)
			{
				ex = ex3;
			}
			catch (RuntimeException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				return null;
			}
			return session;
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000DE8D8 File Offset: 0x000DCAD8
		private PSSession TryGetSessionFromServer(PSSession session)
		{
			if (!(session.Runspace is RemoteRunspace))
			{
				return null;
			}
			RemoteRunspace remoteRunspace = null;
			Runspace[] runspaces = Runspace.GetRunspaces(session.Runspace.ConnectionInfo, base.Host, QueryRunspaces.BuiltInTypesTable);
			foreach (Runspace runspace in runspaces)
			{
				if (runspace.InstanceId == session.Runspace.InstanceId)
				{
					remoteRunspace = (runspace as RemoteRunspace);
					break;
				}
			}
			if (remoteRunspace != null)
			{
				session = (session.InsertRunspace(remoteRunspace) ? session : new PSSession(remoteRunspace));
				return session;
			}
			return null;
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000DE968 File Offset: 0x000DCB68
		private PSRemotingJob FindJobForSession(PSSession session)
		{
			PSRemotingJob psremotingJob = null;
			RemoteRunspace remoteRunspace = session.Runspace as RemoteRunspace;
			if (remoteRunspace == null || remoteRunspace.RemoteCommand != null)
			{
				return null;
			}
			foreach (Job job in base.JobRepository.Jobs)
			{
				if (job is PSRemotingJob)
				{
					foreach (Job job2 in job.ChildJobs)
					{
						PSRemotingChildJob psremotingChildJob = (PSRemotingChildJob)job2;
						if (psremotingChildJob.Runspace.InstanceId.Equals(session.InstanceId) && psremotingChildJob.JobStateInfo.State == JobState.Disconnected)
						{
							psremotingJob = (PSRemotingJob)job;
							break;
						}
					}
					if (psremotingJob != null)
					{
						break;
					}
				}
			}
			return psremotingJob;
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x000DEA58 File Offset: 0x000DCC58
		private PSSession GetSessionById(int id)
		{
			foreach (PSSession pssession in base.RunspaceRepository.Runspaces)
			{
				if (pssession.Id == id)
				{
					return pssession;
				}
			}
			return null;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000DEABC File Offset: 0x000DCCBC
		private PSSession GetSessionByName(string name)
		{
			WildcardPattern wildcardPattern = new WildcardPattern(name, WildcardOptions.IgnoreCase);
			foreach (PSSession pssession in base.RunspaceRepository.Runspaces)
			{
				if (wildcardPattern.IsMatch(pssession.Name))
				{
					return pssession;
				}
			}
			return null;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000DEB2C File Offset: 0x000DCD2C
		private PSSession GetSessionByInstanceId(Guid instanceId)
		{
			foreach (PSSession pssession in base.RunspaceRepository.Runspaces)
			{
				if (instanceId.Equals(pssession.InstanceId))
				{
					return pssession;
				}
			}
			return null;
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000DEB94 File Offset: 0x000DCD94
		private void WriteInvalidArgumentError(PSRemotingErrorId errorId, string resourceString, object errorArgument)
		{
			string message = base.GetMessage(resourceString, new object[]
			{
				errorArgument
			});
			base.WriteError(new ErrorRecord(new ArgumentException(message), errorId.ToString(), ErrorCategory.InvalidArgument, errorArgument));
		}

		// Token: 0x0400138D RID: 5005
		private const string IdParameterSet = "Id";

		// Token: 0x0400138E RID: 5006
		private const string InstanceIdParameterSet = "InstanceId";

		// Token: 0x0400138F RID: 5007
		private const string NameParameterSet = "SessionName";

		// Token: 0x04001390 RID: 5008
		private const string ComputerSessionNameParameterSet = "ComputerSessionName";

		// Token: 0x04001391 RID: 5009
		private const string ComputerInstanceIdParameterSet = "ComputerInstanceId";

		// Token: 0x04001392 RID: 5010
		private const string ConnectionUriSessionNameParameterSet = "ConnectionUriSessionName";

		// Token: 0x04001393 RID: 5011
		private const string ConnectionUriInstanceIdParameterSet = "ConnectionUriInstanceId";

		// Token: 0x04001394 RID: 5012
		private PSSession remotePSSessionInfo;

		// Token: 0x04001395 RID: 5013
		private int id;

		// Token: 0x04001396 RID: 5014
		private string computerName;

		// Token: 0x04001397 RID: 5015
		private string appName;

		// Token: 0x04001398 RID: 5016
		private string shell;

		// Token: 0x04001399 RID: 5017
		private Uri uris;

		// Token: 0x0400139A RID: 5018
		private bool allowRedirection;

		// Token: 0x0400139B RID: 5019
		private Guid instanceId;

		// Token: 0x0400139C RID: 5020
		private string name;

		// Token: 0x0400139D RID: 5021
		private OutTarget outputMode;

		// Token: 0x0400139E RID: 5022
		private string jobName = string.Empty;

		// Token: 0x0400139F RID: 5023
		private PSCredential psCredential;

		// Token: 0x040013A0 RID: 5024
		private AuthenticationMechanism authentication;

		// Token: 0x040013A1 RID: 5025
		private string thumbprint;

		// Token: 0x040013A2 RID: 5026
		private int port;

		// Token: 0x040013A3 RID: 5027
		private SwitchParameter useSSL;

		// Token: 0x040013A4 RID: 5028
		private PSSessionOption sessionOption;

		// Token: 0x040013A5 RID: 5029
		private bool _stopProcessing;

		// Token: 0x040013A6 RID: 5030
		private RemotePipeline _remotePipeline;

		// Token: 0x040013A7 RID: 5031
		private Job _job;

		// Token: 0x040013A8 RID: 5032
		private ManualResetEvent _stopPipelineReceive;

		// Token: 0x040013A9 RID: 5033
		private object _syncObject = new object();
	}
}
