using System;
using System.Globalization;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200034C RID: 844
	public sealed class PSSessionOption
	{
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x000E7650 File Offset: 0x000E5850
		// (set) Token: 0x06002963 RID: 10595 RVA: 0x000E7658 File Offset: 0x000E5858
		public int MaximumConnectionRedirectionCount
		{
			get
			{
				return this.maximumConnectionRedirectionCount;
			}
			set
			{
				this.maximumConnectionRedirectionCount = value;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x000E7661 File Offset: 0x000E5861
		// (set) Token: 0x06002965 RID: 10597 RVA: 0x000E7669 File Offset: 0x000E5869
		public bool NoCompression
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

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x000E7672 File Offset: 0x000E5872
		// (set) Token: 0x06002967 RID: 10599 RVA: 0x000E767A File Offset: 0x000E587A
		public bool NoMachineProfile
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

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x000E7683 File Offset: 0x000E5883
		// (set) Token: 0x06002969 RID: 10601 RVA: 0x000E768B File Offset: 0x000E588B
		public ProxyAccessType ProxyAccessType
		{
			get
			{
				return this.proxyAcessType;
			}
			set
			{
				this.proxyAcessType = value;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x000E7694 File Offset: 0x000E5894
		// (set) Token: 0x0600296B RID: 10603 RVA: 0x000E769C File Offset: 0x000E589C
		public AuthenticationMechanism ProxyAuthentication
		{
			get
			{
				return this.proxyAuthentication;
			}
			set
			{
				switch (value)
				{
				case AuthenticationMechanism.Basic:
				case AuthenticationMechanism.Negotiate:
				case AuthenticationMechanism.Digest:
					this.proxyAuthentication = value;
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

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x000E771D File Offset: 0x000E591D
		// (set) Token: 0x0600296D RID: 10605 RVA: 0x000E7725 File Offset: 0x000E5925
		public PSCredential ProxyCredential
		{
			get
			{
				return this.proxyCredential;
			}
			set
			{
				this.proxyCredential = value;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600296E RID: 10606 RVA: 0x000E772E File Offset: 0x000E592E
		// (set) Token: 0x0600296F RID: 10607 RVA: 0x000E7736 File Offset: 0x000E5936
		public bool SkipCACheck
		{
			get
			{
				return this.skipCACheck;
			}
			set
			{
				this.skipCACheck = value;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002970 RID: 10608 RVA: 0x000E773F File Offset: 0x000E593F
		// (set) Token: 0x06002971 RID: 10609 RVA: 0x000E7747 File Offset: 0x000E5947
		public bool SkipCNCheck
		{
			get
			{
				return this.skipCNCheck;
			}
			set
			{
				this.skipCNCheck = value;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002972 RID: 10610 RVA: 0x000E7750 File Offset: 0x000E5950
		// (set) Token: 0x06002973 RID: 10611 RVA: 0x000E7758 File Offset: 0x000E5958
		public bool SkipRevocationCheck
		{
			get
			{
				return this.skipRevocationCheck;
			}
			set
			{
				this.skipRevocationCheck = value;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x000E7761 File Offset: 0x000E5961
		// (set) Token: 0x06002975 RID: 10613 RVA: 0x000E7769 File Offset: 0x000E5969
		public TimeSpan OperationTimeout
		{
			get
			{
				return this.operationTimeout;
			}
			set
			{
				this.operationTimeout = value;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x000E7772 File Offset: 0x000E5972
		// (set) Token: 0x06002977 RID: 10615 RVA: 0x000E777A File Offset: 0x000E597A
		public bool NoEncryption
		{
			get
			{
				return this.noEncryption;
			}
			set
			{
				this.noEncryption = value;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x000E7783 File Offset: 0x000E5983
		// (set) Token: 0x06002979 RID: 10617 RVA: 0x000E778B File Offset: 0x000E598B
		public bool UseUTF16
		{
			get
			{
				return this.useUtf16;
			}
			set
			{
				this.useUtf16 = value;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000E7794 File Offset: 0x000E5994
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x000E779C File Offset: 0x000E599C
		public bool IncludePortInSPN
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

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000E77A5 File Offset: 0x000E59A5
		// (set) Token: 0x0600297D RID: 10621 RVA: 0x000E77AD File Offset: 0x000E59AD
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

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000E77B6 File Offset: 0x000E59B6
		// (set) Token: 0x0600297F RID: 10623 RVA: 0x000E77BE File Offset: 0x000E59BE
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

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x000E77C7 File Offset: 0x000E59C7
		// (set) Token: 0x06002981 RID: 10625 RVA: 0x000E77CF File Offset: 0x000E59CF
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

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x000E77D8 File Offset: 0x000E59D8
		// (set) Token: 0x06002983 RID: 10627 RVA: 0x000E77E0 File Offset: 0x000E59E0
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

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002984 RID: 10628 RVA: 0x000E77E9 File Offset: 0x000E59E9
		// (set) Token: 0x06002985 RID: 10629 RVA: 0x000E77F1 File Offset: 0x000E59F1
		public int? MaximumReceivedDataSizePerCommand
		{
			get
			{
				return this.maxRecvdDataSizePerCommand;
			}
			set
			{
				this.maxRecvdDataSizePerCommand = value;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002986 RID: 10630 RVA: 0x000E77FA File Offset: 0x000E59FA
		// (set) Token: 0x06002987 RID: 10631 RVA: 0x000E7802 File Offset: 0x000E5A02
		public int? MaximumReceivedObjectSize
		{
			get
			{
				return this.maxRecvdObjectSize;
			}
			set
			{
				this.maxRecvdObjectSize = value;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000E780B File Offset: 0x000E5A0B
		// (set) Token: 0x06002989 RID: 10633 RVA: 0x000E7813 File Offset: 0x000E5A13
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

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000E781C File Offset: 0x000E5A1C
		// (set) Token: 0x0600298B RID: 10635 RVA: 0x000E7824 File Offset: 0x000E5A24
		public TimeSpan OpenTimeout
		{
			get
			{
				return this.openTimeout;
			}
			set
			{
				this.openTimeout = value;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x000E782D File Offset: 0x000E5A2D
		// (set) Token: 0x0600298D RID: 10637 RVA: 0x000E7835 File Offset: 0x000E5A35
		public TimeSpan CancelTimeout
		{
			get
			{
				return this.cancelTimeout;
			}
			set
			{
				this.cancelTimeout = value;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600298E RID: 10638 RVA: 0x000E783E File Offset: 0x000E5A3E
		// (set) Token: 0x0600298F RID: 10639 RVA: 0x000E7846 File Offset: 0x000E5A46
		public TimeSpan IdleTimeout
		{
			get
			{
				return this.idleTimeout;
			}
			set
			{
				this.idleTimeout = value;
			}
		}

		// Token: 0x0400146F RID: 5231
		private int maximumConnectionRedirectionCount = 5;

		// Token: 0x04001470 RID: 5232
		private bool noCompression;

		// Token: 0x04001471 RID: 5233
		private bool noMachineProfile;

		// Token: 0x04001472 RID: 5234
		private ProxyAccessType proxyAcessType;

		// Token: 0x04001473 RID: 5235
		private AuthenticationMechanism proxyAuthentication = AuthenticationMechanism.Negotiate;

		// Token: 0x04001474 RID: 5236
		private PSCredential proxyCredential;

		// Token: 0x04001475 RID: 5237
		private bool skipCACheck;

		// Token: 0x04001476 RID: 5238
		private bool skipCNCheck;

		// Token: 0x04001477 RID: 5239
		private bool skipRevocationCheck;

		// Token: 0x04001478 RID: 5240
		private TimeSpan operationTimeout = TimeSpan.FromMilliseconds(180000.0);

		// Token: 0x04001479 RID: 5241
		private bool noEncryption;

		// Token: 0x0400147A RID: 5242
		private bool useUtf16;

		// Token: 0x0400147B RID: 5243
		private bool includePortInSPN;

		// Token: 0x0400147C RID: 5244
		private OutputBufferingMode outputBufferingMode;

		// Token: 0x0400147D RID: 5245
		private int maxConnectionRetryCount = 5;

		// Token: 0x0400147E RID: 5246
		private CultureInfo culture;

		// Token: 0x0400147F RID: 5247
		private CultureInfo uiCulture;

		// Token: 0x04001480 RID: 5248
		private int? maxRecvdDataSizePerCommand;

		// Token: 0x04001481 RID: 5249
		private int? maxRecvdObjectSize = new int?(209715200);

		// Token: 0x04001482 RID: 5250
		private PSPrimitiveDictionary applicationArguments;

		// Token: 0x04001483 RID: 5251
		private TimeSpan openTimeout = TimeSpan.FromMilliseconds(180000.0);

		// Token: 0x04001484 RID: 5252
		private TimeSpan cancelTimeout = TimeSpan.FromMilliseconds(60000.0);

		// Token: 0x04001485 RID: 5253
		private TimeSpan idleTimeout = TimeSpan.FromMilliseconds(-1.0);
	}
}
