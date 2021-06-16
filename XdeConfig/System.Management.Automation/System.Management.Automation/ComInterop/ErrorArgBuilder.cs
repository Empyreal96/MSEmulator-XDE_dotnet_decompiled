using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A81 RID: 2689
	internal class ErrorArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006ADC RID: 27356 RVA: 0x002186E3 File Offset: 0x002168E3
		internal ErrorArgBuilder(Type parameterType) : base(parameterType)
		{
		}

		// Token: 0x06006ADD RID: 27357 RVA: 0x002186EC File Offset: 0x002168EC
		internal override Expression Marshal(Expression parameter)
		{
			return Expression.Property(Helpers.Convert(base.Marshal(parameter), typeof(ErrorWrapper)), "ErrorCode");
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x00218710 File Offset: 0x00216910
		internal override Expression UnmarshalFromRef(Expression value)
		{
			return base.UnmarshalFromRef(Expression.New(typeof(ErrorWrapper).GetConstructor(new Type[]
			{
				typeof(int)
			}), new Expression[]
			{
				value
			}));
		}
	}
}
