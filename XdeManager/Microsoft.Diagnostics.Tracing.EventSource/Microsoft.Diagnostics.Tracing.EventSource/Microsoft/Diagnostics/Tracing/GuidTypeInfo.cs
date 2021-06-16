using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000069 RID: 105
	internal sealed class GuidTypeInfo : TraceLoggingTypeInfo<Guid>
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000E3DB File Offset: 0x0000C5DB
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddScalar(name, Statics.MakeDataType(TraceLoggingDataType.Guid, format));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public override void WriteData(TraceLoggingDataCollector collector, ref Guid value)
		{
			collector.AddScalar(value);
		}
	}
}
