using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Provider;

namespace System.Management.Automation
{
	// Token: 0x02000037 RID: 55
	public sealed class CmdletProviderManagementIntrinsics
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x0000A4E6 File Offset: 0x000086E6
		private CmdletProviderManagementIntrinsics()
		{
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A4EE File Offset: 0x000086EE
		internal CmdletProviderManagementIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A50B File Offset: 0x0000870B
		public Collection<ProviderInfo> Get(string name)
		{
			return this.sessionState.GetProvider(name);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A519 File Offset: 0x00008719
		public ProviderInfo GetOne(string name)
		{
			return this.sessionState.GetSingleProvider(name);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A527 File Offset: 0x00008727
		public IEnumerable<ProviderInfo> GetAll()
		{
			return this.sessionState.ProviderList;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000A534 File Offset: 0x00008734
		internal static bool CheckProviderCapabilities(ProviderCapabilities capability, ProviderInfo provider)
		{
			return (provider.Capabilities & capability) != ProviderCapabilities.None;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000A544 File Offset: 0x00008744
		internal int Count
		{
			get
			{
				return this.sessionState.ProviderCount;
			}
		}

		// Token: 0x040000EC RID: 236
		private SessionStateInternal sessionState;
	}
}
