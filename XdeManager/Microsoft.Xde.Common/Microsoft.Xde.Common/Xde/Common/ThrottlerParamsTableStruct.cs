using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000044 RID: 68
	public struct ThrottlerParamsTableStruct
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00004EBF File Offset: 0x000030BF
		public ThrottlerParamsTableStruct(uint uploadSpeed, uint downloadSpeed, float uploadLossPercent, float downloadLossPercent, uint uploadLaency, uint downloadLatency)
		{
			this.UploadSpeed = uploadSpeed;
			this.DownloadSpeed = downloadSpeed;
			this.UploadLossPercent = uploadLossPercent;
			this.DownloadLossPercent = downloadLossPercent;
			this.UploadLatency = uploadLaency;
			this.DownloadLatency = downloadLatency;
		}

		// Token: 0x04000132 RID: 306
		public uint UploadSpeed;

		// Token: 0x04000133 RID: 307
		public uint DownloadSpeed;

		// Token: 0x04000134 RID: 308
		public float UploadLossPercent;

		// Token: 0x04000135 RID: 309
		public float DownloadLossPercent;

		// Token: 0x04000136 RID: 310
		public uint UploadLatency;

		// Token: 0x04000137 RID: 311
		public uint DownloadLatency;
	}
}
