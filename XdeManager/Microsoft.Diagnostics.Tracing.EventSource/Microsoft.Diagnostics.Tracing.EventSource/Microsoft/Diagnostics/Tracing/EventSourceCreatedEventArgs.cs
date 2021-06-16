using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000029 RID: 41
	public class EventSourceCreatedEventArgs : EventArgs
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000AF08 File Offset: 0x00009108
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000AF10 File Offset: 0x00009110
		public EventSource EventSource { get; internal set; }
	}
}
