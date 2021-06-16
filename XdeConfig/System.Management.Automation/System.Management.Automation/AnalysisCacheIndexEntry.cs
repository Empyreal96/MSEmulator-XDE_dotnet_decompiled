using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020000A2 RID: 162
	[DataContract]
	internal class AnalysisCacheIndexEntry
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x000275D1 File Offset: 0x000257D1
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x000275D9 File Offset: 0x000257D9
		[DataMember]
		public DateTime LastWriteTime { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x000275E2 File Offset: 0x000257E2
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x000275EA File Offset: 0x000257EA
		[DataMember]
		public string Path { get; set; }
	}
}
