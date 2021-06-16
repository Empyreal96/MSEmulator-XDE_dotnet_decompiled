using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Xde.Hcs.Schema
{
	// Token: 0x02000009 RID: 9
	[DataContract]
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public class VirtualMachine
	{
		// Token: 0x04000018 RID: 24
		[DataMember]
		public Guid XdeVmId;

		// Token: 0x04000019 RID: 25
		[DataMember]
		public string Name;

		// Token: 0x0400001A RID: 26
		[DataMember]
		public Guid HcsId;

		// Token: 0x0400001B RID: 27
		[DataMember]
		public Guid CurrentSettingsId;

		// Token: 0x0400001C RID: 28
		[DataMember]
		public List<VirtualMachineSettings> Settings;

		// Token: 0x0400001D RID: 29
		[DataMember]
		public List<string> Files;
	}
}
