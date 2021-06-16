using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x0200060F RID: 1551
	internal class PSSetDynamicMemberBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004376 RID: 17270 RVA: 0x00162E80 File Offset: 0x00161080
		internal static PSSetDynamicMemberBinder Get(TypeDefinitionAst classScope, bool @static)
		{
			PSSetDynamicMemberBinder pssetDynamicMemberBinder;
			lock (PSSetDynamicMemberBinder._binderCache)
			{
				Type type = (classScope != null) ? classScope.Type : null;
				Tuple<Type, bool> key = Tuple.Create<Type, bool>(type, @static);
				if (!PSSetDynamicMemberBinder._binderCache.TryGetValue(key, out pssetDynamicMemberBinder))
				{
					pssetDynamicMemberBinder = new PSSetDynamicMemberBinder(type, @static);
					PSSetDynamicMemberBinder._binderCache.Add(key, pssetDynamicMemberBinder);
				}
			}
			return pssetDynamicMemberBinder;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x00162EF4 File Offset: 0x001610F4
		private PSSetDynamicMemberBinder(Type classScope, bool @static)
		{
			this._static = @static;
			this._classScope = classScope;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x00162F0C File Offset: 0x0016110C
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			if (!target.HasValue || !args[0].HasValue)
			{
				return base.Defer(target, new DynamicMetaObject[]
				{
					args[0]
				}).WriteToDebugLog(this);
			}
			DynamicMetaObject dynamicMetaObject = args[0];
			object obj = PSObject.Base(dynamicMetaObject.Value);
			string text = obj as string;
			Expression arg;
			if (text != null)
			{
				if (dynamicMetaObject.Value is PSObject)
				{
					arg = Expression.Call(CachedReflectionInfo.PSObject_Base, dynamicMetaObject.Expression).Cast(typeof(string));
				}
				else
				{
					arg = dynamicMetaObject.Expression.Cast(typeof(string));
				}
			}
			else
			{
				text = PSObject.ToStringParser(null, dynamicMetaObject.Value);
				arg = PSToStringBinder.InvokeToString(ExpressionCache.NullConstant, dynamicMetaObject.Expression);
			}
			DynamicExpression dynamicExpression = DynamicExpression.Dynamic(PSSetMemberBinder.Get(text, this._classScope, this._static), typeof(object), target.Expression, args[1].Expression);
			BindingRestrictions bindingRestrictions = BindingRestrictions.Empty;
			bindingRestrictions = bindingRestrictions.Merge(args[0].PSGetTypeRestriction());
			bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Call(CachedReflectionInfo.String_Equals, Expression.Constant(text), arg, ExpressionCache.Ordinal)));
			Expression expression;
			if (target.Value is IDictionary)
			{
				ParameterExpression parameterExpression = Expression.Variable(typeof(Exception));
				DynamicMetaObject dynamicMetaObject2 = PSSetIndexBinder.Get(1, null).FallbackSetIndex(target, new DynamicMetaObject[]
				{
					args[0]
				}, args[1]);
				expression = Expression.TryCatch(dynamicMetaObject2.Expression, new CatchBlock[]
				{
					Expression.Catch(parameterExpression, Expression.Block(Expression.Call(CachedReflectionInfo.CommandProcessorBase_CheckForSevereException, parameterExpression), dynamicExpression))
				});
			}
			else
			{
				expression = dynamicExpression;
			}
			return new DynamicMetaObject(expression, bindingRestrictions).WriteToDebugLog(this);
		}

		// Token: 0x040021A5 RID: 8613
		private static readonly Dictionary<Tuple<Type, bool>, PSSetDynamicMemberBinder> _binderCache = new Dictionary<Tuple<Type, bool>, PSSetDynamicMemberBinder>(new PSDynamicGetOrSetBinderKeyComparer());

		// Token: 0x040021A6 RID: 8614
		private readonly bool _static;

		// Token: 0x040021A7 RID: 8615
		private readonly Type _classScope;
	}
}
