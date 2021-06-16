using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000870 RID: 2160
	[Serializable]
	public class ParentContainsErrorRecordException : SystemException
	{
		// Token: 0x060052D6 RID: 21206 RVA: 0x001B96B1 File Offset: 0x001B78B1
		public ParentContainsErrorRecordException(Exception wrapperException)
		{
			this.wrapperException = wrapperException;
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001B96C0 File Offset: 0x001B78C0
		public ParentContainsErrorRecordException(string message)
		{
			this.message = message;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001B96CF File Offset: 0x001B78CF
		public ParentContainsErrorRecordException()
		{
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x001B96D7 File Offset: 0x001B78D7
		public ParentContainsErrorRecordException(string message, Exception innerException) : base(message, innerException)
		{
			this.message = message;
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x001B96E8 File Offset: 0x001B78E8
		protected ParentContainsErrorRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.message = info.GetString("ParentContainsErrorRecordException_Message");
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x060052DB RID: 21211 RVA: 0x001B9703 File Offset: 0x001B7903
		public override string Message
		{
			get
			{
				if (this.message == null)
				{
					this.message = ((this.wrapperException != null) ? this.wrapperException.Message : string.Empty);
				}
				return this.message;
			}
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001B9733 File Offset: 0x001B7933
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ParentContainsErrorRecordException_Message", this.Message);
		}

		// Token: 0x04002AB0 RID: 10928
		private readonly Exception wrapperException;

		// Token: 0x04002AB1 RID: 10929
		private string message;
	}
}
