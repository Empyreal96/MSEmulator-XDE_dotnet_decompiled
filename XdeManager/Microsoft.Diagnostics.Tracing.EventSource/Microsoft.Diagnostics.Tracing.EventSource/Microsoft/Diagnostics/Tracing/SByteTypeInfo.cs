using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000046 RID: 70
	internal sealed class SByteTypeInfo : TraceLoggingTypeInfo<sbyte>
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000DE15 File Offset: 0x0000C015
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format8(format, TraceLoggingDataType.Int8));
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DE25 File Offset: 0x0000C025
		public override void WriteData(TraceLoggingDataCollector collector, ref sbyte value)
		{
			collector.AddScalar(value);
		}
	}
}
