using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000886 RID: 2182
	[Serializable]
	public class PSNotImplementedException : NotImplementedException, IContainsErrorRecord
	{
		// Token: 0x06005368 RID: 21352 RVA: 0x001BA679 File Offset: 0x001B8879
		public PSNotImplementedException()
		{
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x001BA68C File Offset: 0x001B888C
		protected PSNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x001BA6B2 File Offset: 0x001B88B2
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorId", this._errorId);
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001BA6DB File Offset: 0x001B88DB
		public PSNotImplementedException(string message) : base(message)
		{
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001BA6EF File Offset: 0x001B88EF
		public PSNotImplementedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x0600536D RID: 21357 RVA: 0x001BA704 File Offset: 0x001B8904
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, ErrorCategory.NotImplemented, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x04002ADE RID: 10974
		private ErrorRecord _errorRecord;

		// Token: 0x04002ADF RID: 10975
		private string _errorId = "NotImplemented";
	}
}
