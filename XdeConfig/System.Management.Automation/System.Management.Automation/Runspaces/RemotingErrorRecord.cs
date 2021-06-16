using System;
using System.Management.Automation.Remoting;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000298 RID: 664
	[Serializable]
	public class RemotingErrorRecord : ErrorRecord
	{
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x000B9706 File Offset: 0x000B7906
		public OriginInfo OriginInfo
		{
			get
			{
				return this._originInfo;
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000B970E File Offset: 0x000B790E
		public RemotingErrorRecord(ErrorRecord errorRecord, OriginInfo originInfo) : this(errorRecord, originInfo, null)
		{
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000B9719 File Offset: 0x000B7919
		private RemotingErrorRecord(ErrorRecord errorRecord, OriginInfo originInfo, Exception replaceParentContainsErrorRecordException) : base(errorRecord, replaceParentContainsErrorRecordException)
		{
			if (errorRecord != null)
			{
				base.SetInvocationInfo(errorRecord.InvocationInfo);
			}
			this._originInfo = originInfo;
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000B9739 File Offset: 0x000B7939
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("RemoteErrorRecord_OriginInfo", this._originInfo);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000B9762 File Offset: 0x000B7962
		protected RemotingErrorRecord(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._originInfo = (OriginInfo)info.GetValue("RemoteErrorRecord_OriginInfo", typeof(OriginInfo));
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000B978C File Offset: 0x000B798C
		internal override ErrorRecord WrapException(Exception replaceParentContainsErrorRecordException)
		{
			return new RemotingErrorRecord(this, this.OriginInfo, replaceParentContainsErrorRecordException);
		}

		// Token: 0x04000E1F RID: 3615
		private OriginInfo _originInfo;
	}
}
