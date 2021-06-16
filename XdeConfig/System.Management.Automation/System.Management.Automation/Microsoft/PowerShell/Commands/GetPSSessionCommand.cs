using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000336 RID: 822
	[OutputType(new Type[]
	{
		typeof(PSSession)
	})]
	[Cmdlet("Get", "PSSession", DefaultParameterSetName = "Name", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135219", RemotingCapability = RemotingCapability.OwnedByCommand)]
	public class GetPSSessionCommand : PSRunspaceCmdlet, IDisposable
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000DEE69 File Offset: 0x000DD069
		// (set) Token: 0x060027C8 RID: 10184 RVA: 0x000DEE71 File Offset: 0x000DD071
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
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

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x000DEE7A File Offset: 0x000DD07A
		// (set) Token: 0x060027CA RID: 10186 RVA: 0x000DEE82 File Offset: 0x000DD082
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
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

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x000DEE91 File Offset: 0x000DD091
		// (set) Token: 0x060027CC RID: 10188 RVA: 0x000DEE99 File Offset: 0x000DD099
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUri")]
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
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

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060027CD RID: 10189 RVA: 0x000DEEA2 File Offset: 0x000DD0A2
		// (set) Token: 0x060027CE RID: 10190 RVA: 0x000DEEAA File Offset: 0x000DD0AA
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUri")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerInstanceId")]
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

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x000DEEB9 File Offset: 0x000DD0B9
		// (set) Token: 0x060027D0 RID: 10192 RVA: 0x000DEEC6 File Offset: 0x000DD0C6
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
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

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000DEED4 File Offset: 0x000DD0D4
		// (set) Token: 0x060027D2 RID: 10194 RVA: 0x000DEEDC File Offset: 0x000DD0DC
		[ValidateNotNullOrEmpty]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Name")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ConnectionUri")]
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

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000DEEE5 File Offset: 0x000DD0E5
		// (set) Token: 0x060027D4 RID: 10196 RVA: 0x000DEEED File Offset: 0x000DD0ED
		[Parameter(ParameterSetName = "ConnectionUriInstanceId", Mandatory = true)]
		[Parameter(ParameterSetName = "ComputerInstanceId", Mandatory = true)]
		[ValidateNotNull]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "InstanceId")]
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

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060027D5 RID: 10197 RVA: 0x000DEEF6 File Offset: 0x000DD0F6
		// (set) Token: 0x060027D6 RID: 10198 RVA: 0x000DEEFE File Offset: 0x000DD0FE
		[Credential]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
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

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000DEF1E File Offset: 0x000DD11E
		// (set) Token: 0x060027D8 RID: 10200 RVA: 0x000DEF26 File Offset: 0x000DD126
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ComputerName")]
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

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000DEF46 File Offset: 0x000DD146
		// (set) Token: 0x060027DA RID: 10202 RVA: 0x000DEF4E File Offset: 0x000DD14E
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
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

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000DEF6E File Offset: 0x000DD16E
		// (set) Token: 0x060027DC RID: 10204 RVA: 0x000DEF76 File Offset: 0x000DD176
		[Parameter(ParameterSetName = "ComputerInstanceId")]
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

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x000DEF7F File Offset: 0x000DD17F
		// (set) Token: 0x060027DE RID: 10206 RVA: 0x000DEF87 File Offset: 0x000DD187
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ComputerName")]
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

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000DEF90 File Offset: 0x000DD190
		// (set) Token: 0x060027E0 RID: 10208 RVA: 0x000DEF98 File Offset: 0x000DD198
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ComputerName")]
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

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x000DEFA1 File Offset: 0x000DD1A1
		// (set) Token: 0x060027E2 RID: 10210 RVA: 0x000DEFA9 File Offset: 0x000DD1A9
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
		public SessionFilterState State
		{
			get
			{
				return this.filterState;
			}
			set
			{
				this.filterState = value;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x000DEFB2 File Offset: 0x000DD1B2
		// (set) Token: 0x060027E4 RID: 10212 RVA: 0x000DEFBA File Offset: 0x000DD1BA
		[Parameter(ParameterSetName = "ComputerName")]
		[Parameter(ParameterSetName = "ComputerInstanceId")]
		[Parameter(ParameterSetName = "ConnectionUri")]
		[Parameter(ParameterSetName = "ConnectionUriInstanceId")]
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

		// Token: 0x060027E5 RID: 10213 RVA: 0x000DEFC4 File Offset: 0x000DD1C4
		protected override void ProcessRecord()
		{
			if (base.ParameterSetName == "Name" && (this.Name == null || this.Name.Length == 0))
			{
				base.GetAllRunspaces(true, true);
				return;
			}
			if (base.ParameterSetName == "ComputerName" || base.ParameterSetName == "ComputerInstanceId" || base.ParameterSetName == "ConnectionUri" || base.ParameterSetName == "ConnectionUriInstanceId")
			{
				this.QueryForRemoteSessions();
				return;
			}
			base.GetMatchingRunspaces(true, true);
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000DF057 File Offset: 0x000DD257
		protected override void EndProcessing()
		{
			this.stream.ObjectWriter.Close();
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000DF069 File Offset: 0x000DD269
		protected override void StopProcessing()
		{
			this.queryRunspaces.StopAllOperations();
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000DF078 File Offset: 0x000DD278
		private void QueryForRemoteSessions()
		{
			Collection<WSManConnectionInfo> connectionObjects = this.GetConnectionObjects();
			Collection<PSSession> disconnectedSessions = this.queryRunspaces.GetDisconnectedSessions(connectionObjects, base.Host, this.stream, base.RunspaceRepository, this.throttleLimit, this.filterState, this.InstanceId, this.Name, this.ConfigurationName);
			Collection<object> collection = this.stream.ObjectReader.NonBlockingRead();
			foreach (object obj in collection)
			{
				if (base.IsStopping)
				{
					break;
				}
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
			foreach (PSSession sendToPipeline in disconnectedSessions)
			{
				if (base.IsStopping)
				{
					break;
				}
				base.WriteObject(sendToPipeline);
			}
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000DF170 File Offset: 0x000DD370
		private Collection<WSManConnectionInfo> GetConnectionObjects()
		{
			Collection<WSManConnectionInfo> collection = new Collection<WSManConnectionInfo>();
			if (base.ParameterSetName == "ComputerName" || base.ParameterSetName == "ComputerInstanceId")
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
			else if (base.ParameterSetName == "ConnectionUri" || base.ParameterSetName == "ConnectionUriInstanceId")
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

		// Token: 0x060027EA RID: 10218 RVA: 0x000DF318 File Offset: 0x000DD518
		private void UpdateConnectionInfo(WSManConnectionInfo connectionInfo)
		{
			if (base.ParameterSetName != "ConnectionUri" && base.ParameterSetName != "ConnectionUriInstanceId")
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

		// Token: 0x060027EB RID: 10219 RVA: 0x000DF373 File Offset: 0x000DD573
		public void Dispose()
		{
			this.stream.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x040013B1 RID: 5041
		private const string ConnectionUriParameterSet = "ConnectionUri";

		// Token: 0x040013B2 RID: 5042
		private const string ComputerInstanceIdParameterSet = "ComputerInstanceId";

		// Token: 0x040013B3 RID: 5043
		private const string ConnectionUriInstanceIdParameterSet = "ConnectionUriInstanceId";

		// Token: 0x040013B4 RID: 5044
		private string[] computerNames;

		// Token: 0x040013B5 RID: 5045
		private string appName;

		// Token: 0x040013B6 RID: 5046
		private Uri[] uris;

		// Token: 0x040013B7 RID: 5047
		private string shell;

		// Token: 0x040013B8 RID: 5048
		private bool allowRedirection;

		// Token: 0x040013B9 RID: 5049
		private PSCredential psCredential;

		// Token: 0x040013BA RID: 5050
		private AuthenticationMechanism authentication;

		// Token: 0x040013BB RID: 5051
		private string thumbprint;

		// Token: 0x040013BC RID: 5052
		private int port;

		// Token: 0x040013BD RID: 5053
		private SwitchParameter useSSL;

		// Token: 0x040013BE RID: 5054
		private int throttleLimit;

		// Token: 0x040013BF RID: 5055
		private SessionFilterState filterState;

		// Token: 0x040013C0 RID: 5056
		private PSSessionOption sessionOption;

		// Token: 0x040013C1 RID: 5057
		private QueryRunspaces queryRunspaces = new QueryRunspaces();

		// Token: 0x040013C2 RID: 5058
		private ObjectStream stream = new ObjectStream();
	}
}
