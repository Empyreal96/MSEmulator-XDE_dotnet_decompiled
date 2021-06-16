using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000048 RID: 72
	internal sealed class UInt16TypeInfo : TraceLoggingTypeInfo<ushort>
	{
		// Token: 0x06000219 RID: 537 RVA: 0x0000DE59 File Offset: 0x0000C059
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format16(format, TraceLoggingDataType.UInt16));
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000DE69 File Offset: 0x0000C069
		public override void WriteData(TraceLoggingDataCollector collector, ref ushort value)
		{
			collector.AddScalar(value);
		}
	}
}
