using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004C RID: 76
	internal sealed class UInt64TypeInfo : TraceLoggingTypeInfo<ulong>
	{
		// Token: 0x06000225 RID: 549 RVA: 0x0000DEE2 File Offset: 0x0000C0E2
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format64(format, TraceLoggingDataType.UInt64));
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000DEF3 File Offset: 0x0000C0F3
		public override void WriteData(TraceLoggingDataCollector collector, ref ulong value)
		{
			collector.AddScalar(value);
		}
	}
}
