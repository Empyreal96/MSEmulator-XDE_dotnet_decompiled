using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006D RID: 109
	internal sealed class TimeSpanTypeInfo : TraceLoggingTypeInfo<TimeSpan>
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000E501 File Offset: 0x0000C701
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.MakeDataType(TraceLoggingDataType.Int64, format));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E512 File Offset: 0x0000C712
		public override void WriteData(TraceLoggingDataCollector collector, ref TimeSpan value)
		{
			collector.AddScalar(value.Ticks);
		}
	}
}
