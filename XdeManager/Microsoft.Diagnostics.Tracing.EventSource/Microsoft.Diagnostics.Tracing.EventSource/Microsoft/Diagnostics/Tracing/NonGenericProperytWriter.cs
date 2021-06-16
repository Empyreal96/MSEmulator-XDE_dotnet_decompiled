using System;
using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200003D RID: 61
	internal class NonGenericProperytWriter<ContainerType> : PropertyAccessor<ContainerType>
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x0000DBAF File Offset: 0x0000BDAF
		public NonGenericProperytWriter(PropertyAnalysis property)
		{
			this.getterInfo = property.getterInfo;
			this.typeInfo = property.typeInfo;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		public override void Write(TraceLoggingDataCollector collector, ref ContainerType container)
		{
			object value = (container == null) ? null : this.getterInfo.Invoke(container, null);
			this.typeInfo.WriteObjectData(collector, value);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000DC12 File Offset: 0x0000BE12
		public override object GetData(ContainerType container)
		{
			if (container != null)
			{
				return this.getterInfo.Invoke(container, null);
			}
			return null;
		}

		// Token: 0x0400012C RID: 300
		private readonly TraceLoggingTypeInfo typeInfo;

		// Token: 0x0400012D RID: 301
		private readonly MethodInfo getterInfo;
	}
}
