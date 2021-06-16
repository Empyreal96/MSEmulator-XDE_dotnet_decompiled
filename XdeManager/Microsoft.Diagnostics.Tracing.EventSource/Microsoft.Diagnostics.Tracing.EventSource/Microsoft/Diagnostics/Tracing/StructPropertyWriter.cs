using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200003E RID: 62
	internal class StructPropertyWriter<ContainerType, ValueType> : PropertyAccessor<ContainerType>
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public StructPropertyWriter(PropertyAnalysis property)
		{
			this.valueTypeInfo = (TraceLoggingTypeInfo<ValueType>)property.typeInfo;
			this.getter = (StructPropertyWriter<ContainerType, ValueType>.Getter)Statics.CreateDelegate(typeof(StructPropertyWriter<ContainerType, ValueType>.Getter), property.getterInfo);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		public override void Write(TraceLoggingDataCollector collector, ref ContainerType container)
		{
			ValueType valueType = (container == null) ? default(ValueType) : this.getter(ref container);
			this.valueTypeInfo.WriteData(collector, ref valueType);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		public override object GetData(ContainerType container)
		{
			return (container == null) ? default(ValueType) : this.getter(ref container);
		}

		// Token: 0x0400012E RID: 302
		private readonly TraceLoggingTypeInfo<ValueType> valueTypeInfo;

		// Token: 0x0400012F RID: 303
		private readonly StructPropertyWriter<ContainerType, ValueType>.Getter getter;

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x060001FE RID: 510
		private delegate ValueType Getter(ref ContainerType container);
	}
}
