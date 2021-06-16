using System;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008F0 RID: 2288
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EtwEvent : Attribute
	{
		// Token: 0x060055E3 RID: 21987 RVA: 0x001C35AB File Offset: 0x001C17AB
		public EtwEvent(long eventId)
		{
			this.eventId = eventId;
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x060055E4 RID: 21988 RVA: 0x001C35BA File Offset: 0x001C17BA
		public long EventId
		{
			get
			{
				return this.eventId;
			}
		}

		// Token: 0x04002DB8 RID: 11704
		private long eventId;
	}
}
