using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006A RID: 106
	internal sealed class GuidArrayTypeInfo : TraceLoggingTypeInfo<Guid[]>
	{
		// Token: 0x06000288 RID: 648 RVA: 0x0000E402 File Offset: 0x0000C602
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddArray(name, Statics.MakeDataType(TraceLoggingDataType.Guid, format));
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E413 File Offset: 0x0000C613
		public override void WriteData(TraceLoggingDataCollector collector, ref Guid[] value)
		{
			collector.AddArray(value);
		}
	}
}
