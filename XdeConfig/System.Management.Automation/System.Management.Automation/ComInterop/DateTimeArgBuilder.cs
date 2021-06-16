using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7C RID: 2684
	internal sealed class DateTimeArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006AC4 RID: 27332 RVA: 0x00218175 File Offset: 0x00216375
		internal DateTimeArgBuilder(Type parameterType) : base(parameterType)
		{
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x0021817E File Offset: 0x0021637E
		internal override Expression MarshalToRef(Expression parameter)
		{
			return Expression.Call(this.Marshal(parameter), typeof(DateTime).GetMethod("ToOADate"));
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x002181A0 File Offset: 0x002163A0
		internal override Expression UnmarshalFromRef(Expression value)
		{
			return base.UnmarshalFromRef(Expression.Call(typeof(DateTime).GetMethod("FromOADate"), value));
		}
	}
}
