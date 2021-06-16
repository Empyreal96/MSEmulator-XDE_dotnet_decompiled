using System;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Diagnostics.Telemetry
{
	// Token: 0x02000011 RID: 17
	internal class TelemetryEventSource : EventSource
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00003F93 File Offset: 0x00002193
		public TelemetryEventSource(string eventSourceName) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.MicrosoftTelemetryTraits)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003FA2 File Offset: 0x000021A2
		protected TelemetryEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.MicrosoftTelemetryTraits)
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003FB0 File Offset: 0x000021B0
		public TelemetryEventSource(string eventSourceName, TelemetryGroup telemetryGroup) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, (telemetryGroup == TelemetryGroup.WindowsCoreTelemetry) ? TelemetryEventSource.WindowsCoreTelemetryTraits : TelemetryEventSource.MicrosoftTelemetryTraits)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003FCC File Offset: 0x000021CC
		public static EventSourceOptions TelemetryOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)35184372088832L
			};
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003FF4 File Offset: 0x000021F4
		public static EventSourceOptions MeasuresOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)70368744177664L
			};
		}

		// Token: 0x04000049 RID: 73
		public const EventKeywords Reserved44Keyword = (EventKeywords)17592186044416L;

		// Token: 0x0400004A RID: 74
		public const EventKeywords TelemetryKeyword = (EventKeywords)35184372088832L;

		// Token: 0x0400004B RID: 75
		public const EventKeywords MeasuresKeyword = (EventKeywords)70368744177664L;

		// Token: 0x0400004C RID: 76
		public const EventKeywords CriticalDataKeyword = (EventKeywords)140737488355328L;

		// Token: 0x0400004D RID: 77
		public const EventTags CostDeferredLatency = (EventTags)262144;

		// Token: 0x0400004E RID: 78
		public const EventTags CoreData = (EventTags)524288;

		// Token: 0x0400004F RID: 79
		public const EventTags InjectXToken = (EventTags)1048576;

		// Token: 0x04000050 RID: 80
		public const EventTags RealtimeLatency = (EventTags)2097152;

		// Token: 0x04000051 RID: 81
		public const EventTags NormalLatency = (EventTags)4194304;

		// Token: 0x04000052 RID: 82
		public const EventTags CriticalPersistence = (EventTags)8388608;

		// Token: 0x04000053 RID: 83
		public const EventTags NormalPersistence = (EventTags)16777216;

		// Token: 0x04000054 RID: 84
		public const EventTags DropPii = (EventTags)33554432;

		// Token: 0x04000055 RID: 85
		public const EventTags HashPii = (EventTags)67108864;

		// Token: 0x04000056 RID: 86
		public const EventTags MarkPii = (EventTags)134217728;

		// Token: 0x04000057 RID: 87
		public const EventFieldTags DropPiiField = (EventFieldTags)67108864;

		// Token: 0x04000058 RID: 88
		public const EventFieldTags HashPiiField = (EventFieldTags)134217728;

		// Token: 0x04000059 RID: 89
		private static readonly string[] MicrosoftTelemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
		};

		// Token: 0x0400005A RID: 90
		private static readonly string[] WindowsCoreTelemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{c7de053a-0c2e-4a44-91a2-5222ec2ecdf1}"
		};
	}
}
