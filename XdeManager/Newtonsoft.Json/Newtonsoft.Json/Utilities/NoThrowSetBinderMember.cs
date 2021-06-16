using System;
using System.Dynamic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004D RID: 77
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		// Token: 0x060004FA RID: 1274 RVA: 0x00015706 File Offset: 0x00013906
		public NoThrowSetBinderMember(SetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00015724 File Offset: 0x00013924
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[]
			{
				value
			});
			return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
		}

		// Token: 0x040001BB RID: 443
		private readonly SetMemberBinder _innerBinder;
	}
}
