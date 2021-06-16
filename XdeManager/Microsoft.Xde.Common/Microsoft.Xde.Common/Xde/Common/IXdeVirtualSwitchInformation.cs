using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200005E RID: 94
	public interface IXdeVirtualSwitchInformation
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600020B RID: 523
		string Name { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600020C RID: 524
		string Id { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600020D RID: 525
		bool External { get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600020E RID: 526
		string HostIpAddress { get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600020F RID: 527
		string HostIpMask { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000210 RID: 528
		string HostMacAddress { get; }
	}
}
