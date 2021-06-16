using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200006A RID: 106
	public struct VGPUInformation
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000554D File Offset: 0x0000374D
		public VGPUInformation(VGPUStatus status)
		{
			this = new VGPUInformation(status, null);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00005557 File Offset: 0x00003757
		public VGPUInformation(VGPUStatus status, string additionalInfo)
		{
			this.Status = status;
			this.AdditionalInfo = additionalInfo;
			this.GpuAssignmentMode = GpuAssignmentMode.None;
		}

		// Token: 0x0400017E RID: 382
		public VGPUStatus Status;

		// Token: 0x0400017F RID: 383
		public GpuAssignmentMode GpuAssignmentMode;

		// Token: 0x04000180 RID: 384
		public string AdditionalInfo;
	}
}
