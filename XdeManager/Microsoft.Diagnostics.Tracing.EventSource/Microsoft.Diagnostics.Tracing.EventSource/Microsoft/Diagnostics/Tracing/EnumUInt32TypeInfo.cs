using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000065 RID: 101
	internal sealed class EnumUInt32TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000E30D File Offset: 0x0000C50D
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format32(format, TraceLoggingDataType.UInt32));
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E31D File Offset: 0x0000C51D
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<uint>.Cast<EnumType>(value));
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000E330 File Offset: 0x0000C530
		public override object GetData(object value)
		{
			return value;
		}
	}
}
