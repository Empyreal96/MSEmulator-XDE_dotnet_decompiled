using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001D RID: 29
	[DataContract]
	public class MouseArgsInfo
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00002088 File Offset: 0x00000288
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00002090 File Offset: 0x00000290
		[DataMember]
		public Point Location { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00002099 File Offset: 0x00000299
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000020A1 File Offset: 0x000002A1
		[DataMember]
		public MouseButtons Button { get; set; }
	}
}
