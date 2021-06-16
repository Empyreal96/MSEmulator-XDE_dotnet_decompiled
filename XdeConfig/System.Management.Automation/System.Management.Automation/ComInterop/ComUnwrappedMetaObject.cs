using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A5E RID: 2654
	internal sealed class ComUnwrappedMetaObject : DynamicMetaObject
	{
		// Token: 0x060069EF RID: 27119 RVA: 0x00215089 File Offset: 0x00213289
		internal ComUnwrappedMetaObject(Expression expression, BindingRestrictions restrictions, object value) : base(expression, restrictions, value)
		{
		}
	}
}
