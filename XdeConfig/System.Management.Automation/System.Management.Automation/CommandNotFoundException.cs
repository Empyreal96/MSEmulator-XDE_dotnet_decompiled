using System;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000868 RID: 2152
	[Serializable]
	public class CommandNotFoundException : RuntimeException
	{
		// Token: 0x060052A5 RID: 21157 RVA: 0x001B90A7 File Offset: 0x001B72A7
		internal CommandNotFoundException(string commandName, Exception innerException, string errorIdAndResourceId, string resourceStr, params object[] messageArgs) : base(CommandNotFoundException.BuildMessage(commandName, resourceStr, messageArgs), innerException)
		{
			this.commandName = commandName;
			this._errorId = errorIdAndResourceId;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001B90E6 File Offset: 0x001B72E6
		public CommandNotFoundException()
		{
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x001B910C File Offset: 0x001B730C
		public CommandNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x001B9133 File Offset: 0x001B7333
		public CommandNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x001B915C File Offset: 0x001B735C
		protected CommandNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			this.commandName = info.GetString("CommandName");
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001B91AE File Offset: 0x001B73AE
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("CommandName", this.commandName);
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x060052AB RID: 21163 RVA: 0x001B91D7 File Offset: 0x001B73D7
		public override ErrorRecord ErrorRecord
		{
			get
			{
				if (this._errorRecord == null)
				{
					this._errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), this._errorId, this._errorCategory, this.commandName);
				}
				return this._errorRecord;
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x060052AC RID: 21164 RVA: 0x001B920A File Offset: 0x001B740A
		// (set) Token: 0x060052AD RID: 21165 RVA: 0x001B9212 File Offset: 0x001B7412
		public string CommandName
		{
			get
			{
				return this.commandName;
			}
			set
			{
				this.commandName = value;
			}
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x001B921C File Offset: 0x001B741C
		private static string BuildMessage(string commandName, string resourceStr, params object[] messageArgs)
		{
			object[] array;
			if (messageArgs != null && 0 < messageArgs.Length)
			{
				array = new object[messageArgs.Length + 1];
				array[0] = commandName;
				messageArgs.CopyTo(array, 1);
			}
			else
			{
				array = new object[]
				{
					commandName
				};
			}
			return StringUtil.Format(resourceStr, array);
		}

		// Token: 0x04002A83 RID: 10883
		private ErrorRecord _errorRecord;

		// Token: 0x04002A84 RID: 10884
		private string commandName = string.Empty;

		// Token: 0x04002A85 RID: 10885
		private string _errorId = "CommandNotFoundException";

		// Token: 0x04002A86 RID: 10886
		private ErrorCategory _errorCategory = ErrorCategory.ObjectNotFound;
	}
}
