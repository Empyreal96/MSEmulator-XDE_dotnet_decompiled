using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200047F RID: 1151
	public sealed class BreakException : LoopFlowException
	{
		// Token: 0x06003343 RID: 13123 RVA: 0x00118242 File Offset: 0x00116442
		internal BreakException(string label) : base(label)
		{
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0011824B File Offset: 0x0011644B
		internal BreakException()
		{
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x00118253 File Offset: 0x00116453
		internal BreakException(string label, Exception innerException) : base(label)
		{
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x0011825C File Offset: 0x0011645C
		private BreakException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
