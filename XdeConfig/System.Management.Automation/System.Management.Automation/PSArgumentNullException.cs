using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000883 RID: 2179
	[Serializable]
	public class PSArgumentNullException : ArgumentNullException, IContainsErrorRecord
	{
		// Token: 0x06005351 RID: 21329 RVA: 0x001BA384 File Offset: 0x001B8584
		public PSArgumentNullException()
		{
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x001BA397 File Offset: 0x001B8597
		public PSArgumentNullException(string paramName) : base(paramName)
		{
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x001BA3AB File Offset: 0x001B85AB
		public PSArgumentNullException(string message, Exception innerException) : base(message, innerException)
		{
			this._message = message;
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x001BA3C7 File Offset: 0x001B85C7
		public PSArgumentNullException(string paramName, string message) : base(paramName, message)
		{
			this._message = message;
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x001BA3E3 File Offset: 0x001B85E3
		protected PSArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
			this._message = info.GetString("PSArgumentNullException_MessageOverride");
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x001BA41A File Offset: 0x001B861A
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorId", this._errorId);
			info.AddValue("PSArgumentNullException_MessageOverride", this._message);
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06005357 RID: 21335 RVA: 0x001BA454 File Offset: 0x001B8654
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, ErrorCategory.InvalidArgument, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06005358 RID: 21336 RVA: 0x001BA47D File Offset: 0x001B867D
		public override string Message
		{
			get
			{
				if (!string.IsNullOrEmpty(this._message))
				{
					return this._message;
				}
				return base.Message;
			}
		}

		// Token: 0x04002AD5 RID: 10965
		private ErrorRecord _errorRecord;

		// Token: 0x04002AD6 RID: 10966
		private string _errorId = "ArgumentNull";

		// Token: 0x04002AD7 RID: 10967
		private string _message;
	}
}
