using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace System.Management.Automation.Language
{
	// Token: 0x02000610 RID: 1552
	internal class PSSwitchClauseEvalBinder : DynamicMetaObjectBinder
	{
		// Token: 0x0600437A RID: 17274 RVA: 0x001630D4 File Offset: 0x001612D4
		internal static PSSwitchClauseEvalBinder Get(SwitchFlags flags)
		{
			PSSwitchClauseEvalBinder result;
			lock (PSSwitchClauseEvalBinder._binderCache)
			{
				PSSwitchClauseEvalBinder psswitchClauseEvalBinder;
				if ((psswitchClauseEvalBinder = PSSwitchClauseEvalBinder._binderCache[(int)flags]) == null)
				{
					psswitchClauseEvalBinder = (PSSwitchClauseEvalBinder._binderCache[(int)flags] = new PSSwitchClauseEvalBinder(flags));
				}
				result = psswitchClauseEvalBinder;
			}
			return result;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0016312C File Offset: 0x0016132C
		private PSSwitchClauseEvalBinder(SwitchFlags flags)
		{
			this._flags = flags;
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x0600437C RID: 17276 RVA: 0x0016313B File Offset: 0x0016133B
		public override Type ReturnType
		{
			get
			{
				return typeof(bool);
			}
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x00163148 File Offset: 0x00161348
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue || !args[0].HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[]
				{
					args[0],
					args[1]
				}).WriteToDebugLog(this);
			}
			BindingRestrictions restrictions = target.PSGetTypeRestriction();
			if (target.Value is PSObject)
			{
				return new DynamicMetaObject(DynamicExpression.Dynamic(this, this.ReturnType, Expression.Call(CachedReflectionInfo.PSObject_Base, target.Expression.Cast(typeof(object))), args[0].Expression, args[1].Expression), restrictions).WriteToDebugLog(this);
			}
			if (target.Value == null)
			{
				return new DynamicMetaObject(Expression.Equal(args[0].Expression.Cast(typeof(object)), ExpressionCache.NullConstant), target.PSGetTypeRestriction()).WriteToDebugLog(this);
			}
			if (target.Value is ScriptBlock)
			{
				MethodCallExpression arg = Expression.Call(target.Expression.Cast(typeof(ScriptBlock)), CachedReflectionInfo.ScriptBlock_DoInvokeReturnAsIs, new Expression[]
				{
					ExpressionCache.Constant(true),
					Expression.Constant(ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe),
					args[0].CastOrConvert(typeof(object)),
					ExpressionCache.AutomationNullConstant,
					ExpressionCache.AutomationNullConstant,
					ExpressionCache.NullObjectArray
				});
				return new DynamicMetaObject(DynamicExpression.Dynamic(PSConvertBinder.Get(typeof(bool)), typeof(bool), arg), restrictions).WriteToDebugLog(this);
			}
			Expression expression = args[1].Expression;
			DynamicExpression dynamicExpression = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), args[0].Expression, expression);
			if (target.Value is Regex || (this._flags & SwitchFlags.Regex) != SwitchFlags.None)
			{
				MethodCallExpression expression2 = Expression.Call(CachedReflectionInfo.SwitchOps_ConditionSatisfiedRegex, ExpressionCache.Constant((this._flags & SwitchFlags.CaseSensitive) != SwitchFlags.None), target.Expression.Cast(typeof(object)), ExpressionCache.NullExtent, dynamicExpression, expression);
				return new DynamicMetaObject(expression2, restrictions).WriteToDebugLog(this);
			}
			if (target.Value is WildcardPattern || (this._flags & SwitchFlags.Wildcard) != SwitchFlags.None)
			{
				MethodCallExpression expression3 = Expression.Call(CachedReflectionInfo.SwitchOps_ConditionSatisfiedWildcard, ExpressionCache.Constant((this._flags & SwitchFlags.CaseSensitive) != SwitchFlags.None), target.Expression.Cast(typeof(object)), dynamicExpression, expression);
				return new DynamicMetaObject(expression3, restrictions).WriteToDebugLog(this);
			}
			DynamicExpression left = DynamicExpression.Dynamic(PSToStringBinder.Get(), typeof(string), target.Expression, expression);
			return new DynamicMetaObject(Compiler.CallStringEquals(left, dynamicExpression, (this._flags & SwitchFlags.CaseSensitive) == SwitchFlags.None), restrictions).WriteToDebugLog(this);
		}

		// Token: 0x040021A8 RID: 8616
		private static readonly PSSwitchClauseEvalBinder[] _binderCache = new PSSwitchClauseEvalBinder[32];

		// Token: 0x040021A9 RID: 8617
		private readonly SwitchFlags _flags;
	}
}
