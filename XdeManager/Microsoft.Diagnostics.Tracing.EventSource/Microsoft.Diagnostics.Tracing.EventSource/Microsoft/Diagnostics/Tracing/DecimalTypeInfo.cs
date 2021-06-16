using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006E RID: 110
	internal sealed class DecimalTypeInfo : TraceLoggingTypeInfo<decimal>
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000E528 File Offset: 0x0000C728
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.MakeDataType(TraceLoggingDataType.Double, format));
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000E539 File Offset: 0x0000C739
		public override void WriteData(TraceLoggingDataCollector collector, ref decimal value)
		{
			collector.AddScalar((double)value);
		}
	}
}
