using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000056 RID: 86
	internal sealed class UInt16ArrayTypeInfo : TraceLoggingTypeInfo<ushort[]>
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000E0C3 File Offset: 0x0000C2C3
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.Format16(format, TraceLoggingDataType.UInt16));
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E0D3 File Offset: 0x0000C2D3
		public override void WriteData(TraceLoggingDataCollector collector, ref ushort[] value)
		{
			collector.AddArray(value);
		}
	}
}
