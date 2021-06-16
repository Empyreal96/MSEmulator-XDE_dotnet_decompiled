using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000885 RID: 2181
	[Serializable]
	public class PSInvalidOperationException : InvalidOperationException, IContainsErrorRecord
	{
		// Token: 0x06005360 RID: 21344 RVA: 0x001BA563 File Offset: 0x001B8763
		public PSInvalidOperationException()
		{
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x001BA57D File Offset: 0x001B877D
		protected PSInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._errorId = info.GetString("ErrorId");
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x001BA5AA File Offset: 0x001B87AA
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

		// Token: 0x06005363 RID: 21347 RVA: 0x001BA5D3 File Offset: 0x001B87D3
		public PSInvalidOperationException(string message) : base(message)
		{
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x001BA5EE File Offset: 0x001B87EE
		public PSInvalidOperationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x001BA60A File Offset: 0x001B880A
		internal PSInvalidOperationException(string message, Exception innerException, string errorId, ErrorCategory errorCategory, object target) : base(message, innerException)
		{
			this._errorId = errorId;
			this._errorCategory = errorCategory;
			this._target = target;
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x001BA63D File Offset: 0x001B883D
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, this._errorCategory, this._target);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x001BA670 File Offset: 0x001B8870
		internal void SetErrorId(string errorId)
		{
			this._errorId = errorId;
		}

		// Token: 0x04002ADA RID: 10970
		private ErrorRecord _errorRecord;

		// Token: 0x04002ADB RID: 10971
		private string _errorId = "InvalidOperation";

		// Token: 0x04002ADC RID: 10972
		private ErrorCategory _errorCategory = ErrorCategory.InvalidOperation;

		// Token: 0x04002ADD RID: 10973
		private object _target;
	}
}
