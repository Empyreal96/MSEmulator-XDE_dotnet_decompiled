using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000055 RID: 85
	internal sealed class Int16ArrayTypeInfo : TraceLoggingTypeInfo<short[]>
	{
		// Token: 0x06000240 RID: 576 RVA: 0x0000E0A1 File Offset: 0x0000C2A1
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format16(format, TraceLoggingDataType.Int16));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E0B1 File Offset: 0x0000C2B1
		public override void WriteData(TraceLoggingDataCollector collector, ref short[] value)
		{
			collector.AddArray(value);
		}
	}
}
