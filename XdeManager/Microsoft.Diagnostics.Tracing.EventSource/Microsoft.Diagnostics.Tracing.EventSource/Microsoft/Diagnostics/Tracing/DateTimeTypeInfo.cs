using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006B RID: 107
	internal sealed class DateTimeTypeInfo : TraceLoggingTypeInfo<DateTime>
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000E425 File Offset: 0x0000C625
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.MakeDataType(TraceLoggingDataType.FileTime, format));
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000E438 File Offset: 0x0000C638
		public override void WriteData(TraceLoggingDataCollector collector, ref DateTime value)
		{
			long ticks = value.Ticks;
			collector.AddScalar((ticks < 504911232000000000L) ? 0L : checked(ticks - 504911232000000000L));
		}
	}
}
