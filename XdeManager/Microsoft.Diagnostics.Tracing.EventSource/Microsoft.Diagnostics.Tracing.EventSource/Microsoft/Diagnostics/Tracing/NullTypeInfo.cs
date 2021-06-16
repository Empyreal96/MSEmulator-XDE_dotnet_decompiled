using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000043 RID: 67
	internal sealed class NullTypeInfo<DataType> : TraceLoggingTypeInfo<DataType>
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.AddGroup(name);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		public override void WriteData(TraceLoggingDataCollector collector, ref DataType value)
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000DDC2 File Offset: 0x0000BFC2
		public override object GetData(object value)
		{
			return null;
		}
	}
}
