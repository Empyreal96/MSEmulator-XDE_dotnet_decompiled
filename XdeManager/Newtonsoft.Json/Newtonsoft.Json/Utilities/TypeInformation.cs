using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000040 RID: 64
	internal class TypeInformation
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x000112BE File Offset: 0x0000F4BE
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x000112C6 File Offset: 0x0000F4C6
		public Type Type { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x000112CF File Offset: 0x0000F4CF
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x000112D7 File Offset: 0x0000F4D7
		public PrimitiveTypeCode TypeCode { get; set; }
	}
}
