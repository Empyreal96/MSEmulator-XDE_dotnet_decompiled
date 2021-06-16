using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000066 RID: 102
	internal sealed class EnumInt64TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000279 RID: 633 RVA: 0x0000E33B File Offset: 0x0000C53B
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format64(format, TraceLoggingDataType.Int64));
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000E34C File Offset: 0x0000C54C
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<long>.Cast<EnumType>(value));
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000E35F File Offset: 0x0000C55F
		public override object GetData(object value)
		{
			return value;
		}
	}
}
