using System;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Diagnostics.Telemetry
{
	// Token: 0x02000010 RID: 16
	internal class PartnerTelemetryEventSource : EventSource
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00003F59 File Offset: 0x00002159
		public PartnerTelemetryEventSource(string eventSourceName) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, PartnerTelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003F68 File Offset: 0x00002168
		protected PartnerTelemetryEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat, PartnerTelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x04000038 RID: 56
		private const string PartnerProviderGuid = "5ECB0BAC-B930-47F5-A8A4-E8253529EDB7";

		// Token: 0x04000039 RID: 57
		public const EventKeywords Reserved44Keyword = (EventKeywords)17592186044416L;

		// Token: 0x0400003A RID: 58
		public const EventKeywords TelemetryKeyword = (EventKeywords)35184372088832L;

		// Token: 0x0400003B RID: 59
		public const EventKeywords MeasuresKeyword = (EventKeywords)70368744177664L;

		// Token: 0x0400003C RID: 60
		public const EventKeywords CriticalDataKeyword = (EventKeywords)140737488355328L;

		// Token: 0x0400003D RID: 61
		public const EventTags CoreData = (EventTags)524288;

		// Token: 0x0400003E RID: 62
		public const EventTags InjectXToken = (EventTags)1048576;

		// Token: 0x0400003F RID: 63
		public const EventTags RealtimeLatency = (EventTags)2097152;

		// Token: 0x04000040 RID: 64
		public const EventTags NormalLatency = (EventTags)4194304;

		// Token: 0x04000041 RID: 65
		public const EventTags CriticalPersistence = (EventTags)8388608;

		// Token: 0x04000042 RID: 66
		public const EventTags NormalPersistence = (EventTags)16777216;

		// Token: 0x04000043 RID: 67
		public const EventTags DropPii = (EventTags)33554432;

		// Token: 0x04000044 RID: 68
		public const EventTags HashPii = (EventTags)67108864;

		// Token: 0x04000045 RID: 69
		public const EventTags MarkPii = (EventTags)134217728;

		// Token: 0x04000046 RID: 70
		public const EventFieldTags DropPiiField = (EventFieldTags)67108864;

		// Token: 0x04000047 RID: 71
		public const EventFieldTags HashPiiField = (EventFieldTags)134217728;

		// Token: 0x04000048 RID: 72
		private static readonly string[] telemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{5ECB0BAC-B930-47F5-A8A4-E8253529EDB7}"
		};
	}
}
