using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000851 RID: 2129
	[Serializable]
	public class PSSnapInException : RuntimeException
	{
		// Token: 0x06005200 RID: 20992 RVA: 0x001B5D68 File Offset: 0x001B3F68
		internal PSSnapInException(string PSSnapin, string message)
		{
			this._PSSnapin = PSSnapin;
			this._reason = message;
			this.CreateErrorRecord();
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x001B5D9A File Offset: 0x001B3F9A
		internal PSSnapInException(string PSSnapin, string message, bool warning)
		{
			this._PSSnapin = PSSnapin;
			this._reason = message;
			this._warning = warning;
			this.CreateErrorRecord();
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001B5DD3 File Offset: 0x001B3FD3
		internal PSSnapInException(string PSSnapin, string message, Exception exception) : base(message, exception)
		{
			this._PSSnapin = PSSnapin;
			this._reason = message;
			this.CreateErrorRecord();
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x001B5E07 File Offset: 0x001B4007
		public PSSnapInException()
		{
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x001B5E25 File Offset: 0x001B4025
		public PSSnapInException(string message) : base(message)
		{
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x001B5E44 File Offset: 0x001B4044
		public PSSnapInException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x001B5E64 File Offset: 0x001B4064
		private void CreateErrorRecord()
		{
			if (!string.IsNullOrEmpty(this._PSSnapin) && !string.IsNullOrEmpty(this._reason))
			{
				Assembly assembly = typeof(PSSnapInException).GetTypeInfo().Assembly;
				if (this._warning)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "PSSnapInLoadWarning", ErrorCategory.ResourceUnavailable, null);
					this._errorRecord.ErrorDetails = new ErrorDetails(assembly, "ConsoleInfoErrorStrings", "PSSnapInLoadWarning", new object[]
					{
						this._PSSnapin,
						this._reason
					});
					return;
				}
				this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "PSSnapInLoadFailure", ErrorCategory.ResourceUnavailable, null);
				this._errorRecord.ErrorDetails = new ErrorDetails(assembly, "ConsoleInfoErrorStrings", "PSSnapInLoadFailure", new object[]
				{
					this._PSSnapin,
					this._reason
				});
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06005207 RID: 20999 RVA: 0x001B5F4B File Offset: 0x001B414B
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._isErrorRecordOriginallyNull = true;
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "PSSnapInException", ErrorCategory.NotSpecified, null);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06005208 RID: 21000 RVA: 0x001B5F7A File Offset: 0x001B417A
		public override string Message
		{
			get
			{
				if (this._errorRecord != null && !this._isErrorRecordOriginallyNull)
				{
					return this._errorRecord.ToString();
				}
				return base.Message;
			}
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x001B5FA0 File Offset: 0x001B41A0
		protected PSSnapInException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._PSSnapin = info.GetString("PSSnapIn");
			this._reason = info.GetString("Reason");
			this.CreateErrorRecord();
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x001B5FF3 File Offset: 0x001B41F3
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("PSSnapIn", this._PSSnapin);
			info.AddValue("Reason", this._reason);
		}

		// Token: 0x04002A28 RID: 10792
		private bool _warning;

		// Token: 0x04002A29 RID: 10793
		private ErrorRecord _errorRecord;

		// Token: 0x04002A2A RID: 10794
		private bool _isErrorRecordOriginallyNull;

		// Token: 0x04002A2B RID: 10795
		private string _PSSnapin = "";

		// Token: 0x04002A2C RID: 10796
		private string _reason = "";
	}
}
