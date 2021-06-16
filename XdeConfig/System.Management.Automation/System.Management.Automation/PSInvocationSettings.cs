using System;
using System.Management.Automation.Host;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000234 RID: 564
	public sealed class PSInvocationSettings
	{
		// Token: 0x06001A28 RID: 6696 RVA: 0x0009B770 File Offset: 0x00099970
		public PSInvocationSettings()
		{
			this.apartmentState = ApartmentState.Unknown;
			this.host = null;
			this.remoteStreamOptions = (RemoteStreamOptions)0;
			this.addToHistory = false;
			this.errorActionPreference = null;
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x0009B7A0 File Offset: 0x000999A0
		// (set) Token: 0x06001A2A RID: 6698 RVA: 0x0009B7A8 File Offset: 0x000999A8
		public ApartmentState ApartmentState
		{
			get
			{
				return this.apartmentState;
			}
			set
			{
				this.apartmentState = value;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x0009B7B1 File Offset: 0x000999B1
		// (set) Token: 0x06001A2C RID: 6700 RVA: 0x0009B7B9 File Offset: 0x000999B9
		public PSHost Host
		{
			get
			{
				return this.host;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Host");
				}
				this.host = value;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0009B7D0 File Offset: 0x000999D0
		// (set) Token: 0x06001A2E RID: 6702 RVA: 0x0009B7D8 File Offset: 0x000999D8
		public RemoteStreamOptions RemoteStreamOptions
		{
			get
			{
				return this.remoteStreamOptions;
			}
			set
			{
				this.remoteStreamOptions = value;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x0009B7E1 File Offset: 0x000999E1
		// (set) Token: 0x06001A30 RID: 6704 RVA: 0x0009B7E9 File Offset: 0x000999E9
		public bool AddToHistory
		{
			get
			{
				return this.addToHistory;
			}
			set
			{
				this.addToHistory = value;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0009B7F2 File Offset: 0x000999F2
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x0009B7FA File Offset: 0x000999FA
		public ActionPreference? ErrorActionPreference
		{
			get
			{
				return this.errorActionPreference;
			}
			set
			{
				this.errorActionPreference = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0009B803 File Offset: 0x00099A03
		// (set) Token: 0x06001A34 RID: 6708 RVA: 0x0009B80B File Offset: 0x00099A0B
		public bool FlowImpersonationPolicy
		{
			get
			{
				return this.flowImpersonationPolicy;
			}
			set
			{
				this.flowImpersonationPolicy = value;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0009B814 File Offset: 0x00099A14
		// (set) Token: 0x06001A36 RID: 6710 RVA: 0x0009B81C File Offset: 0x00099A1C
		internal WindowsIdentity WindowsIdentityToImpersonate
		{
			get
			{
				return this.windowsIdentityToImpersonate;
			}
			set
			{
				this.windowsIdentityToImpersonate = value;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0009B825 File Offset: 0x00099A25
		// (set) Token: 0x06001A38 RID: 6712 RVA: 0x0009B82D File Offset: 0x00099A2D
		public bool ExposeFlowControlExceptions { get; set; }

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x0009B836 File Offset: 0x00099A36
		// (set) Token: 0x06001A3A RID: 6714 RVA: 0x0009B83E File Offset: 0x00099A3E
		internal bool InvokeAndDisconnect
		{
			get
			{
				return this.invokeAndDisconnect;
			}
			set
			{
				this.invokeAndDisconnect = value;
			}
		}

		// Token: 0x04000AD2 RID: 2770
		private PSHost host;

		// Token: 0x04000AD3 RID: 2771
		private RemoteStreamOptions remoteStreamOptions;

		// Token: 0x04000AD4 RID: 2772
		private ActionPreference? errorActionPreference;

		// Token: 0x04000AD5 RID: 2773
		private bool addToHistory;

		// Token: 0x04000AD6 RID: 2774
		private ApartmentState apartmentState;

		// Token: 0x04000AD7 RID: 2775
		private bool flowImpersonationPolicy;

		// Token: 0x04000AD8 RID: 2776
		private WindowsIdentity windowsIdentityToImpersonate;

		// Token: 0x04000AD9 RID: 2777
		private bool invokeAndDisconnect;
	}
}
