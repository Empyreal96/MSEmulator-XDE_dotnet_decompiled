using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000414 RID: 1044
	[AttributeUsage(AttributeTargets.Class)]
	public abstract class CmdletCommonMetadataAttribute : CmdletMetadataAttribute
	{
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06002E9E RID: 11934 RVA: 0x00100200 File Offset: 0x000FE400
		// (set) Token: 0x06002E9F RID: 11935 RVA: 0x00100208 File Offset: 0x000FE408
		public string DefaultParameterSetName
		{
			get
			{
				return this.defaultParameterSetName;
			}
			set
			{
				this.defaultParameterSetName = value;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06002EA0 RID: 11936 RVA: 0x00100211 File Offset: 0x000FE411
		// (set) Token: 0x06002EA1 RID: 11937 RVA: 0x00100219 File Offset: 0x000FE419
		public bool SupportsShouldProcess
		{
			get
			{
				return this.supportsShouldProcess;
			}
			set
			{
				this.supportsShouldProcess = value;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002EA2 RID: 11938 RVA: 0x00100222 File Offset: 0x000FE422
		// (set) Token: 0x06002EA3 RID: 11939 RVA: 0x0010022A File Offset: 0x000FE42A
		public bool SupportsPaging
		{
			get
			{
				return this.supportsPaging;
			}
			set
			{
				this.supportsPaging = value;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002EA4 RID: 11940 RVA: 0x00100233 File Offset: 0x000FE433
		// (set) Token: 0x06002EA5 RID: 11941 RVA: 0x0010023B File Offset: 0x000FE43B
		public bool SupportsTransactions
		{
			get
			{
				return this.supportsTransactions;
			}
			set
			{
				this.supportsTransactions = value;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x00100244 File Offset: 0x000FE444
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x0010024C File Offset: 0x000FE44C
		public ConfirmImpact ConfirmImpact
		{
			get
			{
				return this.confirmImpact;
			}
			set
			{
				this.confirmImpact = value;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x00100255 File Offset: 0x000FE455
		// (set) Token: 0x06002EA9 RID: 11945 RVA: 0x0010025D File Offset: 0x000FE45D
		public string HelpUri
		{
			get
			{
				return this.helpUri;
			}
			set
			{
				this.helpUri = value;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06002EAA RID: 11946 RVA: 0x00100266 File Offset: 0x000FE466
		// (set) Token: 0x06002EAB RID: 11947 RVA: 0x0010026E File Offset: 0x000FE46E
		public RemotingCapability RemotingCapability
		{
			get
			{
				return this.remotingCapability;
			}
			set
			{
				this.remotingCapability = value;
			}
		}

		// Token: 0x04001887 RID: 6279
		private string defaultParameterSetName;

		// Token: 0x04001888 RID: 6280
		private bool supportsShouldProcess;

		// Token: 0x04001889 RID: 6281
		private bool supportsPaging;

		// Token: 0x0400188A RID: 6282
		private bool supportsTransactions;

		// Token: 0x0400188B RID: 6283
		private ConfirmImpact confirmImpact = ConfirmImpact.Medium;

		// Token: 0x0400188C RID: 6284
		private string helpUri = string.Empty;

		// Token: 0x0400188D RID: 6285
		private RemotingCapability remotingCapability = RemotingCapability.PowerShell;
	}
}
