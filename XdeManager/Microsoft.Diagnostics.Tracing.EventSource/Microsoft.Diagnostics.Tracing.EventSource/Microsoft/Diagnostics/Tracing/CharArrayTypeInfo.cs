using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005D RID: 93
	internal sealed class CharArrayTypeInfo : TraceLoggingTypeInfo<char[]>
	{
		// Token: 0x06000258 RID: 600 RVA: 0x0000E1BB File Offset: 0x0000C3BB
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format16(format, TraceLoggingDataType.Char16));
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000E1CF File Offset: 0x0000C3CF
		public override void WriteData(TraceLoggingDataCollector collector, ref char[] value)
		{
			collector.AddArray(value);
		}
	}
}
