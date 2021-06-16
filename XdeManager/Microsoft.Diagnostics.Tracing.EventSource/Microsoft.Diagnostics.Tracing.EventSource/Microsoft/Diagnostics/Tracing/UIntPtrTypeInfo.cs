using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200004E RID: 78
	internal sealed class UIntPtrTypeInfo : TraceLoggingTypeInfo<UIntPtr>
	{
		// Token: 0x0600022B RID: 555 RVA: 0x0000DF2F File Offset: 0x0000C12F
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.FormatPtr(format, Statics.UIntPtrType));
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DF43 File Offset: 0x0000C143
		public override void WriteData(TraceLoggingDataCollector collector, ref UIntPtr value)
		{
			collector.AddScalar(value);
		}
	}
}
