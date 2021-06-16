using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000045 RID: 69
	internal sealed class ByteTypeInfo : TraceLoggingTypeInfo<byte>
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000DDF3 File Offset: 0x0000BFF3
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format8(format, TraceLoggingDataType.UInt8));
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000DE03 File Offset: 0x0000C003
		public override void WriteData(TraceLoggingDataCollector collector, ref byte value)
		{
			collector.AddScalar(value);
		}
	}
}
