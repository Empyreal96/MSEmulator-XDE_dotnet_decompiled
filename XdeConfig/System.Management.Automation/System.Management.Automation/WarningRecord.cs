using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000205 RID: 517
	[DataContract]
	public class WarningRecord : InformationalRecord
	{
		// Token: 0x06001805 RID: 6149 RVA: 0x00094343 File Offset: 0x00092543
		public WarningRecord(string message) : base(message)
		{
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0009434C File Offset: 0x0009254C
		public WarningRecord(PSObject record) : base(record)
		{
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00094355 File Offset: 0x00092555
		public WarningRecord(string fullyQualifiedWarningId, string message) : base(message)
		{
			this._fullyQualifiedWarningId = fullyQualifiedWarningId;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00094365 File Offset: 0x00092565
		public WarningRecord(string fullyQualifiedWarningId, PSObject record) : base(record)
		{
			this._fullyQualifiedWarningId = fullyQualifiedWarningId;
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x00094375 File Offset: 0x00092575
		public string FullyQualifiedWarningId
		{
			get
			{
				return this._fullyQualifiedWarningId ?? string.Empty;
			}
		}

		// Token: 0x04000A21 RID: 2593
		private string _fullyQualifiedWarningId;
	}
}
