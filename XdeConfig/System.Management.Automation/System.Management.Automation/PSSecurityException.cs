using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020007B6 RID: 1974
	[Serializable]
	public class PSSecurityException : RuntimeException
	{
		// Token: 0x06004D74 RID: 19828 RVA: 0x00197808 File Offset: 0x00195A08
		public PSSecurityException()
		{
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UnauthorizedAccess", ErrorCategory.SecurityError, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(SessionStateStrings.CanNotRun);
			this._message = this._errorRecord.ErrorDetails.Message;
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x00197860 File Offset: 0x00195A60
		protected PSSecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UnauthorizedAccess", ErrorCategory.SecurityError, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(SessionStateStrings.CanNotRun);
			this._message = this._errorRecord.ErrorDetails.Message;
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x001978B9 File Offset: 0x00195AB9
		public PSSecurityException(string message) : base(message)
		{
			this._message = message;
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UnauthorizedAccess", ErrorCategory.SecurityError, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(message);
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x001978F4 File Offset: 0x00195AF4
		public PSSecurityException(string message, Exception innerException) : base(message, innerException)
		{
			this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UnauthorizedAccess", ErrorCategory.SecurityError, null);
			this._errorRecord.ErrorDetails = new ErrorDetails(message);
			this._message = this._errorRecord.ErrorDetails.Message;
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x00197949 File Offset: 0x00195B49
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "UnauthorizedAccess", ErrorCategory.SecurityError, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x00197972 File Offset: 0x00195B72
		public override string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x04002680 RID: 9856
		private ErrorRecord _errorRecord;

		// Token: 0x04002681 RID: 9857
		private string _message;
	}
}
