using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x0200086B RID: 2155
	[Serializable]
	public class CmdletInvocationException : RuntimeException
	{
		// Token: 0x060052B5 RID: 21173 RVA: 0x001B92EC File Offset: 0x001B74EC
		internal CmdletInvocationException(ErrorRecord errorRecord) : base(RuntimeException.RetrieveMessage(errorRecord), RuntimeException.RetrieveException(errorRecord))
		{
			if (errorRecord == null)
			{
				throw new ArgumentNullException("errorRecord");
			}
			this._errorRecord = errorRecord;
			Exception exception = errorRecord.Exception;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x001B931C File Offset: 0x001B751C
		internal CmdletInvocationException(Exception innerException, InvocationInfo invocationInfo) : base(RuntimeException.RetrieveMessage(innerException), innerException)
		{
			if (innerException == null)
			{
				throw new ArgumentNullException("innerException");
			}
			IContainsErrorRecord containsErrorRecord = innerException as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				this._errorRecord = new ErrorRecord(containsErrorRecord.ErrorRecord, innerException);
			}
			else
			{
				this._errorRecord = new ErrorRecord(innerException, innerException.GetType().FullName, ErrorCategory.NotSpecified, null);
			}
			this._errorRecord.SetInvocationInfo(invocationInfo);
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x001B938F File Offset: 0x001B758F
		public CmdletInvocationException()
		{
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x001B9397 File Offset: 0x001B7597
		public CmdletInvocationException(string message) : base(message)
		{
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x001B93A0 File Offset: 0x001B75A0
		public CmdletInvocationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x001B93AC File Offset: 0x001B75AC
		protected CmdletInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			bool boolean = info.GetBoolean("HasErrorRecord");
			if (boolean)
			{
				this._errorRecord = (ErrorRecord)info.GetValue("ErrorRecord", typeof(ErrorRecord));
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x001B93F0 File Offset: 0x001B75F0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			bool flag = null != this._errorRecord;
			info.AddValue("HasErrorRecord", flag);
			if (flag)
			{
				info.AddValue("ErrorRecord", this._errorRecord);
			}
		}

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x060052BC RID: 21180 RVA: 0x001B9440 File Offset: 0x001B7640
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "CmdletInvocationException", ErrorCategory.NotSpecified, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x04002AAD RID: 10925
		private ErrorRecord _errorRecord;
	}
}
