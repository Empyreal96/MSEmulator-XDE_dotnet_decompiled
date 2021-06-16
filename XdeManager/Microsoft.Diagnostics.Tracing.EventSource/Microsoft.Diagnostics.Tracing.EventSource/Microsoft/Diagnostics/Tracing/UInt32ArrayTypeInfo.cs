using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000058 RID: 88
	internal sealed class UInt32ArrayTypeInfo : TraceLoggingTypeInfo<uint[]>
	{
		// Token: 0x06000249 RID: 585 RVA: 0x0000E107 File Offset: 0x0000C307
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format32(format, TraceLoggingDataType.UInt32));
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E117 File Offset: 0x0000C317
		public override void WriteData(TraceLoggingDataCollector collector, ref uint[] value)
		{
			collector.AddArray(value);
		}
	}
}
