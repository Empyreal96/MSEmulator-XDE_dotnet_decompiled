using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A4F RID: 2639
	internal sealed class BoolArgBuilder : SimpleArgBuilder
	{
		// Token: 0x06006999 RID: 27033 RVA: 0x00213FEE File Offset: 0x002121EE
		internal BoolArgBuilder(Type parameterType) : base(parameterType)
		{
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x00213FF7 File Offset: 0x002121F7
		internal override Expression MarshalToRef(Expression parameter)
		{
			return Expression.Condition(this.Marshal(parameter), Expression.Constant(-1), Expression.Constant(0));
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x0021401B File Offset: 0x0021221B
		internal override Expression UnmarshalFromRef(Expression value)
		{
			return base.UnmarshalFromRef(Expression.NotEqual(value, Expression.Constant(0)));
		}
	}
}
