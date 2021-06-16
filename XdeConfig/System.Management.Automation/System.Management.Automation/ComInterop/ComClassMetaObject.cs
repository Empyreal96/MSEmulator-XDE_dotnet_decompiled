using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A56 RID: 2646
	internal class ComClassMetaObject : DynamicMetaObject
	{
		// Token: 0x060069C3 RID: 27075 RVA: 0x0021483C File Offset: 0x00212A3C
		internal ComClassMetaObject(Expression expression, ComTypeClassDesc cls) : base(expression, BindingRestrictions.Empty, cls)
		{
		}

		// Token: 0x060069C4 RID: 27076 RVA: 0x0021484C File Offset: 0x00212A4C
		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			return new DynamicMetaObject(Expression.Call(Utils.Convert(base.Expression, typeof(ComTypeClassDesc)), typeof(ComTypeClassDesc).GetMethod("CreateInstance")), BindingRestrictions.Combine(args).Merge(BindingRestrictions.GetTypeRestriction(base.Expression, typeof(ComTypeClassDesc))));
		}
	}
}
