using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200029C RID: 668
	[DataContract]
	public class RemotingVerboseRecord : VerboseRecord
	{
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x000B9887 File Offset: 0x000B7A87
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000B988F File Offset: 0x000B7A8F
		public RemotingVerboseRecord(string message, OriginInfo originInfo) : base(message)
		{
			this._originInfo = originInfo;
		}

		// Token: 0x04000E23 RID: 3619
		[DataMember]
		private readonly OriginInfo _originInfo;
	}
}
