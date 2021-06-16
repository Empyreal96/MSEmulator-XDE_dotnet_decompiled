using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004D RID: 77
	internal sealed class IntPtrTypeInfo : TraceLoggingTypeInfo<IntPtr>
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000DF05 File Offset: 0x0000C105
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.FormatPtr(format, Statics.IntPtrType));
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DF19 File Offset: 0x0000C119
		public override void WriteData(TraceLoggingDataCollector collector, ref IntPtr value)
		{
			collector.AddScalar(value);
		}
	}
}
