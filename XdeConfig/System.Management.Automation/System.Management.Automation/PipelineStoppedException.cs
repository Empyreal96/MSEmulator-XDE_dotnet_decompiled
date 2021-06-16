using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200086D RID: 2157
	[Serializable]
	public class PipelineStoppedException : RuntimeException
	{
		// Token: 0x060052C5 RID: 21189 RVA: 0x001B94FA File Offset: 0x001B76FA
		public PipelineStoppedException() : base(GetErrorText.PipelineStoppedException)
		{
			base.SetErrorId("PipelineStopped");
			base.SetErrorCategory(ErrorCategory.OperationStopped);
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x001B951A File Offset: 0x001B771A
		protected PipelineStoppedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x001B9524 File Offset: 0x001B7724
		public PipelineStoppedException(string message) : base(message)
		{
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x001B952D File Offset: 0x001B772D
		public PipelineStoppedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
