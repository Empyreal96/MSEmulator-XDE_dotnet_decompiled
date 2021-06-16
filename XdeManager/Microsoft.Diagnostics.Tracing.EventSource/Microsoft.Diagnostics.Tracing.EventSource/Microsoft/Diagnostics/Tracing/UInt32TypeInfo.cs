using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004A RID: 74
	internal sealed class UInt32TypeInfo : TraceLoggingTypeInfo<uint>
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000DE9D File Offset: 0x0000C09D
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format32(format, TraceLoggingDataType.UInt32));
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000DEAD File Offset: 0x0000C0AD
		public override void WriteData(TraceLoggingDataCollector collector, ref uint value)
		{
			collector.AddScalar(value);
		}
	}
}
