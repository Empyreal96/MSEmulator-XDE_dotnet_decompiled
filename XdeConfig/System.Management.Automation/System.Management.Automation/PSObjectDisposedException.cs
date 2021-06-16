using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000888 RID: 2184
	[Serializable]
	public class PSObjectDisposedException : ObjectDisposedException, IContainsErrorRecord
	{
		// Token: 0x06005374 RID: 21364 RVA: 0x001BA7E3 File Offset: 0x001B89E3
		public PSObjectDisposedException(string objectName) : base(objectName)
		{
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x001BA7F7 File Offset: 0x001B89F7
		public PSObjectDisposedException(string objectName, string message) : base(objectName, message)
		{
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x001BA80C File Offset: 0x001B8A0C
		public PSObjectDisposedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x001BA821 File Offset: 0x001B8A21
		protected PSObjectDisposedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x001BA847 File Offset: 0x001B8A47
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

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06005379 RID: 21369 RVA: 0x001BA870 File Offset: 0x001B8A70
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, ErrorCategory.InvalidOperation, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x04002AE2 RID: 10978
		private ErrorRecord _errorRecord;

		// Token: 0x04002AE3 RID: 10979
		private string _errorId = "ObjectDisposed";
	}
}
