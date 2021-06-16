using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000207 RID: 519
	[DataContract]
	public class VerboseRecord : InformationalRecord
	{
		// Token: 0x0600180C RID: 6156 RVA: 0x00094398 File Offset: 0x00092598
		public VerboseRecord(string message) : base(message)
		{
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000943A1 File Offset: 0x000925A1
		public VerboseRecord(PSObject record) : base(record)
		{
		}
	}
}
