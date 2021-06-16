using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200029D RID: 669
	[DataContract]
	public class RemotingInformationRecord : InformationRecord
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x000B989F File Offset: 0x000B7A9F
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000B98A7 File Offset: 0x000B7AA7
		public RemotingInformationRecord(InformationRecord record, OriginInfo originInfo) : base(record)
		{
			this._originInfo = originInfo;
		}

		// Token: 0x04000E24 RID: 3620
		[DataMember]
		private readonly OriginInfo _originInfo;
	}
}
