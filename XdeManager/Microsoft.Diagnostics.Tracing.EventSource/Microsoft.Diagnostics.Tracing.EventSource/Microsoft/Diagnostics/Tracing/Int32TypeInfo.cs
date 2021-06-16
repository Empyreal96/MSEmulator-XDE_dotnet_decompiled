using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000049 RID: 73
	internal sealed class Int32TypeInfo : TraceLoggingTypeInfo<int>
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000DE7B File Offset: 0x0000C07B
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format32(format, TraceLoggingDataType.Int32));
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000DE8B File Offset: 0x0000C08B
		public override void WriteData(TraceLoggingDataCollector collector, ref int value)
		{
			collector.AddScalar(value);
		}
	}
}
