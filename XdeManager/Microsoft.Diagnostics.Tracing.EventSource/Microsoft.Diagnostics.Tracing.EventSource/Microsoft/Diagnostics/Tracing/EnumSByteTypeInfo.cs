using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000061 RID: 97
	internal sealed class EnumSByteTypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000E255 File Offset: 0x0000C455
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format8(format, TraceLoggingDataType.Int8));
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000E265 File Offset: 0x0000C465
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<sbyte>.Cast<EnumType>(value));
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000E278 File Offset: 0x0000C478
		public override object GetData(object value)
		{
			return value;
		}
	}
}
