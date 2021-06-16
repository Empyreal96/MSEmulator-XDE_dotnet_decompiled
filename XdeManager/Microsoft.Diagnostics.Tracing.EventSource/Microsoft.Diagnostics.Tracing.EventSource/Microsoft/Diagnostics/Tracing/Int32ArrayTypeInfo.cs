using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000057 RID: 87
	internal sealed class Int32ArrayTypeInfo : TraceLoggingTypeInfo<int[]>
	{
		// Token: 0x06000246 RID: 582 RVA: 0x0000E0E5 File Offset: 0x0000C2E5
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format32(format, TraceLoggingDataType.Int32));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E0F5 File Offset: 0x0000C2F5
		public override void WriteData(TraceLoggingDataCollector collector, ref int[] value)
		{
			collector.AddArray(value);
		}
	}
}
