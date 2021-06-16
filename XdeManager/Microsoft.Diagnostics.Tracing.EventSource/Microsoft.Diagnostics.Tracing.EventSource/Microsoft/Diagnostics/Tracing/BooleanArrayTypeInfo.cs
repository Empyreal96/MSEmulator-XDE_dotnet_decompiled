using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000052 RID: 82
	internal sealed class BooleanArrayTypeInfo : TraceLoggingTypeInfo<bool[]>
	{
		// Token: 0x06000237 RID: 567 RVA: 0x0000DFC5 File Offset: 0x0000C1C5
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format8(format, TraceLoggingDataType.Boolean8));
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000DFD9 File Offset: 0x0000C1D9
		public override void WriteData(TraceLoggingDataCollector collector, ref bool[] value)
		{
			collector.AddArray(value);
		}
	}
}
