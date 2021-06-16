using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200000E RID: 14
	[EventSource(Name = "Microsoft.Tasks.Nuget")]
	internal class TplEtwProvider : EventSource
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00008A9A File Offset: 0x00006C9A
		public bool Debug
		{
			get
			{
				return base.IsEnabled(EventLevel.Verbose, (EventKeywords)131072L);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008AA9 File Offset: 0x00006CA9
		public void DebugFacilityMessage(string Facility, string Message)
		{
			base.WriteEvent(1, Facility, Message);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00008AB4 File Offset: 0x00006CB4
		public void DebugFacilityMessage1(string Facility, string Message, string Arg)
		{
			base.WriteEvent(2, Facility, Message, Arg);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public void SetActivityId(Guid Id)
		{
			base.WriteEvent(3, new object[]
			{
				Id
			});
		}

		// Token: 0x0400005E RID: 94
		public static TplEtwProvider Log = new TplEtwProvider();

		// Token: 0x0200000F RID: 15
		public class Keywords
		{
			// Token: 0x0400005F RID: 95
			public const EventKeywords TasksFlowActivityIds = (EventKeywords)128L;

			// Token: 0x04000060 RID: 96
			public const EventKeywords Debug = (EventKeywords)131072L;
		}
	}
}
