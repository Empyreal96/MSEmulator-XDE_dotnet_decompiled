using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000872 RID: 2162
	[Serializable]
	public class ScriptCallDepthException : SystemException, IContainsErrorRecord
	{
		// Token: 0x060052E1 RID: 21217 RVA: 0x001B97B7 File Offset: 0x001B79B7
		public ScriptCallDepthException() : base(GetErrorText.ScriptCallDepthException)
		{
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x001B97C4 File Offset: 0x001B79C4
		public ScriptCallDepthException(string message) : base(message)
		{
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x001B97CD File Offset: 0x001B79CD
		public ScriptCallDepthException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x001B97D7 File Offset: 0x001B79D7
		protected ScriptCallDepthException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x001B97E1 File Offset: 0x001B79E1
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001B97EB File Offset: 0x001B79EB
		public ErrorRecord ErrorRecord
		{
			get
			{
				if (this.errorRecord == null)
				{
					this.errorRecord = new ErrorRecord(new ParentContainsErrorRecordException(this), "CallDepthOverflow", ErrorCategory.InvalidOperation, this.CallDepth);
				}
				return this.errorRecord;
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x001B981D File Offset: 0x001B7A1D
		public int CallDepth
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04002AB2 RID: 10930
		private ErrorRecord errorRecord;
	}
}
