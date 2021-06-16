using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Tracing;
using System.Net;
using System.Net.Sockets;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002CE RID: 718
	public sealed class WSManConnectionInfo : RunspaceConnectionInfo
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000C1D40 File Offset: 0x000BFF40
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x000C1D48 File Offset: 0x000BFF48
		public Uri ConnectionUri
		{
			get
			{
				return this._connectionUri;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this.UpdateUri(value);
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x000C1D65 File Offset: 0x000BFF65
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x000C1D70 File Offset: 0x000BFF70
		public override string ComputerName
		{
			get
			{
				return this._computerName;
			}
			set
			{
				this.ConstructUri(this._scheme, value, null, this._appName);
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x000C1D99 File Offset: 0x000BFF99
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x000C1DA4 File Offset: 0x000BFFA4
		public string Scheme
		{
			get
			{
				return this._scheme;
			}
			set
			{
				this.ConstructUri(value, this._computerName, null, this._appName);
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x000C1DCD File Offset: 0x000BFFCD
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x000C1DDA File Offset: 0x000BFFDA
		public int Port
		{
			get
			{
				return this.ConnectionUri.Port;
			}
			set
			{
				this.ConstructUri(this._scheme, this._computerName, new int?(value), this._appName);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x000C1DFA File Offset: 0x000BFFFA
		// (set) Token: 0x06002208 RID: 8712 RVA: 0x000C1E04 File Offset: 0x000C0004
		public string AppName
		{
			get
			{
				return this._appName;
			}
			set
			{
				this.ConstructUri(this._scheme, this._computerName, null, value);
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x000C1E2D File Offset: 0x000C002D
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x000C1E35 File Offset: 0x000C0035
		public override PSCredential Credential
		{
			get
			{
				return this._credential;
			}
			set
			{
				this._credential = value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000C1E3E File Offset: 0x000C003E
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x000C1E46 File Offset: 0x000C0046
		public string ShellUri
		{
			get
			{
				return this._shellUri;
			}
			set
			{
				this._shellUri = this.ResolveShellUri(value);
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000C1E58 File Offset: 0x000C0058
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x000C1EB8 File Offset: 0x000C00B8
		public override AuthenticationMechanism AuthenticationMechanism
		{
			get
			{
				WSManNativeApi.WSManAuthenticationMechanism authMechanism = this._authMechanism;
				if (authMechanism <= WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_BASIC)
				{
					switch (authMechanism)
					{
					case WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_DEFAULT_AUTHENTICATION:
						return AuthenticationMechanism.Default;
					case WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_NO_AUTHENTICATION:
					case WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_NO_AUTHENTICATION | WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_DIGEST:
						break;
					case WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_DIGEST:
						return AuthenticationMechanism.Digest;
					case WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_NEGOTIATE:
						if (this._allowImplicitCredForNegotiate)
						{
							return AuthenticationMechanism.NegotiateWithImplicitCredential;
						}
						return AuthenticationMechanism.Negotiate;
					default:
						if (authMechanism == WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_BASIC)
						{
							return AuthenticationMechanism.Basic;
						}
						break;
					}
				}
				else
				{
					if (authMechanism == WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_KERBEROS)
					{
						return AuthenticationMechanism.Kerberos;
					}
					if (authMechanism == WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_CREDSSP)
					{
						return AuthenticationMechanism.Credssp;
					}
				}
				return AuthenticationMechanism.Default;
			}
			set
			{
				switch (value)
				{
				case AuthenticationMechanism.Default:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_DEFAULT_AUTHENTICATION;
					break;
				case AuthenticationMechanism.Basic:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_BASIC;
					break;
				case AuthenticationMechanism.Negotiate:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_NEGOTIATE;
					break;
				case AuthenticationMechanism.NegotiateWithImplicitCredential:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_NEGOTIATE;
					this._allowImplicitCredForNegotiate = true;
					break;
				case AuthenticationMechanism.Credssp:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_CREDSSP;
					break;
				case AuthenticationMechanism.Digest:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_DIGEST;
					break;
				case AuthenticationMechanism.Kerberos:
					this._authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_KERBEROS;
					break;
				default:
					throw new PSNotSupportedException();
				}
				this.ValidateSpecifiedAuthentication();
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x000C1F42 File Offset: 0x000C0142
		internal WSManNativeApi.WSManAuthenticationMechanism WSManAuthenticationMechanism
		{
			get
			{
				return this._authMechanism;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x000C1F4A File Offset: 0x000C014A
		internal bool AllowImplicitCredentialForNegotiate
		{
			get
			{
				return this._allowImplicitCredForNegotiate;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000C1F52 File Offset: 0x000C0152
		internal int PortSetting
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x000C1F5A File Offset: 0x000C015A
		// (set) Token: 0x06002213 RID: 8723 RVA: 0x000C1F62 File Offset: 0x000C0162
		public override string CertificateThumbprint
		{
			get
			{
				return this._thumbPrint;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this._thumbPrint = value;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x000C1F79 File Offset: 0x000C0179
		// (set) Token: 0x06002215 RID: 8725 RVA: 0x000C1F81 File Offset: 0x000C0181
		public int MaximumConnectionRedirectionCount
		{
			get
			{
				return this._maxUriRedirectionCount;
			}
			set
			{
				this._maxUriRedirectionCount = value;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x000C1F8A File Offset: 0x000C018A
		// (set) Token: 0x06002217 RID: 8727 RVA: 0x000C1F92 File Offset: 0x000C0192
		public int? MaximumReceivedDataSizePerCommand
		{
			get
			{
				return this._maxRecvdDataSizePerCommand;
			}
			set
			{
				this._maxRecvdDataSizePerCommand = value;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000C1F9B File Offset: 0x000C019B
		// (set) Token: 0x06002219 RID: 8729 RVA: 0x000C1FA3 File Offset: 0x000C01A3
		public int? MaximumReceivedObjectSize
		{
			get
			{
				return this._maxRecvdObjectSize;
			}
			set
			{
				this._maxRecvdObjectSize = value;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000C1FAC File Offset: 0x000C01AC
		// (set) Token: 0x0600221B RID: 8731 RVA: 0x000C1FB4 File Offset: 0x000C01B4
		public bool UseCompression
		{
			get
			{
				return this._useCompression;
			}
			set
			{
				this._useCompression = value;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000C1FBD File Offset: 0x000C01BD
		// (set) Token: 0x0600221D RID: 8733 RVA: 0x000C1FC5 File Offset: 0x000C01C5
		public bool NoMachineProfile
		{
			get
			{
				return this._noMachineProfile;
			}
			set
			{
				this._noMachineProfile = value;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x000C1FCE File Offset: 0x000C01CE
		// (set) Token: 0x0600221F RID: 8735 RVA: 0x000C1FD6 File Offset: 0x000C01D6
		public ProxyAccessType ProxyAccessType
		{
			get
			{
				return this._proxyAcessType;
			}
			set
			{
				this._proxyAcessType = value;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x000C1FDF File Offset: 0x000C01DF
		// (set) Token: 0x06002221 RID: 8737 RVA: 0x000C1FE8 File Offset: 0x000C01E8
		public AuthenticationMechanism ProxyAuthentication
		{
			get
			{
				return this._proxyAuthentication;
			}
			set
			{
				switch (value)
				{
				case AuthenticationMechanism.Basic:
				case AuthenticationMechanism.Negotiate:
				case AuthenticationMechanism.Digest:
					this._proxyAuthentication = value;
					return;
				}
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ProxyAmbiguosAuthentication, new object[]
				{
					value,
					AuthenticationMechanism.Basic.ToString(),
					AuthenticationMechanism.Negotiate.ToString(),
					AuthenticationMechanism.Digest.ToString()
				});
				throw new ArgumentException(message);
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x000C2069 File Offset: 0x000C0269
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x000C2074 File Offset: 0x000C0274
		public PSCredential ProxyCredential
		{
			get
			{
				return this._proxyCredential;
			}
			set
			{
				if (this._proxyAcessType == ProxyAccessType.None)
				{
					string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ProxyCredentialWithoutAccess, new object[]
					{
						ProxyAccessType.None
					});
					throw new ArgumentException(message);
				}
				this._proxyCredential = value;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x000C20B3 File Offset: 0x000C02B3
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x000C20BB File Offset: 0x000C02BB
		public bool SkipCACheck
		{
			get
			{
				return this._skipCaCheck;
			}
			set
			{
				this._skipCaCheck = value;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x000C20C4 File Offset: 0x000C02C4
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x000C20CC File Offset: 0x000C02CC
		public bool SkipCNCheck
		{
			get
			{
				return this._skipCnCheck;
			}
			set
			{
				this._skipCnCheck = value;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000C20D5 File Offset: 0x000C02D5
		// (set) Token: 0x06002229 RID: 8745 RVA: 0x000C20DD File Offset: 0x000C02DD
		public bool SkipRevocationCheck
		{
			get
			{
				return this._skipRevocationCheck;
			}
			set
			{
				this._skipRevocationCheck = value;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000C20E6 File Offset: 0x000C02E6
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x000C20EE File Offset: 0x000C02EE
		public bool NoEncryption
		{
			get
			{
				return this._noEncryption;
			}
			set
			{
				this._noEncryption = value;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000C20F7 File Offset: 0x000C02F7
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x000C20FF File Offset: 0x000C02FF
		public bool UseUTF16
		{
			get
			{
				return this._useUtf16;
			}
			set
			{
				this._useUtf16 = value;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000C2108 File Offset: 0x000C0308
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x000C2110 File Offset: 0x000C0310
		public OutputBufferingMode OutputBufferingMode
		{
			get
			{
				return this._outputBufferingMode;
			}
			set
			{
				this._outputBufferingMode = value;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x000C2119 File Offset: 0x000C0319
		// (set) Token: 0x06002231 RID: 8753 RVA: 0x000C2121 File Offset: 0x000C0321
		public bool IncludePortInSPN
		{
			get
			{
				return this._includePortInSPN;
			}
			set
			{
				this._includePortInSPN = value;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x000C212A File Offset: 0x000C032A
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x000C2132 File Offset: 0x000C0332
		public bool EnableNetworkAccess
		{
			get
			{
				return this._enableNetworkAccess;
			}
			set
			{
				this._enableNetworkAccess = value;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x000C213B File Offset: 0x000C033B
		// (set) Token: 0x06002235 RID: 8757 RVA: 0x000C2143 File Offset: 0x000C0343
		public int MaxConnectionRetryCount
		{
			get
			{
				return this._maxConnectionRetryCount;
			}
			set
			{
				this._maxConnectionRetryCount = value;
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000C214C File Offset: 0x000C034C
		public WSManConnectionInfo(string scheme, string computerName, int port, string appName, string shellUri, PSCredential credential, int openTimeout)
		{
			this.Scheme = scheme;
			this.ComputerName = computerName;
			this.Port = port;
			this.AppName = appName;
			this.ShellUri = shellUri;
			this.Credential = credential;
			base.OpenTimeout = openTimeout;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000C21E5 File Offset: 0x000C03E5
		public WSManConnectionInfo(string scheme, string computerName, int port, string appName, string shellUri, PSCredential credential) : this(scheme, computerName, port, appName, shellUri, credential, 180000)
		{
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000C21FB File Offset: 0x000C03FB
		public WSManConnectionInfo(bool useSsl, string computerName, int port, string appName, string shellUri, PSCredential credential) : this(useSsl ? "https" : "http", computerName, port, appName, shellUri, credential)
		{
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000C221A File Offset: 0x000C041A
		public WSManConnectionInfo(bool useSsl, string computerName, int port, string appName, string shellUri, PSCredential credential, int openTimeout) : this(useSsl ? "https" : "http", computerName, port, appName, shellUri, credential, openTimeout)
		{
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000C223C File Offset: 0x000C043C
		public WSManConnectionInfo()
		{
			this.UseDefaultWSManPort = true;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000C22A8 File Offset: 0x000C04A8
		public WSManConnectionInfo(Uri uri, string shellUri, PSCredential credential)
		{
			if (uri == null)
			{
				this.ShellUri = shellUri;
				this.Credential = credential;
				this.UseDefaultWSManPort = true;
				return;
			}
			if (!uri.IsAbsoluteUri)
			{
				throw new NotSupportedException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RelativeUriForRunspacePathNotSupported, new object[0]));
			}
			if (uri.AbsolutePath.Equals("/", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment))
			{
				this.ConstructUri(uri.Scheme, uri.Host, new int?(uri.Port), WSManConnectionInfo.DefaultAppName);
			}
			else
			{
				this.ConnectionUri = uri;
			}
			this.ShellUri = shellUri;
			this.Credential = credential;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000C23AF File Offset: 0x000C05AF
		public WSManConnectionInfo(Uri uri, string shellUri, string certificateThumbprint) : this(uri, shellUri, null)
		{
			this._thumbPrint = certificateThumbprint;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000C23C1 File Offset: 0x000C05C1
		public WSManConnectionInfo(Uri uri) : this(uri, "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", null)
		{
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000C23D0 File Offset: 0x000C05D0
		public override void SetSessionOptions(PSSessionOption options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (options.ProxyAccessType == ProxyAccessType.None && options.ProxyCredential != null)
			{
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ProxyCredentialWithoutAccess, new object[]
				{
					ProxyAccessType.None
				});
				throw new ArgumentException(message);
			}
			base.SetSessionOptions(options);
			this.MaximumConnectionRedirectionCount = ((options.MaximumConnectionRedirectionCount >= 0) ? options.MaximumConnectionRedirectionCount : int.MaxValue);
			this.MaximumReceivedDataSizePerCommand = options.MaximumReceivedDataSizePerCommand;
			this.MaximumReceivedObjectSize = options.MaximumReceivedObjectSize;
			this.UseCompression = !options.NoCompression;
			this.NoMachineProfile = options.NoMachineProfile;
			this._proxyAcessType = options.ProxyAccessType;
			this._proxyAuthentication = options.ProxyAuthentication;
			this._proxyCredential = options.ProxyCredential;
			this._skipCaCheck = options.SkipCACheck;
			this._skipCnCheck = options.SkipCNCheck;
			this._skipRevocationCheck = options.SkipRevocationCheck;
			this._noEncryption = options.NoEncryption;
			this._useUtf16 = options.UseUTF16;
			this._includePortInSPN = options.IncludePortInSPN;
			this._outputBufferingMode = options.OutputBufferingMode;
			this._maxConnectionRetryCount = options.MaxConnectionRetryCount;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000C24F8 File Offset: 0x000C06F8
		internal override RunspaceConnectionInfo InternalCopy()
		{
			return this.Copy();
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000C2500 File Offset: 0x000C0700
		public WSManConnectionInfo Copy()
		{
			return new WSManConnectionInfo
			{
				_connectionUri = this._connectionUri,
				_computerName = this._computerName,
				_scheme = this._scheme,
				_port = this._port,
				_appName = this._appName,
				_shellUri = this._shellUri,
				_credential = this._credential,
				UseDefaultWSManPort = this.UseDefaultWSManPort,
				_authMechanism = this._authMechanism,
				_maxUriRedirectionCount = this._maxUriRedirectionCount,
				_maxRecvdDataSizePerCommand = this._maxRecvdDataSizePerCommand,
				_maxRecvdObjectSize = this._maxRecvdObjectSize,
				OpenTimeout = base.OpenTimeout,
				IdleTimeout = base.IdleTimeout,
				MaxIdleTimeout = base.MaxIdleTimeout,
				CancelTimeout = base.CancelTimeout,
				OperationTimeout = base.OperationTimeout,
				Culture = base.Culture,
				UICulture = base.UICulture,
				_thumbPrint = this._thumbPrint,
				_allowImplicitCredForNegotiate = this._allowImplicitCredForNegotiate,
				UseCompression = this._useCompression,
				NoMachineProfile = this._noMachineProfile,
				_proxyAcessType = this.ProxyAccessType,
				_proxyAuthentication = this.ProxyAuthentication,
				_proxyCredential = this.ProxyCredential,
				_skipCaCheck = this.SkipCACheck,
				_skipCnCheck = this.SkipCNCheck,
				_skipRevocationCheck = this.SkipRevocationCheck,
				_noEncryption = this.NoEncryption,
				_useUtf16 = this.UseUTF16,
				_includePortInSPN = this.IncludePortInSPN,
				_enableNetworkAccess = this.EnableNetworkAccess,
				UseDefaultWSManPort = this.UseDefaultWSManPort,
				_outputBufferingMode = this._outputBufferingMode,
				DisconnectedOn = this.DisconnectedOn,
				ExpiresOn = this.ExpiresOn,
				MaxConnectionRetryCount = this.MaxConnectionRetryCount
			};
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000C26DC File Offset: 0x000C08DC
		internal override BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			return new WSManClientSessionTransportManager(instanceId, this, cryptoHelper, sessionName);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000C26E8 File Offset: 0x000C08E8
		private string ResolveShellUri(string shell)
		{
			string text = shell;
			if (string.IsNullOrEmpty(text))
			{
				text = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
			}
			if (text.IndexOf("http://schemas.microsoft.com/powershell/", StringComparison.OrdinalIgnoreCase) == -1)
			{
				text = "http://schemas.microsoft.com/powershell/" + text;
			}
			return text;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000C2724 File Offset: 0x000C0924
		internal static T ExtractPropertyAsWsManConnectionInfo<T>(RunspaceConnectionInfo rsCI, string property, T defaultValue)
		{
			WSManConnectionInfo wsmanConnectionInfo = rsCI as WSManConnectionInfo;
			if (wsmanConnectionInfo == null)
			{
				return defaultValue;
			}
			return (T)((object)typeof(WSManConnectionInfo).GetProperty(property, typeof(T)).GetValue(wsmanConnectionInfo, null));
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000C2763 File Offset: 0x000C0963
		internal void SetConnectionUri(Uri newUri)
		{
			this._connectionUri = newUri;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000C276C File Offset: 0x000C096C
		internal void ConstructUri(string scheme, string computerName, int? port, string appName)
		{
			this._scheme = scheme;
			if (string.IsNullOrEmpty(this._scheme))
			{
				this._scheme = "http";
			}
			if (!this._scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && !this._scheme.Equals("https", StringComparison.OrdinalIgnoreCase) && !this._scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
			{
				string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.InvalidSchemeValue, new object[]
				{
					this._scheme
				});
				ArgumentException ex = new ArgumentException(message);
				throw ex;
			}
			if (string.IsNullOrEmpty(computerName) || string.Equals(computerName, ".", StringComparison.OrdinalIgnoreCase))
			{
				this._computerName = "localhost";
			}
			else
			{
				this._computerName = computerName.Trim();
				IPAddress ipaddress = null;
				bool flag = IPAddress.TryParse(this._computerName, out ipaddress);
				if (flag && ipaddress.AddressFamily == AddressFamily.InterNetworkV6 && (this._computerName.Length == 0 || this._computerName[0] != '['))
				{
					this._computerName = "[" + this._computerName + "]";
				}
			}
			PSEtwLog.LogAnalyticVerbose(PSEventId.ComputerName, PSOpcode.Method, PSTask.CreateRunspace, (PSKeyword)4611686018427387905UL, new object[]
			{
				this._computerName
			});
			if (port != null)
			{
				if (port.Value == 0)
				{
					this._port = -1;
					this.UseDefaultWSManPort = true;
				}
				else if (port.Value == 80 || port.Value == 443)
				{
					this._port = port.Value;
					this.UseDefaultWSManPort = false;
				}
				else
				{
					if (port.Value < 0 || port.Value > 65535)
					{
						string message2 = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.PortIsOutOfRange, new object[]
						{
							port
						});
						ArgumentException ex2 = new ArgumentException(message2);
						throw ex2;
					}
					this._port = port.Value;
					this.UseDefaultWSManPort = false;
				}
			}
			this._appName = appName;
			if (string.IsNullOrEmpty(this._appName))
			{
				this._appName = WSManConnectionInfo.DefaultAppName;
			}
			UriBuilder uriBuilder = new UriBuilder(this._scheme, this._computerName, this._port, this._appName);
			this._connectionUri = uriBuilder.Uri;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000C29A4 File Offset: 0x000C0BA4
		internal static string GetConnectionString(Uri connectionUri, out bool isSSLSpecified)
		{
			isSSLSpecified = connectionUri.Scheme.Equals("https");
			string text = connectionUri.OriginalString.TrimStart(new char[0]);
			if (isSSLSpecified)
			{
				return text.Substring("https".Length + 3);
			}
			return text.Substring("http".Length + 3);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000C2A00 File Offset: 0x000C0C00
		private void ValidateSpecifiedAuthentication()
		{
			if (this._authMechanism != WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_DEFAULT_AUTHENTICATION && this._thumbPrint != null)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NewRunspaceAmbiguosAuthentication, new object[]
				{
					"CertificateThumbPrint",
					this.AuthenticationMechanism.ToString()
				});
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000C2A4C File Offset: 0x000C0C4C
		private void UpdateUri(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
			{
				throw new NotSupportedException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.RelativeUriForRunspacePathNotSupported, new object[0]));
			}
			if (uri.OriginalString.LastIndexOf(":", StringComparison.OrdinalIgnoreCase) > uri.AbsoluteUri.IndexOf("//", StringComparison.OrdinalIgnoreCase))
			{
				this._useDefaultWSManPort = false;
			}
			if (uri.AbsolutePath.Equals("/", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment))
			{
				string defaultAppName = WSManConnectionInfo.DefaultAppName;
				this.ConstructUri(uri.Scheme, uri.Host, new int?(uri.Port), defaultAppName);
				return;
			}
			this._connectionUri = uri;
			this._scheme = uri.Scheme;
			this._appName = uri.AbsolutePath;
			this._port = uri.Port;
			this._computerName = uri.Host;
			this._useDefaultWSManPort = false;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x000C2B32 File Offset: 0x000C0D32
		// (set) Token: 0x0600224A RID: 8778 RVA: 0x000C2B3A File Offset: 0x000C0D3A
		internal bool UseDefaultWSManPort
		{
			get
			{
				return this._useDefaultWSManPort;
			}
			set
			{
				this._useDefaultWSManPort = value;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x000C2B43 File Offset: 0x000C0D43
		internal bool IsLocalhostAndNetworkAccess
		{
			get
			{
				return this.EnableNetworkAccess && this.Credential == null && (this.ComputerName.Equals("localhost", StringComparison.OrdinalIgnoreCase) || this.ComputerName.IndexOf('.') == -1);
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x000C2B7E File Offset: 0x000C0D7E
		// (set) Token: 0x0600224D RID: 8781 RVA: 0x000C2B86 File Offset: 0x000C0D86
		internal DateTime? DisconnectedOn { get; set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x000C2B8F File Offset: 0x000C0D8F
		// (set) Token: 0x0600224F RID: 8783 RVA: 0x000C2B97 File Offset: 0x000C0D97
		internal DateTime? ExpiresOn { get; set; }

		// Token: 0x06002250 RID: 8784 RVA: 0x000C2BA0 File Offset: 0x000C0DA0
		internal void NullDisconnectedExpiresOn()
		{
			this.DisconnectedOn = null;
			this.ExpiresOn = null;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000C2BCC File Offset: 0x000C0DCC
		internal void SetDisconnectedExpiresOnToNow()
		{
			TimeSpan value = TimeSpan.FromSeconds((double)(base.IdleTimeout / 1000));
			DateTime now = DateTime.Now;
			this.DisconnectedOn = new DateTime?(now);
			this.ExpiresOn = new DateTime?(now.Add(value));
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000C2C14 File Offset: 0x000C0E14
		public WSManConnectionInfo(PSSessionType configurationType) : this()
		{
			this.ComputerName = string.Empty;
			switch (configurationType)
			{
			case PSSessionType.DefaultRemoteShell:
				break;
			case PSSessionType.Workflow:
				this.ShellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell.Workflow";
				break;
			default:
				return;
			}
		}

		// Token: 0x0400102E RID: 4142
		internal const int defaultMaximumConnectionRedirectionCount = 5;

		// Token: 0x0400102F RID: 4143
		public const string HttpScheme = "http";

		// Token: 0x04001030 RID: 4144
		public const string HttpsScheme = "https";

		// Token: 0x04001031 RID: 4145
		internal const OutputBufferingMode DefaultOutputBufferingMode = OutputBufferingMode.None;

		// Token: 0x04001032 RID: 4146
		internal const int DefaultMaxConnectionRetryCount = 5;

		// Token: 0x04001033 RID: 4147
		private const string DefaultScheme = "http";

		// Token: 0x04001034 RID: 4148
		private const string DefaultSslScheme = "https";

		// Token: 0x04001035 RID: 4149
		private const int DefaultPortHttp = 80;

		// Token: 0x04001036 RID: 4150
		private const int DefaultPortHttps = 443;

		// Token: 0x04001037 RID: 4151
		private const int DefaultPort = 0;

		// Token: 0x04001038 RID: 4152
		private const string DefaultComputerName = "localhost";

		// Token: 0x04001039 RID: 4153
		private const int MaxPort = 65535;

		// Token: 0x0400103A RID: 4154
		private const int MinPort = 0;

		// Token: 0x0400103B RID: 4155
		private const string LocalHostUriString = "http://localhost/wsman";

		// Token: 0x0400103C RID: 4156
		private const string DefaultShellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

		// Token: 0x0400103D RID: 4157
		private const PSCredential DefaultCredential = null;

		// Token: 0x0400103E RID: 4158
		private const string DefaultM3PShellName = "Microsoft.PowerShell.Workflow";

		// Token: 0x0400103F RID: 4159
		private const string DefaultM3PEndpoint = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell.Workflow";

		// Token: 0x04001040 RID: 4160
		private string _scheme = "http";

		// Token: 0x04001041 RID: 4161
		private string _computerName = "localhost";

		// Token: 0x04001042 RID: 4162
		private string _appName = WSManConnectionInfo.DefaultAppName;

		// Token: 0x04001043 RID: 4163
		private int _port = -1;

		// Token: 0x04001044 RID: 4164
		private Uri _connectionUri = new Uri("http://localhost/wsman");

		// Token: 0x04001045 RID: 4165
		private PSCredential _credential;

		// Token: 0x04001046 RID: 4166
		private string _shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

		// Token: 0x04001047 RID: 4167
		private WSManNativeApi.WSManAuthenticationMechanism _authMechanism;

		// Token: 0x04001048 RID: 4168
		private bool _allowImplicitCredForNegotiate;

		// Token: 0x04001049 RID: 4169
		private string _thumbPrint;

		// Token: 0x0400104A RID: 4170
		private int _maxUriRedirectionCount;

		// Token: 0x0400104B RID: 4171
		private int? _maxRecvdDataSizePerCommand;

		// Token: 0x0400104C RID: 4172
		private int? _maxRecvdObjectSize;

		// Token: 0x0400104D RID: 4173
		private bool _useCompression = true;

		// Token: 0x0400104E RID: 4174
		private bool _noMachineProfile;

		// Token: 0x0400104F RID: 4175
		private ProxyAccessType _proxyAcessType;

		// Token: 0x04001050 RID: 4176
		private AuthenticationMechanism _proxyAuthentication;

		// Token: 0x04001051 RID: 4177
		private PSCredential _proxyCredential;

		// Token: 0x04001052 RID: 4178
		private bool _skipCaCheck;

		// Token: 0x04001053 RID: 4179
		private bool _skipCnCheck;

		// Token: 0x04001054 RID: 4180
		private bool _skipRevocationCheck;

		// Token: 0x04001055 RID: 4181
		private bool _noEncryption;

		// Token: 0x04001056 RID: 4182
		private bool _useUtf16;

		// Token: 0x04001057 RID: 4183
		private OutputBufferingMode _outputBufferingMode;

		// Token: 0x04001058 RID: 4184
		private bool _includePortInSPN;

		// Token: 0x04001059 RID: 4185
		private bool _enableNetworkAccess;

		// Token: 0x0400105A RID: 4186
		private int _maxConnectionRetryCount = 5;

		// Token: 0x0400105B RID: 4187
		private static readonly string DefaultAppName = "/wsman";

		// Token: 0x0400105C RID: 4188
		private bool _useDefaultWSManPort;
	}
}
