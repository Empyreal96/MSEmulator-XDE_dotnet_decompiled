using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x020008A5 RID: 2213
	[Serializable]
	public class SessionStateException : RuntimeException
	{
		// Token: 0x06005499 RID: 21657 RVA: 0x001BF0EC File Offset: 0x001BD2EC
		internal SessionStateException(string itemName, SessionStateCategory sessionStateCategory, string errorIdAndResourceId, string resourceStr, ErrorCategory errorCategory, params object[] messageArgs) : base(SessionStateException.BuildMessage(itemName, resourceStr, messageArgs))
		{
			this._itemName = itemName;
			this._sessionStateCategory = sessionStateCategory;
			this._errorId = errorIdAndResourceId;
			this._errorCategory = errorCategory;
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x001BF143 File Offset: 0x001BD343
		public SessionStateException()
		{
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x001BF168 File Offset: 0x001BD368
		public SessionStateException(string message) : base(message)
		{
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x001BF18E File Offset: 0x001BD38E
		public SessionStateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x001BF1B5 File Offset: 0x001BD3B5
		protected SessionStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._sessionStateCategory = (SessionStateCategory)info.GetInt32("SessionStateCategory");
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x001BF1ED File Offset: 0x001BD3ED
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("SessionStateCategory", (int)this._sessionStateCategory);
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x0600549F RID: 21663 RVA: 0x001BF216 File Offset: 0x001BD416
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, this._errorCategory, this._itemName);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x001BF249 File Offset: 0x001BD449
		public string ItemName
		{
			get
			{
				return this._itemName;
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060054A1 RID: 21665 RVA: 0x001BF251 File Offset: 0x001BD451
		public SessionStateCategory SessionStateCategory
		{
			get
			{
				return this._sessionStateCategory;
			}
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x001BF25C File Offset: 0x001BD45C
		private static string BuildMessage(string itemName, string resourceStr, params object[] messageArgs)
		{
			object[] array;
			if (messageArgs != null && 0 < messageArgs.Length)
			{
				array = new object[messageArgs.Length + 1];
				array[0] = itemName;
				messageArgs.CopyTo(array, 1);
			}
			else
			{
				array = new object[]
				{
					itemName
				};
			}
			return StringUtil.Format(resourceStr, array);
		}

		// Token: 0x04002B79 RID: 11129
		private ErrorRecord _errorRecord;

		// Token: 0x04002B7A RID: 11130
		private string _itemName = string.Empty;

		// Token: 0x04002B7B RID: 11131
		private SessionStateCategory _sessionStateCategory;

		// Token: 0x04002B7C RID: 11132
		private string _errorId = "SessionStateException";

		// Token: 0x04002B7D RID: 11133
		private ErrorCategory _errorCategory = ErrorCategory.InvalidArgument;
	}
}
