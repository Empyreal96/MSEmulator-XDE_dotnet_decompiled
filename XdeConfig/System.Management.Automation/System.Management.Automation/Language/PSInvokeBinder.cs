using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x0200061F RID: 1567
	internal class PSInvokeBinder : InvokeBinder
	{
		// Token: 0x06004435 RID: 17461 RVA: 0x0016B6E1 File Offset: 0x001698E1
		internal PSInvokeBinder(CallInfo callInfo) : base(callInfo)
		{
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0016B6EA File Offset: 0x001698EA
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			return errorSuggestion ?? target.ThrowRuntimeError(args, BindingRestrictions.Empty, "CannotInvoke", ParserStrings.CannotInvoke, new Expression[0]);
		}
	}
}
