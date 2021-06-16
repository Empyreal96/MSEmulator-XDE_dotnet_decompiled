using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000884 RID: 2180
	[Serializable]
	public class PSArgumentOutOfRangeException : ArgumentOutOfRangeException, IContainsErrorRecord
	{
		// Token: 0x06005359 RID: 21337 RVA: 0x001BA499 File Offset: 0x001B8699
		public PSArgumentOutOfRangeException()
		{
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x001BA4AC File Offset: 0x001B86AC
		public PSArgumentOutOfRangeException(string paramName) : base(paramName)
		{
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x001BA4C0 File Offset: 0x001B86C0
		public PSArgumentOutOfRangeException(string paramName, object actualValue, string message) : base(paramName, actualValue, message)
		{
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x001BA4D6 File Offset: 0x001B86D6
		protected PSArgumentOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x001BA4FC File Offset: 0x001B86FC
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

		// Token: 0x0600535E RID: 21342 RVA: 0x001BA525 File Offset: 0x001B8725
		public PSArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x0600535F RID: 21343 RVA: 0x001BA53A File Offset: 0x001B873A
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

		// Token: 0x04002AD8 RID: 10968
		private ErrorRecord _errorRecord;

		// Token: 0x04002AD9 RID: 10969
		private string _errorId = "ArgumentOutOfRange";
	}
}
