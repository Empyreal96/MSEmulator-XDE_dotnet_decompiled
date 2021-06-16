using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004F RID: 79
	internal sealed class DoubleTypeInfo : TraceLoggingTypeInfo<double>
	{
		// Token: 0x0600022E RID: 558 RVA: 0x0000DF59 File Offset: 0x0000C159
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format64(format, TraceLoggingDataType.Double));
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000DF6A File Offset: 0x0000C16A
		public override void WriteData(TraceLoggingDataCollector collector, ref double value)
		{
			collector.AddScalar(value);
		}
	}
}
