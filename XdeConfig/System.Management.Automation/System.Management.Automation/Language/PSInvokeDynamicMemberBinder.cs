using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Management.Automation.Language
{
	// Token: 0x0200060B RID: 1547
	internal class PSInvokeDynamicMemberBinder : DynamicMetaObjectBinder
	{
		// Token: 0x06004367 RID: 17255 RVA: 0x001628F4 File Offset: 0x00160AF4
		internal static PSInvokeDynamicMemberBinder Get(CallInfo callInfo, TypeDefinitionAst classScopeAst, bool @static, bool propertySetter, PSMethodInvocationConstraints constraints)
		{
			Type type = (classScopeAst != null) ? classScopeAst.Type : null;
			PSInvokeDynamicMemberBinder psinvokeDynamicMemberBinder;
			lock (PSInvokeDynamicMemberBinder._binderCache)
			{
				Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type> key = Tuple.Create<CallInfo, PSMethodInvocationConstraints, bool, bool, Type>(callInfo, constraints, propertySetter, @static, type);
				if (!PSInvokeDynamicMemberBinder._binderCache.TryGetValue(key, out psinvokeDynamicMemberBinder))
				{
					psinvokeDynamicMemberBinder = new PSInvokeDynamicMemberBinder(callInfo, @static, propertySetter, constraints, type);
					PSInvokeDynamicMemberBinder._binderCache.Add(key, psinvokeDynamicMemberBinder);
				}
			}
			return psinvokeDynamicMemberBinder;
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00162970 File Offset: 0x00160B70
		private PSInvokeDynamicMemberBinder(CallInfo callInfo, bool @static, bool propertySetter, PSMethodInvocationConstraints constraints, Type classScope)
		{
			this._callInfo = callInfo;
			this._static = @static;
			this._propertySetter = propertySetter;
			this._constraints = constraints;
			this._classScope = classScope;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x001629A0 File Offset: 0x00160BA0
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
			DynamicMetaObject dynamicMetaObject2 = PSInvokeMemberBinder.Get(text, this._callInfo, this._static, this._propertySetter, this._constraints, this._classScope).FallbackInvokeMember(target, args.Skip(1).ToArray<DynamicMetaObject>());
			BindingRestrictions bindingRestrictions = dynamicMetaObject2.Restrictions;
			bindingRestrictions = bindingRestrictions.Merge(args[0].PSGetTypeRestriction());
			bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(Expression.Call(CachedReflectionInfo.String_Equals, Expression.Constant(text), arg, ExpressionCache.Ordinal)));
			return new DynamicMetaObject(dynamicMetaObject2.Expression, bindingRestrictions).WriteToDebugLog(this);
		}

		// Token: 0x0400219C RID: 8604
		private static readonly Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type>, PSInvokeDynamicMemberBinder> _binderCache = new Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type>, PSInvokeDynamicMemberBinder>(new PSInvokeDynamicMemberBinder.KeyComparer());

		// Token: 0x0400219D RID: 8605
		private readonly CallInfo _callInfo;

		// Token: 0x0400219E RID: 8606
		private readonly bool _static;

		// Token: 0x0400219F RID: 8607
		private readonly bool _propertySetter;

		// Token: 0x040021A0 RID: 8608
		private readonly PSMethodInvocationConstraints _constraints;

		// Token: 0x040021A1 RID: 8609
		private readonly Type _classScope;

		// Token: 0x0200060C RID: 1548
		private class KeyComparer : IEqualityComparer<Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type>>
		{
			// Token: 0x0600436B RID: 17259 RVA: 0x00162AF8 File Offset: 0x00160CF8
			public bool Equals(Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type> x, Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type> y)
			{
				if (!x.Item1.Equals(y.Item1) || x.Item2 != null)
				{
					return x.Item2.Equals(y.Item2) && x.Item3 == y.Item3 && x.Item4 == y.Item4 && x.Item5 == y.Item5;
				}
				return y.Item2 == null;
			}

			// Token: 0x0600436C RID: 17260 RVA: 0x00162B6C File Offset: 0x00160D6C
			public int GetHashCode(Tuple<CallInfo, PSMethodInvocationConstraints, bool, bool, Type> obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
