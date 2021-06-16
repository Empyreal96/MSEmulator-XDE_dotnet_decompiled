using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A79 RID: 2681
	internal class ConvertArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006ABA RID: 27322 RVA: 0x00218069 File Offset: 0x00216269
		internal ConvertArgBuilder(Type parameterType, Type marshalType) : base(parameterType)
		{
			this._marshalType = marshalType;
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x00218079 File Offset: 0x00216279
		internal override Expression Marshal(Expression parameter)
		{
			parameter = base.Marshal(parameter);
			return Expression.Convert(parameter, this._marshalType);
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x00218090 File Offset: 0x00216290
		internal override Expression UnmarshalFromRef(Expression newValue)
		{
			return base.UnmarshalFromRef(Expression.Convert(newValue, base.ParameterType));
		}

		// Token: 0x04003324 RID: 13092
		private readonly Type _marshalType;
	}
}
