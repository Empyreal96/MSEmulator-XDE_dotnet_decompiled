using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000067 RID: 103
	internal sealed class EnumUInt64TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000E36A File Offset: 0x0000C56A
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format64(format, TraceLoggingDataType.UInt64));
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000E37B File Offset: 0x0000C57B
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<ulong>.Cast<EnumType>(value));
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E38E File Offset: 0x0000C58E
		public override object GetData(object value)
		{
			return value;
		}
	}
}
