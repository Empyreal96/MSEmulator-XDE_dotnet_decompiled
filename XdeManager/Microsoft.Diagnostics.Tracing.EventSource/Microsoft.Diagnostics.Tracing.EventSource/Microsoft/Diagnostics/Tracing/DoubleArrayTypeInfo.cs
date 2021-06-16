using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005E RID: 94
	internal sealed class DoubleArrayTypeInfo : TraceLoggingTypeInfo<double[]>
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000E1E1 File Offset: 0x0000C3E1
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format64(format, TraceLoggingDataType.Double));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000E1F2 File Offset: 0x0000C3F2
		public override void WriteData(TraceLoggingDataCollector collector, ref double[] value)
		{
			collector.AddArray(value);
		}
	}
}
