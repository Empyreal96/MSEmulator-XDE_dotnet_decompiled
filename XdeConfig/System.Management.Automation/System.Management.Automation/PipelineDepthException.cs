using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000873 RID: 2163
	[Serializable]
	public class PipelineDepthException : SystemException, IContainsErrorRecord
	{
		// Token: 0x060052E8 RID: 21224 RVA: 0x001B9820 File Offset: 0x001B7A20
		public PipelineDepthException() : base(GetErrorText.PipelineDepthException)
		{
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x001B982D File Offset: 0x001B7A2D
		public PipelineDepthException(string message) : base(message)
		{
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x001B9836 File Offset: 0x001B7A36
		public PipelineDepthException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x001B9840 File Offset: 0x001B7A40
		protected PipelineDepthException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x001B984A File Offset: 0x001B7A4A
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x001B9854 File Offset: 0x001B7A54
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

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060052EE RID: 21230 RVA: 0x001B9886 File Offset: 0x001B7A86
		public int CallDepth
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04002AB3 RID: 10931
		private ErrorRecord errorRecord;
	}
}
