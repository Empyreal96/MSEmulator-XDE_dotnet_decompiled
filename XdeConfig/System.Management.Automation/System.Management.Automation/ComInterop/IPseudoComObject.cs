using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A7E RID: 2686
	internal interface IPseudoComObject
	{
		// Token: 0x06006ACB RID: 27339
		DynamicMetaObject GetMetaObject(Expression expression);
	}
}
