using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000068 RID: 104
	internal sealed class StringTypeInfo : TraceLoggingTypeInfo<string>
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000E399 File Offset: 0x0000C599
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddBinary(name, Statics.MakeDataType(TraceLoggingDataType.CountedUtf16String, format));
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E3AA File Offset: 0x0000C5AA
		public override void WriteData(TraceLoggingDataCollector collector, ref string value)
		{
			collector.AddBinary(value);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public override object GetData(object value)
		{
			object obj = base.GetData(value);
			if (obj == null)
			{
				obj = "";
			}
			return obj;
		}
	}
}
