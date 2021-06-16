using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200005B RID: 91
	internal sealed class IntPtrArrayTypeInfo : TraceLoggingTypeInfo<IntPtr[]>
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000E16F File Offset: 0x0000C36F
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.FormatPtr(format, Statics.IntPtrType));
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000E183 File Offset: 0x0000C383
		public override void WriteData(TraceLoggingDataCollector collector, ref IntPtr[] value)
		{
			collector.AddArray(value);
		}
	}
}
