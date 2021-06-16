using System;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200003A RID: 58
	internal sealed class InvokeTypeInfo<ContainerType> : TraceLoggingTypeInfo<ContainerType>
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0000D830 File Offset: 0x0000BA30
		public InvokeTypeInfo(TypeAnalysis typeAnalysis) : base(typeAnalysis.name, typeAnalysis.level, typeAnalysis.opcode, typeAnalysis.keywords, typeAnalysis.tags)
		{
			checked
			{
				if (typeAnalysis.properties.Length != 0)
				{
					this.properties = typeAnalysis.properties;
					this.accessors = new PropertyAccessor<ContainerType>[this.properties.Length];
					for (int i = 0; i < this.accessors.Length; i++)
					{
						this.accessors[i] = PropertyAccessor<ContainerType>.Create(this.properties[i]);
					}
				}
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			TraceLoggingMetadataCollector traceLoggingMetadataCollector = collector.AddGroup(name);
			if (this.properties != null)
			{
				foreach (PropertyAnalysis propertyAnalysis in this.properties)
				{
					EventFieldFormat format2 = EventFieldFormat.Default;
					EventFieldAttribute fieldAttribute = propertyAnalysis.fieldAttribute;
					if (fieldAttribute != null)
					{
						traceLoggingMetadataCollector.Tags = fieldAttribute.Tags;
						format2 = fieldAttribute.Format;
					}
					propertyAnalysis.typeInfo.WriteMetadata(traceLoggingMetadataCollector, propertyAnalysis.name, format2);
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D924 File Offset: 0x0000BB24
		public override void WriteData(TraceLoggingDataCollector collector, ref ContainerType value)
		{
			if (this.accessors != null)
			{
				foreach (PropertyAccessor<ContainerType> propertyAccessor in this.accessors)
				{
					propertyAccessor.Write(collector, ref value);
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D95C File Offset: 0x0000BB5C
		public override object GetData(object value)
		{
			checked
			{
				if (this.properties != null)
				{
					List<string> list = new List<string>();
					List<object> list2 = new List<object>();
					for (int i = 0; i < this.properties.Length; i++)
					{
						object data = this.accessors[i].GetData((ContainerType)((object)value));
						list.Add(this.properties[i].name);
						list2.Add(this.properties[i].typeInfo.GetData(data));
					}
					return new EventPayload(list, list2);
				}
				return null;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public override void WriteObjectData(TraceLoggingDataCollector collector, object valueObj)
		{
			if (this.accessors != null)
			{
				ContainerType containerType = (valueObj == null) ? default(ContainerType) : ((ContainerType)((object)valueObj));
				this.WriteData(collector, ref containerType);
			}
		}

		// Token: 0x04000125 RID: 293
		private readonly PropertyAnalysis[] properties;

		// Token: 0x04000126 RID: 294
		private readonly PropertyAccessor<ContainerType>[] accessors;
	}
}
