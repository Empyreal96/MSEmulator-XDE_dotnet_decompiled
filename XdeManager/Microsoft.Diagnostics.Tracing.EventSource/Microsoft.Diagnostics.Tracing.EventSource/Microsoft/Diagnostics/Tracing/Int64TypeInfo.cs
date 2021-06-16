using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004B RID: 75
	internal sealed class Int64TypeInfo : TraceLoggingTypeInfo<long>
	{
		// Token: 0x06000222 RID: 546 RVA: 0x0000DEBF File Offset: 0x0000C0BF
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format64(format, TraceLoggingDataType.Int64));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		public override void WriteData(TraceLoggingDataCollector collector, ref long value)
		{
			collector.AddScalar(value);
		}
	}
}
