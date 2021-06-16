using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200029A RID: 666
	[DataContract]
	public class RemotingWarningRecord : WarningRecord
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x000B983C File Offset: 0x000B7A3C
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000B9844 File Offset: 0x000B7A44
		public RemotingWarningRecord(string message, OriginInfo originInfo) : base(message)
		{
			this._originInfo = originInfo;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000B9854 File Offset: 0x000B7A54
		internal RemotingWarningRecord(WarningRecord warningRecord, OriginInfo originInfo) : base(warningRecord.FullyQualifiedWarningId, warningRecord.Message)
		{
			this._originInfo = originInfo;
		}

		// Token: 0x04000E21 RID: 3617
		[DataMember]
		private readonly OriginInfo _originInfo;
	}
}
