using System;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000070 RID: 112
	internal sealed class NullableTypeInfo<T> : TraceLoggingTypeInfo<T?> where T : struct
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000E64B File Offset: 0x0000C84B
		public NullableTypeInfo(List<Type> recursionCheck)
		{
			this.valueInfo = TraceLoggingTypeInfo<T>.GetInstance(recursionCheck);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000E660 File Offset: 0x0000C860
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			TraceLoggingMetadataCollector traceLoggingMetadataCollector = collector.AddGroup(name);
			traceLoggingMetadataCollector.AddScalar("HasValue", TraceLoggingDataType.Boolean8);
			this.valueInfo.WriteMetadata(traceLoggingMetadataCollector, "Value", format);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000E698 File Offset: 0x0000C898
		public override void WriteData(TraceLoggingDataCollector collector, ref T? value)
		{
			bool flag = value != null;
			collector.AddScalar(flag);
			T t = flag ? value.Value : default(T);
			this.valueInfo.WriteData(collector, ref t);
		}

		// Token: 0x04000138 RID: 312
		private readonly TraceLoggingTypeInfo<T> valueInfo;
	}
}
