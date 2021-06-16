using System;
using System.Runtime.Serialization;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200029E RID: 670
	[DataContract]
	[Serializable]
	public class OriginInfo
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x000B98B7 File Offset: 0x000B7AB7
		public string PSComputerName
		{
			get
			{
				return this._computerName;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x000B98BF File Offset: 0x000B7ABF
		public Guid RunspaceID
		{
			get
			{
				return this._runspaceID;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x000B98C7 File Offset: 0x000B7AC7
		// (set) Token: 0x06002007 RID: 8199 RVA: 0x000B98CF File Offset: 0x000B7ACF
		public Guid InstanceID
		{
			get
			{
				return this._instanceId;
			}
			set
			{
				this._instanceId = value;
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000B98D8 File Offset: 0x000B7AD8
		public OriginInfo(string computerName, Guid runspaceID) : this(computerName, runspaceID, Guid.Empty)
		{
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000B98E7 File Offset: 0x000B7AE7
		public OriginInfo(string computerName, Guid runspaceID, Guid instanceID)
		{
			this._computerName = computerName;
			this._runspaceID = runspaceID;
			this._instanceId = instanceID;
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000B9904 File Offset: 0x000B7B04
		public override string ToString()
		{
			return this.PSComputerName;
		}

		// Token: 0x04000E25 RID: 3621
		[DataMember]
		private string _computerName;

		// Token: 0x04000E26 RID: 3622
		[DataMember]
		private Guid _runspaceID;

		// Token: 0x04000E27 RID: 3623
		[DataMember]
		private Guid _instanceId;
	}
}
