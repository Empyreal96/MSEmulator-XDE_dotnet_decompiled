using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7B RID: 2683
	internal sealed class CurrencyArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006AC0 RID: 27328 RVA: 0x002180C5 File Offset: 0x002162C5
		internal CurrencyArgBuilder(Type parameterType) : base(parameterType)
		{
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x002180CE File Offset: 0x002162CE
		internal override Expression Marshal(Expression parameter)
		{
			return Expression.Property(Helpers.Convert(base.Marshal(parameter), typeof(CurrencyWrapper)), "WrappedObject");
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x002180F0 File Offset: 0x002162F0
		internal override Expression MarshalToRef(Expression parameter)
		{
			return Expression.Call(typeof(decimal).GetMethod("ToOACurrency"), this.Marshal(parameter));
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x00218114 File Offset: 0x00216314
		internal override Expression UnmarshalFromRef(Expression value)
		{
			return base.UnmarshalFromRef(Expression.New(typeof(CurrencyWrapper).GetConstructor(new Type[]
			{
				typeof(decimal)
			}), new Expression[]
			{
				Expression.Call(typeof(decimal).GetMethod("FromOACurrency"), value)
			}));
		}
	}
}
