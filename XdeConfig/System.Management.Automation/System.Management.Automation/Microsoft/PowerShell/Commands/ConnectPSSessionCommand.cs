using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200032D RID: 813
	[Cmdlet("Connect", "PSSession", SupportsShouldProcess = true, DefaultParameterSetName = "Name", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210604", RemotingCapability = RemotingCapability.OwnedByCommand)]
	[OutputType(new Type[]
	{
		typeof(PSSession)
	})]
	public class ConnectPSSessionCommand : PSRunspaceCmdlet, IDisposable
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x000DC00D File Offset: 0x000DA20D
		// (set) Token: 0x06002745 RID: 10053 RVA: 0x000DC015 File Offset: 0x000DA215
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Session")]
		public PSSession[] Session
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

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x000DC01E File Offset: 0x000DA21E
		// (set) Token: 0x06002747 RID: 10055 RVA: 0x000DC026 File Offset: 0x000DA226
		[Parameter(ParameterSetName = "ComputerNameGuid", Mandatory = true)]
		[Parameter(Position = 0, ParameterSetName = "ComputerName", Mandatory = true)]
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"Cn"
		})]
		public override string[] ComputerName
		{
			get
			{
				return this.computerNames;
			}
			set
			{
				this.computerNames = value;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000DC02F File Offset: 0x000DA22F
		// (set) Token: 0x06002749 RID: 10057 RVA: 0x000DC037 File Offset: 0x000DA237
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerNameGuid")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
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

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x000DC046 File Offset: 0x000DA246
		// (set) Token: 0x0600274B RID: 10059 RVA: 0x000DC04E File Offset: 0x000DA24E
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerNameGuid")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriGuid")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUri")]
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

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x000DC05D File Offset: 0x000DA25D
		// (set) Token: 0x0600274D RID: 10061 RVA: 0x000DC065 File Offset: 0x000DA265
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUri")]
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriGuid")]
		public Uri[] ConnectionUri
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

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600274E RID: 10062 RVA: 0x000DC06E File Offset: 0x000DA26E
		// (set) Token: 0x0600274F RID: 10063 RVA: 0x000DC07B File Offset: 0x000DA27B
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
		[Parameter(ParameterSetName = "ConnectionUri")]
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

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x000DC089 File Offset: 0x000DA289
		// (set) Token: 0x06002751 RID: 10065 RVA: 0x000DC091 File Offset: 0x000DA291
		[Parameter(ParameterSetName = "InstanceId", Mandatory = true)]
		[ValidateNotNull]
		[Parameter(ParameterSetName = "ComputerNameGuid", Mandatory = true)]
		[Parameter(ParameterSetName = "ConnectionUriGuid", Mandatory = true)]
		public override Guid[] InstanceId
		{
			get
			{
				return base.InstanceId;
			}
			set
			{
				base.InstanceId = value;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x000DC09A File Offset: 0x000DA29A
		// (set) Token: 0x06002753 RID: 10067 RVA: 0x000DC0A2 File Offset: 0x000DA2A2
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "Name", Mandatory = true)]
		[Parameter(ParameterSetName = "ComputerName")]
		public override string[] Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06002754 RID: 10068 RVA: 0x000DC0AB File Offset: 0x000DA2AB
		// (set) Token: 0x06002755 RID: 10069 RVA: 0x000DC0B3 File Offset: 0x000DA2B3
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
		[Credential]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ComputerName")]
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

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06002756 RID: 10070 RVA: 0x000DC0D3 File Offset: 0x000DA2D3
		// (set) Token: 0x06002757 RID: 10071 RVA: 0x000DC0DB File Offset: 0x000DA2DB
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ConnectionUri")]
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

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x000DC0FB File Offset: 0x000DA2FB
		// (set) Token: 0x06002759 RID: 10073 RVA: 0x000DC103 File Offset: 0x000DA303
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
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

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000DC123 File Offset: 0x000DA323
		// (set) Token: 0x0600275B RID: 10075 RVA: 0x000DC12B File Offset: 0x000DA32B
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ComputerName")]
		[ValidateRange(1, 65535)]
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

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x000DC134 File Offset: 0x000DA334
		// (set) Token: 0x0600275D RID: 10077 RVA: 0x000DC13C File Offset: 0x000DA33C
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerNameGuid")]
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

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000DC145 File Offset: 0x000DA345
		// (set) Token: 0x0600275F RID: 10079 RVA: 0x000DC14D File Offset: 0x000DA34D
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ConnectionUri")]
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

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x000DC156 File Offset: 0x000DA356
		// (set) Token: 0x06002761 RID: 10081 RVA: 0x000DC15E File Offset: 0x000DA35E
		[Parameter(ParameterSetName = "ConnectionUriGuid")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "Id")]
		[Parameter(ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "InstanceId")]
		[Parameter(ParameterSetName = "ComputerNameGuid")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "Session")]
		public int ThrottleLimit
		{
			get
			{
				return this.throttleLimit;
			}
			set
			{
				this.throttleLimit = value;
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000DC167 File Offset: 0x000DA367
		protected override void BeginProcessing()
		{
			base.BeginProcessing();
			this.throttleManager.ThrottleLimit = this.ThrottleLimit;
			this.throttleManager.ThrottleComplete += this.HandleThrottleConnectComplete;
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000DC198 File Offset: 0x000DA398
		protected override void ProcessRecord()
		{
			Collection<PSSession> psSessions = new Collection<PSSession>();
			try
			{
				if (base.ParameterSetName == "ComputerName" || base.ParameterSetName == "ComputerNameGuid" || base.ParameterSetName == "ConnectionUri" || base.ParameterSetName == "ConnectionUriGuid")
				{
					psSessions = this.QueryForDisconnectedSessions();
				}
				else
				{
					psSessions = this.CollectDisconnectedSessions(ConnectPSSessionCommand.OverrideParameter.None);
				}
			}
			catch (PSRemotingDataStructureException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (PSRemotingTransportException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (RemoteException)
			{
				this.operationsComplete.Set();
				throw;
			}
			catch (InvalidRunspaceStateException)
			{
				this.operationsComplete.Set();
				throw;
			}
			this.ConnectSessions(psSessions);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000DC280 File Offset: 0x000DA480
		protected override void EndProcessing()
		{
			this.throttleManager.EndSubmitOperations();
			this.operationsComplete.WaitOne();
			if (this.failedSessions.Count > 0)
			{
				this.RetryFailedSessions();
			}
			while (this.stream.ObjectReader.Count > 0)
			{
				object obj = this.stream.ObjectReader.Read();
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
			this.stream.ObjectWriter.Close();
			foreach (PSSession pssession in this.allSessions)
			{
				if (pssession.Runspace.RunspaceStateInfo.State == RunspaceState.Opened)
				{
					base.RunspaceRepository.AddOrReplace(pssession);
				}
			}
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000DC354 File Offset: 0x000DA554
		protected override void StopProcessing()
		{
			this.stream.ObjectWriter.Close();
			this.queryRunspaces.StopAllOperations();
			this.throttleManager.StopAllOperations();
			this.retryThrottleManager.StopAllOperations();
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000DC388 File Offset: 0x000DA588
		private Collection<PSSession> QueryForDisconnectedSessions()
		{
			Collection<WSManConnectionInfo> connectionObjects = this.GetConnectionObjects();
			Collection<PSSession> disconnectedSessions = this.queryRunspaces.GetDisconnectedSessions(connectionObjects, base.Host, this.stream, base.RunspaceRepository, this.throttleLimit, SessionFilterState.Disconnected, this.InstanceId, this.Name, this.ConfigurationName);
			Collection<object> collection = this.stream.ObjectReader.NonBlockingRead();
			foreach (object obj in collection)
			{
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
			return disconnectedSessions;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000DC42C File Offset: 0x000DA62C
		private Collection<PSSession> CollectDisconnectedSessions(ConnectPSSessionCommand.OverrideParameter overrideParam = ConnectPSSessionCommand.OverrideParameter.None)
		{
			Collection<PSSession> collection = new Collection<PSSession>();
			if (base.ParameterSetName == "Session")
			{
				if (this.remotePSSessionInfo != null)
				{
					foreach (PSSession item in this.remotePSSessionInfo)
					{
						collection.Add(item);
					}
				}
			}
			else
			{
				Dictionary<Guid, PSSession> dictionary = null;
				switch (overrideParam)
				{
				case ConnectPSSessionCommand.OverrideParameter.None:
					dictionary = base.GetMatchingRunspaces(false, true);
					break;
				case ConnectPSSessionCommand.OverrideParameter.Name:
					dictionary = base.GetMatchingRunspacesByName(false, true);
					break;
				case ConnectPSSessionCommand.OverrideParameter.InstanceId:
					dictionary = base.GetMatchingRunspacesByRunspaceId(false, true);
					break;
				}
				if (dictionary != null)
				{
					foreach (PSSession item2 in dictionary.Values)
					{
						collection.Add(item2);
					}
				}
			}
			return collection;
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000DC50C File Offset: 0x000DA70C
		private void ConnectSessions(Collection<PSSession> psSessions)
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			foreach (PSSession pssession in psSessions)
			{
				if (base.ShouldProcess(pssession.Name, "Connect"))
				{
					if (pssession.Runspace.RunspaceStateInfo.State == RunspaceState.Disconnected && pssession.Runspace.RunspaceAvailability == RunspaceAvailability.None)
					{
						this.UpdateConnectionInfo(pssession.Runspace.ConnectionInfo as WSManConnectionInfo);
						ConnectPSSessionCommand.ConnectRunspaceOperation item = new ConnectPSSessionCommand.ConnectRunspaceOperation(pssession, this.stream, base.Host, null, this.failedSessions);
						list.Add(item);
					}
					else if (pssession.Runspace.RunspaceStateInfo.State != RunspaceState.Opened)
					{
						string message = StringUtil.Format(RemotingErrorIdStrings.RunspaceCannotBeConnected, pssession.Name);
						Exception exception = new RuntimeException(message);
						ErrorRecord errorRecord = new ErrorRecord(exception, "PSSessionConnectFailed", ErrorCategory.InvalidOperation, pssession);
						base.WriteError(errorRecord);
					}
					else
					{
						base.WriteObject(pssession);
					}
				}
				this.allSessions.Add(pssession);
			}
			if (list.Count > 0)
			{
				this.operationsComplete.Reset();
				this.throttleManager.SubmitOperations(list);
				Collection<object> collection = this.stream.ObjectReader.NonBlockingRead();
				foreach (object obj in collection)
				{
					base.WriteStreamObject((Action<Cmdlet>)obj);
				}
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000DC6A0 File Offset: 0x000DA8A0
		private void HandleThrottleConnectComplete(object sender, EventArgs eventArgs)
		{
			this.operationsComplete.Set();
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000DC6B0 File Offset: 0x000DA8B0
		private Collection<WSManConnectionInfo> GetConnectionObjects()
		{
			Collection<WSManConnectionInfo> collection = new Collection<WSManConnectionInfo>();
			if (base.ParameterSetName == "ComputerName" || base.ParameterSetName == "ComputerNameGuid")
			{
				string scheme = this.UseSSL.IsPresent ? "https" : "http";
				foreach (string computerName2 in this.ComputerName)
				{
					WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
					wsmanConnectionInfo.Scheme = scheme;
					wsmanConnectionInfo.ComputerName = base.ResolveComputerName(computerName2);
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
					collection.Add(wsmanConnectionInfo);
				}
			}
			else if (base.ParameterSetName == "ConnectionUri" || base.ParameterSetName == "ConnectionUriGuid")
			{
				foreach (Uri connectionUri2 in this.ConnectionUri)
				{
					WSManConnectionInfo wsmanConnectionInfo2 = new WSManConnectionInfo();
					wsmanConnectionInfo2.ConnectionUri = connectionUri2;
					wsmanConnectionInfo2.ShellUri = this.ConfigurationName;
					if (this.CertificateThumbprint != null)
					{
						wsmanConnectionInfo2.CertificateThumbprint = this.CertificateThumbprint;
					}
					else
					{
						wsmanConnectionInfo2.Credential = this.Credential;
					}
					wsmanConnectionInfo2.AuthenticationMechanism = this.Authentication;
					this.UpdateConnectionInfo(wsmanConnectionInfo2);
					collection.Add(wsmanConnectionInfo2);
				}
			}
			return collection;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000DC858 File Offset: 0x000DAA58
		private void UpdateConnectionInfo(WSManConnectionInfo connectionInfo)
		{
			if (base.ParameterSetName != "ConnectionUri" && base.ParameterSetName != "ConnectionUriGuid")
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

		// Token: 0x0600276C RID: 10092 RVA: 0x000DC8EC File Offset: 0x000DAAEC
		private void RetryFailedSessions()
		{
			using (ManualResetEvent retrysComplete = new ManualResetEvent(false))
			{
				Collection<PSSession> collection = new Collection<PSSession>();
				List<IThrottleOperation> list = new List<IThrottleOperation>();
				this.retryThrottleManager.ThrottleLimit = this.ThrottleLimit;
				this.retryThrottleManager.ThrottleComplete += delegate(object sender, EventArgs eventArgs)
				{
					try
					{
						retrysComplete.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				};
				foreach (PSSession session in this.failedSessions)
				{
					list.Add(new ConnectPSSessionCommand.ConnectRunspaceOperation(session, this.stream, base.Host, new QueryRunspaces(), collection));
				}
				this.retryThrottleManager.SubmitOperations(list);
				this.retryThrottleManager.EndSubmitOperations();
				retrysComplete.WaitOne();
				foreach (PSSession item in collection)
				{
					base.RunspaceRepository.AddOrReplace(item);
				}
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000DCA34 File Offset: 0x000DAC34
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000DCA44 File Offset: 0x000DAC44
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.throttleManager.Dispose();
				this.operationsComplete.WaitOne();
				this.operationsComplete.Dispose();
				this.throttleManager.ThrottleComplete -= this.HandleThrottleConnectComplete;
				this.retryThrottleManager.Dispose();
				this.stream.Dispose();
			}
		}

		// Token: 0x04001361 RID: 4961
		private const string ComputerNameGuidParameterSet = "ComputerNameGuid";

		// Token: 0x04001362 RID: 4962
		private const string ConnectionUriParameterSet = "ConnectionUri";

		// Token: 0x04001363 RID: 4963
		private const string ConnectionUriGuidParameterSet = "ConnectionUriGuid";

		// Token: 0x04001364 RID: 4964
		private PSSession[] remotePSSessionInfo;

		// Token: 0x04001365 RID: 4965
		private string[] computerNames;

		// Token: 0x04001366 RID: 4966
		private string appName;

		// Token: 0x04001367 RID: 4967
		private string shell;

		// Token: 0x04001368 RID: 4968
		private Uri[] uris;

		// Token: 0x04001369 RID: 4969
		private bool allowRedirection;

		// Token: 0x0400136A RID: 4970
		private PSCredential psCredential;

		// Token: 0x0400136B RID: 4971
		private AuthenticationMechanism authentication;

		// Token: 0x0400136C RID: 4972
		private string thumbprint;

		// Token: 0x0400136D RID: 4973
		private int port;

		// Token: 0x0400136E RID: 4974
		private SwitchParameter useSSL;

		// Token: 0x0400136F RID: 4975
		private PSSessionOption sessionOption;

		// Token: 0x04001370 RID: 4976
		private int throttleLimit;

		// Token: 0x04001371 RID: 4977
		private Collection<PSSession> allSessions = new Collection<PSSession>();

		// Token: 0x04001372 RID: 4978
		private ThrottleManager throttleManager = new ThrottleManager();

		// Token: 0x04001373 RID: 4979
		private ManualResetEvent operationsComplete = new ManualResetEvent(true);

		// Token: 0x04001374 RID: 4980
		private QueryRunspaces queryRunspaces = new QueryRunspaces();

		// Token: 0x04001375 RID: 4981
		private ObjectStream stream = new ObjectStream();

		// Token: 0x04001376 RID: 4982
		private ThrottleManager retryThrottleManager = new ThrottleManager();

		// Token: 0x04001377 RID: 4983
		private Collection<PSSession> failedSessions = new Collection<PSSession>();

		// Token: 0x0200032E RID: 814
		private class ConnectRunspaceOperation : IThrottleOperation
		{
			// Token: 0x06002770 RID: 10096 RVA: 0x000DCB08 File Offset: 0x000DAD08
			internal ConnectRunspaceOperation(PSSession session, ObjectStream stream, PSHost host, QueryRunspaces queryRunspaces, Collection<PSSession> retryList)
			{
				this._session = session;
				this._writeStream = stream;
				this._host = host;
				this._queryRunspaces = queryRunspaces;
				this._retryList = retryList;
				this._session.Runspace.StateChanged += this.StateCallBackHandler;
			}

			// Token: 0x06002771 RID: 10097 RVA: 0x000DCB5C File Offset: 0x000DAD5C
			internal override void StartOperation()
			{
				bool flag = true;
				Exception ex = null;
				try
				{
					if (this._queryRunspaces != null)
					{
						PSSession pssession = this.QueryForSession(this._session);
						if (pssession != null)
						{
							this._session.Runspace.StateChanged -= this.StateCallBackHandler;
							this._oldSession = this._session;
							this._session = pssession;
							this._session.Runspace.StateChanged += this.StateCallBackHandler;
							this._session.Runspace.ConnectAsync();
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						this._session.Runspace.ConnectAsync();
					}
				}
				catch (PSInvalidOperationException ex2)
				{
					ex = ex2;
				}
				catch (InvalidRunspacePoolStateException ex3)
				{
					ex = ex3;
				}
				catch (RuntimeException ex4)
				{
					ex = ex4;
				}
				if (ex != null)
				{
					flag = false;
					this.WriteConnectFailed(ex, this._session);
				}
				if (!flag)
				{
					this._session.Runspace.StateChanged -= this.StateCallBackHandler;
					this.SendStartComplete();
				}
			}

			// Token: 0x06002772 RID: 10098 RVA: 0x000DCC6C File Offset: 0x000DAE6C
			internal override void StopOperation()
			{
				if (this._queryRunspaces != null)
				{
					this._queryRunspaces.StopAllOperations();
				}
				this._session.Runspace.StateChanged -= this.StateCallBackHandler;
				this.SendStopComplete();
			}

			// Token: 0x14000086 RID: 134
			// (add) Token: 0x06002773 RID: 10099 RVA: 0x000DCCA4 File Offset: 0x000DAEA4
			// (remove) Token: 0x06002774 RID: 10100 RVA: 0x000DCCDC File Offset: 0x000DAEDC
			internal override event EventHandler<OperationStateEventArgs> OperationComplete;

			// Token: 0x06002775 RID: 10101 RVA: 0x000DCD14 File Offset: 0x000DAF14
			internal PSSession QueryForSession(PSSession session)
			{
				Collection<WSManConnectionInfo> collection = new Collection<WSManConnectionInfo>();
				collection.Add(session.Runspace.ConnectionInfo as WSManConnectionInfo);
				Exception ex = null;
				Collection<PSSession> collection2 = null;
				try
				{
					collection2 = this._queryRunspaces.GetDisconnectedSessions(collection, this._host, this._writeStream, null, 0, SessionFilterState.Disconnected, new Guid[]
					{
						session.InstanceId
					}, null, null);
				}
				catch (RuntimeException ex2)
				{
					ex = ex2;
				}
				if (ex != null)
				{
					this.WriteConnectFailed(ex, session);
					return null;
				}
				if (collection2.Count != 1)
				{
					ex = new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.CannotFindSessionForConnect, session.Name, session.ComputerName));
					this.WriteConnectFailed(ex, session);
					return null;
				}
				return collection2[0];
			}

			// Token: 0x06002776 RID: 10102 RVA: 0x000DCDD8 File Offset: 0x000DAFD8
			private void StateCallBackHandler(object sender, RunspaceStateEventArgs eArgs)
			{
				if (eArgs.RunspaceStateInfo.State == RunspaceState.Connecting || eArgs.RunspaceStateInfo.State == RunspaceState.Disconnecting || eArgs.RunspaceStateInfo.State == RunspaceState.Disconnected)
				{
					return;
				}
				if (eArgs.RunspaceStateInfo.State == RunspaceState.Opened)
				{
					this.WriteConnectedPSSession();
				}
				else
				{
					bool flag = true;
					if (this._queryRunspaces == null)
					{
						PSRemotingTransportException ex = eArgs.RunspaceStateInfo.Reason as PSRemotingTransportException;
						if (ex != null && ex.ErrorCode == -2144108083)
						{
							lock (ConnectPSSessionCommand.ConnectRunspaceOperation.s_LockObject)
							{
								this._retryList.Add(this._session);
							}
							flag = false;
						}
					}
					if (flag)
					{
						this.WriteConnectFailed(eArgs.RunspaceStateInfo.Reason, this._session);
					}
				}
				this._session.Runspace.StateChanged -= this.StateCallBackHandler;
				this.SendStartComplete();
			}

			// Token: 0x06002777 RID: 10103 RVA: 0x000DCECC File Offset: 0x000DB0CC
			private void SendStartComplete()
			{
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StartComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x06002778 RID: 10104 RVA: 0x000DCEF4 File Offset: 0x000DB0F4
			private void SendStopComplete()
			{
				OperationStateEventArgs operationStateEventArgs = new OperationStateEventArgs();
				operationStateEventArgs.OperationState = OperationState.StopComplete;
				this.OperationComplete.SafeInvoke(this, operationStateEventArgs);
			}

			// Token: 0x06002779 RID: 10105 RVA: 0x000DCF34 File Offset: 0x000DB134
			private void WriteConnectedPSSession()
			{
				PSSession outSession = this._session;
				if (this._queryRunspaces != null)
				{
					lock (ConnectPSSessionCommand.ConnectRunspaceOperation.s_LockObject)
					{
						if (this._oldSession != null && this._oldSession.InsertRunspace(this._session.Runspace as RemoteRunspace))
						{
							outSession = this._oldSession;
							this._retryList.Add(this._oldSession);
						}
						else
						{
							this._retryList.Add(this._session);
						}
					}
				}
				if (this._writeStream.ObjectWriter.IsOpen)
				{
					Action<Cmdlet> obj2 = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteObject(outSession);
					};
					this._writeStream.ObjectWriter.Write(obj2);
				}
			}

			// Token: 0x0600277A RID: 10106 RVA: 0x000DD030 File Offset: 0x000DB230
			private void WriteConnectFailed(Exception e, PSSession session)
			{
				if (this._writeStream.ObjectWriter.IsOpen)
				{
					string text = "PSSessionConnectFailed";
					Exception exception;
					if (e != null && !string.IsNullOrEmpty(e.Message))
					{
						PSRemotingTransportException ex = e as PSRemotingTransportException;
						if (ex != null)
						{
							text = WSManTransportManagerUtils.GetFQEIDFromTransportError(ex.ErrorCode, text);
						}
						exception = new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.RunspaceConnectFailedWithMessage, session.Name, e.Message), e);
					}
					else
					{
						exception = new RuntimeException(StringUtil.Format(RemotingErrorIdStrings.RunspaceConnectFailed, session.Name, session.Runspace.RunspaceStateInfo.State.ToString()), null);
					}
					ErrorRecord errorRecord = new ErrorRecord(exception, text, ErrorCategory.InvalidOperation, null);
					Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteError(errorRecord);
					};
					this._writeStream.ObjectWriter.Write(obj);
				}
			}

			// Token: 0x04001378 RID: 4984
			private PSSession _session;

			// Token: 0x04001379 RID: 4985
			private PSSession _oldSession;

			// Token: 0x0400137A RID: 4986
			private ObjectStream _writeStream;

			// Token: 0x0400137B RID: 4987
			private Collection<PSSession> _retryList;

			// Token: 0x0400137C RID: 4988
			private PSHost _host;

			// Token: 0x0400137D RID: 4989
			private QueryRunspaces _queryRunspaces;

			// Token: 0x0400137E RID: 4990
			private static object s_LockObject = new object();
		}

		// Token: 0x0200032F RID: 815
		private enum OverrideParameter
		{
			// Token: 0x04001381 RID: 4993
			None,
			// Token: 0x04001382 RID: 4994
			Name,
			// Token: 0x04001383 RID: 4995
			InstanceId
		}
	}
}
