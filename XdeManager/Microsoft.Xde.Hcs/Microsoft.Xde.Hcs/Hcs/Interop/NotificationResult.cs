using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000013 RID: 19
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public class NotificationResult
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004264 File Offset: 0x00002464
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000426C File Offset: 0x0000246C
		public int Status { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004275 File Offset: 0x00002475
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000427D File Offset: 0x0000247D
		public string Data { get; set; }
	}
}
