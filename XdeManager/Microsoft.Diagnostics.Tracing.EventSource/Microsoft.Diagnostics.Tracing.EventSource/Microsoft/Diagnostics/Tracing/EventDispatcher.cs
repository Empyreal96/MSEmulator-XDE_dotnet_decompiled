using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000032 RID: 50
	internal class EventDispatcher
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0000B405 File Offset: 0x00009605
		internal EventDispatcher(EventDispatcher next, bool[] eventEnabled, EventListener listener)
		{
			this.m_Next = next;
			this.m_EventEnabled = eventEnabled;
			this.m_Listener = listener;
		}

		// Token: 0x040000F5 RID: 245
		internal readonly EventListener m_Listener;

		// Token: 0x040000F6 RID: 246
		internal bool[] m_EventEnabled;

		// Token: 0x040000F7 RID: 247
		internal EventDispatcher m_Next;
	}
}
