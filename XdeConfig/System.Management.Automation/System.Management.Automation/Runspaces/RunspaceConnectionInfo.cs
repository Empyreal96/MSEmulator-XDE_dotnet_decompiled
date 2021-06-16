using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002CD RID: 717
	public abstract class RunspaceConnectionInfo
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060021E4 RID: 8676
		// (set) Token: 0x060021E5 RID: 8677
		public abstract string ComputerName { get; set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060021E6 RID: 8678
		// (set) Token: 0x060021E7 RID: 8679
		public abstract PSCredential Credential { get; set; }

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060021E8 RID: 8680
		// (set) Token: 0x060021E9 RID: 8681
		public abstract AuthenticationMechanism AuthenticationMechanism { get; set; }

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060021EA RID: 8682
		// (set) Token: 0x060021EB RID: 8683
		public abstract string CertificateThumbprint { get; set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000C1AE7 File Offset: 0x000BFCE7
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x000C1AEF File Offset: 0x000BFCEF
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.culture = value;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000C1B06 File Offset: 0x000BFD06
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x000C1B0E File Offset: 0x000BFD0E
		public CultureInfo UICulture
		{
			get
			{
				return this.uiCulture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.uiCulture = value;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000C1B25 File Offset: 0x000BFD25
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000C1B30 File Offset: 0x000BFD30
		public int OpenTimeout
		{
			get
			{
				return this.openTimeout;
			}
			set
			{
				this.openTimeout = value;
				if (this is WSManConnectionInfo && this.openTimeout == -1)
				{
					this.openTimeout = 180000;
					return;
				}
				if (this is WSManConnectionInfo && this.openTimeout == 0)
				{
					this.openTimeout = int.MaxValue;
				}
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000C1B7C File Offset: 0x000BFD7C
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x000C1B84 File Offset: 0x000BFD84
		public int CancelTimeout
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

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000C1B8D File Offset: 0x000BFD8D
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x000C1B95 File Offset: 0x000BFD95
		public int OperationTimeout
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

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x000C1B9E File Offset: 0x000BFD9E
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x000C1BA6 File Offset: 0x000BFDA6
		public int IdleTimeout
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

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x000C1BAF File Offset: 0x000BFDAF
		// (set) Token: 0x060021F9 RID: 8697 RVA: 0x000C1BB7 File Offset: 0x000BFDB7
		public int MaxIdleTimeout
		{
			get
			{
				return this.maxIdleTimeout;
			}
			internal set
			{
				this.maxIdleTimeout = value;
			}
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000C1BC0 File Offset: 0x000BFDC0
		public virtual void SetSessionOptions(PSSessionOption options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (options.Culture != null)
			{
				this.Culture = options.Culture;
			}
			if (options.UICulture != null)
			{
				this.UICulture = options.UICulture;
			}
			this.openTimeout = this.TimeSpanToTimeOutMs(options.OpenTimeout);
			this.cancelTimeout = this.TimeSpanToTimeOutMs(options.CancelTimeout);
			this.operationTimeout = this.TimeSpanToTimeOutMs(options.OperationTimeout);
			this.idleTimeout = ((options.IdleTimeout.TotalMilliseconds >= -1.0 && options.IdleTimeout.TotalMilliseconds < 2147483647.0) ? ((int)options.IdleTimeout.TotalMilliseconds) : int.MaxValue);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000C1C88 File Offset: 0x000BFE88
		internal int TimeSpanToTimeOutMs(TimeSpan t)
		{
			if (t.TotalMilliseconds > 2147483647.0 || t == TimeSpan.MaxValue || t.TotalMilliseconds < 0.0)
			{
				return int.MaxValue;
			}
			return (int)t.TotalMilliseconds;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000C1CD4 File Offset: 0x000BFED4
		internal virtual BaseClientSessionTransportManager CreateClientSessionTransportManager(Guid instanceId, string sessionName, PSRemotingCryptoHelper cryptoHelper)
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000C1CDB File Offset: 0x000BFEDB
		internal virtual RunspaceConnectionInfo InternalCopy()
		{
			throw new PSNotImplementedException();
		}

		// Token: 0x04001022 RID: 4130
		internal const int DefaultOpenTimeout = 180000;

		// Token: 0x04001023 RID: 4131
		internal const int DefaultTimeout = -1;

		// Token: 0x04001024 RID: 4132
		internal const int InfiniteTimeout = 0;

		// Token: 0x04001025 RID: 4133
		internal const int defaultCancelTimeout = 60000;

		// Token: 0x04001026 RID: 4134
		internal const int DefaultIdleTimeout = -1;

		// Token: 0x04001027 RID: 4135
		private CultureInfo culture = CultureInfo.CurrentCulture;

		// Token: 0x04001028 RID: 4136
		private CultureInfo uiCulture = CultureInfo.CurrentUICulture;

		// Token: 0x04001029 RID: 4137
		private int openTimeout = 180000;

		// Token: 0x0400102A RID: 4138
		private int cancelTimeout = 60000;

		// Token: 0x0400102B RID: 4139
		private int operationTimeout = 180000;

		// Token: 0x0400102C RID: 4140
		private int idleTimeout = -1;

		// Token: 0x0400102D RID: 4141
		private int maxIdleTimeout = int.MaxValue;
	}
}
