using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A64 RID: 2660
	internal sealed class ComInvokeAction : InvokeBinder
	{
		// Token: 0x060069F5 RID: 27125 RVA: 0x00215094 File Offset: 0x00213294
		internal ComInvokeAction(CallInfo callInfo) : base(callInfo)
		{
		}

		// Token: 0x060069F6 RID: 27126 RVA: 0x0021509D File Offset: 0x0021329D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060069F7 RID: 27127 RVA: 0x002150A5 File Offset: 0x002132A5
		public override bool Equals(object obj)
		{
			return base.Equals(obj as ComInvokeAction);
		}

		// Token: 0x060069F8 RID: 27128 RVA: 0x002150B4 File Offset: 0x002132B4
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject result;
			if (ComBinder.TryBindInvoke(this, target, args, out result))
			{
				return result;
			}
			return errorSuggestion ?? new DynamicMetaObject(Expression.Throw(Expression.New(typeof(NotSupportedException).GetConstructor(new Type[]
			{
				typeof(string)
			}), new Expression[]
			{
				Expression.Constant(ParserStrings.CannotCall)
			})), target.Restrictions.Merge(BindingRestrictions.Combine(args)));
		}
	}
}
