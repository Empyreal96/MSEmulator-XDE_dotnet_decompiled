using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000050 RID: 80
	internal sealed class SingleTypeInfo : TraceLoggingTypeInfo<float>
	{
		// Token: 0x06000231 RID: 561 RVA: 0x0000DF7C File Offset: 0x0000C17C
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format32(format, TraceLoggingDataType.Float));
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DF8D File Offset: 0x0000C18D
		public override void WriteData(TraceLoggingDataCollector collector, ref float value)
		{
			collector.AddScalar(value);
		}
	}
}
