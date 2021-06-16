using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000053 RID: 83
	internal sealed class ByteArrayTypeInfo : TraceLoggingTypeInfo<byte[]>
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			switch (format)
			{
			case EventFieldFormat.String:
				collector.AddBinary(name, TraceLoggingDataType.CountedMbcsString);
				return;
			case EventFieldFormat.Boolean:
				collector.AddArray(name, TraceLoggingDataType.Boolean8);
				return;
			case EventFieldFormat.Hexadecimal:
				collector.AddArray(name, TraceLoggingDataType.HexInt8);
				return;
			default:
				switch (format)
				{
				case EventFieldFormat.Xml:
					collector.AddBinary(name, TraceLoggingDataType.CountedMbcsXml);
					return;
				case EventFieldFormat.Json:
					collector.AddBinary(name, TraceLoggingDataType.CountedMbcsJson);
					return;
				default:
					collector.AddBinary(name, Statics.MakeDataType(TraceLoggingDataType.Binary, format));
					return;
				}
				break;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000E06D File Offset: 0x0000C26D
		public override void WriteData(TraceLoggingDataCollector collector, ref byte[] value)
		{
			collector.AddBinary(value);
		}
	}
}
