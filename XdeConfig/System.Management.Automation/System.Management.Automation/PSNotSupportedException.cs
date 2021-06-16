using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000887 RID: 2183
	[Serializable]
	public class PSNotSupportedException : NotSupportedException, IContainsErrorRecord
	{
		// Token: 0x0600536E RID: 21358 RVA: 0x001BA72E File Offset: 0x001B892E
		public PSNotSupportedException()
		{
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x001BA741 File Offset: 0x001B8941
		protected PSNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x001BA767 File Offset: 0x001B8967
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

		// Token: 0x06005371 RID: 21361 RVA: 0x001BA790 File Offset: 0x001B8990
		public PSNotSupportedException(string message) : base(message)
		{
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001BA7A4 File Offset: 0x001B89A4
		public PSNotSupportedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06005373 RID: 21363 RVA: 0x001BA7B9 File Offset: 0x001B89B9
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

		// Token: 0x04002AE0 RID: 10976
		private ErrorRecord _errorRecord;

		// Token: 0x04002AE1 RID: 10977
		private string _errorId = "NotSupported";
	}
}
