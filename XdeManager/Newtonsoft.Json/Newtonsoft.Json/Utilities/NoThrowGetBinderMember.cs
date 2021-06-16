using System;
using System.Dynamic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004C RID: 76
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x000156B0 File Offset: 0x000138B0
		public NoThrowGetBinderMember(GetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000156CC File Offset: 0x000138CC
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x040001BA RID: 442
		private readonly GetMemberBinder _innerBinder;
	}
}
