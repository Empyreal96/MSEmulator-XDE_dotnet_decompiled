using System;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000104 RID: 260
	internal class ParameterInformation
	{
		// Token: 0x06000E56 RID: 3670 RVA: 0x0004E590 File Offset: 0x0004C790
		internal ParameterInformation(ParameterInfo parameter)
		{
			this.isOptional = parameter.IsOptional;
			this.defaultValue = parameter.DefaultValue;
			this.parameterType = parameter.ParameterType;
			if (this.parameterType.IsByRef)
			{
				this.isByRef = true;
				this.parameterType = this.parameterType.GetElementType();
				return;
			}
			this.isByRef = false;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0004E5F4 File Offset: 0x0004C7F4
		internal ParameterInformation(Type parameterType, bool isOptional, object defaultValue, bool isByRef)
		{
			this.parameterType = parameterType;
			this.isOptional = isOptional;
			this.defaultValue = defaultValue;
			this.isByRef = isByRef;
		}

		// Token: 0x04000655 RID: 1621
		internal Type parameterType;

		// Token: 0x04000656 RID: 1622
		internal object defaultValue;

		// Token: 0x04000657 RID: 1623
		internal bool isOptional;

		// Token: 0x04000658 RID: 1624
		internal bool isByRef;

		// Token: 0x04000659 RID: 1625
		internal bool isParamArray;
	}
}
