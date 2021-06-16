using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200003C RID: 60
	internal abstract class PropertyAccessor<ContainerType>
	{
		// Token: 0x060001F3 RID: 499
		public abstract void Write(TraceLoggingDataCollector collector, ref ContainerType value);

		// Token: 0x060001F4 RID: 500
		public abstract object GetData(ContainerType value);

		// Token: 0x060001F5 RID: 501 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		public static PropertyAccessor<ContainerType> Create(PropertyAnalysis property)
		{
			Type returnType = property.getterInfo.ReturnType;
			if (!Statics.IsValueType(typeof(ContainerType)))
			{
				if (returnType == typeof(int))
				{
					return new ClassPropertyWriter<ContainerType, int>(property);
				}
				if (returnType == typeof(long))
				{
					return new ClassPropertyWriter<ContainerType, long>(property);
				}
				if (returnType == typeof(string))
				{
					return new ClassPropertyWriter<ContainerType, string>(property);
				}
			}
			return new NonGenericProperytWriter<ContainerType>(property);
		}
	}
}
