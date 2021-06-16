using System;
using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000042 RID: 66
	internal sealed class PropertyAnalysis
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000DD91 File Offset: 0x0000BF91
		public PropertyAnalysis(string name, MethodInfo getterInfo, TraceLoggingTypeInfo typeInfo, EventFieldAttribute fieldAttribute)
		{
			this.name = name;
			this.getterInfo = getterInfo;
			this.typeInfo = typeInfo;
			this.fieldAttribute = fieldAttribute;
		}

		// Token: 0x04000132 RID: 306
		internal readonly string name;

		// Token: 0x04000133 RID: 307
		internal readonly MethodInfo getterInfo;

		// Token: 0x04000134 RID: 308
		internal readonly TraceLoggingTypeInfo typeInfo;

		// Token: 0x04000135 RID: 309
		internal readonly EventFieldAttribute fieldAttribute;
	}
}
