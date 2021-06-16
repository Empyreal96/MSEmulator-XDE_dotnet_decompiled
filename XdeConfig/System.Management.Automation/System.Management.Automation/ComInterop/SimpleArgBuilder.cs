using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A4E RID: 2638
	internal class SimpleArgBuilder : ArgBuilder
	{
		// Token: 0x06006995 RID: 27029 RVA: 0x00213FC0 File Offset: 0x002121C0
		internal SimpleArgBuilder(Type parameterType)
		{
			this._parameterType = parameterType;
		}

		// Token: 0x17001D93 RID: 7571
		// (get) Token: 0x06006996 RID: 27030 RVA: 0x00213FCF File Offset: 0x002121CF
		internal Type ParameterType
		{
			get
			{
				return this._parameterType;
			}
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x00213FD7 File Offset: 0x002121D7
		internal override Expression Marshal(Expression parameter)
		{
			return Helpers.Convert(parameter, this._parameterType);
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x00213FE5 File Offset: 0x002121E5
		internal override Expression UnmarshalFromRef(Expression newValue)
		{
			return base.UnmarshalFromRef(newValue);
		}

		// Token: 0x04003295 RID: 12949
		private readonly Type _parameterType;
	}
}
