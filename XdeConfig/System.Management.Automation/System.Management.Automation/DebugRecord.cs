using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000206 RID: 518
	[DataContract]
	public class DebugRecord : InformationalRecord
	{
		// Token: 0x0600180A RID: 6154 RVA: 0x00094386 File Offset: 0x00092586
		public DebugRecord(string message) : base(message)
		{
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0009438F File Offset: 0x0009258F
		public DebugRecord(PSObject record) : base(record)
		{
		}
	}
}
