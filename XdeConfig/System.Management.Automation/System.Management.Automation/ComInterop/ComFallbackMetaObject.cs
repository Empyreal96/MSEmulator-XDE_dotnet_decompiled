using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A5D RID: 2653
	internal class ComFallbackMetaObject : DynamicMetaObject
	{
		// Token: 0x060069E8 RID: 27112 RVA: 0x00215000 File Offset: 0x00213200
		internal ComFallbackMetaObject(Expression expression, BindingRestrictions restrictions, object arg) : base(expression, restrictions, arg)
		{
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x0021500B File Offset: 0x0021320B
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			return binder.FallbackGetIndex(this.UnwrapSelf(), indexes);
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x0021501A File Offset: 0x0021321A
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			return binder.FallbackSetIndex(this.UnwrapSelf(), indexes, value);
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x0021502A File Offset: 0x0021322A
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			return binder.FallbackGetMember(this.UnwrapSelf());
		}

		// Token: 0x060069EC RID: 27116 RVA: 0x00215038 File Offset: 0x00213238
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			return binder.FallbackInvokeMember(this.UnwrapSelf(), args);
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x00215047 File Offset: 0x00213247
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			return binder.FallbackSetMember(this.UnwrapSelf(), value);
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x00215056 File Offset: 0x00213256
		protected virtual ComUnwrappedMetaObject UnwrapSelf()
		{
			return new ComUnwrappedMetaObject(ComObject.RcwFromComObject(base.Expression), base.Restrictions.Merge(ComBinderHelpers.GetTypeRestrictionForDynamicMetaObject(this)), ((ComObject)base.Value).RuntimeCallableWrapper);
		}
	}
}
