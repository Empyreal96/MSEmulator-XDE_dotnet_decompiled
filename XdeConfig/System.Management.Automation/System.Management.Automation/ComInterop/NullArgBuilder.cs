using System;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A88 RID: 2696
	internal sealed class NullArgBuilder : ArgBuilder
	{
		// Token: 0x06006B28 RID: 27432 RVA: 0x00219DCA File Offset: 0x00217FCA
		internal NullArgBuilder()
		{
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x00219DD2 File Offset: 0x00217FD2
		internal override Expression Marshal(Expression parameter)
		{
			return Expression.Constant(null);
		}
	}
}
