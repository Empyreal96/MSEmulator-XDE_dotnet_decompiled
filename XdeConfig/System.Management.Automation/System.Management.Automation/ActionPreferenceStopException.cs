using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x0200086F RID: 2159
	[Serializable]
	public class ActionPreferenceStopException : RuntimeException
	{
		// Token: 0x060052CD RID: 21197 RVA: 0x001B955C File Offset: 0x001B775C
		public ActionPreferenceStopException() : this(GetErrorText.ActionPreferenceStop)
		{
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001B9569 File Offset: 0x001B7769
		internal ActionPreferenceStopException(ErrorRecord error) : this(RuntimeException.RetrieveMessage(error))
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			this._errorRecord = error;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x001B958C File Offset: 0x001B778C
		internal ActionPreferenceStopException(InvocationInfo invocationInfo, string message) : this(message)
		{
			base.ErrorRecord.SetInvocationInfo(invocationInfo);
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001B95A1 File Offset: 0x001B77A1
		internal ActionPreferenceStopException(InvocationInfo invocationInfo, ErrorRecord errorRecord, string message) : this(invocationInfo, message)
		{
			if (errorRecord == null)
			{
				throw new ArgumentNullException("errorRecord");
			}
			this._errorRecord = errorRecord;
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x001B95C0 File Offset: 0x001B77C0
		protected ActionPreferenceStopException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			bool boolean = info.GetBoolean("HasErrorRecord");
			if (boolean)
			{
				this._errorRecord = (ErrorRecord)info.GetValue("ErrorRecord", typeof(ErrorRecord));
			}
			base.SuppressPromptInInterpreter = true;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x001B960C File Offset: 0x001B780C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			if (info != null)
			{
				bool flag = null != this._errorRecord;
				info.AddValue("HasErrorRecord", flag);
				if (flag)
				{
					info.AddValue("ErrorRecord", this._errorRecord);
				}
			}
			base.SuppressPromptInInterpreter = true;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x001B9658 File Offset: 0x001B7858
		public ActionPreferenceStopException(string message) : base(message)
		{
			base.SetErrorCategory(ErrorCategory.OperationStopped);
			base.SetErrorId("ActionPreferenceStop");
			base.SuppressPromptInInterpreter = true;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001B967B File Offset: 0x001B787B
		public ActionPreferenceStopException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCategory(ErrorCategory.OperationStopped);
			base.SetErrorId("ActionPreferenceStop");
			base.SuppressPromptInInterpreter = true;
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x060052D5 RID: 21205 RVA: 0x001B969F File Offset: 0x001B789F
		public override ErrorRecord ErrorRecord
		{
			get
			{
				return this._errorRecord ?? base.ErrorRecord;
			}
		}

		// Token: 0x04002AAF RID: 10927
		private readonly ErrorRecord _errorRecord;
	}
}
