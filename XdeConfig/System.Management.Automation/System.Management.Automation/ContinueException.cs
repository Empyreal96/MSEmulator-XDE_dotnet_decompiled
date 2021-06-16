using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000480 RID: 1152
	public sealed class ContinueException : LoopFlowException
	{
		// Token: 0x06003347 RID: 13127 RVA: 0x00118266 File Offset: 0x00116466
		internal ContinueException(string label) : base(label)
		{
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x0011826F File Offset: 0x0011646F
		internal ContinueException()
		{
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x00118277 File Offset: 0x00116477
		internal ContinueException(string label, Exception innerException) : base(label)
		{
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x00118280 File Offset: 0x00116480
		private ContinueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
