using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005A RID: 90
	internal sealed class UInt64ArrayTypeInfo : TraceLoggingTypeInfo<ulong[]>
	{
		// Token: 0x0600024F RID: 591 RVA: 0x0000E14C File Offset: 0x0000C34C
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format64(format, TraceLoggingDataType.UInt64));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E15D File Offset: 0x0000C35D
		public override void WriteData(TraceLoggingDataCollector collector, ref ulong[] value)
		{
			collector.AddArray(value);
		}
	}
}
