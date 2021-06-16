using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200034D RID: 845
	[Cmdlet("New", "PSSessionOption", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144305", RemotingCapability = RemotingCapability.None)]
	[OutputType(new Type[]
	{
		typeof(PSSessionOption)
	})]
	public sealed class NewPSSessionOptionCommand : PSCmdlet
	{
		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002990 RID: 10640 RVA: 0x000E784F File Offset: 0x000E5A4F
		// (set) Token: 0x06002991 RID: 10641 RVA: 0x000E785C File Offset: 0x000E5A5C
		[Parameter]
		public int MaximumRedirection
		{
			get
			{
				return this.maximumRedirection.Value;
			}
			set
			{
				this.maximumRedirection = new int?(value);
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x000E786A File Offset: 0x000E5A6A
		// (set) Token: 0x06002993 RID: 10643 RVA: 0x000E7872 File Offset: 0x000E5A72
		[Parameter]
		public SwitchParameter NoCompression
		{
			get
			{
				return this.noCompression;
			}
			set
			{
				this.noCompression = value;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000E787B File Offset: 0x000E5A7B
		// (set) Token: 0x06002995 RID: 10645 RVA: 0x000E7883 File Offset: 0x000E5A83
		[Parameter]
		public SwitchParameter NoMachineProfile
		{
			get
			{
				return this.noMachineProfile;
			}
			set
			{
				this.noMachineProfile = value;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x000E788C File Offset: 0x000E5A8C
		// (set) Token: 0x06002997 RID: 10647 RVA: 0x000E7894 File Offset: 0x000E5A94
		[ValidateNotNull]
		[Parameter]
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
			set
			{
				this.culture = value;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x000E789D File Offset: 0x000E5A9D
		// (set) Token: 0x06002999 RID: 10649 RVA: 0x000E78A5 File Offset: 0x000E5AA5
		[ValidateNotNull]
		[Parameter]
		public CultureInfo UICulture
		{
			get
			{
				return this.uiCulture;
			}
			set
			{
				this.uiCulture = value;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000E78AE File Offset: 0x000E5AAE
		// (set) Token: 0x0600299B RID: 10651 RVA: 0x000E78BB File Offset: 0x000E5ABB
		[Parameter]
		public int MaximumReceivedDataSizePerCommand
		{
			get
			{
				return this.maxRecvdDataSizePerCommand.Value;
			}
			set
			{
				this.maxRecvdDataSizePerCommand = new int?(value);
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000E78C9 File Offset: 0x000E5AC9
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x000E78D6 File Offset: 0x000E5AD6
		[Parameter]
		public int MaximumReceivedObjectSize
		{
			get
			{
				return this.maxRecvdObjectSize.Value;
			}
			set
			{
				this.maxRecvdObjectSize = new int?(value);
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x000E78E4 File Offset: 0x000E5AE4
		// (set) Token: 0x0600299F RID: 10655 RVA: 0x000E78EC File Offset: 0x000E5AEC
		[Parameter]
		public OutputBufferingMode OutputBufferingMode
		{
			get
			{
				return this.outputBufferingMode;
			}
			set
			{
				this.outputBufferingMode = value;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000E78F5 File Offset: 0x000E5AF5
		// (set) Token: 0x060029A1 RID: 10657 RVA: 0x000E78FD File Offset: 0x000E5AFD
		[ValidateRange(0, 2147483647)]
		[Parameter]
		public int MaxConnectionRetryCount
		{
			get
			{
				return this.maxConnectionRetryCount;
			}
			set
			{
				this.maxConnectionRetryCount = value;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000E7906 File Offset: 0x000E5B06
		// (set) Token: 0x060029A3 RID: 10659 RVA: 0x000E790E File Offset: 0x000E5B0E
		[Parameter]
		[ValidateNotNull]
		public PSPrimitiveDictionary ApplicationArguments
		{
			get
			{
				return this.applicationArguments;
			}
			set
			{
				this.applicationArguments = value;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000E7917 File Offset: 0x000E5B17
		// (set) Token: 0x060029A5 RID: 10661 RVA: 0x000E7937 File Offset: 0x000E5B37
		[Alias(new string[]
		{
			"OpenTimeoutMSec"
		})]
		[ValidateRange(0, 2147483647)]
		[Parameter]
		public int OpenTimeout
		{
			get
			{
				if (this.openTimeout == null)
				{
					return 180000;
				}
				return this.openTimeout.Value;
			}
			set
			{
				this.openTimeout = new int?(value);
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060029A6 RID: 10662 RVA: 0x000E7945 File Offset: 0x000E5B45
		// (set) Token: 0x060029A7 RID: 10663 RVA: 0x000E7965 File Offset: 0x000E5B65
		[ValidateRange(0, 2147483647)]
		[Parameter]
		[Alias(new string[]
		{
			"CancelTimeoutMSec"
		})]
		public int CancelTimeout
		{
			get
			{
				if (this.cancelTimeout == null)
				{
					return 60000;
				}
				return this.cancelTimeout.Value;
			}
			set
			{
				this.cancelTimeout = new int?(value);
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x000E7973 File Offset: 0x000E5B73
		// (set) Token: 0x060029A9 RID: 10665 RVA: 0x000E798F File Offset: 0x000E5B8F
		[ValidateRange(-1, 2147483647)]
		[Alias(new string[]
		{
			"IdleTimeoutMSec"
		})]
		[Parameter]
		public int IdleTimeout
		{
			get
			{
				if (this.idleTimeout == null)
				{
					return -1;
				}
				return this.idleTimeout.Value;
			}
			set
			{
				this.idleTimeout = new int?(value);
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000E799D File Offset: 0x000E5B9D
		// (set) Token: 0x060029AB RID: 10667 RVA: 0x000E79A5 File Offset: 0x000E5BA5
		[ValidateNotNullOrEmpty]
		[Parameter]
		public ProxyAccessType ProxyAccessType
		{
			get
			{
				return this._proxyacesstype;
			}
			set
			{
				this._proxyacesstype = value;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x000E79AE File Offset: 0x000E5BAE
		// (set) Token: 0x060029AD RID: 10669 RVA: 0x000E79B6 File Offset: 0x000E5BB6
		[Parameter]
		public AuthenticationMechanism ProxyAuthentication
		{
			get
			{
				return this.proxyauthentication;
			}
			set
			{
				this.proxyauthentication = value;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000E79BF File Offset: 0x000E5BBF
		// (set) Token: 0x060029AF RID: 10671 RVA: 0x000E79C7 File Offset: 0x000E5BC7
		[ValidateNotNullOrEmpty]
		[Parameter]
		[Credential]
		public PSCredential ProxyCredential
		{
			get
			{
				return this._proxycredential;
			}
			set
			{
				this._proxycredential = value;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x000E79D0 File Offset: 0x000E5BD0
		// (set) Token: 0x060029B1 RID: 10673 RVA: 0x000E79DD File Offset: 0x000E5BDD
		[Parameter]
		public SwitchParameter SkipCACheck
		{
			get
			{
				return this.skipcacheck;
			}
			set
			{
				this.skipcacheck = value;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000E79EB File Offset: 0x000E5BEB
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x000E79F8 File Offset: 0x000E5BF8
		[Parameter]
		public SwitchParameter SkipCNCheck
		{
			get
			{
				return this.skipcncheck;
			}
			set
			{
				this.skipcncheck = value;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000E7A06 File Offset: 0x000E5C06
		// (set) Token: 0x060029B5 RID: 10677 RVA: 0x000E7A13 File Offset: 0x000E5C13
		[Parameter]
		public SwitchParameter SkipRevocationCheck
		{
			get
			{
				return this.skiprevocationcheck;
			}
			set
			{
				this.skiprevocationcheck = value;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x000E7A21 File Offset: 0x000E5C21
		// (set) Token: 0x060029B7 RID: 10679 RVA: 0x000E7A41 File Offset: 0x000E5C41
		[ValidateRange(0, 2147483647)]
		[Alias(new string[]
		{
			"OperationTimeoutMSec"
		})]
		[Parameter]
		public int OperationTimeout
		{
			get
			{
				if (this.operationtimeout == null)
				{
					return 180000;
				}
				return this.operationtimeout.Value;
			}
			set
			{
				this.operationtimeout = new int?(value);
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000E7A4F File Offset: 0x000E5C4F
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x000E7A5C File Offset: 0x000E5C5C
		[Parameter]
		public SwitchParameter NoEncryption
		{
			get
			{
				return this.noencryption;
			}
			set
			{
				this.noencryption = value;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000E7A6A File Offset: 0x000E5C6A
		// (set) Token: 0x060029BB RID: 10683 RVA: 0x000E7A77 File Offset: 0x000E5C77
		[Parameter]
		public SwitchParameter UseUTF16
		{
			get
			{
				return this.useutf16;
			}
			set
			{
				this.useutf16 = value;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x000E7A85 File Offset: 0x000E5C85
		// (set) Token: 0x060029BD RID: 10685 RVA: 0x000E7A92 File Offset: 0x000E5C92
		[Parameter]
		public SwitchParameter IncludePortInSPN
		{
			get
			{
				return this.includePortInSPN;
			}
			set
			{
				this.includePortInSPN = value;
			}
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000E7AA0 File Offset: 0x000E5CA0
		protected override void BeginProcessing()
		{
			PSSessionOption pssessionOption = new PSSessionOption();
			pssessionOption.ProxyAccessType = this.ProxyAccessType;
			pssessionOption.ProxyAuthentication = this.ProxyAuthentication;
			pssessionOption.ProxyCredential = this.ProxyCredential;
			pssessionOption.SkipCACheck = this.SkipCACheck;
			pssessionOption.SkipCNCheck = this.SkipCNCheck;
			pssessionOption.SkipRevocationCheck = this.SkipRevocationCheck;
			if (this.operationtimeout != null)
			{
				pssessionOption.OperationTimeout = TimeSpan.FromMilliseconds((double)this.operationtimeout.Value);
			}
			pssessionOption.NoEncryption = this.NoEncryption;
			pssessionOption.UseUTF16 = this.UseUTF16;
			pssessionOption.IncludePortInSPN = this.IncludePortInSPN;
			if (this.maximumRedirection != null)
			{
				pssessionOption.MaximumConnectionRedirectionCount = this.MaximumRedirection;
			}
			pssessionOption.NoCompression = this.NoCompression.IsPresent;
			pssessionOption.NoMachineProfile = this.NoMachineProfile.IsPresent;
			pssessionOption.MaximumReceivedDataSizePerCommand = this.maxRecvdDataSizePerCommand;
			pssessionOption.MaximumReceivedObjectSize = this.maxRecvdObjectSize;
			if (this.Culture != null)
			{
				pssessionOption.Culture = this.Culture;
			}
			if (this.UICulture != null)
			{
				pssessionOption.UICulture = this.UICulture;
			}
			if (this.openTimeout != null)
			{
				pssessionOption.OpenTimeout = TimeSpan.FromMilliseconds((double)this.openTimeout.Value);
			}
			if (this.cancelTimeout != null)
			{
				pssessionOption.CancelTimeout = TimeSpan.FromMilliseconds((double)this.cancelTimeout.Value);
			}
			if (this.idleTimeout != null)
			{
				pssessionOption.IdleTimeout = TimeSpan.FromMilliseconds((double)this.idleTimeout.Value);
			}
			pssessionOption.OutputBufferingMode = this.outputBufferingMode;
			pssessionOption.MaxConnectionRetryCount = this.maxConnectionRetryCount;
			if (this.ApplicationArguments != null)
			{
				pssessionOption.ApplicationArguments = this.ApplicationArguments;
			}
			base.WriteObject(pssessionOption);
		}

		// Token: 0x04001486 RID: 5254
		private int? maximumRedirection;

		// Token: 0x04001487 RID: 5255
		private SwitchParameter noCompression;

		// Token: 0x04001488 RID: 5256
		private SwitchParameter noMachineProfile;

		// Token: 0x04001489 RID: 5257
		private CultureInfo culture;

		// Token: 0x0400148A RID: 5258
		private CultureInfo uiCulture;

		// Token: 0x0400148B RID: 5259
		private int? maxRecvdDataSizePerCommand;

		// Token: 0x0400148C RID: 5260
		private int? maxRecvdObjectSize;

		// Token: 0x0400148D RID: 5261
		private OutputBufferingMode outputBufferingMode;

		// Token: 0x0400148E RID: 5262
		private int maxConnectionRetryCount;

		// Token: 0x0400148F RID: 5263
		private PSPrimitiveDictionary applicationArguments;

		// Token: 0x04001490 RID: 5264
		private int? openTimeout;

		// Token: 0x04001491 RID: 5265
		private int? cancelTimeout;

		// Token: 0x04001492 RID: 5266
		private int? idleTimeout;

		// Token: 0x04001493 RID: 5267
		private ProxyAccessType _proxyacesstype;

		// Token: 0x04001494 RID: 5268
		private AuthenticationMechanism proxyauthentication = AuthenticationMechanism.Negotiate;

		// Token: 0x04001495 RID: 5269
		private PSCredential _proxycredential;

		// Token: 0x04001496 RID: 5270
		private bool skipcacheck;

		// Token: 0x04001497 RID: 5271
		private bool skipcncheck;

		// Token: 0x04001498 RID: 5272
		private bool skiprevocationcheck;

		// Token: 0x04001499 RID: 5273
		private int? operationtimeout;

		// Token: 0x0400149A RID: 5274
		private bool noencryption;

		// Token: 0x0400149B RID: 5275
		private bool useutf16;

		// Token: 0x0400149C RID: 5276
		private bool includePortInSPN;
	}
}
