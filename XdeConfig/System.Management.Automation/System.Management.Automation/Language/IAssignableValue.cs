using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000535 RID: 1333
	internal interface IAssignableValue
	{
		// Token: 0x0600376F RID: 14191
		Expression GetValue(Compiler compiler, List<Expression> exprs, List<ParameterExpression> temps);

		// Token: 0x06003770 RID: 14192
		Expression SetValue(Compiler compiler, Expression rhs);
	}
}
