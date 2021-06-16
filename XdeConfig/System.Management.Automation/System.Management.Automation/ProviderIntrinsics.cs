using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000005 RID: 5
	public sealed class ProviderIntrinsics
	{
		// Token: 0x06000012 RID: 18 RVA: 0x0000223E File Offset: 0x0000043E
		private ProviderIntrinsics()
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002248 File Offset: 0x00000448
		internal ProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.item = new ItemCmdletProviderIntrinsics(cmdlet);
			this.childItem = new ChildItemCmdletProviderIntrinsics(cmdlet);
			this.content = new ContentCmdletProviderIntrinsics(cmdlet);
			this.property = new PropertyCmdletProviderIntrinsics(cmdlet);
			this.securityDescriptor = new SecurityDescriptorCmdletProviderIntrinsics(cmdlet);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022AC File Offset: 0x000004AC
		internal ProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.item = new ItemCmdletProviderIntrinsics(sessionState);
			this.childItem = new ChildItemCmdletProviderIntrinsics(sessionState);
			this.content = new ContentCmdletProviderIntrinsics(sessionState);
			this.property = new PropertyCmdletProviderIntrinsics(sessionState);
			this.securityDescriptor = new SecurityDescriptorCmdletProviderIntrinsics(sessionState);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002309 File Offset: 0x00000509
		public ItemCmdletProviderIntrinsics Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002311 File Offset: 0x00000511
		public ChildItemCmdletProviderIntrinsics ChildItem
		{
			get
			{
				return this.childItem;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002319 File Offset: 0x00000519
		public ContentCmdletProviderIntrinsics Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002321 File Offset: 0x00000521
		public PropertyCmdletProviderIntrinsics Property
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002329 File Offset: 0x00000529
		public SecurityDescriptorCmdletProviderIntrinsics SecurityDescriptor
		{
			get
			{
				return this.securityDescriptor;
			}
		}

		// Token: 0x0400000B RID: 11
		private InternalCommand cmdlet;

		// Token: 0x0400000C RID: 12
		private ItemCmdletProviderIntrinsics item;

		// Token: 0x0400000D RID: 13
		private ChildItemCmdletProviderIntrinsics childItem;

		// Token: 0x0400000E RID: 14
		private ContentCmdletProviderIntrinsics content;

		// Token: 0x0400000F RID: 15
		private PropertyCmdletProviderIntrinsics property;

		// Token: 0x04000010 RID: 16
		private SecurityDescriptorCmdletProviderIntrinsics securityDescriptor;
	}
}
