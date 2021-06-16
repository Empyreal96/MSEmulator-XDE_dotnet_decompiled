using System;
using Windows.Services.Store;

namespace Microsoft.Xde.DeviceManagement
{
	// Token: 0x02000006 RID: 6
	internal class InstallItemStatus : IDownloadableItemStatus
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000026BA File Offset: 0x000008BA
		public InstallItemStatus(StoreQueueItemState installState, StorePackageUpdateStatus updateStatus, Exception error)
		{
			this.ExtendedError = error;
			this.PackageInstallState = installState;
			this.UpdateStatus = updateStatus;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000026D7 File Offset: 0x000008D7
		public Exception ExtendedError { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000026DF File Offset: 0x000008DF
		public StoreQueueItemState PackageInstallState { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000026E7 File Offset: 0x000008E7
		public StorePackageUpdateStatus UpdateStatus { get; }
	}
}
