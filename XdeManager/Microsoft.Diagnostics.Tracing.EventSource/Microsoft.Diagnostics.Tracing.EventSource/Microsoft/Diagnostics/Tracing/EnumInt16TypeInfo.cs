using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000062 RID: 98
	internal sealed class EnumInt16TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000269 RID: 617 RVA: 0x0000E283 File Offset: 0x0000C483
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format16(format, TraceLoggingDataType.Int16));
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000E293 File Offset: 0x0000C493
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<short>.Cast<EnumType>(value));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000E2A6 File Offset: 0x0000C4A6
		public override object GetData(object value)
		{
			return value;
		}
	}
}
