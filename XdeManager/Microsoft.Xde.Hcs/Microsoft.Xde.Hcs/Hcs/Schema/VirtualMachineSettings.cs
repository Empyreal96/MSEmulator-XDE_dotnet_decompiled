using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using HCS.Schema;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Hcs.Schema
{
	// Token: 0x0200000A RID: 10
	[DataContract]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public class VirtualMachineSettings
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003E6E File Offset: 0x0000206E
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003E9F File Offset: 0x0000209F
		public string Vhd
		{
			get
			{
				return this.ComputeSystem.VirtualMachine.Devices.Scsi["Boot Disk Controller"].Attachments[0U].Path;
			}
			set
			{
				this.ComputeSystem.VirtualMachine.Devices.Scsi["Boot Disk Controller"].Attachments[0U].Path = value;
			}
		}

		// Token: 0x0400001E RID: 30
		[DataMember]
		public DateTime CreatedTime;

		// Token: 0x0400001F RID: 31
		[DataMember]
		public string Name;

		// Token: 0x04000020 RID: 32
		[DataMember]
		public Guid Id;

		// Token: 0x04000021 RID: 33
		[DataMember]
		public string Notes;

		// Token: 0x04000022 RID: 34
		[DataMember]
		public ComputeSystem ComputeSystem;

		// Token: 0x04000023 RID: 35
		[DataMember]
		public VGPUStatus VgpuStatus;

		// Token: 0x04000024 RID: 36
		[DataMember]
		public GpuAssignmentMode GpuAssignmentMode;
	}
}
