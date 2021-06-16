using System;
using System.Runtime.Serialization;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001F RID: 31
	[DataContract]
	public class GuestUpdatedEventArgs : EventArgs
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004BFE File Offset: 0x00002DFE
		public GuestUpdatedEventArgs(string id, string result)
		{
			this.NotificationId = id;
			this.Payload = result;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004C14 File Offset: 0x00002E14
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00004C1C File Offset: 0x00002E1C
		[DataMember]
		public string NotificationId { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004C25 File Offset: 0x00002E25
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00004C2D File Offset: 0x00002E2D
		[DataMember]
		public string Payload { get; private set; }
	}
}
