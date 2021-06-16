using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006C RID: 108
	internal sealed class DateTimeOffsetTypeInfo : TraceLoggingTypeInfo<DateTimeOffset>
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000E478 File Offset: 0x0000C678
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			TraceLoggingMetadataCollector traceLoggingMetadataCollector = collector.AddGroup(name);
			traceLoggingMetadataCollector.AddScalar("Ticks", Statics.MakeDataType(TraceLoggingDataType.FileTime, format));
			traceLoggingMetadataCollector.AddScalar("Offset", TraceLoggingDataType.Int64);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		public override void WriteData(TraceLoggingDataCollector collector, ref DateTimeOffset value)
		{
			long ticks = value.Ticks;
			collector.AddScalar((ticks < 504911232000000000L) ? 0L : checked(ticks - 504911232000000000L));
			collector.AddScalar(value.Offset.Ticks);
		}
	}
}
