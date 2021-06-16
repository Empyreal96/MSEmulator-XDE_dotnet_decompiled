using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000609 RID: 1545
	internal class PSToStringBinder : DynamicMetaObjectBinder
	{
		// Token: 0x0600435C RID: 17244 RVA: 0x00162667 File Offset: 0x00160867
		internal static PSToStringBinder Get()
		{
			return PSToStringBinder._binder;
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x0600435D RID: 17245 RVA: 0x0016266E File Offset: 0x0016086E
		public override Type ReturnType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0016267A File Offset: 0x0016087A
		private PSToStringBinder()
		{
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00162684 File Offset: 0x00160884
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue || !args[0].HasValue)
			{
				return base.Defer(target, args).WriteToDebugLog(this);
			}
			if (target.Value is PSObject && PSObject.Base(target.Value) != target.Value)
			{
				return this.DeferForPSObject(new DynamicMetaObject[]
				{
					target,
					args[0]
				}).WriteToDebugLog(this);
			}
			BindingRestrictions restrictions = target.PSGetTypeRestriction();
			if (target.LimitType == typeof(string))
			{
				return new DynamicMetaObject(target.Expression.Cast(typeof(string)), restrictions).WriteToDebugLog(this);
			}
			return new DynamicMetaObject(PSToStringBinder.InvokeToString(args[0].Expression, target.Expression), restrictions).WriteToDebugLog(this);
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00162750 File Offset: 0x00160950
		internal static Expression InvokeToString(Expression context, Expression target)
		{
			if (target.Type == typeof(string))
			{
				return target;
			}
			return Expression.Call(CachedReflectionInfo.PSObject_ToStringParser, context.Cast(typeof(ExecutionContext)), target.Cast(typeof(object)));
		}

		// Token: 0x0400219A RID: 8602
		private static readonly PSToStringBinder _binder = new PSToStringBinder();
	}
}
