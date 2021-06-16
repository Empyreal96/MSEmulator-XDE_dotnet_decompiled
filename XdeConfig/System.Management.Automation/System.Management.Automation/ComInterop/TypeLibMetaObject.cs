using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A8D RID: 2701
	internal class TypeLibMetaObject : DynamicMetaObject
	{
		// Token: 0x06006B38 RID: 27448 RVA: 0x0021A1BE File Offset: 0x002183BE
		internal TypeLibMetaObject(Expression expression, ComTypeLibDesc lib) : base(expression, BindingRestrictions.Empty, lib)
		{
			this._lib = lib;
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x0021A1D4 File Offset: 0x002183D4
		private DynamicMetaObject TryBindGetMember(string name)
		{
			if (this._lib.HasMember(name))
			{
				BindingRestrictions restrictions = BindingRestrictions.GetTypeRestriction(base.Expression, typeof(ComTypeLibDesc)).Merge(BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Property(Utils.Convert(base.Expression, typeof(ComTypeLibDesc)), typeof(ComTypeLibDesc).GetProperty("Guid")), Utils.Constant(this._lib.Guid))));
				return new DynamicMetaObject(Utils.Constant(((ComTypeLibDesc)base.Value).GetTypeLibObjectDesc(name)), restrictions);
			}
			return null;
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x0021A278 File Offset: 0x00218478
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			return this.TryBindGetMember(binder.Name) ?? base.BindGetMember(binder);
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x0021A294 File Offset: 0x00218494
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			DynamicMetaObject dynamicMetaObject = this.TryBindGetMember(binder.Name);
			if (dynamicMetaObject != null)
			{
				return binder.FallbackInvoke(dynamicMetaObject, args, null);
			}
			return base.BindInvokeMember(binder, args);
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x0021A2C3 File Offset: 0x002184C3
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._lib.GetMemberNames();
		}

		// Token: 0x0400333C RID: 13116
		private readonly ComTypeLibDesc _lib;
	}
}
