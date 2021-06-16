using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000018 RID: 24
	internal sealed class ArrayTypeInfo<ElementType> : TraceLoggingTypeInfo<ElementType[]>
	{
		// Token: 0x060000FE RID: 254 RVA: 0x0000964B File Offset: 0x0000784B
		public ArrayTypeInfo(TraceLoggingTypeInfo<ElementType> elementInfo)
		{
			this.elementInfo = elementInfo;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000965A File Offset: 0x0000785A
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.BeginBufferedArray();
			this.elementInfo.WriteMetadata(collector, name, format);
			collector.EndBufferedArray();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009678 File Offset: 0x00007878
		public override void WriteData(TraceLoggingDataCollector collector, ref ElementType[] value)
		{
			int bookmark = collector.BeginBufferedArray();
			int count = 0;
			checked
			{
				if (value != null)
				{
					count = value.Length;
					for (int i = 0; i < value.Length; i++)
					{
						this.elementInfo.WriteData(collector, ref value[i]);
					}
				}
				collector.EndBufferedArray(bookmark, count);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000096C4 File Offset: 0x000078C4
		public override object GetData(object value)
		{
			ElementType[] array = (ElementType[])value;
			object[] array2 = new object[array.Length];
			checked
			{
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = this.elementInfo.GetData(array[i]);
				}
				return array2;
			}
		}

		// Token: 0x04000087 RID: 135
		private readonly TraceLoggingTypeInfo<ElementType> elementInfo;
	}
}
