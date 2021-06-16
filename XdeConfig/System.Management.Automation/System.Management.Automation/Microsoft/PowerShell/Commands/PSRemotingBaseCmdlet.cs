using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000324 RID: 804
	public abstract class PSRemotingBaseCmdlet : PSRemotingCmdlet
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002656 RID: 9814 RVA: 0x000D6C07 File Offset: 0x000D4E07
		// (set) Token: 0x06002657 RID: 9815 RVA: 0x000D6C0F File Offset: 0x000D4E0F
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "Session")]
		public virtual PSSession[] Session
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

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x000D6C18 File Offset: 0x000D4E18
		// (set) Token: 0x06002659 RID: 9817 RVA: 0x000D6C20 File Offset: 0x000D4E20
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Alias(new string[]
		{
			"Cn"
		})]
		public virtual string[] ComputerName
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

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x000D6C29 File Offset: 0x000D4E29
		// (set) Token: 0x0600265B RID: 9819 RVA: 0x000D6C31 File Offset: 0x000D4E31
		protected string[] ResolvedComputerNames
		{
			get
			{
				return this.resolvedComputerNames;
			}
			set
			{
				this.resolvedComputerNames = value;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x000D6C3A File Offset: 0x000D4E3A
		// (set) Token: 0x0600265D RID: 9821 RVA: 0x000D6C42 File Offset: 0x000D4E42
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Credential]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		public virtual PSCredential Credential
		{
			get
			{
				return this.pscredential;
			}
			set
			{
				this.pscredential = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x000D6C62 File Offset: 0x000D4E62
		// (set) Token: 0x0600265F RID: 9823 RVA: 0x000D6C6A File Offset: 0x000D4E6A
		[ValidateRange(1, 65535)]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual int Port
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

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x000D6C73 File Offset: 0x000D4E73
		// (set) Token: 0x06002661 RID: 9825 RVA: 0x000D6C7B File Offset: 0x000D4E7B
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual SwitchParameter UseSSL
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

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x000D6C84 File Offset: 0x000D4E84
		// (set) Token: 0x06002663 RID: 9827 RVA: 0x000D6C8C File Offset: 0x000D4E8C
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		public virtual string ConfigurationName
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

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x000D6C9B File Offset: 0x000D4E9B
		// (set) Token: 0x06002665 RID: 9829 RVA: 0x000D6CA3 File Offset: 0x000D4EA3
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		public virtual string ApplicationName
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

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x000D6CBB File Offset: 0x000D4EBB
		// (set) Token: 0x06002666 RID: 9830 RVA: 0x000D6CB2 File Offset: 0x000D4EB2
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "Session")]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual int ThrottleLimit
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

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002668 RID: 9832 RVA: 0x000D6CC3 File Offset: 0x000D4EC3
		// (set) Token: 0x06002669 RID: 9833 RVA: 0x000D6CCB File Offset: 0x000D4ECB
		[Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		[ValidateNotNullOrEmpty]
		[Alias(new string[]
		{
			"URI",
			"CU"
		})]
		public virtual Uri[] ConnectionUri
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

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600266A RID: 9834 RVA: 0x000D6CD4 File Offset: 0x000D4ED4
		// (set) Token: 0x0600266B RID: 9835 RVA: 0x000D6CE1 File Offset: 0x000D4EE1
		[Parameter(ParameterSetName = "Uri")]
		public virtual SwitchParameter AllowRedirection
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

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600266C RID: 9836 RVA: 0x000D6CF0 File Offset: 0x000D4EF0
		// (set) Token: 0x0600266D RID: 9837 RVA: 0x000D6D3D File Offset: 0x000D4F3D
		[Parameter(ParameterSetName = "Uri")]
		[ValidateNotNull]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual PSSessionOption SessionOption
		{
			get
			{
				if (this.sessionOption == null)
				{
					object value = base.SessionState.PSVariable.GetValue("PSSessionOption");
					if (value == null || !LanguagePrimitives.TryConvertTo<PSSessionOption>(value, out this.sessionOption))
					{
						this.sessionOption = new PSSessionOption();
					}
				}
				return this.sessionOption;
			}
			set
			{
				this.sessionOption = value;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600266E RID: 9838 RVA: 0x000D6D46 File Offset: 0x000D4F46
		// (set) Token: 0x0600266F RID: 9839 RVA: 0x000D6D4E File Offset: 0x000D4F4E
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual AuthenticationMechanism Authentication
		{
			get
			{
				return this.authMechanism;
			}
			set
			{
				this.authMechanism = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06002670 RID: 9840 RVA: 0x000D6D6E File Offset: 0x000D4F6E
		// (set) Token: 0x06002671 RID: 9841 RVA: 0x000D6D76 File Offset: 0x000D4F76
		[Parameter(ParameterSetName = "Uri")]
		[Parameter(ParameterSetName = "ComputerName")]
		public virtual string CertificateThumbprint
		{
			get
			{
				return this.thumbPrint;
			}
			set
			{
				this.thumbPrint = value;
				PSRemotingBaseCmdlet.ValidateSpecifiedAuthentication(this.Credential, this.CertificateThumbprint, this.Authentication);
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000D6D98 File Offset: 0x000D4F98
		internal static void ValidateSpecifiedAuthentication(PSCredential credential, string thumbprint, AuthenticationMechanism authentication)
		{
			if (credential != null && thumbprint != null)
			{
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.NewRunspaceAmbiguosAuthentication, new object[]
				{
					"CertificateThumbPrint",
					"Credential"
				});
				throw new InvalidOperationException(message);
			}
			if (authentication != AuthenticationMechanism.Default && thumbprint != null)
			{
				string message2 = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.NewRunspaceAmbiguosAuthentication, new object[]
				{
					"CertificateThumbPrint",
					authentication.ToString()
				});
				throw new InvalidOperationException(message2);
			}
			if (authentication == AuthenticationMechanism.NegotiateWithImplicitCredential && credential != null)
			{
				string message3 = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.NewRunspaceAmbiguosAuthentication, new object[]
				{
					"Credential",
					authentication.ToString()
				});
				throw new InvalidOperationException(message3);
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000D6E4C File Offset: 0x000D504C
		protected void ValidateRemoteRunspacesSpecified()
		{
			if (RemotingCommandUtil.HasRepeatingRunspaces(this.Session))
			{
				base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.RemoteRunspaceInfoHasDuplicates)), PSRemotingErrorId.RemoteRunspaceInfoHasDuplicates.ToString(), ErrorCategory.InvalidArgument, this.Session));
			}
			if (RemotingCommandUtil.ExceedMaximumAllowableRunspaces(this.Session))
			{
				base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(base.GetMessage(RemotingErrorIdStrings.RemoteRunspaceInfoLimitExceeded)), PSRemotingErrorId.RemoteRunspaceInfoLimitExceeded.ToString(), ErrorCategory.InvalidArgument, this.Session));
			}
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000D6ED5 File Offset: 0x000D50D5
		internal void UpdateConnectionInfo(WSManConnectionInfo connectionInfo)
		{
			connectionInfo.SetSessionOptions(this.SessionOption);
			if (!base.ParameterSetName.Equals("Uri", StringComparison.OrdinalIgnoreCase))
			{
				connectionInfo.MaximumConnectionRedirectionCount = 0;
			}
			if (!this.allowRedirection)
			{
				connectionInfo.MaximumConnectionRedirectionCount = 0;
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000D6F0C File Offset: 0x000D510C
		protected void ValidateComputerName(string[] computerNames)
		{
			foreach (string name in computerNames)
			{
				UriHostNameType uriHostNameType = Uri.CheckHostName(name);
				if (uriHostNameType != UriHostNameType.Dns && uriHostNameType != UriHostNameType.IPv4 && uriHostNameType != UriHostNameType.IPv6)
				{
					base.ThrowTerminatingError(new ErrorRecord(new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidComputerName, new object[0])), "PSSessionInvalidComputerName", ErrorCategory.InvalidArgument, computerNames));
				}
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000D6F68 File Offset: 0x000D5168
		protected override void BeginProcessing()
		{
			base.BeginProcessing();
			int num = (int)this.SessionOption.IdleTimeout.TotalMilliseconds;
			if (num != -1 && num < 60000)
			{
				throw new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.InvalidIdleTimeoutOption, num / 1000, 60));
			}
			if (string.IsNullOrEmpty(this.shell))
			{
				this.shell = base.ResolveShell(null);
			}
			if (string.IsNullOrEmpty(this.appName))
			{
				this.appName = base.ResolveAppName(null);
			}
		}

		// Token: 0x040012F1 RID: 4849
		internal const string DEFAULT_SESSION_OPTION = "PSSessionOption";

		// Token: 0x040012F2 RID: 4850
		protected const string UriParameterSet = "Uri";

		// Token: 0x040012F3 RID: 4851
		private PSSession[] remoteRunspaceInfos;

		// Token: 0x040012F4 RID: 4852
		private string[] computerNames;

		// Token: 0x040012F5 RID: 4853
		private string[] resolvedComputerNames;

		// Token: 0x040012F6 RID: 4854
		private PSCredential pscredential;

		// Token: 0x040012F7 RID: 4855
		private int port;

		// Token: 0x040012F8 RID: 4856
		private SwitchParameter useSSL;

		// Token: 0x040012F9 RID: 4857
		private string shell;

		// Token: 0x040012FA RID: 4858
		private string appName;

		// Token: 0x040012FB RID: 4859
		private int throttleLimit;

		// Token: 0x040012FC RID: 4860
		private Uri[] uris;

		// Token: 0x040012FD RID: 4861
		private bool allowRedirection;

		// Token: 0x040012FE RID: 4862
		private PSSessionOption sessionOption;

		// Token: 0x040012FF RID: 4863
		private AuthenticationMechanism authMechanism;

		// Token: 0x04001300 RID: 4864
		private string thumbPrint;
	}
}
