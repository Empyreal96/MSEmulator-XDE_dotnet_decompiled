using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000299 RID: 665
	[DataContract]
	public class RemotingProgressRecord : ProgressRecord
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x000B979B File Offset: 0x000B799B
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000B97A4 File Offset: 0x000B79A4
		public RemotingProgressRecord(ProgressRecord progressRecord, OriginInfo originInfo) : base(RemotingProgressRecord.Validate(progressRecord).ActivityId, RemotingProgressRecord.Validate(progressRecord).Activity, RemotingProgressRecord.Validate(progressRecord).StatusDescription)
		{
			this._originInfo = originInfo;
			if (progressRecord != null)
			{
				base.PercentComplete = progressRecord.PercentComplete;
				base.ParentActivityId = progressRecord.ParentActivityId;
				base.RecordType = progressRecord.RecordType;
				base.SecondsRemaining = progressRecord.SecondsRemaining;
				if (!string.IsNullOrEmpty(progressRecord.CurrentOperation))
				{
					base.CurrentOperation = progressRecord.CurrentOperation;
				}
			}
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000B982B File Offset: 0x000B7A2B
		private static ProgressRecord Validate(ProgressRecord progressRecord)
		{
			if (progressRecord == null)
			{
				throw new ArgumentNullException("progressRecord");
			}
			return progressRecord;
		}

		// Token: 0x04000E20 RID: 3616
		[DataMember]
		private readonly OriginInfo _originInfo;
	}
}
