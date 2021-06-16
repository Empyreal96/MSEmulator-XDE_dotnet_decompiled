using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A67 RID: 2663
	internal class ComMetaObject : DynamicMetaObject
	{
		// Token: 0x06006A0F RID: 27151 RVA: 0x00215C74 File Offset: 0x00213E74
		internal ComMetaObject(Expression expression, BindingRestrictions restrictions, object arg) : base(expression, restrictions, arg)
		{
		}

		// Token: 0x06006A10 RID: 27152 RVA: 0x00215C7F File Offset: 0x00213E7F
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			return binder.Defer(args.AddFirst(this.WrapSelf()));
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x00215C93 File Offset: 0x00213E93
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			return binder.Defer(args.AddFirst(this.WrapSelf()));
		}

		// Token: 0x06006A12 RID: 27154 RVA: 0x00215CA7 File Offset: 0x00213EA7
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			return binder.Defer(this.WrapSelf(), new DynamicMetaObject[0]);
		}

		// Token: 0x06006A13 RID: 27155 RVA: 0x00215CBC File Offset: 0x00213EBC
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			return binder.Defer(this.WrapSelf(), new DynamicMetaObject[]
			{
				value
			});
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x00215CE1 File Offset: 0x00213EE1
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			return binder.Defer(this.WrapSelf(), indexes);
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x00215CF0 File Offset: 0x00213EF0
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			return binder.Defer(this.WrapSelf(), indexes.AddLast(value));
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x00215D08 File Offset: 0x00213F08
		private DynamicMetaObject WrapSelf()
		{
			return new DynamicMetaObject(ComObject.RcwToComObject(base.Expression), BindingRestrictions.GetExpressionRestriction(Expression.Call(typeof(ComObject).GetMethod("IsComObject", BindingFlags.Static | BindingFlags.NonPublic), Helpers.Convert(base.Expression, typeof(object)))));
		}
	}
}
