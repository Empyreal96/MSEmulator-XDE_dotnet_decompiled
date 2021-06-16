using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005C RID: 92
	internal sealed class UIntPtrArrayTypeInfo : TraceLoggingTypeInfo<UIntPtr[]>
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000E195 File Offset: 0x0000C395
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.FormatPtr(format, Statics.UIntPtrType));
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E1A9 File Offset: 0x0000C3A9
		public override void WriteData(TraceLoggingDataCollector collector, ref UIntPtr[] value)
		{
			collector.AddArray(value);
		}
	}
}
