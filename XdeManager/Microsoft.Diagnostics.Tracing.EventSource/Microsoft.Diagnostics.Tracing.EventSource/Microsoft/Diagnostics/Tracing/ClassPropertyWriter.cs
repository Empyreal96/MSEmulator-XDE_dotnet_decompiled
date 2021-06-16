using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000040 RID: 64
	internal class ClassPropertyWriter<ContainerType, ValueType> : PropertyAccessor<ContainerType>
	{
		// Token: 0x06000201 RID: 513 RVA: 0x0000DCDE File Offset: 0x0000BEDE
		public ClassPropertyWriter(PropertyAnalysis property)
		{
			this.valueTypeInfo = (TraceLoggingTypeInfo<ValueType>)property.typeInfo;
			this.getter = (ClassPropertyWriter<ContainerType, ValueType>.Getter)Statics.CreateDelegate(typeof(ClassPropertyWriter<ContainerType, ValueType>.Getter), property.getterInfo);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000DD18 File Offset: 0x0000BF18
		public override void Write(TraceLoggingDataCollector collector, ref ContainerType container)
		{
			ValueType valueType = (container == null) ? default(ValueType) : this.getter(container);
			this.valueTypeInfo.WriteData(collector, ref valueType);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DD60 File Offset: 0x0000BF60
		public override object GetData(ContainerType container)
		{
			return (container == null) ? default(ValueType) : this.getter(container);
		}

		// Token: 0x04000130 RID: 304
		private readonly TraceLoggingTypeInfo<ValueType> valueTypeInfo;

		// Token: 0x04000131 RID: 305
		private readonly ClassPropertyWriter<ContainerType, ValueType>.Getter getter;

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x06000205 RID: 517
		private delegate ValueType Getter(ContainerType container);
	}
}
