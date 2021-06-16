using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020000A1 RID: 161
	[DataContract]
	internal class AnalysisCacheIndex
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00027587 File Offset: 0x00025787
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0002758F File Offset: 0x0002578F
		[DataMember]
		public DateTime LastMaintenance { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00027598 File Offset: 0x00025798
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x000275A0 File Offset: 0x000257A0
		[DataMember]
		public Dictionary<string, AnalysisCacheIndexEntry> Entries { get; set; }

		// Token: 0x060007C8 RID: 1992 RVA: 0x000275A9 File Offset: 0x000257A9
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (this.Entries != null)
			{
				this.Entries = new Dictionary<string, AnalysisCacheIndexEntry>(this.Entries, StringComparer.OrdinalIgnoreCase);
			}
		}
	}
}
