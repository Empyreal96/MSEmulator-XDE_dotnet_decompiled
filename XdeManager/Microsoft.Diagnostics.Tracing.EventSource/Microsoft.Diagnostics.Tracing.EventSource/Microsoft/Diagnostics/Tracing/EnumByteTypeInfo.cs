using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000060 RID: 96
	internal sealed class EnumByteTypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000E227 File Offset: 0x0000C427
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format8(format, TraceLoggingDataType.UInt8));
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000E237 File Offset: 0x0000C437
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<byte>.Cast<EnumType>(value));
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000E24A File Offset: 0x0000C44A
		public override object GetData(object value)
		{
			return value;
		}
	}
}
