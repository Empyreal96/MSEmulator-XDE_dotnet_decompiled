using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000064 RID: 100
	internal sealed class EnumInt32TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x06000271 RID: 625 RVA: 0x0000E2DF File Offset: 0x0000C4DF
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format32(format, TraceLoggingDataType.Int32));
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000E2EF File Offset: 0x0000C4EF
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<int>.Cast<EnumType>(value));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E302 File Offset: 0x0000C502
		public override object GetData(object value)
		{
			return value;
		}
	}
}
