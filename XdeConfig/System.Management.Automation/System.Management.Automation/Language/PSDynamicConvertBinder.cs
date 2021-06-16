using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000613 RID: 1555
	internal class PSDynamicConvertBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004388 RID: 17288 RVA: 0x00163A97 File Offset: 0x00161C97
		internal static PSDynamicConvertBinder Get()
		{
			return PSDynamicConvertBinder._binder;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x00163A9E File Offset: 0x00161C9E
		private PSDynamicConvertBinder()
		{
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00163AA8 File Offset: 0x00161CA8
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			DynamicMetaObject dynamicMetaObject = args[0];
			if (!target.HasValue || !dynamicMetaObject.HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[]
				{
					dynamicMetaObject
				}).WriteToDebugLog(this);
			}
			Type type = target.Value as Type;
			BindingRestrictions restrictions = BindingRestrictions.GetInstanceRestriction(target.Expression, type).Merge(dynamicMetaObject.PSGetTypeRestriction());
			return new DynamicMetaObject(DynamicExpression.Dynamic(PSConvertBinder.Get(type), type, dynamicMetaObject.Expression).Cast(typeof(object)), restrictions).WriteToDebugLog(this);
		}

		// Token: 0x040021AD RID: 8621
		private static readonly PSDynamicConvertBinder _binder = new PSDynamicConvertBinder();
	}
}
