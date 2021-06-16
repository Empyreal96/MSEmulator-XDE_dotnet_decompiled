using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8B RID: 2699
	internal class TypeEnumMetaObject : DynamicMetaObject
	{
		// Token: 0x06006B30 RID: 27440 RVA: 0x00219F5F File Offset: 0x0021815F
		internal TypeEnumMetaObject(ComTypeEnumDesc desc, Expression expression) : base(expression, BindingRestrictions.Empty, desc)
		{
			this._desc = desc;
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x00219F78 File Offset: 0x00218178
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (this._desc.HasMember(binder.Name))
			{
				return new DynamicMetaObject(Expression.Constant(((ComTypeEnumDesc)base.Value).GetValue(binder.Name), typeof(object)), this.EnumRestrictions());
			}
			throw new NotImplementedException();
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x00219FCE File Offset: 0x002181CE
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._desc.GetMemberNames();
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x00219FDC File Offset: 0x002181DC
		private BindingRestrictions EnumRestrictions()
		{
			return BindingRestrictions.GetTypeRestriction(base.Expression, typeof(ComTypeEnumDesc)).Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Property(Expression.Property(Utils.Convert(base.Expression, typeof(ComTypeEnumDesc)), typeof(ComTypeDesc).GetProperty("TypeLib")), typeof(ComTypeLibDesc).GetProperty("Guid")), Utils.Constant(this._desc.TypeLib.Guid)))).Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Property(Utils.Convert(base.Expression, typeof(ComTypeEnumDesc)), typeof(ComTypeEnumDesc).GetProperty("TypeName")), Utils.Constant(this._desc.TypeName))));
		}

		// Token: 0x0400333A RID: 13114
		private readonly ComTypeEnumDesc _desc;
	}
}
