using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200029B RID: 667
	[DataContract]
	public class RemotingDebugRecord : DebugRecord
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x000B986F File Offset: 0x000B7A6F
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000B9877 File Offset: 0x000B7A77
		public RemotingDebugRecord(string message, OriginInfo originInfo) : base(message)
		{
			this._originInfo = originInfo;
		}

		// Token: 0x04000E22 RID: 3618
		[DataMember]
		private readonly OriginInfo _originInfo;
	}
}
