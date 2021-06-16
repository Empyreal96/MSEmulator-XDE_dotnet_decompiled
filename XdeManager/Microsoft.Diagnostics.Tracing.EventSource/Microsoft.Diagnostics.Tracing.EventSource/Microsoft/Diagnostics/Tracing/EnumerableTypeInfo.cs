using System;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000021 RID: 33
	internal sealed class EnumerableTypeInfo<IterableType, ElementType> : TraceLoggingTypeInfo<IterableType> where IterableType : IEnumerable<ElementType>
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00009F80 File Offset: 0x00008180
		public EnumerableTypeInfo(TraceLoggingTypeInfo<ElementType> elementInfo)
		{
			this.elementInfo = elementInfo;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009F8F File Offset: 0x0000818F
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			collector.BeginBufferedArray();
			this.elementInfo.WriteMetadata(collector, name, format);
			collector.EndBufferedArray();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009FAC File Offset: 0x000081AC
		public override void WriteData(TraceLoggingDataCollector collector, ref IterableType value)
		{
			int bookmark = collector.BeginBufferedArray();
			int num = 0;
			checked
			{
				if (value != null)
				{
					foreach (ElementType elementType in value)
					{
						ElementType elementType2 = elementType;
						this.elementInfo.WriteData(collector, ref elementType2);
						num++;
					}
				}
				collector.EndBufferedArray(bookmark, num);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000A02C File Offset: 0x0000822C
		public override object GetData(object value)
		{
			IterableType iterableType = (IterableType)((object)value);
			List<object> list = new List<object>();
			foreach (ElementType elementType in iterableType)
			{
				list.Add(this.elementInfo.GetData(elementType));
			}
			return list.ToArray();
		}

		// Token: 0x0400009F RID: 159
		private readonly TraceLoggingTypeInfo<ElementType> elementInfo;
	}
}
