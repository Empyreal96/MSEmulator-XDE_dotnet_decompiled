using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000063 RID: 99
	internal sealed class EnumUInt16TypeInfo<EnumType> : TraceLoggingTypeInfo<EnumType>
	{
		// Token: 0x0600026D RID: 621 RVA: 0x0000E2B1 File Offset: 0x0000C4B1
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.Format16(format, TraceLoggingDataType.UInt16));
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E2C1 File Offset: 0x0000C4C1
		public override void WriteData(TraceLoggingDataCollector collector, ref EnumType value)
		{
			collector.AddScalar(EnumHelper<ushort>.Cast<EnumType>(value));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
		public override object GetData(object value)
		{
			return value;
		}
	}
}
