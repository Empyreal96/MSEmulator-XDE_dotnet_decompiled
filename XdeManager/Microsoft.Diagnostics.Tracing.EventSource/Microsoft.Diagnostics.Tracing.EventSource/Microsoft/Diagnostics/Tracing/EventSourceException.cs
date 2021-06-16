using System;
using System.Runtime.Serialization;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class EventSourceException : Exception
	{
		// Token: 0x060001DD RID: 477 RVA: 0x0000D54C File Offset: 0x0000B74C
		public EventSourceException() : base(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerWriteFailure", new object[0]))
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D564 File Offset: 0x0000B764
		public EventSourceException(string message) : base(message)
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D56D File Offset: 0x0000B76D
		public EventSourceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D577 File Offset: 0x0000B777
		protected EventSourceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D581 File Offset: 0x0000B781
		internal EventSourceException(Exception innerException) : base(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ListenerWriteFailure", new object[0]), innerException)
		{
		}
	}
}
