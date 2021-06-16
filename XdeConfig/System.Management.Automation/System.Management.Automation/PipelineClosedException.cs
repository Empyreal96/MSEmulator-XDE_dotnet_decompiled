using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200086E RID: 2158
	[Serializable]
	public class PipelineClosedException : RuntimeException
	{
		// Token: 0x060052C9 RID: 21193 RVA: 0x001B9537 File Offset: 0x001B7737
		public PipelineClosedException()
		{
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001B953F File Offset: 0x001B773F
		public PipelineClosedException(string message) : base(message)
		{
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x001B9548 File Offset: 0x001B7748
		public PipelineClosedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001B9552 File Offset: 0x001B7752
		protected PipelineClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
