using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000044 RID: 68
	internal sealed class BooleanTypeInfo : TraceLoggingTypeInfo<bool>
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000DDCD File Offset: 0x0000BFCD
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format8(format, TraceLoggingDataType.Boolean8));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000DDE1 File Offset: 0x0000BFE1
		public override void WriteData(TraceLoggingDataCollector collector, ref bool value)
		{
			collector.AddScalar(value);
		}
	}
}
