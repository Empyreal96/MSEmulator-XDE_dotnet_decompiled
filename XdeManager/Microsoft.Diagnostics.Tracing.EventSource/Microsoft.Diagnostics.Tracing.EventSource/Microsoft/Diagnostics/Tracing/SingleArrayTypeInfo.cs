using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005F RID: 95
	internal sealed class SingleArrayTypeInfo : TraceLoggingTypeInfo<float[]>
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0000E204 File Offset: 0x0000C404
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format32(format, TraceLoggingDataType.Float));
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000E215 File Offset: 0x0000C415
		public override void WriteData(TraceLoggingDataCollector collector, ref float[] value)
		{
			collector.AddArray(value);
		}
	}
}
