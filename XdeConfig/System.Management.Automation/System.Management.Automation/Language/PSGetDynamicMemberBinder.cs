using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Language
{
	// Token: 0x0200060E RID: 1550
	internal class PSGetDynamicMemberBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004371 RID: 17265 RVA: 0x00162BB4 File Offset: 0x00160DB4
		internal static PSGetDynamicMemberBinder Get(TypeDefinitionAst classScope, bool @static)
		{
			PSGetDynamicMemberBinder psgetDynamicMemberBinder;
			lock (PSGetDynamicMemberBinder._binderCache)
			{
				Type type = (classScope != null) ? classScope.Type : null;
				Tuple<Type, bool> key = Tuple.Create<Type, bool>(type, @static);
				if (!PSGetDynamicMemberBinder._binderCache.TryGetValue(key, out psgetDynamicMemberBinder))
				{
					psgetDynamicMemberBinder = new PSGetDynamicMemberBinder(type, @static);
					PSGetDynamicMemberBinder._binderCache.Add(key, psgetDynamicMemberBinder);
				}
			}
			return psgetDynamicMemberBinder;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x00162C28 File Offset: 0x00160E28
		private PSGetDynamicMemberBinder(Type classScope, bool @static)
		{
			this._static = @static;
			this._classScope = classScope;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x00162C40 File Offset: 0x00160E40
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
			BindingRestrictions bindingRestrictions;
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
				if (target.Value is IDictionary)
				{
					bindingRestrictions = target.PSGetTypeRestriction();
					bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Not(Expression.TypeIs(args[0].Expression, typeof(string)))));
					return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.PSGetDynamicMemberBinder_GetIDictionaryMember, target.Expression.Cast(typeof(IDictionary)), args[0].Expression.Cast(typeof(object))), bindingRestrictions).WriteToDebugLog(this);
				}
				text = PSObject.ToStringParser(null, dynamicMetaObject.Value);
				arg = PSToStringBinder.InvokeToString(ExpressionCache.NullConstant, dynamicMetaObject.Expression);
			}
			DynamicExpression expression = DynamicExpression.Dynamic(PSGetMemberBinder.Get(text, this._classScope, this._static), typeof(object), target.Expression);
			bindingRestrictions = BindingRestrictions.Empty;
			bindingRestrictions = bindingRestrictions.Merge(args[0].PSGetTypeRestriction());
			bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Call(CachedReflectionInfo.String_Equals, Expression.Constant(text), arg, ExpressionCache.Ordinal)));
			return new DynamicMetaObject(expression, bindingRestrictions).WriteToDebugLog(this);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x00162DF8 File Offset: 0x00160FF8
		internal static object GetIDictionaryMember(IDictionary hash, object key)
		{
			try
			{
				key = PSObject.Base(key);
				if (hash.Contains(key))
				{
					return hash[key];
				}
			}
			catch (InvalidOperationException)
			{
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS.IsStrictVersion(2))
			{
				throw new PropertyNotFoundException("PropertyNotFoundStrict", null, ParserStrings.PropertyNotFoundStrict, new object[]
				{
					LanguagePrimitives.ConvertTo<string>(key)
				});
			}
			return null;
		}

		// Token: 0x040021A2 RID: 8610
		private static readonly Dictionary<Tuple<Type, bool>, PSGetDynamicMemberBinder> _binderCache = new Dictionary<Tuple<Type, bool>, PSGetDynamicMemberBinder>(new PSDynamicGetOrSetBinderKeyComparer());

		// Token: 0x040021A3 RID: 8611
		private readonly bool _static;

		// Token: 0x040021A4 RID: 8612
		private readonly Type _classScope;
	}
}
