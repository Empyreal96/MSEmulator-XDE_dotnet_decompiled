using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000054 RID: 84
	internal sealed class SByteArrayTypeInfo : TraceLoggingTypeInfo<sbyte[]>
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000E07F File Offset: 0x0000C27F
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format8(format, TraceLoggingDataType.Int8));
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E08F File Offset: 0x0000C28F
		public override void WriteData(TraceLoggingDataCollector collector, ref sbyte[] value)
		{
			collector.AddArray(value);
		}
	}
}
