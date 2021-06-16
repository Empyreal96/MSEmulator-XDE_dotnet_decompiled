using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000047 RID: 71
	internal sealed class Int16TypeInfo : TraceLoggingTypeInfo<short>
	{
		// Token: 0x06000216 RID: 534 RVA: 0x0000DE37 File Offset: 0x0000C037
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format16(format, TraceLoggingDataType.Int16));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000DE47 File Offset: 0x0000C047
		public override void WriteData(TraceLoggingDataCollector collector, ref short value)
		{
			collector.AddScalar(value);
		}
	}
}
