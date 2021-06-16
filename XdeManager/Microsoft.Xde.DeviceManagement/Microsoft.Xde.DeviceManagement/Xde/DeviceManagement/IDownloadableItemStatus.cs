using System;
using Windows.Services.Store;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000004 RID: 4
	public interface IDownloadableItemStatus
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002B RID: 43
		Exception ExtendedError { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002C RID: 44
		StoreQueueItemState PackageInstallState { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002D RID: 45
		StorePackageUpdateStatus UpdateStatus { get; }
	}
}
