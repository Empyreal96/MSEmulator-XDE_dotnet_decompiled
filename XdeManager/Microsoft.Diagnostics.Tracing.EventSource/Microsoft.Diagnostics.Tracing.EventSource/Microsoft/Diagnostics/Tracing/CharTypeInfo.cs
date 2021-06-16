using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000051 RID: 81
	internal sealed class CharTypeInfo : TraceLoggingTypeInfo<char>
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000DF9F File Offset: 0x0000C19F
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format16(format, TraceLoggingDataType.Char16));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000DFB3 File Offset: 0x0000C1B3
		public override void WriteData(TraceLoggingDataCollector collector, ref char value)
		{
			collector.AddScalar(value);
		}
	}
}
