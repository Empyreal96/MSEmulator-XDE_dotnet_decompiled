using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000059 RID: 89
	internal sealed class Int64ArrayTypeInfo : TraceLoggingTypeInfo<long[]>
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0000E129 File Offset: 0x0000C329
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format64(format, TraceLoggingDataType.Int64));
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E13A File Offset: 0x0000C33A
		public override void WriteData(TraceLoggingDataCollector collector, ref long[] value)
		{
			collector.AddArray(value);
		}
	}
}
