using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000625 RID: 1573
	internal class PSInvokeBaseCtorBinder : InvokeMemberBinder
	{
		// Token: 0x0600446C RID: 17516 RVA: 0x0016D83C File Offset: 0x0016BA3C
		public static PSInvokeBaseCtorBinder Get(CallInfo callInfo, PSMethodInvocationConstraints constraints)
		{
			PSInvokeBaseCtorBinder psinvokeBaseCtorBinder;
			lock (PSInvokeBaseCtorBinder._binderCache)
			{
				Tuple<CallInfo, PSMethodInvocationConstraints> key = Tuple.Create<CallInfo, PSMethodInvocationConstraints>(callInfo, constraints);
				if (!PSInvokeBaseCtorBinder._binderCache.TryGetValue(key, out psinvokeBaseCtorBinder))
				{
					psinvokeBaseCtorBinder = new PSInvokeBaseCtorBinder(callInfo, constraints);
					PSInvokeBaseCtorBinder._binderCache.Add(key, psinvokeBaseCtorBinder);
				}
			}
			return psinvokeBaseCtorBinder;
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0016D8A4 File Offset: 0x0016BAA4
		internal PSInvokeBaseCtorBinder(CallInfo callInfo, PSMethodInvocationConstraints constraints) : base(".ctor", false, callInfo)
		{
			this._callInfo = callInfo;
			this._constraints = constraints;
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0016D8EC File Offset: 0x0016BAEC
		public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			if (target.HasValue)
			{
				if (!args.Any((DynamicMetaObject arg) => !arg.HasValue))
				{
					ConstructorInfo[] constructors = this._constraints.MethodTargetType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					BindingRestrictions bindingRestrictions = (target.Value is PSObject) ? BindingRestrictions.GetTypeRestriction(target.Expression, target.Value.GetType()) : target.PSGetTypeRestriction();
					bindingRestrictions = args.Aggregate(bindingRestrictions, (BindingRestrictions current, DynamicMetaObject arg) => current.Merge(arg.PSGetMethodArgumentRestriction()));
					MethodInformation[] methodInformationArray = DotNetAdapter.GetMethodInformationArray((from c in constructors
					where c.IsPublic || c.IsFamily
					select c).ToArray<ConstructorInfo>());
					return PSInvokeMemberBinder.InvokeDotNetMethod(this._callInfo, "new", this._constraints, PSInvokeMemberBinder.MethodInvocationType.BaseCtor, target, args, bindingRestrictions, methodInformationArray, typeof(MethodException));
				}
			}
			return base.Defer(args.Prepend(target).ToArray<DynamicMetaObject>());
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0016D9F8 File Offset: 0x0016BBF8
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject result = errorSuggestion;
			if (errorSuggestion == null)
			{
				result = new DynamicMetaObject(DynamicExpression.Dynamic(new PSInvokeBinder(base.CallInfo), typeof(object), from dmo in args.Prepend(target)
				select dmo.Expression), target.Restrictions.Merge(BindingRestrictions.Combine(args)));
			}
			return result;
		}

		// Token: 0x0400220B RID: 8715
		private readonly CallInfo _callInfo;

		// Token: 0x0400220C RID: 8716
		private readonly PSMethodInvocationConstraints _constraints;

		// Token: 0x0400220D RID: 8717
		private static readonly Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints>, PSInvokeBaseCtorBinder> _binderCache = new Dictionary<Tuple<CallInfo, PSMethodInvocationConstraints>, PSInvokeBaseCtorBinder>(new PSInvokeBaseCtorBinder.KeyComparer());

		// Token: 0x02000626 RID: 1574
		private class KeyComparer : IEqualityComparer<Tuple<CallInfo, PSMethodInvocationConstraints>>
		{
			// Token: 0x06004475 RID: 17525 RVA: 0x0016DA74 File Offset: 0x0016BC74
			public bool Equals(Tuple<CallInfo, PSMethodInvocationConstraints> x, Tuple<CallInfo, PSMethodInvocationConstraints> y)
			{
				if (!x.Item1.Equals(y.Item1))
				{
					return false;
				}
				if (x.Item2 != null)
				{
					return x.Item2.Equals(y.Item2);
				}
				return y.Item2 == null;
			}

			// Token: 0x06004476 RID: 17526 RVA: 0x0016DAAE File Offset: 0x0016BCAE
			public int GetHashCode(Tuple<CallInfo, PSMethodInvocationConstraints> obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
