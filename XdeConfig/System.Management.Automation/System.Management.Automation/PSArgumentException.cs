using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000882 RID: 2178
	[Serializable]
	public class PSArgumentException : ArgumentException, IContainsErrorRecord
	{
		// Token: 0x06005349 RID: 21321 RVA: 0x001BA26F File Offset: 0x001B846F
		public PSArgumentException()
		{
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x001BA282 File Offset: 0x001B8482
		public PSArgumentException(string message) : base(message)
		{
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x001BA296 File Offset: 0x001B8496
		public PSArgumentException(string message, string paramName) : base(message, paramName)
		{
			this._message = message;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x001BA2B2 File Offset: 0x001B84B2
		protected PSArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
			this._message = info.GetString("PSArgumentException_MessageOverride");
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001BA2E9 File Offset: 0x001B84E9
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ErrorId", this._errorId);
			info.AddValue("PSArgumentException_MessageOverride", this._message);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001BA323 File Offset: 0x001B8523
		public PSArgumentException(string message, Exception innerException) : base(message, innerException)
		{
			this._message = message;
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600534F RID: 21327 RVA: 0x001BA33F File Offset: 0x001B853F
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

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06005350 RID: 21328 RVA: 0x001BA368 File Offset: 0x001B8568
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

		// Token: 0x04002AD2 RID: 10962
		private ErrorRecord _errorRecord;

		// Token: 0x04002AD3 RID: 10963
		private string _errorId = "Argument";

		// Token: 0x04002AD4 RID: 10964
		private string _message;
	}
}
