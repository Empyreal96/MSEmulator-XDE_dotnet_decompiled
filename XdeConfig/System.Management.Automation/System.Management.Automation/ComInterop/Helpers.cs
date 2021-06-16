using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A85 RID: 2693
	internal static class Helpers
	{
		// Token: 0x06006B00 RID: 27392 RVA: 0x00218B95 File Offset: 0x00216D95
		internal static Expression Convert(Expression expression, Type type)
		{
			if (expression.Type == type)
			{
				return expression;
			}
			return Expression.Convert(expression, type);
		}
	}
}
